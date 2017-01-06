using Delfi.EntityFramework.Attributes;
using Delfi.EntityFramework.Extensions;
using Delfi.QueryProvider.RDF;
using Sparql.Algebra.Filters;
using Sparql.Algebra.RDF;
using Sparql.Algebra.Trees;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Delfi.EntityFramework.Filters
{
    /// <summary>
    /// Allows easy generation of regex filters
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RegexFilterGenerator<T>:IFilterGenerator<T>
    {
        private string _regex;
        private Expression<Func<T, string>> _propertyLambda;

        /// <summary>
        /// Constructor
        /// </summary>
        public RegexFilterGenerator(Expression<Func<T, string>> propertyLambda, string value)
        {
            _propertyLambda = propertyLambda;
            _regex = value;
        }

        /// <summary>
        /// Generate a bgp regex filter using the variable names in the object graph
        /// </summary>
        /// <param name="queryGraph">graph model of the query</param>
        /// <returns></returns>
        public IFilter Generate(LabelledTreeNode<object, Term> queryGraph)
        {
            var propertyInfo = typeof(T).GetPropertyInfo(_propertyLambda);

            if (propertyInfo.Name == "Id")
            {
                var variable = queryGraph.Value as Variable;
                return variable != null ? new RegexFilter($"str({variable})", _regex) : null;
            }

            var attribute = propertyInfo.CustomAttributes.OfType<PropertyBindAttribute>().SingleOrDefault();

            if (attribute != null)
            {
                var node = queryGraph.Descend(attribute.Property).Single();
                var variable = node.Value as Variable;
                return variable != null ? new RegexFilter(variable.ToString(), _regex) : null;
            }

            return null;
        }
    }
}