using Delfi.QueryProvider.RDF;

namespace Delfi.QueryProvider.StandardNamespaces
{
    public class Rdf : Resource
    {
        private const string NS = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";

        public Rdf(string term) : base(NS + term)
        {
        }
    }
}