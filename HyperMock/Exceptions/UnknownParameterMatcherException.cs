namespace HyperMock.Universal.Exceptions
{
    using System.Linq.Expressions;

    public class UnknownParameterMatcherException : HyperMockException
    {
        public UnknownParameterMatcherException(Expression expression)
            : base($"Could not find a parameter matcher for the following parameter expression: {expression}")
        {
        }
    }
}