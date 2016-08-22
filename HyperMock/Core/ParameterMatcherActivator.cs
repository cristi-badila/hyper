namespace HyperMock.Universal.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Exceptions;

    public static class ParameterMatcherActivator
    {
        public static ParameterMatcher CreateInstance(Type matcherType, IEnumerable<Expression> argumentExpressions)
        {
            var ctorArguments = argumentExpressions
                .Select(ResolveArgumentExpression)
                .ToArray();
            var matcher = ctorArguments.Length == 0
                ? Activator.CreateInstance(matcherType)
                : Activator.CreateInstance(matcherType, ctorArguments);

            if (!(matcher is ParameterMatcher))
            {
                throw new InvalidParameterMatcherException(matcher.GetType());
            }

            return (ParameterMatcher)matcher;
        }

        private static object ResolveArgumentExpression(Expression expression)
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
    }
}
