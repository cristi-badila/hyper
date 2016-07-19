namespace HyperMock.Universal.Setup
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Exceptions;
    using ParameterMatchers;
    using Universal.ExtensionMethods;

    public static class ExtensionMethods
    {
        public static CallInfo AddHandlingForPropertyGet<TMock, TReturn>(this Mock<TMock> mockProxyDispatcher, Expression<Func<TMock, TReturn>> expression)
            where TMock : class
        {
            string name;
            if (!mockProxyDispatcher.Dispatcher.TryGetReadPropertyName(expression, out name))
            {
                throw new UnableToSetupException(Convert.ToString(expression));
            }

            var callInfo = new CallInfo { Name = name };
            mockProxyDispatcher.Dispatcher.RegisteredCallInfoList.Add(callInfo);
            return callInfo;
        }

        public static CallInfo AddHandlingForPropertySet<TMock, TReturn>(this Mock<TMock> mockProxyDispatcher, Expression<Func<TMock, TReturn>> expression)
            where TMock : class
        {
            return mockProxyDispatcher.AddHandlingForPropertySet(expression, new AnyMatcher());
        }

        public static CallInfo AddHandlingForPropertySet<TMock, TReturn>(this Mock<TMock> mockProxyDispatcher, Expression<Func<TMock, TReturn>> expression, object value)
            where TMock : class
        {
            return mockProxyDispatcher.AddHandlingForPropertySet(expression, new ExactMatcher(value));
        }

        public static CallInfo AddHandlingForPropertySet<TMock, TReturn>(this Mock<TMock> mockProxyDispatcher, Expression<Func<TMock, TReturn>> expression, Parameter parameter)
            where TMock : class
        {
            string name;
            if (!mockProxyDispatcher.Dispatcher.TryGetWritePropertyName(expression, out name))
            {
                throw new UnableToSetupException(Convert.ToString(expression));
            }

            var callInfo = new CallInfo
            {
                Name = name,
                Parameters = new ParameterMatchersCollection { parameter }
            };

            mockProxyDispatcher.Dispatcher.RegisteredCallInfoList.Add(callInfo);
            return callInfo;
        }

        public static CallInfo AddHandling<TMock, TLambda>(this Mock<TMock> mockProxyDispatcher, Expression<TLambda> expression)
            where TMock : class
        {
            DispatchParams dispatchParams;
            if (!mockProxyDispatcher.Dispatcher.TryGetDispatchParams(expression, out dispatchParams))
            {
                throw new UnableToSetupException(Convert.ToString(expression));
            }

            var callInfo = new CallInfo
            {
                Name = dispatchParams.Name,
                Parameters = new ParameterMatchersCollection(dispatchParams.Arguments
                    .Select(argument => Expression.Lambda(argument, expression.Parameters))
                    .Select(LambdaExtensionMethods.GetParameterMatcher))
            };

            mockProxyDispatcher.Dispatcher.RegisteredCallInfoList.Add(callInfo);
            return callInfo;
        }
    }
}