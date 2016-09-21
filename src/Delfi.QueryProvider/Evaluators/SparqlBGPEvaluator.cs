using Delfi.QueryProvider.EndPointClients;
using Delfi.QueryProvider.RDF;
using Delfi.QueryProvider.Writers;
using Sparql.Algebra.Evaluators;
using Sparql.Algebra.GraphSources;
using Sparql.Algebra.Rows;
using System;
using System.Collections.Generic;

namespace Delfi.QueryProvider.Evaluators
{
    public class SparqlBGPEvaluator:IEvaluator
    {
        private Guid _id;

        public IGraphSource Source { get; }

        public SparqlBGPEvaluator(IGraphSource source)
        {
            Source = source;
            _id = Guid.NewGuid();
        }

        /// <summary>
        /// Evaluates the query generated from sparql triple patterns against a sparql endpoint
        /// </summary>
        /// <param name="inputList"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public IEnumerable<IMultiSetRow> Evaluate(IEnumerable<object> inputList, int? offset, int? limit)
        {
            var statementList = new List<Statement>();

            foreach (var item in inputList)
            {
                statementList.Add((Statement)item);
            }

            string query = SparqlBGPWriter.Write(statementList, offset, limit);

            var client = new SparqlJsonClient();
            client.ExecuteQuery(query, Source.EndPoint);

            foreach (var item in client.GetResults())
            {
                yield return item;
            }
        }
    }
}
