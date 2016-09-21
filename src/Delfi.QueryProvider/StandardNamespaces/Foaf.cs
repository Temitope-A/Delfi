using Delfi.QueryProvider.RDF;

namespace Delfi.QueryProvider.StandardNamespaces
{
    public class Foaf : Resource
    {
        private const string NS = "http://xmlns.com/foaf/0.1/";

        public Foaf(string term) : base(NS + term)
        {
        }
    }
}
