using Sparql.Algebra.Trees;
using Sparql.Algebra.RDF;

namespace Delfi.QueryProvider.Writers
{
    /// <summary>
    ///Sparql query writer
    /// </summary>
    public class SparqlBgpWriter
    {
        /// <summary>
        /// Converts a tree query model into a sparql query 
        /// </summary>
        public static string Write(LabelledTreeNode<object, Term> queryModel, int? offset = null, int? limit = null)
        {
            var template = "SELECT * WHERE {{ {0} }} {1} {2}";

            var whereBody = ConvertQueryModelToSparql(queryModel);
            var limitBody = "";
            var offsetBody = "";

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

        /// <summary>
        /// Converts a tree query model into a bgp graph pattern
        /// </summary>
        public static string ConvertQueryModelToSparql(LabelledTreeNode<object, Term> queryModel)
        {
            string result = "";

            foreach (var child in queryModel.Children)
            {
                result += string.Format(" {0} {1} {2}.", queryModel.Data, child.Key, child.Value.Data) + ConvertQueryModelToSparql(child.Value);
            }

            return result;
        }
    }
}
