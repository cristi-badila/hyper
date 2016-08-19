namespace HyperMock.Universal.ExtensionMethods
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Core;

    public static class ExpressionInfoExtensionMethods
    {
        public static ParameterMatchersList GetParameterMatchers(
            this ExpressionInfo expressionInfo,
            IEnumerable<ParameterExpression> parameters)
        {
            return new ParameterMatchersList(expressionInfo.Arguments
                .Select(argument => Expression.Lambda(argument, parameters))
                .Select(LambdaExtensionMethods.GetParameterMatcher));
        }
    }
}