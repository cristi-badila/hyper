﻿using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using HyperMock.Universal.ExtensionMethods;

namespace HyperMock.Universal.Setup
{
    public static class SetupExtensionMethods
    {
        public static CallInfo AddHandlingForAction<TMock>(this Mock<TMock> mockProxyDispatcher, Expression<Action<TMock>> expression)
            where TMock : class
        {
            string name;
            ReadOnlyCollection<Expression> arguments;

            if (mockProxyDispatcher.Dispatcher.TryGetMethodNameAndArgs(expression, out name, out arguments))
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

                mockProxyDispatcher.Dispatcher.RegisteredCallInfoList.Add(callInfo);
                return callInfo;
            }

            return null;
        }

        public static CallInfo AddHandlingForFunction<TMock, TReturn>(this Mock<TMock> mockProxyDispatcher, Expression<Func<TMock, TReturn>> expression)
            where TMock : class
        {
            string name;
            ReadOnlyCollection<Expression> arguments;

            if (mockProxyDispatcher.Dispatcher.TryGetMethodNameAndArgs(expression, out name, out arguments))
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

                mockProxyDispatcher.Dispatcher.RegisteredCallInfoList.Add(callInfo);
                return callInfo;
            }

            return null;
        }

        public static CallInfo AddHandlingPropertyGet<TMock, TReturn>(this Mock<TMock> mockProxyDispatcher, Expression<Func<TMock, TReturn>> expression)
            where TMock : class
        {
            string name;

            if (mockProxyDispatcher.Dispatcher.TryGetReadPropertyNameAndArgs(expression, out name))
            {
                var callInfo = new CallInfo { Name = name };
                mockProxyDispatcher.Dispatcher.RegisteredCallInfoList.Add(callInfo);
                return callInfo;
            }

            return null;
        }

        public static CallInfo AddHandlingForPropertySet<TMock, TReturn>(this Mock<TMock> mockProxyDispatcher, Expression<Func<TMock, TReturn>> expression, object value)
            where TMock : class
        {
            return mockProxyDispatcher.AddHandlingPropertySet(expression, new Parameter { Value = value, Type = ParameterType.AsDefined });
        }

        public static CallInfo AddHandlingForPropertySet<TMock, TReturn>(this Mock<TMock> mockProxyDispatcher, Expression<Func<TMock, TReturn>> expression)
            where TMock : class
        {
            return mockProxyDispatcher.AddHandlingPropertySet(expression, new Parameter { Value = null, Type = ParameterType.Anything });
        }

        public static CallInfo AddHandlingPropertySet<TMock, TReturn>(this Mock<TMock> mockProxyDispatcher, Expression<Func<TMock, TReturn>> expression, Parameter parameter)
            where TMock : class
        {
            string name;
            if (!mockProxyDispatcher.Dispatcher.TryGetWritePropertyNameAndArgs(expression, out name))
            {
                return null;
            }

            var callInfo = new CallInfo { Name = name };

            var parameters = new ParameterCollection { parameter };
            callInfo.Parameters = parameters;

            mockProxyDispatcher.Dispatcher.RegisteredCallInfoList.Add(callInfo);
            return callInfo;
        }
    }
}