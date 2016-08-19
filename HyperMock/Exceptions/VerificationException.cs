namespace HyperMock.Universal.Exceptions
{
    public class VerificationException : HyperMockException
    {
        public VerificationException(string message)
            : base(message)
        {
        }
    }
}