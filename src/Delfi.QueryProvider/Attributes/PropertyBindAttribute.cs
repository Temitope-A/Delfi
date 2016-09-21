using Delfi.QueryProvider.RDF;
using System;

namespace Delfi.QueryProvider.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyBindAttribute : Attribute
    {
        public Resource Property { get; }

        public PropertyBindAttribute(string iri)
        {
            Property = new Resource(iri);
        }

        public PropertyBindAttribute(string prefix, string terminal)
        {
            Property = new Resource(prefix, terminal);
        }
    }
}
