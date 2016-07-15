using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace HyperMock.Universal.ExtensionMethods
{
    public static class MockProxyDispatcherExtensionMethods
    {
        public static bool TryGetWritePropertyNameAndArgs(this MockProxyDispatcher mockProxyDispatcher, Expression expression, out string name)
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

        public static bool TryGetMethodNameAndArgs(
            this MockProxyDispatcher mockProxyDispatcher,
            Expression expression,
            out string name,
            out ReadOnlyCollection<Expression> arguments)
        {
            var lambda = (LambdaExpression) expression;
            var body = lambda.Body as MethodCallExpression;

            if (body != null)
            {
                name = body.Method.Name;
                arguments = body.Arguments;

                return true;
            }

            name = null;
            arguments = new ReadOnlyCollection<Expression>(new List<Expression>());
            return false;
        }

        public static bool TryGetReadPropertyNameAndArgs(this MockProxyDispatcher mockProxyDispatcher, Expression expression, out string name)
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