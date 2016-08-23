namespace HyperMock.Universal.ExtensionMethods
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Core;
    using Exceptions;
    using ParameterMatchers;

    public static class LambdaExtensionMethods
    {
        public static ParameterMatcher GetParameterMatcher(this LambdaExpression lambda)
        {
            var methodCallExpression = lambda.GetNestedMethodCallExpression();
            var matcherType = methodCallExpression?.GetMatcherType();
            return matcherType == null
                ? lambda.GetExactValueMatcher()
                : ParameterMatcherActivator.CreateInstance(matcherType, methodCallExpression.Arguments);
        }

        public static ParameterMatcher GetExactValueMatcher(this LambdaExpression lambda)
        {
            try
            {
                return new ExactMatcher(lambda.Compile().DynamicInvoke(new object[lambda.Parameters.Count]));
            }
            catch (Exception exception) when (exception is MemberAccessException || exception is ArgumentException || exception is TargetInvocationException)
            {
                throw new InvalidParameterExpressionException(lambda);
            }
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

        public static MethodCallExpression GetNestedMethodCallExpression(this LambdaExpression expression)
        {
            var expressionBody = expression.Body;
            MethodCallExpression result = null;
            var methodCallExpression = expressionBody as MethodCallExpression;
            if (methodCallExpression != null)
            {
                result = methodCallExpression;
            }
            else
            {
                // Try to handle expressions that result from automatic type conversions
                var unaryExpression = expressionBody as UnaryExpression;
                if (unaryExpression?.NodeType == ExpressionType.Convert)
                {
                    result = unaryExpression.Operand as MethodCallExpression;
                }
            }

            return result;
        }
    }
}