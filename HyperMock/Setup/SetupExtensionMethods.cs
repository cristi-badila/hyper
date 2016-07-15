using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using HyperMock.Universal.ExtensionMethods;

namespace HyperMock.Universal.Setup
{
    public static class SetupExtensionMethods
    {
        public static CallInfo CreateForMethod<TMock>(this MockProxyDispatcher mockProxyDispatcher, Expression<Action<TMock>> expression)
        {
            string name;
            ReadOnlyCollection<Expression> arguments;

            if (mockProxyDispatcher.TryGetMethodNameAndArgs(expression, out name, out arguments))
            {
                var callInfo = new CallInfo { Name = name };

                var parameters = new ParameterCollection();

                foreach (var argument in arguments)
                {
                    var lambda = Expression.Lambda(argument, expression.Parameters);
                    var compiledDelegate = lambda.Compile();
                    var value = compiledDelegate.DynamicInvoke(new object[1]);
                    parameters.Add(new Parameter { Value = value, Type = lambda.GetParameterType() });
                }

                callInfo.Parameters = parameters;

                mockProxyDispatcher.RegisteredCallInfoList.Add(callInfo);
                return callInfo;
            }

            return null;
        }

        public static CallInfo CreateForFunction<TMock, TReturn>(this MockProxyDispatcher mockProxyDispatcher, Expression<Func<TMock, TReturn>> expression)
        {
            string name;
            ReadOnlyCollection<Expression> arguments;

            if (mockProxyDispatcher.TryGetMethodNameAndArgs(expression, out name, out arguments))
            {
                var callInfo = new CallInfo { Name = name };

                var parameters = new ParameterCollection();

                foreach (var argument in arguments)
                {
                    var lambda = Expression.Lambda(argument, expression.Parameters);
                    var compiledDelegate = lambda.Compile();
                    var value = compiledDelegate.DynamicInvoke(new object[1]);
                    parameters.Add(new Parameter { Value = value, Type = lambda.GetParameterType() });
                }

                callInfo.Parameters = parameters;

                mockProxyDispatcher.RegisteredCallInfoList.Add(callInfo);
                return callInfo;
            }

            return null;
        }

        public static CallInfo CreateForReadProperty<TMock, TReturn>(this MockProxyDispatcher mockProxyDispatcher, Expression<Func<TMock, TReturn>> expression)
        {
            string name;

            if (mockProxyDispatcher.TryGetReadPropertyNameAndArgs(expression, out name))
            {
                var callInfo = new CallInfo { Name = name };
                mockProxyDispatcher.RegisteredCallInfoList.Add(callInfo);
                return callInfo;
            }

            return null;
        }

        public static CallInfo CreateForWriteProperty<TMock, TReturn>(this MockProxyDispatcher mockProxyDispatcher, Expression<Func<TMock, TReturn>> expression, object value)
        {
            return mockProxyDispatcher.CreateForWritePropertyCore(expression, new Parameter { Value = value, Type = ParameterType.AsDefined });
        }

        public static CallInfo CreateForWriteProperty<TMock, TReturn>(this MockProxyDispatcher mockProxyDispatcher, Expression<Func<TMock, TReturn>> expression)
        {
            return mockProxyDispatcher.CreateForWritePropertyCore(expression, new Parameter { Value = null, Type = ParameterType.Anything });
        }

        public static CallInfo CreateForWritePropertyCore<TMock, TReturn>(this MockProxyDispatcher mockProxyDispatcher, Expression<Func<TMock, TReturn>> expression, Parameter parameter)
        {
            string name;
            if (!mockProxyDispatcher.TryGetWritePropertyNameAndArgs(expression, out name))
            {
                return null;
            }

            var callInfo = new CallInfo { Name = name };

            var parameters = new ParameterCollection { parameter };
            callInfo.Parameters = parameters;

            mockProxyDispatcher.RegisteredCallInfoList.Add(callInfo);
            return callInfo;
        }
    }
}