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
            var dispatchParams = expression.GetDispatchParamsForGet();
            if (dispatchParams == null)
            {
                throw new UnableToSetupException(Convert.ToString(expression));
            }

            var callInfo = new CallInfo { Name = dispatchParams.Name };
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

        public static CallInfo AddHandlingForPropertySet<TMock, TReturn>(this Mock<TMock> mockProxyDispatcher, Expression<Func<TMock, TReturn>> expression, ParameterMatcher parameterMatcher)
            where TMock : class
        {
            var dispatchParams = expression.GetDispatchParamsForSet();
            if (dispatchParams == null)
            {
                throw new UnableToSetupException(Convert.ToString(expression));
            }

            var callInfo = new CallInfo
            {
                Name = dispatchParams.Name,
                Parameters = new ParameterMatchersCollection { parameterMatcher }
            };

            mockProxyDispatcher.Dispatcher.RegisteredCallInfoList.Add(callInfo);
            return callInfo;
        }

        public static CallInfo AddHandling<TMock, TLambda>(this Mock<TMock> mockProxyDispatcher, Expression<TLambda> expression)
            where TMock : class
        {
            var dispatchParams = expression.GetDispatchParamsForMethod();
            if (dispatchParams == null)
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