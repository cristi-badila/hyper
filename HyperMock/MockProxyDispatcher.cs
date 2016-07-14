using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using HyperMock.Universal.Exceptions;
using HyperMock.Universal.ExtensionMethods;

namespace HyperMock.Universal
{
    /// <summary>
    ///     Instance of the dispatcher proxy. This acts like an interceptor for the mocking framework.
    /// </summary>
    public class MockProxyDispatcher : DispatchProxy
    {
        private readonly List<CallInfo> _callInfoList = new List<CallInfo>();

        public CallInfo FindByParameterMatch(string name, object[] args)
        {
            var callInfoListForName = _callInfoList.Where(ci => ci.Name == name).ToList();

            return callInfoListForName.FirstOrDefault(ci => ci.IsMatchFor(args));
        }

        public CallInfo FindByReturnMatch(string name, object returnValue)
        {
            return _callInfoList.FirstOrDefault(ci => ci.Name == name && ci.ReturnValue == returnValue);
        }

        public CallInfo CreateForMethod<TMock>(Expression<Action<TMock>> expression)
        {
            string name;
            ReadOnlyCollection<Expression> arguments;

            if (TryGetMethodNameAndArgs(expression, out name, out arguments))
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

                _callInfoList.Add(callInfo);
                return callInfo;
            }

            return null;
        }

        public CallInfo CreateForFunction<TMock, TReturn>(Expression<Func<TMock, TReturn>> expression)
        {
            string name;
            ReadOnlyCollection<Expression> arguments;

            if (TryGetMethodNameAndArgs(expression, out name, out arguments))
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

                _callInfoList.Add(callInfo);
                return callInfo;
            }

            return null;
        }

        public CallInfo CreateForReadProperty<TMock, TReturn>(Expression<Func<TMock, TReturn>> expression)
        {
            string name;

            if (TryGetReadPropertyNameAndArgs(expression, out name))
            {
                var callInfo = new CallInfo { Name = name };
                _callInfoList.Add(callInfo);
                return callInfo;
            }

            return null;
        }

        public CallInfo CreateForWriteProperty<TMock, TReturn>(Expression<Func<TMock, TReturn>> expression)
        {
            return CreateForWritePropertyCore(expression, new Parameter { Value = null, Type = ParameterType.Anything });
        }

        public CallInfo CreateForWriteProperty<TMock, TReturn>(Expression<Func<TMock, TReturn>> expression, object value)
        {
            return CreateForWritePropertyCore(expression, new Parameter { Value = value, Type = ParameterType.AsDefined });
        }

        public bool TryGetMethodNameAndArgs(
            Expression expression,
            out string name,
            out ReadOnlyCollection<Expression> arguments)
        {
            var lambda = (LambdaExpression)expression;
            var body = lambda.Body as MethodCallExpression;

            if (body != null)
            {
                name = body.Method.Name;
                arguments = body.Arguments;

                return true;
            }

            name = null;
            arguments = new ReadOnlyCollection<Expression>(new List<Expression>());
            return false;
        }

        public bool TryGetReadPropertyNameAndArgs(Expression expression, out string name)
        {
            var lambda = (LambdaExpression)expression;
            var body = lambda.Body as MemberExpression;

            if (body != null)
            {
                var propInfo = (PropertyInfo)body.Member;
                name = propInfo.GetMethod.Name;
                return true;
            }

            name = null;
            return false;
        }

        public bool TryGetWritePropertyNameAndArgs(Expression expression, out string name)
        {
            name = null;

            var lambda = (LambdaExpression)expression;
            var body = lambda.Body as MemberExpression;

            if (body != null)
            {
                var propInfo = (PropertyInfo)body.Member;
                name = propInfo.SetMethod.Name;
                return true;
            }

            return false;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var name = targetMethod.Name;
            var callInfoListForName = _callInfoList.Where(ci => ci.Name == name).ToList();
            var matchedCallInfo = callInfoListForName.FirstOrDefault(ci => ci.IsMatchFor(args));
            if (matchedCallInfo == null)
            {
                throw new MockException(targetMethod, args);
            }

            if (matchedCallInfo.ExceptionType != null)
            {
                var exception = (Exception)Activator.CreateInstance(matchedCallInfo.ExceptionType);
                matchedCallInfo.Visited++;
                throw exception;
            }

            matchedCallInfo.Visited++;

            return matchedCallInfo.ReturnValue;
        }

        private CallInfo CreateForWritePropertyCore<TMock, TReturn>(Expression<Func<TMock, TReturn>> expression, Parameter parameter)
        {
            string name;

            if (TryGetWritePropertyNameAndArgs(expression, out name))
            {
                var callInfo = new CallInfo { Name = name };

                var parameters = new ParameterCollection { parameter };
                callInfo.Parameters = parameters;

                _callInfoList.Add(callInfo);
                return callInfo;
            }

            return null;
        }

    }
}