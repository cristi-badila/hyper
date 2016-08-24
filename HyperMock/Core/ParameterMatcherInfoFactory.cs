namespace HyperMock.Universal.Core
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using ParameterMatchers;

    public class ParameterMatcherInfoFactory : IParameterMatcherInfoFactory
    {
        public ParameterMatcherInfo Create(LambdaExpression lambdaExpression)
        {
            var methodCallExpression = ReduceToMethodCallExpression(lambdaExpression);
            var matcherType = GetMatcherType(methodCallExpression);

            return new ParameterMatcherInfo(matcherType, methodCallExpression?.Arguments);
        }

        protected static MethodCallExpression ReduceToMethodCallExpression(LambdaExpression expression)
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

        protected static Type GetMatcherType(MethodCallExpression methodCallExpression)
        {
            var attribute = methodCallExpression?.Method.GetCustomAttribute<ParameterMatcherAttribute>();
            if (attribute == null)
            {
                return typeof(ExactMatcher);
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