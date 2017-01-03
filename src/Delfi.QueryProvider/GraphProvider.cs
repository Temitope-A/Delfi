using Sparql.Algebra.GraphEvaluators;
using Sparql.Algebra.GraphSources;
using Sparql.Algebra.RDF;
using Sparql.Algebra.Trees;
using System.Collections.Generic;

namespace Delfi.QueryProvider
{
    /// <summary>
    /// Base implementation of the IGraphProvider interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GraphProvider<T>:IGraphProvider where T:IEvaluator, new()
    {
        /// <summary>
        /// Default Source
        /// </summary>
        public IGraphSource DefaultSource { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source">default source</param>
        public GraphProvider(IGraphSource source)
        {
            DefaultSource = source;
        }

        /// <summary>
        /// Execute the query codified in the graph expression on the source
        /// </summary>
        /// <returns>A collection of graph results that match the graph expression</returns>
        public IEnumerable<LabelledTreeNode<object, Term>> Execute(GraphExpression graphExpression, IGraphSource source = null)
        {
            return graphExpression.Map.Evaluate<T>(source ?? DefaultSource);
        }
    }
}
