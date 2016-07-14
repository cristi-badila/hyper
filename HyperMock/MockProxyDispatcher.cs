using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using HyperMock.Universal.Exceptions;

namespace HyperMock.Universal
{
    /// <summary>
    ///     Instance of the dispatcher proxy. This acts like an interceptor for the mocking framework.
    /// </summary>
    public class MockProxyDispatcher : DispatchProxy
    {
        private readonly List<CallInfo> _callInfoList = new List<CallInfo>();

        private static ParameterType FindParameterType(LambdaExpression lambda)
        {
            var methodCall = lambda.Body as MethodCallExpression;
            return methodCall != null && methodCall.Method.DeclaringType == typeof(Param)
                ? ParameterType.Anything
                : ParameterType.AsDefined;
        }

        private static string CreateMissingMockMethodMessage(MemberInfo targetMethod, IReadOnlyCollection<object> args)
        {
            var name = targetMethod.Name;

            if (name.StartsWith("get_")) name = name.Remove(0, 4);
            if (name.StartsWith("set_")) name = name.Remove(0, 4);
            if (args.Count == 0) return name;

            var values = args.Select(p => p ?? "null");
            return string.Format("{0}({1})", name, string.Join(", ", values));
        }

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
                    parameters.Add(new Parameter { Value = value, Type = FindParameterType(lambda) });
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
                    parameters.Add(new Parameter { Value = value, Type = FindParameterType(lambda) });
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
                throw new MockException(CreateMissingMockMethodMessage(targetMethod, args));
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