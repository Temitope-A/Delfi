namespace Delfi.QueryProvider.EndPointClients.SparqlJson
{
    /// <summary>
    /// Strongly typed representation of a json sparql query result
    /// </summary>
    public class SparqlJsonResponse
    {
        /// <summary>
        /// Contains the variables mentioned in the results and may contain a "link" member.
        /// </summary>
        public SparqlJsonResponseHead Head { get; set; }
        /// <summary>
        /// Contains the bindings.
        /// </summary>
        public SparqlJsonResponseResults Results { get; set; }
    }
}
