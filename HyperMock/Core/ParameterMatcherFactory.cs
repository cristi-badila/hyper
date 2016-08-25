namespace HyperMock.Universal.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Exceptions;
    using ParameterMatchers;

    public class ParameterMatcherFactory : Singleton<ParameterMatcherFactory>, IParameterMatcherFactory
    {
        private readonly IParameterMatcherInfoFactory _parameterMatcherInfoFactory;

        public ParameterMatcherFactory()
            : this(ParameterMatcherInfoFactory.Instance)
        {
        }

        public ParameterMatcherFactory(IParameterMatcherInfoFactory parameterMatcherInfoFactory = null)
        {
            _parameterMatcherInfoFactory = parameterMatcherInfoFactory ?? ParameterMatcherInfoFactory.Instance;
        }

        public ParameterMatcher Create(LambdaExpression argumentExpression)
        {
            var parameterMatcherInfo = _parameterMatcherInfoFactory.Create(argumentExpression);
            return parameterMatcherInfo.MatcherType == typeof(ExactMatcher)
                ? GetExactValueMatcher(argumentExpression)
                : CreateInstance(parameterMatcherInfo.MatcherType, parameterMatcherInfo.CtorArguments);
        }

        protected static ExactMatcher GetExactValueMatcher(LambdaExpression lambda)
        {
            var expression = lambda.Body;
            if (expression is ParameterExpression)
            {
                throw new UnsupportedArgumentExpressionException(expression);
            }

            try
            {
                return new ExactMatcher(lambda.Compile().DynamicInvoke(new object[lambda.Parameters.Count]));
            }
            catch (Exception exception)
                when (
                    exception is MemberAccessException ||
                    exception is ArgumentException ||
                    exception is TargetInvocationException)
            {
                throw new UnsupportedArgumentExpressionException(expression, exception);
            }
        }

        protected static ParameterMatcher CreateInstance(Type matcherType, IEnumerable<Expression> argumentExpressions)
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

        protected static object ResolveArgumentExpression(Expression expression)
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
