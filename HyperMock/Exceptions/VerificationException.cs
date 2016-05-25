using System;

namespace HyperMock.Universal.Exceptions
{
    public class VerificationException : Exception
    {
        public VerificationException(string message) : base(message)
        {

        }
    }
}