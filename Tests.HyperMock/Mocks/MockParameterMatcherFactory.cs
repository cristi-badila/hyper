namespace HyperMock.Universal.Tests.Mocks
{
    using System;
    using System.Linq.Expressions;
    using Universal.Core;

    public class MockParameterMatcherFactory : IParameterMatcherFactory
    {
        public int CallCount { get; private set; }

        public ParameterMatcher ReturnValue { get; set; } = null;

        public Exception ExceptionToThrow { get; set; } = null;

        public ParameterMatcher Create(LambdaExpression lambda)
        {
            CallCount++;
            if (ExceptionToThrow != null)
            {
                throw ExceptionToThrow;
            }

            return ReturnValue;
        }
    }
}