namespace HyperMock.Universal.ExtensionMethods
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Core;

    public static class MethodCallInfoExtensionMethods
    {
        public static ParameterMatchersList GetParameterMatchers(
            this MethodCallInfo methodCallInfo,
            IEnumerable<ParameterExpression> parameters)
        {
            return new ParameterMatchersList(methodCallInfo.Arguments
                .Select(argument => Expression.Lambda(argument, parameters))
                .Select(LambdaExtensionMethods.GetParameterMatcher));
        }
    }
}