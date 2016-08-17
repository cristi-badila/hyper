namespace HyperMock.Universal.Exceptions
{
    public class NoMatchingCallFoundException : VerificationException
    {
        public NoMatchingCallFoundException(string message)
            : base($"Could not find any matching call. {message}")
        {
        }
    }
}