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
        /// Execute the query codified in the graph expression on the source
        /// </summary>
        /// <param name="graphExpression">graph expression, contains the sparql algebra map</param>
        /// <param name="source">graph source, leave null to use a default source</param>
        /// <returns>A collection of graph results that match the graph expression</returns>
        IEnumerable<LabelledTreeNode<object, Term>> Execute(GraphExpression graphExpression, IGraphSource source = null);
    }
}