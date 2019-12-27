using System;
using System.Data;

namespace YesSql.Sql.Schema
{
    public class CreateViewCommand : SchemaCommand, ICreateViewCommand
    {
        public CreateViewCommand(string name)
            : base(name, SchemaCommandType.CreateView)
        {
        }

        public ICreateViewCommand Column(string columnName, string property)
        {
            var command = new ViewColumnCommand(Name, columnName, property);
            TableCommands.Add(command);
            return this;
        }
    }
}
