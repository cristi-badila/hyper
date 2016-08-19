namespace HyperMock.Universal.Exceptions
{
    using System;
    using System.Linq.Expressions;

    public class UnknownExpressionException : Exception
    {
        public UnknownExpressionException(Expression expression)
            : base($"Could not get expression info for expression {expression}")
        {
        }
    }
}