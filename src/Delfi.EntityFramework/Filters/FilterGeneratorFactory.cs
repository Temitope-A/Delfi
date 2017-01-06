using System;
using System.Linq.Expressions;

namespace Delfi.EntityFramework.Filters
{
    /// <summary>
    /// Filter generators abstract factory
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Filter<T>
    {
        /// <summary>
        /// Regex filter generators factory method
        /// </summary>
        /// <param name="propertyLambda"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static RegexFilterGenerator<T> Regex(Expression<Func<T, string>> propertyLambda, string value)
        {
            return new RegexFilterGenerator<T>(propertyLambda, value);
        }
    }
}