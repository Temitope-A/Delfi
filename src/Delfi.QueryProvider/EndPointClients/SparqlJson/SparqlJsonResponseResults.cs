using System.Collections.Generic;
namespace Delfi.QueryProvider.EndPointClients.SparqlJson
{
    /// <summary>
    /// Strongly typed representation of the results part of a json sparql query result
    /// </summary>
    public class SparqlJsonResponseResults
    {
        /// <summary>
        /// The value of the "bindings" member is an array with zero or more elements, one element per query solution.
        /// Each query solution is a JSON object. Each key of this object is a variable name from the query solution.
        /// The value for a given variable name is a JSON object that encodes the variable's bound value, an RDF term.
        /// There are zero elements in the array if the query returned an empty solution sequence.
        /// Variables names do not include the initial "?" or "$" character.
        /// Each variable name that appears as a key within the "bindings" array will have appeared in the "vars" array in the results header.
        /// A variable does not appear in an array element if it is not bound in that particular query solution.
        /// The order of elements in the bindings array reflects the order, if any, of the query solution sequence.
        /// </summary>
        public Dictionary<string, Binding>[] Bindings { get; set; }
    }
}