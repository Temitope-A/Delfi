using System;
using System.Reflection;
using Delfi.QueryProvider.Attributes;
using Sparql.Algebra.RDF;

namespace Delfi.QueryProvider.RDF
{
    /// <summary>
    /// Represents a Sparql variable
    /// </summary>
    public class Variable : Term
    {
        public Resource ResourceType { get {return GetResourceType(); } }

        public bool IsResourceInternal { get { return Id.Contains("·"); } }

        public Type Type { get; }

        public string Id { get; }

        public Variable (string name = null)
        {
            
        }

        public Variable (Type underlyingResourceType, string name = null)
        {
            if (name == null)
            {
                Id = Guid.NewGuid().ToString().Replace('-', 'o');
            }
            else
            {
                Id = name;
            }

            Type = underlyingResourceType;
        }

        public override string ToString()
        {
            return "?" + Id;
        }

        private Resource GetResourceType()
        {
            var attr = Type.GetTypeInfo().GetCustomAttribute<EntityBindAttribute>();

            if (attr != null)
            {
                return attr.Type;
            }

            return null;
        }
    }
}
