using Delfi.QueryProvider.RDF;
using System.Collections;
using Sparql.Algebra.RDF;

namespace Delfi.QueryProvider
{
    /// <summary>
    /// Represents a queryable graph
    /// </summary>
    public interface IQueryableGraph : IEnumerable
    {
        /// <summary>
        /// Underlying graph expression for this query
        /// </summary>
        GraphExpression GraphExpression { get; }

        /// <summary>
        /// Graph provider for this query
        /// </summary>
        IGraphProvider GraphProvider { get; }

        /// <summary>
        /// Expands the graph expression with the specified property and object.
        /// The new graph expression will bind a given context if and only if the old expression binds,
        /// while the new addition is allowed not to bind
        /// </summary>
        /// <param name="property"></param>
        /// <param name="object"></param>
        /// <returns></returns>
        IQueryableGraph Expand(Resource property, Term @object = null);

        /// <summary>
        /// Expands the graph expression with the specified property and query.
        /// The new graph expression will bind a given context if and only if the old expression binds,
        /// while the expansion is allowed not to bind
        /// </summary>
        /// <param name="property"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryableGraph Expand(Resource property, IQueryableGraph query);

        /// <summary>
        /// Expands the graph expression with the specified property and object.
        /// The new graph expression will bind a given context if and only if both the old expression and the addition bind 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="object"></param>
        /// <returns>a queryable graph</returns>
        IQueryableGraph Require(Resource property, Term @object = null);

        /// <summary>
        /// Expands the graph expression with the specified property and query.
        /// The new graph expression will bind a given context if and only if both the old expression and the addition bind 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="query"></param>
        /// <returns>a queryable graph</returns>
        IQueryableGraph Require(Resource property, IQueryableGraph query);

        /// <summary>
        /// Returns the union of two queries, the graph expressions are left unaltered. 
        /// </summary>
        /// <param name="query"></param>
        /// <returns>a queryable graph</returns>
        IQueryableGraph Union(IQueryableGraph query);

        /// <summary>
        /// Returns the first count bindings
        /// </summary>
        /// <param name="count">maximum number of elements to return</param>
        /// <returns>a queryable graph</returns>
        IQueryableGraph Limit(int count);
    }
}