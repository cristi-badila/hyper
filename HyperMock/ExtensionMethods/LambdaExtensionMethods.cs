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
                : ParameterMatcherFactory.CreateInstance(matcherType, methodCallExpression.Arguments);
        }

        public static ExactMatcher GetExactValueMatcher(this LambdaExpression lambda)
        {
            var expression = lambda.Body;
            if (expression is ParameterExpression)
            {
                throw new InvalidParameterExpressionException(lambda);
            }

            try
            {
                return new ExactMatcher(lambda.Compile().DynamicInvoke(new object[lambda.Parameters.Count]));
            }
            catch (Exception exception) when (exception is MemberAccessException || exception is ArgumentException || exception is TargetInvocationException)
            {
                throw new InvalidParameterExpressionException(lambda);
            }
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