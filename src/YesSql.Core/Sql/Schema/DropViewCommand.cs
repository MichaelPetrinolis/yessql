namespace YesSql.Sql.Schema
{
    public class DropViewCommand : SchemaCommand, IDropViewCommand
    {
        public DropViewCommand(string name)
            : base(name, SchemaCommandType.DropView)
        {
        }
    }
}
