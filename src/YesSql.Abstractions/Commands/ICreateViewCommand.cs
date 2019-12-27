using System;
using System.Data;

namespace YesSql.Sql.Schema
{
    public interface ICreateViewCommand : ISchemaCommand
    {
        ICreateViewCommand Column(string columnName, string documentProperty);
    }
}
