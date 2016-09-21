using Delfi.QueryProvider;
using Delfi.QueryProvider.EndPointClients;
using Delfi.QueryProvider.RDF;
using Sparql.Algebra.GraphSources;
using System.Collections.Generic;

namespace Delfi.EntityFramework
{
    public class GraphUpdater : IGraphUpdater
    {
        /// <summary>
        /// Map evaluator
        /// </summary>
        public IGraphSource DefaultSource { get; }

        protected SparqlJsonClient Client { get; }

        public GraphUpdater(IGraphSource source = null)
        {
            DefaultSource = source ?? new GraphSource(Configuration.Instance.EndPoint);
            Client = new SparqlJsonClient();
        }

        public void Delete(IEnumerable<Statement> statements)
        {
            var body = "";

            foreach (var item in statements)
            {
                body += " " + item.ToString() + ".";
            }

            var query = $"DELETE DATA {{{body}}}";

            Client.ExecuteQuery(query, DefaultSource.EndPoint);
        }

        public void Insert(IEnumerable<Statement> statements)
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
