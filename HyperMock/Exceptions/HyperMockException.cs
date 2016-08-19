namespace HyperMock.Universal.Exceptions
{
    using System;

    public class HyperMockException : Exception
    {
        public HyperMockException()
        {
        }

        public HyperMockException(string message)
            : base(message)
        {
        }

        public HyperMockException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}