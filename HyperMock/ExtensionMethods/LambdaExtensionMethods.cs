namespace HyperMock.Universal.ExtensionMethods
{
    using System;
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
            return methodCall != null && typeof(It).IsAssignableFrom(methodCall.Method.DeclaringType);
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
            else if (expression is MemberExpression)
            {
                var objectMember = Expression.Convert(expression, typeof(object));
                var getterLambda = Expression.Lambda<Func<object>>(objectMember);
                var getter = getterLambda.Compile();
                result = getter();
            }
            else if (expression is LambdaExpression)
            {
                result = ((LambdaExpression)expression).Compile();
            }
            else
            {
                result = expression;
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

        public static bool TryGetDispatchParams(
            this LambdaExpression expression,
            out DispatchParams dispatchParams)
        {
            var body = expression.Body as MethodCallExpression;

            if (body == null)
            {
                dispatchParams = new DispatchParams();
                return false;
            }

            dispatchParams = new DispatchParams(body.Method.Name, body.Arguments);
            return true;
        }

        public static bool TryGetWritePropertyName(this LambdaExpression expression, out string name)
        {
            name = null;
            var body = expression.Body as MemberExpression;

            if (body == null)
            {
                return false;
            }

            var propInfo = (PropertyInfo)body.Member;
            name = propInfo.SetMethod.Name;
            return true;
        }

        public static bool TryGetReadPropertyName(this LambdaExpression expression, out string name)
        {
            var body = expression.Body as MemberExpression;

            if (body == null)
            {
                name = null;
                return false;
            }

            var propInfo = (PropertyInfo)body.Member;
            name = propInfo.GetMethod.Name;
            return true;
        }
    }
}