namespace HyperMock.Universal.ExtensionMethods
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public static class ExpressionInfoExtensionMethods
    {
        public static ParameterMatchersCollection GetParameterMatchers(
            this ExpressionInfo expressionInfo,
            IEnumerable<ParameterExpression> parameters)
        {
            return new ParameterMatchersCollection(expressionInfo.Arguments
                .Select(argument => Expression.Lambda(argument, parameters))
                .Select(LambdaExtensionMethods.GetParameterMatcher));
        }
    }
}