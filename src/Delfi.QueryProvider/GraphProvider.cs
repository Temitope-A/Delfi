using Sparql.Algebra.GraphEvaluators;
using Sparql.Algebra.GraphSources;
using Sparql.Algebra.RDF;
using Sparql.Algebra.Trees;
using System.Collections.Generic;

namespace Delfi.QueryProvider
{
    /// <summary>
    /// Graph provider
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GraphProvider<T>:IGraphProvider where T:IEvaluator, new()
    {
        /// <summary>
        /// Map evaluator
        /// </summary>
        public IGraphSource DefaultSource { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public GraphProvider(IGraphSource source)
        {
            DefaultSource = source;
        }

        /// <summary>
        /// Executes the query codified in graphExpression
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LabelledTreeNode<object, Term>> Execute(GraphExpression graphExpression, IGraphSource source = null)
        {
            foreach (var result in graphExpression.Map.Evaluate<T>( source ?? DefaultSource))
            {
                yield return result;
            }
        }
    }
}
