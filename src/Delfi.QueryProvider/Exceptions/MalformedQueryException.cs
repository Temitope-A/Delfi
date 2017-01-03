using System;

namespace Delfi.QueryProvider.Exceptions
{
    /// <summary>
    /// Represents an error which occurs during the execution of a query on a graph source due to problems in the query 
    /// </summary>
    public class BadQueryException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the BadQueryException class
        /// </summary>
        public BadQueryException(string message) : base(message) { }
    }
}
