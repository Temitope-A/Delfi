using System;

namespace Delfi.QueryProvider.Exceptions
{
    /// <summary>
    /// Represents an error which occurs during the execution of a query on a graph source
    /// </summary>
    public class EndpointException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the EndpointException class
        /// </summary>
        public EndpointException(string message, Exception innerException):base(message, innerException) { }
    }
}
