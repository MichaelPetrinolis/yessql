namespace YesSql.Sql.Schema
{
    public interface IViewColumnCommand : IViewCommand
    {
        string ColumnName { get; set; }
        string DocumentProperty { get; set; }
    }
}
