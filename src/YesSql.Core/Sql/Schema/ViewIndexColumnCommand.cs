using System.Collections.Generic;
using System.Data;

namespace YesSql.Sql.Schema
{
    public class ViewIndexColumnCommand : ViewCommand, IViewIndexColumnCommand
    {
        public string ColumnName { get; }

        public DbType DbType { get; }

        public byte Scale { get; }

        public byte Precision { get; }

        public int? Length { get; }

        public ViewIndexColumnCommand(string tableName, string name, DbType dbType, byte precision, byte scale, int? length)
            : base(tableName)
        {
            ColumnName = name;
            DbType = dbType;
            Precision = precision;
            Scale = scale;
            Length = length;
        }
    }
}
