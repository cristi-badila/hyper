using System;
using System.Linq.Expressions;

namespace HyperMock.Universal.Exceptions
{
    public class UnknownDispatchParamsException : Exception
    {
        public UnknownDispatchParamsException(Expression expression)
            : base($"Could not get dispatch params for expression {expression}")
        {
        }
    }
}