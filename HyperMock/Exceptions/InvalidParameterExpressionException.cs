namespace HyperMock.Universal.Exceptions
{
    using System.Linq.Expressions;

    public class InvalidParameterExpressionException : HyperMockException
    {
        public InvalidParameterExpressionException(Expression expression)
            : base($"A parameter expression was encountered which could not be evaluated: {expression}")
        {
        }
    }
}