using Sparql.Algebra.RDF;
using Sparql.Algebra.Trees;

namespace Delfi.EntityFramework
{
    /// <summary>
    /// Enables manipulation of labelled graphs
    /// </summary>
    public interface IGraphWriter
    {
        /// <summary>
        /// Removes a graph portion from the repository
        /// </summary>
        void Delete(LabelledTreeNode<object, Term> graph);

        /// <summary>
        /// Adds a graph portion to the repository
        /// </summary>
        void Insert(LabelledTreeNode<object, Term> graph);
    }
}