using Delfi.QueryProvider.RDF;
using System.Collections.Generic;
using System.Collections;

namespace Delfi.QueryProvider
{
    /// <summary>
    /// Base implementation of the IQueryable interface
    /// </summary>
    public class QueryableGraph : IQueryableGraph
    {
        public GraphExpression GraphExpression { get; }

        public IGraphProvider GraphProvider { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryableGraph(IGraphProvider graphProvider, GraphExpression graphExpression = null)
        {
            GraphProvider = graphProvider;
            GraphExpression = graphExpression ?? GraphExpression.Empty;
        }

        public IQueryableGraph Expand(Resource property, Term @object = null)
        {
            return new QueryableGraph(GraphProvider, GraphExpression.LeftJoin(property, @object));
        }

        public IQueryableGraph Require(Resource property, Term @object = null)
        {
            return new QueryableGraph(GraphProvider, GraphExpression.Join(property, @object));
        }

        public IQueryableGraph Select(Resource property, Term @object = null)
        {
            return new QueryableGraph(GraphProvider, GraphExpression.Select(property, @object));
        }

        public IQueryableGraph Union(IQueryableGraph query)
        {
            return new QueryableGraph(GraphProvider, GraphExpression.Union(query.GraphExpression));
        }

        public IQueryableGraph Limit(int count)
        {
            return new QueryableGraph(GraphProvider, GraphExpression.Slice(null, count));
        }

        public IQueryableGraph Match(IEnumerable<Statement> statementList)
        {
            return new QueryableGraph(GraphProvider, GraphExpression.Join(statementList));
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var item in GraphProvider.Execute(GraphExpression))
            {
                yield return item;
            }
        }
    }
}
