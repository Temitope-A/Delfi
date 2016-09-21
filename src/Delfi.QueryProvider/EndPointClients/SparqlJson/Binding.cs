namespace Delfi.QueryProvider.EndPointClients.SparqlJson
{
    /// <summary>
    /// Strongly typed representation of a solution binding
    /// </summary>
    public class Binding
    {
        public string Type { get; set; }
        public string value { get; set; }
        public string DataType { get; set; }
    }
}