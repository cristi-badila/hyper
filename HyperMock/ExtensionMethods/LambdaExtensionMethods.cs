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
        public static Parameter GetParameterMatcher(this LambdaExpression lambda)
        {
            return lambda.UsesParameterMatcher() ? lambda.GetContainedParameterMatcher() : lambda.GetDefaultParameterMatcher();
        }

        public static bool UsesParameterMatcher(this LambdaExpression lambda)
        {
            var methodCall = lambda.Body as MethodCallExpression;
            return methodCall != null && typeof(Param).IsAssignableFrom(methodCall.Method.DeclaringType);
        }

        public static Parameter GetContainedParameterMatcher(this LambdaExpression lambda)
        {
            var methodCall = (MethodCallExpression)lambda.Body;
            var matcherType = GetMatcherType(methodCall);

            return CreateMathcerInstance(methodCall, matcherType) as Parameter;
        }

        public static Parameter GetDefaultParameterMatcher(this LambdaExpression lambda)
        {
            var compiledDelegate = lambda.Compile();
            var value = compiledDelegate.DynamicInvoke(new object[1]);
            return new ExactMatcher(value);
        }

        public static object CreateMathcerInstance(MethodCallExpression methodCall, Type matcherType)
        {
            var ctorArguments = methodCall.Arguments
                .Select(ResolveExpression)
                .ToArray();
            var createInstanceArguments = new List<object>(ctorArguments);
            createInstanceArguments.Insert(0, matcherType);
            return ctorArguments.Length == 0
                ? Activator.CreateInstance(matcherType)
                : Activator.CreateInstance(matcherType, ctorArguments);
        }

        public static object ResolveExpression(Expression expression)
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
            var attribute = methodCall.Method.GetCustomAttribute<ParameterMatcherAttribute>();
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
                // Could not identify any way of previously determining if a type is actually a generic type
                // before invoking GetGenericTypeDefinition
            }

            return matcherType;
        }
    }
}