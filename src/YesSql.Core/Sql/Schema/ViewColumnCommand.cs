using System.Collections.Generic;
using System.Data;

namespace YesSql.Sql.Schema
{
    public class ViewColumnCommand : ViewCommand, IViewColumnCommand
    {
        public string ColumnName { get; set; }
        public string DocumentProperty { get; set; }

        public ViewColumnCommand(string tableName, string name, string property)
            : base(tableName)
        {
            ColumnName = name;
            DocumentProperty = property;
        }
    }
}
