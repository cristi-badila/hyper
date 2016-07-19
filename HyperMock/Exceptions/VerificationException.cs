namespace HyperMock.Universal.Exceptions
{
    using System;

    public class VerificationException : Exception
    {
        public VerificationException(string message)
            : base(message)
        {
        }
    }
}