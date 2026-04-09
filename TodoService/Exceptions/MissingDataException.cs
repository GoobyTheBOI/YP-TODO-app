using System;

namespace TodoServices.Exceptions
{
    public class MissingDataException : Exception
    {
        public MissingDataException(string message) : base(message) { }
    }
}
