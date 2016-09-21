using Delfi.QueryProvider.RDF;

namespace Delfi.QueryProvider.StandardNamespaces
{
    public class Owl : Resource
    {
        private const string NS = "http://www.w3.org/2002/07/owl#";

        public Owl(string term) : base(NS + term)
        {
        }
    }
}
