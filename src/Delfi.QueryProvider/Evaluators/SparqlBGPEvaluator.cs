using Delfi.QueryProvider.EndPointClients;
using Delfi.QueryProvider.EndPointClients.SparqlJson;
using Delfi.QueryProvider.RDF;
using Delfi.QueryProvider.Writers;
using Sparql.Algebra.GraphEvaluators;
using Sparql.Algebra.GraphSources;
using Sparql.Algebra.RDF;
using Sparql.Algebra.Trees;
using System.Collections.Generic;

namespace Delfi.QueryProvider.Evaluators
{
    /// <summary>
    /// Sparql evaluator
    /// </summary>
    public class SparqlBgpEvaluator:IEvaluator
    {
        /// <summary>
        /// Evaluates a graph query model against a graph source
        /// </summary>
        /// <param name="queryModel">tree model of the query</param>
        /// <param name="offset">number of solutions to skip</param>
        /// <param name="limit">maximum number of solutions to take</param>
        /// <param name="source">query target</param>
        /// <returns>A collection of trees</returns>
        public IEnumerable<LabelledTreeNode<object, Term>> Evaluate(LabelledTreeNode<object, Term> queryModel, int? offset, int? limit, IGraphSource source)
        {
            var queryString = SparqlBgpWriter.Write(queryModel, offset, limit);
            var client = new SparqlJsonClient();
            var responseModel = client.ExecuteQuery(queryString, source.EndPoint);

            foreach (var solution in responseModel.Results.Bindings)
            {
                yield return CreateResultGraph(solution, queryModel);
            }            
        }

        //Converts a JSON sparql solution binding row into a graph result
        private LabelledTreeNode<object, Term> CreateResultGraph(Dictionary<string, Binding> solution, LabelledTreeNode<object, Term> queryModel)
        {
            TreeNodeVisitor<object, object> visitor = (object nodeData) => ResolveNode(nodeData, solution);

            var result = queryModel.Copy().Traverse(visitor);

            return result;
        }

        private object ResolveNode(object inputNodeData, Dictionary<string, Binding> solutionSet)
        {
            var variable = inputNodeData as Variable;

            return variable == null ? inputNodeData : ResolveNode(variable, solutionSet);
        }

        private object ResolveNode(Variable inputNodeData, Dictionary<string, Binding> solutionSet)
        {
            if (solutionSet.ContainsKey(inputNodeData.Id))
            {
                object value = solutionSet[inputNodeData.Id].value;
                return value;
            }

            return inputNodeData;
        }
    }
}
