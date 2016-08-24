namespace HyperMock.Universal.Exceptions
{
    using System;
    using System.Linq.Expressions;

    public class UnsupportedArgumentExpressionException : HyperMockException
    {
        private static readonly Func<Expression, string> GetMessage = expression => $"Unsupported argument expression: {expression}";

        public UnsupportedArgumentExpressionException(Expression expression)
            : base(GetMessage(expression))
        {
        }

        public UnsupportedArgumentExpressionException(Expression expression, Exception innerException)
            : base(GetMessage(expression), innerException)
        {
        }
    }
}
