namespace HyperMock.Universal.Setup
{
    using System;
    using System.Linq.Expressions;
    using Exceptions;
    using ParameterMatchers;
    using Universal.ExtensionMethods;

    public static class ExtensionMethods
    {
        public static CallDescriptor AddHandlingForPropertyGet<TMock, TReturn>(this Mock<TMock> mockProxyDispatcher, Expression<Func<TMock, TReturn>> expression)
            where TMock : class
        {
            var expressionInfo = expression.GetExpressionInfoForGet();
            if (expressionInfo == null)
            {
                throw new UnableToSetupException(Convert.ToString(expression));
            }

            var callDescriptor = new CallDescriptor { MemberName = expressionInfo.Name };
            mockProxyDispatcher.Dispatcher.RegisteredCalls.Add(callDescriptor);
            return callDescriptor;
        }

        public static CallDescriptor AddHandlingForPropertySet<TMock, TReturn>(this Mock<TMock> mockProxyDispatcher, Expression<Func<TMock, TReturn>> expression)
            where TMock : class
        {
            return mockProxyDispatcher.AddHandlingForPropertySet(expression, new AnyMatcher());
        }

        public static CallDescriptor AddHandlingForPropertySet<TMock, TReturn>(this Mock<TMock> mockProxyDispatcher, Expression<Func<TMock, TReturn>> expression, object value)
            where TMock : class
        {
            return mockProxyDispatcher.AddHandlingForPropertySet(expression, new ExactMatcher(value));
        }

        public static CallDescriptor AddHandlingForPropertySet<TMock, TReturn>(this Mock<TMock> mockProxyDispatcher, Expression<Func<TMock, TReturn>> expression, ParameterMatcher parameterMatcher)
            where TMock : class
        {
            var expressionInfo = expression.GetExpressionInfoForSet();
            if (expressionInfo == null)
            {
                throw new UnableToSetupException(Convert.ToString(expression));
            }

            var callDescriptor = new CallDescriptor
            {
                MemberName = expressionInfo.Name,
                Parameters = new ParameterMatchersCollection { parameterMatcher }
            };

            mockProxyDispatcher.Dispatcher.RegisteredCalls.Add(callDescriptor);
            return callDescriptor;
        }

        public static CallDescriptor AddHandling<TMock, TLambda>(this Mock<TMock> mockProxyDispatcher, Expression<TLambda> expression)
            where TMock : class
        {
            var expressionInfo = expression.GetExpressionInfoForMethod();
            if (expressionInfo == null)
            {
                throw new UnableToSetupException(Convert.ToString(expression));
            }

            var callDescriptor = new CallDescriptor
            {
                MemberName = expressionInfo.Name,
                Parameters = expressionInfo.GetParameterMatchers(expression.Parameters)
            };

            mockProxyDispatcher.Dispatcher.RegisteredCalls.Add(callDescriptor);
            return callDescriptor;
        }
    }
}