using Delfi.EntityFramework.Filters;
using Delfi.QueryProvider;
using Delfi.QueryProvider.RDF;
using Sparql.Algebra.RDF;
using Sparql.Algebra.Trees;
using System;

namespace Delfi.EntityFramework
{
    /// <summary>
    /// An IQueryableGraph that supports generic type parametres expansions. The results are graphs whose nodes are typed objects
    /// </summary>
    public interface ITypedQueryableGraph : IQueryableGraph
    {
        /// <summary>
        /// Type graph for this query. THe results will be type checked against this graph
        /// </summary>
        LabelledTreeNode<Type, Term> TypeGraph { get; }

        /// <summary>
        /// Expands the graph expression along the specified property.
        /// The new graph expression will bind a given context if and only if the old expression binds,
        /// while the new addition is allowed not to bind
        /// </summary>
        /// <typeparam name="Y">type of expansion</typeparam>
        /// <param name="property">property along which to expand</param>
        /// <param name="filterGenerator">a filter generator to restrict results</param>
        /// <returns>a TypedQueryableGraph</returns>
        ITypedQueryableGraph Expand<Y>(Resource property, IFilterGenerator<Y> filterGenerator = null) where Y:Resource;

        /// <summary>
        /// Expands the graph expression along the specified property.
        /// The new graph expression will bind a given context if and only if both the old expression and the addition bind 
        /// </summary>
        /// <typeparam name="Y">type of expansion</typeparam>
        /// <param name="property">property along which to expand</param>
        /// <param name="filterGenerator">a filter generator to restrict results</param>
        /// <returns>a TypedQueryableGraph</returns>
        ITypedQueryableGraph Require<Y>(Resource property, IFilterGenerator<Y> filterGenerator = null) where Y : Resource;

        /// <summary>
        /// Expands the graph expression along the specified property, but returns only the terminal type
        /// The new graph expression will bind a given context if and only if both the old expression and the addition bind 
        /// </summary>
        /// <typeparam name="Y">type of query</typeparam>
        /// <param name="property">property along which to expand</param>
        /// <param name="filterGenerator">a filter generator to restrict results</param>
        /// <returns>a TypedQueryableGraph</returns>
        ITypedQueryableGraph Select<Y>(Resource property, IFilterGenerator<Y> filterGenerator = null) where Y : Resource;

        /// <summary>
        /// New query, returns objects of the specified type
        /// </summary>
        /// <typeparam name="Y">type of query</typeparam>
        /// <returns>a TypedQueryableGraph</returns>
        ITypedQueryableGraph Select<Y>(IFilterGenerator<Y> filterGenerator = null) where Y : Resource;
    }
}
