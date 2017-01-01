using Delfi.QueryProvider.EndPointClients;
using Delfi.QueryProvider.EndPointClients.SparqlJson;
using Delfi.QueryProvider.Writers;
using Sparql.Algebra.GraphEvaluators;
using Sparql.Algebra.GraphSources;
using Sparql.Algebra.RDF;
using Sparql.Algebra.Trees;
using System;
using System.Collections.Generic;

namespace Delfi.QueryProvider.Evaluators
{
    /// <summary>
    /// Sparql evatuator
    /// </summary>
    public class SparqlBgpEvaluator:IEvaluator
    {
        private readonly Guid _id;

        /// <summary>
        /// Constructor
        /// </summary>
        public SparqlBgpEvaluator()
        {
            _id = Guid.NewGuid();
        }

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


        private LabelledTreeNode<object, Term> CreateResultGraph(Dictionary<string, Binding> solution, LabelledTreeNode<object, Term> queryModel)
        {
            var result = queryModel.Copy();

            foreach (var pair in solution)
            {
                //find node with given id
                //node.data = pair.Value
            }

            return result;
        }

    }
}
