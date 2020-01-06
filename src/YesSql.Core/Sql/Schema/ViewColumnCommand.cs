using System.Collections.Generic;
using System.Data;

namespace YesSql.Sql.Schema
{
    public class ViewColumnCommand : ViewCommand, IViewColumnCommand
    {
        public string ColumnName { get; }

        public string DocumentProperty { get; }

        public DbType DbType { get; }

        public byte Scale { get; }

        public byte Precision { get; }

        public int? Length { get; }

        public ViewColumnCommand(string tableName, string name, string property, DbType dbType, byte precision, byte scale, int? length)
            : base(tableName)
        {
            ColumnName = name;
            DocumentProperty = property;
            DbType = dbType;
            Precision = precision;
            Scale = scale;
            Length = length;
        }
    }
}
