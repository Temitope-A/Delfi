using System;
using Delfi.QueryProvider.RDF;

namespace Delfi.EntityFramework.Attributes
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
