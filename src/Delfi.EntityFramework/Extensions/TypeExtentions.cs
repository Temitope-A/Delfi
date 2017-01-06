using System;
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
    }
}