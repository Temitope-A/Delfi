using Delfi.QueryProvider.EndPointClients.SparqlJson;
using Delfi.QueryProvider.Exceptions;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace Delfi.QueryProvider.EndPointClients
{
    /// <summary>
    /// Executes a query against a sparql endpoint and returns results via json 
    /// </summary>
    public class SparqlJsonClient
    {
        /// <summary>
        /// Sends a POST request, with the query unencoded as the content body
        /// </summary>
        /// <param name="query">query</param>
        /// <param name="baseUri">base uri</param>
        /// <returns></returns>
        public SparqlJsonResponse ExecuteQuery(string query, Uri baseUri)
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

                try
                {
                    task.Wait();
                }
                catch(HttpRequestException ex)
                {
                    throw new EndpointException($"Connection to the enpoint {baseUri} failed. Check your network status or contact the endpoint owner", ex);
                }

                var statusCode = task.Result.StatusCode;

                var content = task.Result.Content;
                var readTask = content.ReadAsStringAsync();
                readTask.Wait();

                var resultString = readTask.Result;

                if (statusCode != System.Net.HttpStatusCode.OK)
                {
                    var queryError = new BadQueryException(resultString);
                    throw new EndpointException($"{statusCode}. The query is malformed or the end point was unable to execute it. Check InnerException for details", queryError);
                }
                else
                {
                    return JsonConvert.DeserializeObject<SparqlJsonResponse>(resultString);
                }                
            }
        }
    }
}
