using Delfi.QueryProvider.EndPointClients.SparqlJson;
using Newtonsoft.Json;
using Sparql.Algebra.Rows;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Delfi.QueryProvider.EndPointClients
{
    public class SparqlJsonClient
    {
        private string _resultString;

        /// <summary>
        /// Sends a POST request, with the query unencoded as the content body
        /// </summary>
        /// <param name="query">query</param>
        /// <param name="baseUri">base uri</param>
        /// <returns></returns>
        public void ExecuteQuery(string query, Uri baseUri)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = baseUri;

                HttpRequestMessage request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;
                request.Content = new StringContent(query);
                request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/sparql-query");
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/sparql-results+json"));

                var task = client.SendAsync(request);
                task.Wait();

                var content = task.Result.Content;
                var readTask = content.ReadAsStringAsync();
                readTask.Wait();

                _resultString = readTask.Result;
            }
        }

        public IEnumerable<IMultiSetRow> GetResults()
        {
            var jsonObj = JsonConvert.DeserializeObject<SparqlJsonResponse>(_resultString);

            yield return new SignatureRow(jsonObj.Head.Vars);

            foreach (var solution in jsonObj.Results.Bindings)
            {
                yield return CreateResultRow(solution, jsonObj.Head.Vars);
            }
        }

        private ResultRow CreateResultRow(Dictionary<string, Binding> solution, string[] Headers)
        {
            var resultDict = new Dictionary<string, object>();

            foreach (var header in Headers)
            {
                Binding x;
                if (solution.TryGetValue(header, out x))
                {
                    resultDict.Add(header, x.value);
                }
                else
                {
                    resultDict.Add(header, null);
                }
            }

            return new ResultRow(resultDict);
        }
    }
}
