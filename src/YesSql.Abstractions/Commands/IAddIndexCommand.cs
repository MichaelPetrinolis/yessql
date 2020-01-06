namespace YesSql.Sql.Schema
{
    public interface IAddIndexCommand : ITableCommand
    {
        string IndexName { get; }
        string[] ColumnNames { get; }
    }
}
