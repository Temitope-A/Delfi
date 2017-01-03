using Delfi.QueryProvider.RDF;
using Sparql.Algebra.RDF;
using System.Collections;

namespace Delfi.QueryProvider
{
    /// <summary>
    /// Represents a queryable graph
    /// </summary>
    public interface IQueryableGraph : IEnumerable
    {
        /// <summary>
        /// Underlying graph expression for this query. Codifies the query to be executed
        /// </summary>
        GraphExpression GraphExpression { get; }

        /// <summary>
        /// Underlying graph provider for this query. Manages the translation of graph expression to generic graph query result
        /// </summary>
        IGraphProvider GraphProvider { get; }

        /// <summary>
        /// Expands the graph expression with the specified property and object.
        /// The new graph expression will bind a given context if and only if the old expression binds,
        /// while the new addition is allowed not to bind
        /// </summary>
        /// <param name="property">property along which to expand</param>
        /// <param name="object">object required at the end of the property, can be left null to insert a variable</param>
        /// <returns>a queryable graph</returns>
        IQueryableGraph Expand(Resource property, Term @object = null);

        /// <summary>
        /// Expands the graph expression with the specified property and query.
        /// The new graph expression will bind a given context if and only if the old expression binds,
        /// while the expansion is allowed not to bind
        /// </summary>
        /// <param name="property">property along which to expand</param>
        /// <param name="query">queryable graph required at the end of the property</param>
        /// <returns>a queryable graph</returns>
        IQueryableGraph Expand(Resource property, IQueryableGraph query);

        /// <summary>
        /// Expands the graph expression with the specified property and object.
        /// The new graph expression will bind a given context if and only if both the old expression and the addition bind 
        /// </summary>
        /// <param name="property">property along which to expand</param>
        /// <param name="object">object required at the end of the property, can be left null to insert a variable</param>
        /// <returns>a queryable graph</returns>
        IQueryableGraph Require(Resource property, Term @object = null);

        /// <summary>
        /// Expands the graph expression with the specified property and query.
        /// The new graph expression will bind a given context if and only if both the old expression and the addition bind 
        /// </summary>
        /// <param name="property">property along which to expand</param>
        /// <param name="query">queryable graph required at the end of the property</param>
        /// <returns>a queryable graph</returns>
        IQueryableGraph Require(Resource property, IQueryableGraph query);

        /// <summary>
        /// Expands the graph expression with the specified property and terminal object, the return the terminal object
        /// The new graph expression will bind a given context if and only if both the old expression and the addition bind 
        /// </summary>
        /// <param name="property">property along which to expand</param>
        /// <param name="object">object required at the end of the property, can be left null to insert a variable</param>
        /// <returns>a queryable graph</returns>
        IQueryableGraph Select(Resource property, Term @object = null);

        /// <summary>
        /// Expands the graph expression with the specified property and terminal query, then return the terminal query
        /// The new graph expression will bind a given context if and only if both the old expression and the addition bind 
        /// </summary>
        /// <param name="property">property along which to expand</param>
        /// <param name="query">queryable graph required at the end of the property</param>
        /// <returns>a queryable graph</returns>
        IQueryableGraph Select(Resource property, IQueryableGraph query);

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