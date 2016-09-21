using Delfi.QueryProvider.RDF;
using System.Collections.Generic;

namespace Delfi.QueryProvider.Writers
{
    public class SparqlBGPWriter
    {
        /// <summary>
        /// Converts a list of statements (triple patterns) into a sparql query 
        /// </summary>
        /// <param name="statementList"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static string Write(IEnumerable<Statement> statementList, int? offset = null, int? limit = null)
        {
            var template = "SELECT * WHERE {{ {0} }} {1} {2}";

            var whereBody = "";
            var limitBody = "";
            var offsetBody = "";

            foreach (var statement in statementList)
            {
                whereBody += statement.ToString() + "."; 
            }

            if (limit.HasValue)
            {
                limitBody = "LIMIT " + limit.Value.ToString();
            }

            if (offset.HasValue)
            {
                offsetBody = "OFFSET " + offset.Value.ToString();
            }

            return string.Format(template, whereBody, limitBody, offsetBody);
        }
    }
}
