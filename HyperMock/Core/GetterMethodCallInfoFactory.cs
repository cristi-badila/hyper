namespace HyperMock.Universal.Core
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public class GetterMethodCallInfoFactory : IMethodCallInfoFactory
    {
        public MethodCallInfo Create(LambdaExpression expression)
        {
            var body = expression.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("Expression is not a MemberExpression");
            }

            var getMethodInfo = ((PropertyInfo)body.Member)?.GetMethod;
            if (getMethodInfo == null)
            {
                throw new ArgumentException("Expression refers to property with no getter");
            }

            return new MethodCallInfo(getMethodInfo.Name);
        }
    }
}