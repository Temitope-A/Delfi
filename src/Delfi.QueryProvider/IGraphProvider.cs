using Sparql.Algebra.GraphSources;
using System.Collections.Generic;
using Sparql.Algebra.Trees;
using Sparql.Algebra.RDF;

namespace Delfi.QueryProvider
{
    /// <summary>
    /// A bridge between the local graph expression and the remote graph source
    /// </summary>
    public interface IGraphProvider
    {
        /// <summary>
        /// Execute the query codified in the graph expression to the source
        /// </summary>
        /// <param name="graphExpression"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        IEnumerable<LabelledTreeNode<object, Term>> Execute(GraphExpression graphExpression, IGraphSource source = null);
    }
}