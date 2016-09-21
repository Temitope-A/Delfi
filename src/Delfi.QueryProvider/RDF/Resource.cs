using System.Linq;

namespace Delfi.QueryProvider.RDF
{
    /// <summary>
    /// Represents an RDF resource
    /// </summary>
    public class Resource : Term
    {
        public Resource (string iri)
        {
                Id = iri;
        }

        public Resource(string prefix, string terminal)
        {
            var namespaces = Configuration.Instance.Namespaces;
            var @namespace = namespaces.First(a => a.Prefix == prefix);

            Id = @namespace.Iri + terminal;
        }

        public string Id { get; }

        public override string ToString()
        {
            return "<" + Id + ">";
        }        
    }
}