namespace HyperMock.Universal.Exceptions
{
    public class InvalidParameterMatcherException : HyperMockException
    {
        public InvalidParameterMatcherException(object parameterMatcher)
            : base($"The following parameter matcher doesn't inherit from the ParameterMatcher class: {parameterMatcher.GetType()}")
        {
        }
    }
}