using System;

namespace Utilities
{
    public class InvalidPathException : Exception
    {
        public InvalidPathException() { }
        public InvalidPathException(string message) : base(message) { }
    }
}
