using Delfi.QueryProvider.RDF;
using System;

namespace Delfi.QueryProvider.Attributes
{
    /// <summary>
    /// Represents an RDF type statement
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityBindAttribute : Attribute
    {
        public Resource Type { get; }

        public EntityBindAttribute(string iri)
        {
            Type = new Resource(iri);
        }

        public EntityBindAttribute(string prefix, string terminal)
        {
            Type = new Resource(prefix, terminal);
        }
    }
}
