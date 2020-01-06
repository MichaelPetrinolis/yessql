using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using YesSql.Collections;
using YesSql.Sql.Schema;

namespace YesSql.Sql
{
    public class SchemaBuilder : ISchemaBuilder
    {
        private ICommandInterpreter _builder;
        private readonly ILogger _logger;

        public string TablePrefix { get; private set; }
        public ISqlDialect Dialect { get; private set; }
        public DbConnection Connection { get; private set; }
        public DbTransaction Transaction { get; private set; }
        public bool ThrowOnError { get; set; } = true;

        public SchemaBuilder(IConfiguration configuration, DbTransaction transaction, bool throwOnError = true)
        {
            Transaction = transaction;
            _logger = configuration.Logger;
            Connection = Transaction.Connection;
            _builder = CommandInterpreterFactory.For(Connection);
            Dialect = SqlDialectFactory.For(configuration.ConnectionFactory.DbConnectionType);
            TablePrefix = configuration.TablePrefix;
            ThrowOnError = throwOnError;
        }

        private void Execute(IEnumerable<string> statements)
        {
            foreach (var statement in statements)
            {
                _logger.LogTrace(statement);
                Connection.Execute(statement, null, Transaction);
            }
        }

        private string Prefix(string table)
        {
            return TablePrefix + table;
        }

        public ISchemaBuilder CreateMapIndexTable(string name, Action<ICreateTableCommand> table)
        {
            return CreateMapIndexTable(name, null, table);
        }

        public ISchemaBuilder CreateMapIndexTable(string name, string documentType, Action<ICreateTableCommand> table)
        {
            try
            {
                var createTable = new CreateTableCommand(Prefix(name));
                var collection = CollectionHelper.Current;
                var documentTable = collection.GetPrefixedName(Store.DocumentTable);

                createTable
                    .Column<int>("Id", column => column.PrimaryKey().Identity().NotNull())
                    .Column<int>("DocumentId");

                table(createTable);

                if (Dialect.SupportsJson)
                {
                    var createView = new CreateViewCommand(Prefix(name), Prefix("Document"), documentType);
                    foreach (var item in createTable.TableCommands)
                    {
                        var createColumnCommand = item as ICreateColumnCommand;
                        if (createColumnCommand != null)
                        {
                            string columnName = createColumnCommand.ColumnName;
                            string property = createColumnCommand.DocumentProperty;
                            if (columnName == "DocumentId")
                            {
                                property = "Id";
                            }
                            createView.Column(columnName, property, createColumnCommand.DbType, createColumnCommand.Precision, createColumnCommand.Scale, createColumnCommand.Length);
                        }
                    }
                    string indexName = "IX_" + (Dialect.PrefixIndex ? TablePrefix + name : name);
                    var indexColumns = createView.TableCommands.OfType<IViewColumnCommand>()
                        .Where(c => c.DocumentProperty != "Id" && c.DocumentProperty != "DocumentId")
                        .Select(c => new ViewIndexColumnCommand(name, c.ColumnName, c.DbType, c.Precision, c.Scale, c.Length))
                        .ToArray();

                    createView.AddIndex(indexName, documentType, indexColumns);
                    Execute(_builder.CreateSql(createView));
                }
                else
                {
                    Execute(_builder.CreateSql(createTable));
                    CreateForeignKey("FK_" + name, name, new[] { "DocumentId" }, documentTable, new[] { "Id" });
                }
            }
            catch
            {
                if (ThrowOnError)
                {
                    throw;
                }
            }

            return this;
        }

        public ISchemaBuilder CreateReduceIndexTable(string name, Action<ICreateTableCommand> table)
        {
            try
            {
                var createTable = new CreateTableCommand(Prefix(name));
                var collection = CollectionHelper.Current;
                var documentTable = collection.GetPrefixedName(Store.DocumentTable);

                createTable
                    .Column<int>("Id", column => column.Identity().NotNull());

                table(createTable);
                Execute(_builder.CreateSql(createTable));

                var bridgeTableName = name + "_" + documentTable;

                CreateTable(bridgeTableName, bridge => bridge
                    .Column<int>(name + "Id", column => column.NotNull())
                    .Column<int>("DocumentId", column => column.NotNull())
                );

                CreateForeignKey("FK_" + bridgeTableName + "_Id", bridgeTableName, new[] { name + "Id" }, name, new[] { "Id" });
                CreateForeignKey("FK_" + bridgeTableName + "_DocumentId", bridgeTableName, new[] { "DocumentId" }, documentTable, new[] { "Id" });
            }
            catch
            {
                if (ThrowOnError)
                {
                    throw;
                }
            }

            return this;
        }

        public ISchemaBuilder DropReduceIndexTable(string name)
        {
            try
            {
                var collection = CollectionHelper.Current;
                var documentTable = collection.GetPrefixedName(Store.DocumentTable);

                var bridgeTableName = name + "_" + documentTable;

                if (String.IsNullOrEmpty(Dialect.CascadeConstraintsString))
                {
                    DropForeignKey(bridgeTableName, "FK_" + bridgeTableName + "_Id");
                    DropForeignKey(bridgeTableName, "FK_" + bridgeTableName + "_DocumentId");
                }

                DropTable(bridgeTableName);
                DropTable(name);
            }
            catch
            {
                if (ThrowOnError)
                {
                    throw;
                }
            }

            return this;
        }

        public ISchemaBuilder DropMapIndexTable(string name)
        {
            try
            {
                if (Dialect.SupportsJson)
                {
                    DropView(name);
                }
                else
                {
                    if (String.IsNullOrEmpty(Dialect.CascadeConstraintsString))
                    {
                        DropForeignKey(name, "FK_" + name);
                    }

                    DropTable(name);
                }
            }
            catch
            {
                if (ThrowOnError)
                {
                    throw;
                }
            }

            return this;
        }

        public ISchemaBuilder CreateTable(string name, Action<ICreateTableCommand> table)
        {
            try
            {
                var createTable = new CreateTableCommand(Prefix(name));
                table(createTable);
                Execute(_builder.CreateSql(createTable));
            }
            catch
            {
                if (ThrowOnError)
                {
                    throw;
                }
            }

            return this;
        }

        public ISchemaBuilder AlterTable(string name, Action<IAlterTableCommand> table)
        {
            try
            {
                var alterTable = new AlterTableCommand(Prefix(name), Dialect, TablePrefix);
                table(alterTable);
                Execute(_builder.CreateSql(alterTable));
            }
            catch
            {
                if (ThrowOnError)
                {
                    throw;
                }
            }

            return this;
        }

        public ISchemaBuilder DropTable(string name)
        {
            try
            {
                var deleteTable = new DropTableCommand(Prefix(name));
                Execute(_builder.CreateSql(deleteTable));
            }
            catch
            {
                if (ThrowOnError)
                {
                    throw;
                }
            }

            return this;
        }

        public ISchemaBuilder DropView(string name)
        {
            try
            {
                var deleteView = new DropViewCommand(Prefix(name));
                Execute(_builder.CreateSql(deleteView));
            }
            catch
            {
                if (ThrowOnError)
                {
                    throw;
                }
            }

            return this;
        }

        public ISchemaBuilder CreateForeignKey(string name, string srcTable, string[] srcColumns, string destTable, string[] destColumns)
        {
            try
            {
                var command = new CreateForeignKeyCommand(Prefix(name), Prefix(srcTable), srcColumns, Prefix(destTable), destColumns);
                Execute(_builder.CreateSql(command));
            }
            catch
            {
                if (ThrowOnError)
                {
                    throw;
                }
            }

            return this;
        }

        public ISchemaBuilder CreateForeignKey(string name, string srcModule, string srcTable, string[] srcColumns, string destTable, string[] destColumns)
        {
            try
            {
                var command = new CreateForeignKeyCommand(Prefix(name), Prefix(srcTable), srcColumns, Prefix(destTable), destColumns);
                Execute(_builder.CreateSql(command));
            }
            catch
            {
                if (ThrowOnError)
                {
                    throw;
                }
            }
            return this;
        }

        public ISchemaBuilder CreateForeignKey(string name, string srcTable, string[] srcColumns, string destModule, string destTable, string[] destColumns)
        {
            try
            {
                var command = new CreateForeignKeyCommand(Prefix(name), Prefix(srcTable), srcColumns, Prefix(destTable), destColumns);
                Execute(_builder.CreateSql(command));

            }
            catch
            {
                if (ThrowOnError)
                {
                    throw;
                }
            }

            return this;
        }

        public ISchemaBuilder CreateForeignKey(string name, string srcModule, string srcTable, string[] srcColumns, string destModule, string destTable, string[] destColumns)
        {
            try
            {
                var command = new CreateForeignKeyCommand(Prefix(name), Prefix(srcTable), srcColumns, Prefix(destTable), destColumns);
                Execute(_builder.CreateSql(command));
            }
            catch
            {
                if (ThrowOnError)
                {
                    throw;
                }
            }

            return this;
        }

        public ISchemaBuilder DropForeignKey(string srcTable, string name)
        {
            try
            {
                var command = new DropForeignKeyCommand(Prefix(srcTable), Prefix(name));
                Execute(_builder.CreateSql(command));
            }
            catch
            {
                if (ThrowOnError)
                {
                    throw;
                }
            }

            return this;
        }
    }
}
