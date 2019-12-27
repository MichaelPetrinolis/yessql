namespace YesSql.Sql.Schema
{
    public class AddViewColumnCommand : ViewColumnCommand, IAddViewColumnCommand
    {
        public AddViewColumnCommand(string tableName, string name,string property) : base(tableName, name,property)
        {
        }
    }
}
