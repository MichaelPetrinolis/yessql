using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Threading.Tasks;

namespace YesSql.Commands
{
    public abstract class DocumentCommand : IIndexCommand
    {
        protected static readonly PropertyInfo[] AllProperties = new PropertyInfo[]
        {
            typeof(Document).GetProperty("Type")
        };

        protected static readonly PropertyInfo[] AllKeys = new PropertyInfo[]
        {
            typeof(Document).GetProperty("Id")
        };

        private readonly Dictionary<Document, object> _documents;

        public abstract int ExecutionOrder { get; }

        public DocumentCommand(Document document)
        {
            Document = document;
        }

        public DocumentCommand()
        {
            _documents = new Dictionary<Document, object>();
        }

        public void AddDocument(Document doc, object obj)
        {
            _documents[doc] = obj;
        }

        public IEnumerable<KeyValuePair<Document, object>> GetDocumentsMap()
        {
            using (var enumerator = _documents.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
        }

        public IEnumerable<Document> GetDocuments()
        {
            using (var enumerator = _documents.Keys.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
        }

        public Document Document { get; }

        public abstract Task ExecuteAsync(DbConnection connection, DbTransaction transaction, ISqlDialect dialect, ILogger logger);
    }
}
