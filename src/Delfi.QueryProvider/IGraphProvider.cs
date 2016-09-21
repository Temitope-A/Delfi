using Delfi.QueryProvider.Tree;
using Sparql.Algebra.GraphSources;
using System.Collections.Generic;

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
        IEnumerable<TreeNode<object>> Execute(GraphExpression graphExpression, IGraphSource source = null);
    }
}