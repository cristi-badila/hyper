namespace HyperMock.Universal.Core
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public class SetterMethodCallInfoFactory : IMethodCallInfoFactory
    {
        public MethodCallInfo Create(LambdaExpression expression)
        {
            var body = expression.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("Expression is not a MemberExpression");
            }

            var setMethodInfo = ((PropertyInfo)body.Member)?.SetMethod;
            if (setMethodInfo == null)
            {
                throw new ArgumentException("Expression refers to property with no setter");
            }

            return new MethodCallInfo(setMethodInfo.Name);
        }
    }
}