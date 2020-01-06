namespace YesSql.Sql.Schema
{
    public class AddViewIndexCommand : ViewCommand, IAddViewIndexCommand
    {
        public string IndexName { get; }
        public string DocumentType { get; }

        public AddViewIndexCommand(string tableName, string indexName, string documentType, params IViewIndexColumnCommand[] columns)
            : base(tableName)
        {
            Columns = columns;
            IndexName = indexName;
            DocumentType = documentType;
        }

        public IViewIndexColumnCommand[] Columns { get; }
    }
}
