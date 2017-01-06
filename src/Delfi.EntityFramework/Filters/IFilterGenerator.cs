using Sparql.Algebra.Filters;
using Sparql.Algebra.RDF;
using Sparql.Algebra.Trees;

namespace Delfi.EntityFramework.Filters
{
    /// <summary>
    /// Allows easy generation of filters
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFilterGenerator<T>
    {
        /// <summary>
        /// Generate a bgp filter using the variable names in the object graph
        /// </summary>
        /// <param name="objectGraph"></param>
        /// <returns></returns>
        IFilter Generate(LabelledTreeNode<object, Term> objectGraph);
    }
}