using Delfi.QueryProvider.Writers;
using Sparql.Algebra.GraphSources;
using Sparql.Algebra.RDF;
using Sparql.Algebra.Trees;

namespace Delfi.EntityFramework
{
    /// <summary>
    /// Base implementation of a IGraphWriter, enables manipulation of labelled graphs
    /// </summary>
    public class GraphWriter : IGraphWriter
    {
        /// <summary>
        /// Default source
        /// </summary>
        public IGraphSource DefaultSource { get; }

        /// <summary>
        /// Update client
        /// </summary>
        protected UpdateClient Client { get; }

        /// <summary>
        /// Initializes a new instance of the GraphWriter class
        /// </summary>
        /// <param name="source"></param>
        public GraphWriter(IGraphSource source)
        {
            DefaultSource = source;
            Client = new UpdateClient();
        }

        /// <summary>
        /// Removes a graph portion from the repository
        /// </summary>
        public void Delete(LabelledTreeNode<object, Term> graph)
        {
            var body = SparqlBgpWriter.ConvertQueryModelToSparql(graph);

            var query = $"DELETE DATA {{{body}}}";

            Client.ExecuteQuery(query, DefaultSource.EndPoint);
        }

        /// <summary>
        /// Adds a graph portion to the repository
        /// </summary>
        public void Insert(LabelledTreeNode<object, Term> graph)
        {
            var body = SparqlBgpWriter.ConvertQueryModelToSparql(graph);

            var query = $"INSERT DATA {{{body}}}";

            Client.ExecuteQuery(query, DefaultSource.EndPoint);
        }
    }
}