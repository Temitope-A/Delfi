using System;

namespace Delfi.QueryProvider.Exceptions
{
    public class BadQueryException : Exception
    {
        public BadQueryException(string message) : base(message) { }
    }
}
