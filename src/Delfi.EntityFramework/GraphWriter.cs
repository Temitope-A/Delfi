using Delfi.QueryProvider.EndPointClients;
using Delfi.QueryProvider.RDF;
using Sparql.Algebra.GraphSources;
using System.Collections.Generic;
using System.Linq;

namespace Delfi.EntityFramework
{
    /// <summary>
    /// Graph Writer
    /// </summary>
    public class GraphWriter : IGraphWriter
    {
        /// <summary>
        /// Map evaluator
        /// </summary>
        public IGraphSource DefaultSource { get; }

        protected UpdateClient Client { get; }

        public GraphWriter(IGraphSource source)
        {
            DefaultSource = source;
            Client = new UpdateClient();
        }

        public void Delete(IEnumerable<Statement> statements)
        {
            if (statements.Any())
            {
                var body = "";

                foreach (var item in statements)
                {
                    body += " " + item.ToString() + ".";
                }

                var query = $"DELETE DATA {{{body}}}";

                Client.ExecuteQuery(query, DefaultSource.EndPoint);
            }
        }

        public void Insert(IEnumerable<Statement> statements)
        {
            if (statements.Any())
            {
                var body = "";

                foreach (var item in statements)
                {
                    body += " " + item.ToString() + ".";
                }

                var query = $"INSERT DATA {{{body}}}";

                Client.ExecuteQuery(query, DefaultSource.EndPoint);
            }
        }
    }
}
