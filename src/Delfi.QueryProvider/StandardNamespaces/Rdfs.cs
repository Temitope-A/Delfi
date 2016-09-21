using Delfi.QueryProvider.RDF;

namespace Delfi.QueryProvider.StandardNamespaces
{
    public class Rdfs : Resource
    {
        private const string NS = "http://www.w3.org/2000/01/rdf-schema#";

        public Rdfs(string term) : base(NS + term)
        {
        }
    }
}
