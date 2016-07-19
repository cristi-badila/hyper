namespace HyperMock.Universal.ExtensionMethods
{
    using System.Linq.Expressions;
    using System.Reflection;

    public static class MockProxyDispatcherExtensionMethods
    {
        public static bool TryGetDispatchParams(
            this MockProxyDispatcher mockProxyDispatcher,
            Expression expression,
            out DispatchParams dispatchParams)
        {
            var lambda = (LambdaExpression)expression;
            var body = lambda.Body as MethodCallExpression;

            if (body != null)
            {
                dispatchParams = new DispatchParams(body.Method.Name, body.Arguments);
                return true;
            }

            dispatchParams = new DispatchParams();
            return false;
        }

        public static bool TryGetWritePropertyName(this MockProxyDispatcher mockProxyDispatcher, Expression expression, out string name)
        {
            name = null;
            var lambda = (LambdaExpression)expression;
            var body = lambda.Body as MemberExpression;

            if (body == null)
            {
                return false;
            }

            var propInfo = (PropertyInfo)body.Member;
            name = propInfo.SetMethod.Name;
            return true;
        }

        public static bool TryGetReadPropertyName(this MockProxyDispatcher mockProxyDispatcher, Expression expression, out string name)
        {
            var lambda = (LambdaExpression)expression;
            var body = lambda.Body as MemberExpression;

            if (body != null)
            {
                var propInfo = (PropertyInfo)body.Member;
                name = propInfo.GetMethod.Name;
                return true;
            }

            name = null;
            return false;
        }
    }
}