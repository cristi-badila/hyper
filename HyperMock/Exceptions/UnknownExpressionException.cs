namespace HyperMock.Universal.Exceptions
{
    using System.Linq.Expressions;

    public class UnknownExpressionException : HyperMockException
    {
        public UnknownExpressionException(Expression expression)
            : base($"Could not get expression info for expression {expression}")
        {
        }
    }
}