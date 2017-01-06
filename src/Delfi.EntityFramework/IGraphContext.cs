using Delfi.QueryProvider.RDF;
using Sparql.Algebra.RDF;
using Sparql.Algebra.Trees;

namespace Delfi.EntityFramework
{
    /// <summary>
    /// A simple read and write context
    /// </summary>
    public interface IGraphContext
    {
        /// <summary>
        /// Returns a queryable graph
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>a TypedQueryableGraph</returns>
        ITypedQueryableGraph Select<T>() where T:Resource;

        /// <summary>
        /// Adds a typed object to the graph
        /// </summary>
        void Add(Resource resource);

        /// <summary>
        /// Appends a graph
        /// </summary>
        void Append(LabelledTreeNode<object, Term> graph);

        /// <summary>
        /// Removes a graph
        /// </summary>
        void Remove(LabelledTreeNode<object, Term> graph);
    }
}
