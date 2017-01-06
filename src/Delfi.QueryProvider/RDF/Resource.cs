using System.Linq;
using Sparql.Algebra.RDF;

namespace Delfi.QueryProvider.RDF
{
    /// <summary>
    /// Represents an RDF resource
    /// </summary>
    public class Resource : Term
    {
        /// <summary>
        /// Id of the Resource
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Initializes a new instance of the Resource class
        /// </summary>
        /// <param name="iri">ful iri</param>
        public Resource (string iri)
        {
                Id = iri;
        }

        /// <summary>
        /// Initializes a new instance of the Resource class
        /// </summary>
        /// <param name="prefix">namespace prefix</param>
        /// <param name="terminal"></param>
        public Resource(string prefix, string terminal)
        {
            var namespaces = Configuration.Instance.Namespaces;
            var @namespace = namespaces.First(a => a.Prefix == prefix);

            Id = @namespace.Iri + terminal;
        }

        /// <summary>
        /// Returns true if two resources have the same iri
        /// </summary>
        /// <param name="obj"></param>
        public override bool Equals(object obj)
        {
            return Id == ((Resource)obj).Id;
        }

        /// <summary>
        /// Full string representation of the resource
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "<" + Id + ">";
        }        
    }
}