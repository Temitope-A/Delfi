using System;
using Sparql.Algebra.RDF;

namespace Delfi.QueryProvider.RDF
{
    /// <summary>
    /// Represents a Sparql variable
    /// </summary>
    public class Variable : Term
    {
        /// <summary>
        /// Variable identifier
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public Variable (string name = null)
        {
            if (name == null)
            {
                Id = Guid.NewGuid().ToString().Replace('-', 'o');
            }
            else
            {
                Id = name;
            }
        }
        /// <summary>
        /// Returns a string representation of the variable
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "?" + Id;
        }
    }
}
