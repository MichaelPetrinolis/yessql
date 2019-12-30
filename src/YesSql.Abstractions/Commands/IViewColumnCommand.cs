using System.Data;

namespace YesSql.Sql.Schema
{
    public interface IViewColumnCommand : IViewCommand
    {

        string ColumnName { get; set; }

        DbType DbType { get; }

        byte Scale { get; }

        byte Precision { get; }

        int? Length { get; }

        string DocumentProperty { get; set; }
    }
}
