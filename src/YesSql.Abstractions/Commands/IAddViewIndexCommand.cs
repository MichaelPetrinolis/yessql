namespace YesSql.Sql.Schema
{
    public interface IAddViewIndexCommand : IViewCommand
    {
        string IndexName { get; }
        string DocumentType { get; }

        IViewIndexColumnCommand[] Columns { get; }
    }
}
