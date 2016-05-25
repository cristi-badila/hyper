using System;

namespace HyperMock.Exceptions
{
    public class VerificationException : Exception
    {
        public VerificationException(string message) : base(message)
        {

        }
    }
}