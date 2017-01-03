namespace Delfi.QueryProvider.RDF
{
    /// <summary>
    /// Represents an RDF namesapce
    /// </summary>
    public class NamespaceDefinition
    {
        /// <summary>
        /// Prefix associated with this namespace
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Resource Identifier associated with this namespace
        /// </summary>
        public string Iri { get; set; }
    }
}
