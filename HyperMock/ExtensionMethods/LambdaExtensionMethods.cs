namespace HyperMock.Universal.ExtensionMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using ParameterMatchers;

    public static class LambdaExtensionMethods
    {
        public static ParameterMatcher GetParameterMatcher(this LambdaExpression lambda)
        {
            var methodCall = lambda.Body as MethodCallExpression;
            var matcherType = GetMatcherType(methodCall);
            return matcherType == null
                ? lambda.GetDefaultParameterMatcher()
                : CreateMathcerInstance(methodCall, matcherType) as ParameterMatcher;
        }

        public static bool UsesParameterMatcher(this LambdaExpression lambda)
        {
            var methodCall = lambda.Body as MethodCallExpression;
            return methodCall != null && typeof(Param).IsAssignableFrom(methodCall.Method.DeclaringType);
        }

        public static ParameterMatcher GetDefaultParameterMatcher(this LambdaExpression lambda)
        {
            var compiledDelegate = lambda.Compile();
            var value = compiledDelegate.DynamicInvoke(new object[1]);
            return new ExactMatcher(value);
        }

        public static object CreateMathcerInstance(MethodCallExpression methodCall, Type matcherType)
        {
            var ctorArguments = methodCall.Arguments
                .Select(ResolveArgumentExpression)
                .ToArray();
            var createInstanceArguments = new List<object>(ctorArguments);
            createInstanceArguments.Insert(0, matcherType);
            return ctorArguments.Length == 0
                ? Activator.CreateInstance(matcherType)
                : Activator.CreateInstance(matcherType, ctorArguments);
        }

        public static object ResolveArgumentExpression(Expression expression)
        {
            object result;
            var constantExpression = expression as ConstantExpression;
            if (constantExpression != null)
            {
                result = constantExpression.Value;
            }
            else if (expression is LambdaExpression)
            {
                result = ((LambdaExpression)expression).Compile();
            }
            else
            {
                throw new Exception("Could not parse argument exception");
            }

            return result;
        }

        public static Type GetMatcherType(MethodCallExpression methodCall)
        {
            var attribute = methodCall?.Method.GetCustomAttribute<ParameterMatcherAttribute>();
            if (attribute == null)
            {
                return null;
            }

            var matcherType = attribute.MatcherType;
            try
            {
                if (matcherType.GetGenericTypeDefinition() == matcherType)
                {
                    var genericArguments = methodCall.Method.GetGenericArguments();
                    matcherType = matcherType.MakeGenericType(genericArguments);
                }
            }
            catch (Exception)
            {
                // Could not identify any way of determining if a type is actually a generic type
                // before invoking GetGenericTypeDefinition
            }

            return matcherType;
        }
    }
}