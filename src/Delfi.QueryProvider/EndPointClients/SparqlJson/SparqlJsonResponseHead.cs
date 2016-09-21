namespace Delfi.QueryProvider.EndPointClients.SparqlJson
{
    /// <summary>
    /// Strongly typed representation of the head part of a json sparql query result
    /// </summary>
    public class SparqlJsonResponseHead
    {
        /// <summary>
        /// The "vars" member is an array giving the names of the variables used in the results.
        /// These are the projected variables from the query.
        /// A variable is not necessarily given a value in every query solution of the results.
        /// </summary>
        public string[] Vars { get; set; }
        /// <summary>
        /// The optional "link" member gives an array of URIs, as strings, to refer for further information.
        /// The format and content of these link references is not defined by this document.
        /// </summary>
        public string[] Link { get; set; }
    }
}
