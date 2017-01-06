using Sparql.Algebra.Trees;
using Sparql.Algebra.RDF;
using Sparql.Algebra.Filters;

namespace Delfi.QueryProvider.Writers
{
    /// <summary>
    /// Sparql query writer
    /// </summary>
    public class SparqlBgpWriter
    {
        /// <summary>
        /// Converts a tree query model into a sparql query string
        /// </summary>
        public static string Write(LabelledTreeNode<object, Term> queryModel, int? offset = null, int? limit = null, IFilter filter = null)
        {
            var template = "SELECT * WHERE {{ {0} {1}}} {2} {3}";

            var whereBody = ConvertQueryModelToSparql(queryModel);
            var limitBody = limit.HasValue ? "LIMIT " + limit.Value : "";
            var offsetBody = offset.HasValue ? "OFFSET " + offset.Value : "";
            var filterBody = filter != null ? "FILTER " + filter.ToString() : "";

            return string.Format(template, whereBody, filterBody, limitBody, offsetBody);
        }

        /// <summary>
        /// Converts a tree query model into a sparql basic graph pattern
        /// </summary>
        public static string ConvertQueryModelToSparql(LabelledTreeNode<object, Term> queryModel)
        {
            string result = "";

            foreach (var child in queryModel.Children)
            {
                result += string.Format(" {0} {1} {2}.", queryModel.Value, child.Edge, child.TerminalNode.Value) + ConvertQueryModelToSparql(child.TerminalNode);
            }

            return result;
        }
    }
}
