using System;
using System.Data;

namespace YesSql.Sql.Schema
{
    public class CreateMaterializedViewCommand : CreateViewCommand, ICreateMaterializedViewCommand
    {
        public CreateMaterializedViewCommand(string name, string srcTableName)
            : base(name, srcTableName)
        {
        }
    }
}
