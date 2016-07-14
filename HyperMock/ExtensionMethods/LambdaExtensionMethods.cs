using System.Linq.Expressions;

namespace HyperMock.Universal.ExtensionMethods
{
    public static class LambdaExtensionMethods
    {
        public static ParameterType GetParameterType(this LambdaExpression lambda)
        {
            var methodCall = lambda.Body as MethodCallExpression;
            return methodCall != null && methodCall.Method.DeclaringType == typeof(Param)
                ? ParameterType.Anything
                : ParameterType.AsDefined;
        }
    }
}