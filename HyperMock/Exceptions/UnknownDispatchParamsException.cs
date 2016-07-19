namespace HyperMock.Universal.Exceptions
{
    using System;
    using System.Linq.Expressions;

    public class UnknownDispatchParamsException : Exception
    {
        public UnknownDispatchParamsException(Expression expression)
            : base($"Could not get dispatch params for expression {expression}")
        {
        }
    }
}