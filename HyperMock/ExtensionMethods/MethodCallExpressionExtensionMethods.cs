namespace HyperMock.Universal.ExtensionMethods
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class MethodCallExpressionExtensionMethods
    {
        public static Type GetMatcherType(this MethodCallExpression methodCallExpression)
        {
            var attribute = methodCallExpression?.Method.GetCustomAttribute<ParameterMatcherAttribute>();
            if (attribute == null)
            {
                return null;
            }

            var matcherType = attribute.MatcherType;
            try
            {
                if (matcherType.GetGenericTypeDefinition() == matcherType)
                {
                    var genericArguments = methodCallExpression.Method.GetGenericArguments();
                    matcherType = matcherType.MakeGenericType(genericArguments);
                }
            }
            catch (Exception)
            {
                // Could not identify any way of determining if a type is actually a generic type
                // before invoking GetGenericTypeDefinition so this is the next best thing
            }

            return matcherType;
        }
    }
}
