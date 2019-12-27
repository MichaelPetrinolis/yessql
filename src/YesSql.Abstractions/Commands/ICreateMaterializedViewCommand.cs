using System;
using System.Data;

namespace YesSql.Sql.Schema
{
    public interface ICreateMaterializedViewCommand : ISchemaCommand, ICreateViewCommand
    {
    }
}
