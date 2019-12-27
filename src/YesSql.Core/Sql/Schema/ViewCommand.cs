using System;
using System.Collections.Generic;

namespace YesSql.Sql.Schema
{
    public abstract class ViewCommand : ISchemaCommand, IViewCommand,ITableCommand
    {
        public string Name { get; private set; }

        public List<ITableCommand> TableCommands { get; private set; }

        public ViewCommand(string tableName)
        {
            Name = tableName;
        }
    }
}
