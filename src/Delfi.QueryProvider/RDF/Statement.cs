namespace Delfi.QueryProvider.RDF
{
    /// <summary>
    /// Represents an RDF statement
    /// </summary>
    public class Statement
    {
        public Term Subject { get; }
        public Term Predicate { get; }
        public Term Object { get; }

        public Statement(Term subject, Term predicate, Term @object)
        {
            Subject = subject;
            Predicate = predicate;
            Object = @object;
        }

        public override string ToString()
        {
            return Subject.ToString() + " " + Predicate.ToString() + " " + Object.ToString();
        }      
    }
}
