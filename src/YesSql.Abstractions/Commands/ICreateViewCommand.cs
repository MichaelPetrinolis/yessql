using System;
using System.Data;

namespace YesSql.Sql.Schema
{
    public interface ICreateViewCommand : ISchemaCommand
    {
        string SrcTableName { get; }

        ICreateViewCommand Column(string columnName, string property, DbType dbType, byte precision, byte scale, int? length);
    }
}
