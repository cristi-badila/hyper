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
    /// Instance of the dispatcher proxy. This acts like an interceptor for the mocking framework.
    /// </summary>
    public class MockProxyDispatcher : DispatchProxy
    {
        private readonly List<CallInfo> _callInfoList = new List<CallInfo>();

        internal CallInfo FindByParameterMatch(string name, object[] args)
        {
            var callInfoListForName = _callInfoList.Where(ci => ci.Name == name).ToList();

            return callInfoListForName.FirstOrDefault(ci => ci.IsMatchFor(args));
        }

        internal CallInfo FindByReturnMatch(string name, object returnValue)
        {
            return _callInfoList.FirstOrDefault(ci => ci.Name == name && ci.ReturnValue == returnValue);
        }

        internal CallInfo CreateForMethod<TMock>(Expression<Action<TMock>> expression)
        {
            string name;
            ReadOnlyCollection<Expression> arguments;

            if (TryGetMethodNameAndArgs(expression, out name, out arguments))
            {
                var callInfo = new CallInfo();
                callInfo.Name = name;

                var parameters = new List<Parameter>();

                foreach (var argument in arguments)
                {
                    var lambda = Expression.Lambda(argument, expression.Parameters);
                    var compiledDelegate = lambda.Compile();
                    var value = compiledDelegate.DynamicInvoke(new object[1]);
                    parameters.Add(new Parameter { Value = value, Type = FindParameterType(lambda) });
                }

                callInfo.Parameters = parameters.ToArray();

                _callInfoList.Add(callInfo);
                return callInfo;
            }

            return null;
        }

        internal CallInfo CreateForFunction<TMock, TReturn>(Expression<Func<TMock, TReturn>> expression)
        {
            string name;
            ReadOnlyCollection<Expression> arguments;
            
            if (TryGetMethodNameAndArgs(expression, out name, out arguments))
            {
                var callInfo = new CallInfo();
                callInfo.Name = name;

                var parameters = new List<Parameter>();

                foreach (var argument in arguments)
                {
                    var lambda = Expression.Lambda(argument, expression.Parameters);
                    var compiledDelegate = lambda.Compile();
                    var value = compiledDelegate.DynamicInvoke(new object[1]);
                    parameters.Add(new Parameter { Value = value, Type = FindParameterType(lambda) });
                }

                callInfo.Parameters = parameters.ToArray();

                _callInfoList.Add(callInfo);
                return callInfo;
            }

            return null;
        }

        internal CallInfo CreateForReadProperty<TMock, TReturn>(Expression<Func<TMock, TReturn>> expression)
        {
            string name;

            if (TryGetReadPropertyNameAndArgs(expression, out name))
            {
                var callInfo = new CallInfo();
                callInfo.Name = name;
                _callInfoList.Add(callInfo);
                return callInfo;
            }

            return null;
        }

        internal CallInfo CreateForWriteProperty<TMock, TReturn>(Expression<Func<TMock, TReturn>> expression)
        {
            string name;

            if (TryGetWritePropertyNameAndArgs(expression, out name))
            {
                var callInfo = new CallInfo();
                callInfo.Name = name;

                var parameters = new List<Parameter>();
                parameters.Add(new Parameter { Value = null, Type = ParameterType.Anything });

                callInfo.Parameters = parameters.ToArray();

                _callInfoList.Add(callInfo);
                return callInfo;
            }

            return null;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var name = targetMethod.Name;

            var callInfoListForName = _callInfoList.Where(ci => ci.Name == name).ToList();

            var matchedCallInfo = callInfoListForName.FirstOrDefault(ci => ci.IsMatchFor(args));

            if (matchedCallInfo == null)
            {
                if (IsSetProperty(targetMethod))
                {
                    _callInfoList.Add(new CallInfo
                    {
                        Name = name,
                        Parameters = new[] {new Parameter {Value = args[0], Type = ParameterType.AsDefined}},
                        Visited = 1
                    });

                    return null;
                }

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

        private static bool IsSetProperty(MethodBase method)
        {
            return method.DeclaringType.GetProperties().Any(p => p.SetMethod != null && p.SetMethod.Equals(method));
        }
        
        private static ParameterType FindParameterType(LambdaExpression lambda)
        {
            var methodCall = lambda.Body as MethodCallExpression;
            return methodCall != null && methodCall.Method.DeclaringType == typeof(Param)
                ? ParameterType.Anything
                : ParameterType.AsDefined;
        }
        
        internal bool TryGetMethodNameAndArgs(
            Expression expression,
            out string name,
            out ReadOnlyCollection<Expression> arguments)
        {
            var ex = expression as LambdaExpression;

            var y = ex.Body as MethodCallExpression;

            if (y != null)
            {
                name = y.Method.Name;
                arguments = y.Arguments;

                return true;
            }
            
            name = null;
            arguments = new ReadOnlyCollection<Expression>(new List<Expression>());
            return false;
            
        }

        internal bool TryGetReadPropertyNameAndArgs(Expression expression, out string name)
        {
            var ex = expression as LambdaExpression;
            var z = ex.Body as MemberExpression;

            if (z != null)
            {
                var s = z.Member as PropertyInfo;
                name = s.GetMethod.Name;
                return true;
            }

            name = null;
            return false;

        }

        internal bool TryGetWritePropertyNameAndArgs(Expression expression, out string name)
        {
            name = null;

            var ex = expression as LambdaExpression;
            var z = ex.Body as MemberExpression;

            if (z != null)
            {
                var s = z.Member as PropertyInfo;
                name = s.SetMethod.Name;
                return true;
            }

            

            return false;

        }

        private static string CreateMissingMockMethodMessage(MethodInfo targetMethod, object[] args)
        {
            var name = targetMethod.Name;

            if (name.StartsWith("get_")) name = name.Remove(0, 4);
            if (name.StartsWith("set_")) name = name.Remove(0, 4);
            if (args.Length == 0) return name;

            var values = args.Select(p => p ?? "null");
            return string.Format("{0}({1})", name, string.Join(", ", values));
        }
    }
}