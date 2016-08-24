namespace HyperMock.Universal.Exceptions
{
    using System;
    using System.Linq.Expressions;

    public class UnknownCallExpressionException : HyperMockException
    {
        private static readonly Func<Expression, string> GetMessage = expression => $"Invalid call expression: {expression}";

        public UnknownCallExpressionException(Expression expression)
            : base(GetMessage(expression))
        {
        }

        public UnknownCallExpressionException(Expression expression, Exception innerException)
            : base(GetMessage(expression), innerException)
        {
        }
    }
}