using System;
using System.Data;

namespace YesSql.Sql.Schema
{
    public interface ICreateViewCommand : ISchemaCommand
    {
        string SrcTableName { get; }

        string DocumentType { get; }

        ICreateViewCommand Column(string columnName, string property, DbType dbType, byte precision, byte scale, int? length);

        void AddIndex(string indexName, string documentType, IViewIndexColumnCommand[] indexColumns);
    }
}
