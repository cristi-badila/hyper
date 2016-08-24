namespace HyperMock.Universal.Tests.Mocks
{
    using System;
    using System.Linq.Expressions;
    using Universal.Core;

    public class MockParameterMatcherInfoFactory : IParameterMatcherInfoFactory
    {
        public int CallCount { get; private set; }

        public ParameterMatcherInfo ReturnValue { get; set; } = null;

        public Exception ExceptionToThrow { get; set; } = null;

        public ParameterMatcherInfo Create(LambdaExpression parrentExpression)
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