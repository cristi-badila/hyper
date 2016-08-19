namespace HyperMock.Universal.ExtensionMethods
{
    using System.Linq.Expressions;
    using System.Reflection;
    using Core;
    using ParameterMatchers;

    public static class LambdaExtensionMethods
    {
        public static ParameterMatcher GetParameterMatcher(this LambdaExpression lambda)
        {
            var methodCallExpression = lambda.Body as MethodCallExpression;
            return methodCallExpression == null
                ? lambda.GetDefaultParameterMatcher()
                : ParameterMatcherActivator.CreateInstance(methodCallExpression.GetMatcherType(), methodCallExpression.Arguments);
        }

        public static ParameterMatcher GetDefaultParameterMatcher(this LambdaExpression lambda)
        {
            return new ExactMatcher(lambda.Compile().DynamicInvoke());
        }

        public static ExpressionInfo GetExpressionInfoForMethod(this LambdaExpression expression)
        {
            var body = expression.Body as MethodCallExpression;
            return body == null
                ? null
                : new ExpressionInfo(body.Method.Name, body.Arguments);
        }

        public static ExpressionInfo GetExpressionInfoForGet(this LambdaExpression expression)
        {
            var body = expression.Body as MemberExpression;
            return body == null
                ? null
                : new ExpressionInfo(((PropertyInfo)body.Member).GetMethod.Name, null);
        }

        public static ExpressionInfo GetExpressionInfoForSet(this LambdaExpression expression)
        {
            var body = expression.Body as MemberExpression;
            return body == null
                ? null
                : new ExpressionInfo(((PropertyInfo)body.Member).SetMethod.Name, null);
        }
    }
}