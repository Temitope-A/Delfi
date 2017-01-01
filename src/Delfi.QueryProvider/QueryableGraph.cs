using Delfi.QueryProvider.RDF;
using System.Collections;
using Sparql.Algebra.RDF;

namespace Delfi.QueryProvider
{
    /// <summary>
    /// Base implementation of the IQueryable interface
    /// </summary>
    public class QueryableGraph : IQueryableGraph
    {
        /// <summary>
        /// Underlying graph expression for this query
        /// </summary>
        public GraphExpression GraphExpression { get; }

        /// <summary>
        /// Graph provider for this query
        /// </summary>
        public IGraphProvider GraphProvider { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryableGraph(IGraphProvider graphProvider, GraphExpression graphExpression = null)
        {
            GraphProvider = graphProvider;
            GraphExpression = graphExpression ?? GraphExpression.Empty;
        }

        /// <summary>
        /// Expands the graph expression with the specified property and object.
        /// The new graph expression will bind a given context if and only if the old expression binds,
        /// while the new addition is allowed not to bind
        /// </summary>
        /// <param name="property"></param>
        /// <param name="object"></param>
        /// <returns></returns>
        public IQueryableGraph Expand(Resource property, Term @object = null)
        {
            return new QueryableGraph(GraphProvider, GraphExpression.LeftJoin(property, @object));
        }

        /// <summary>
        /// Expands the graph expression with the specified property and query.
        /// The new graph expression will bind a given context if and only if the old expression binds,
        /// while the expansion is allowed not to bind
        /// </summary>
        /// <param name="property"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQueryableGraph Expand(Resource property, IQueryableGraph query)
        {
            return new QueryableGraph(GraphProvider, GraphExpression.LeftJoin(property, query.GraphExpression));
        }

        /// <summary>
        /// Expands the graph expression with the specified property and object.
        /// The new graph expression will bind a given context if and only if both the old expression and the addition bind 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="object"></param>
        /// <returns>a queryable graph</returns>
        public IQueryableGraph Require(Resource property, Term @object = null)
        {
            return new QueryableGraph(GraphProvider, GraphExpression.Join(property, @object));
        }

        /// <summary>
        /// Expands the graph expression with the specified property and query.
        /// The new graph expression will bind a given context if and only if both the old expression and the addition bind 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="query"></param>
        /// <returns>a queryable graph</returns>
        public IQueryableGraph Require(Resource property, IQueryableGraph query)
        {
            return new QueryableGraph(GraphProvider, GraphExpression.Join(property, query.GraphExpression));
        }

        /// <summary>
        /// Returns the union of two queries, the graph expressions are left unaltered. 
        /// </summary>
        /// <param name="query"></param>
        /// <returns>a queryable graph</returns>
        public IQueryableGraph Union(IQueryableGraph query)
        {
            return new QueryableGraph(GraphProvider, GraphExpression.Union(query.GraphExpression));
        }

        /// <summary>
        /// Returns the first count bindings
        /// </summary>
        /// <param name="count">maximum number of elements to return</param>
        /// <returns>a queryable graph</returns>
        public IQueryableGraph Limit(int count)
        {
            return new QueryableGraph(GraphProvider, GraphExpression.Slice(null, count));
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An System.Collections.IEnumerator object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            foreach (var item in GraphProvider.Execute(GraphExpression))
            {
                yield return item;
            }
        }
    }
}
