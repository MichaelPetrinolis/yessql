using System.Data;

namespace YesSql.Sql.Schema
{
    public interface IViewIndexColumnCommand : IViewCommand
    {

        string ColumnName { get; }

        DbType DbType { get; }

        byte Scale { get; }

        byte Precision { get; }

        int? Length { get; }
    }
}
