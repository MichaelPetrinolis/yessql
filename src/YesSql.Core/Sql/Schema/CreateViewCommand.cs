using System;
using System.Data;
using System.Linq;

namespace YesSql.Sql.Schema
{
    public class CreateViewCommand : SchemaCommand, ICreateViewCommand
    {
        public string SrcTableName { get; }
        public string DocumentType { get; }
        public CreateViewCommand(string name, string srcTableName, string documentType)
            : base(name, SchemaCommandType.CreateView)
        {
            SrcTableName = srcTableName;
            DocumentType = documentType;
        }

        public ICreateViewCommand Column(string columnName, string property, DbType dbType, byte precision, byte scale, int? length)
        {
            var command = new ViewColumnCommand(Name, columnName, property, dbType, precision, scale, length);
            TableCommands.Add(command);
            return this;
        }

        public void AddIndex(string indexName, string documentType, IViewIndexColumnCommand[] indexColumns)
        {
            var command = new AddViewIndexCommand(SrcTableName, indexName, documentType, indexColumns);
            TableCommands.Add(command);
        }

    }
}
