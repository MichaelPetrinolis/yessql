using Dapper;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Threading.Tasks;
using YesSql.Collections;

namespace YesSql.Commands
{
    public sealed class CreateDocumentCommand : DocumentCommand
    {
        private readonly string _tablePrefix;

        public override int ExecutionOrder { get; } = 0;

        public CreateDocumentCommand(string tablePrefix) 
        {
            _tablePrefix = tablePrefix;
        }

        public CreateDocumentCommand(Document document, string tablePrefix) : base(document)
        {
            _tablePrefix = tablePrefix;
        }

        public override Task ExecuteAsync(DbConnection connection, DbTransaction transaction, ISqlDialect dialect, ILogger logger)
        {
            var documentTable = CollectionHelper.Current.GetPrefixedName(Store.DocumentTable);
            var insertCmd = "insert into " + dialect.QuoteForTableName(_tablePrefix + documentTable) + " (" + dialect.QuoteForColumnName("Id") + ", " + dialect.QuoteForColumnName("Type") + ", " + dialect.QuoteForColumnName("Content") + ") values (@Id, @Type, @Content);";

            logger.LogTrace(insertCmd);

            if (Document != null)
            {
                return connection.ExecuteScalarAsync<int>(insertCmd, Document, transaction);
            }
            else
            {
                return connection.ExecuteScalarAsync<int>(insertCmd, GetDocuments(), transaction);
            }

        }
    }
}
