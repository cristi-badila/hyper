namespace HyperMock.Universal.Tests.Mocks
{
    using System;
    using System.Linq.Expressions;
    using Universal.Core;

    public class MockMethodCallInfoFactory : IMethodCallInfoFactory
    {
        public int CallCount { get; private set; }

        public MethodCallInfo ReturnValue { get; set; } = null;

        public Exception ExceptionToThrow { get; set; } = null;

        public MethodCallInfo Create(LambdaExpression expression)
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