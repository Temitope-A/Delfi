using Sparql.Algebra.RDF;
using Sparql.Algebra.Trees;

namespace Delfi.QueryProvider.RDF
{
    /// <summary>
    /// Represents an RDF statement
    /// </summary>
    public class Statement
    {
        /// <summary>
        /// Subject
        /// </summary>
        public Term Subject { get; }

        /// <summary>
        /// Predicate
        /// </summary>
        public Term Predicate { get; }

        /// <summary>
        /// Object
        /// </summary>
        public Term Object { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="predicate"></param>
        /// <param name="object"></param>
        public Statement(Term subject, Term predicate, Term @object)
        {
            Subject = subject;
            Predicate = predicate;
            Object = @object;
        }

        /// <summary>
        /// String representation of a statement
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Subject.ToString() + " " + Predicate.ToString() + " " + Object.ToString();
        }

        /// <summary>
        /// Tree form of a statement
        /// </summary>
        /// <returns></returns>
        public LabelledTreeNode<object, Term> ToGraph()
        {
            var graph = new LabelledTreeNode<object, Term>(Subject);
            graph.AddChild(Predicate, Object);
            return graph;
        }
    }
}
