using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Delfi.EntityFramework.Extensions
{
    /// <summary>
    /// Type helper methods
    /// </summary>
    public static class TypeExtentions
    {
        /// <summary>
        /// Returns true if the type implements the IList interface
        /// </summary>
        public static bool IsListType(this Type type)
        {
            return (type.GetTypeInfo().GetInterface("IList") != null);
        }

        /// <summary>
        /// This method returns the PropertyInfo object for the expression. It throws an exception if the expression is not a property.
        /// </summary>
        public static PropertyInfo GetPropertyInfo<TSource, TProperty>( this Type sourceType, Expression<Func<TSource, TProperty>> propertyLambda)
        {
            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda.ToString()));

            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda.ToString()));

            //if (sourceType != propInfo.ReflectedType &&
            //    !sourceType.IsSubclassOf(propInfo.ReflectedType))
            //    throw new ArgumentException(string.Format(
            //        "Expresion '{0}' refers to a property that is not from type {1}.",
            //        propertyLambda.ToString(),
            //        sourceType));

            return propInfo;
        }
    }
}