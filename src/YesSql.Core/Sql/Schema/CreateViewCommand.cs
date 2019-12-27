using System;
using System.Data;

namespace YesSql.Sql.Schema
{
    public class CreateViewCommand : SchemaCommand, ICreateViewCommand
    {
        public string SrcTableName { get; }
        public CreateViewCommand(string name, string srcTableName)
            : base(name, SchemaCommandType.CreateView)
        {
            SrcTableName = srcTableName;
        }

        public ICreateViewCommand Column(string columnName, string property)
        {
            var command = new ViewColumnCommand(Name, columnName, property);
            TableCommands.Add(command);
            return this;
        }
    }
}
