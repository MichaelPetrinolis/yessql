using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Common;
using System.Threading.Tasks;
using YesSql.Collections;

namespace YesSql.Commands
{
    public sealed class CreateDocumentCommand : DocumentCommand
    {
        private readonly string _tablePrefix;

        public override int ExecutionOrder { get; } = 0;

        public CreateDocumentCommand(string tablePrefix)
        {
            _tablePrefix = tablePrefix;
        }

        public CreateDocumentCommand(Document document, string tablePrefix) : base(document)
        {
            _tablePrefix = tablePrefix;
        }

        public override Task ExecuteAsync(DbConnection connection, DbTransaction transaction, ISqlDialect dialect, ILogger logger)
        {
            var documentTable = CollectionHelper.Current.GetPrefixedName(Store.DocumentTable);
            var insertCmd = "insert into " + dialect.QuoteForTableName(_tablePrefix + documentTable) + " (" + dialect.QuoteForColumnName("Id") + ", " + dialect.QuoteForColumnName("Type") + ", " + dialect.QuoteForColumnName("Content") + ") values (@Id, @Type, @Content);";

            logger.LogTrace(insertCmd);

            var flags= CommandFlags.None;
            if (connection.ConnectionString.ToLowerInvariant().Trim().Contains("multipleactiveresultsets=true"))
            {
                flags = CommandFlags.Pipelined;
            }

            if (Document != null)
            {
                return connection.ExecuteScalarAsync<int>(new CommandDefinition(insertCmd, Document, transaction, flags: flags));
            }
            else
            {
                return connection.ExecuteAsync(new CommandDefinition(insertCmd, GetDocuments(), transaction, flags: flags));
            }

        }
    }
}
