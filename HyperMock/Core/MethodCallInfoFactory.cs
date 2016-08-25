namespace HyperMock.Universal.Core
{
    using System;
    using System.Linq.Expressions;

    public class MethodCallInfoFactory : Singleton<MethodCallInfoFactory>, IMethodCallInfoFactory
    {
        public MethodCallInfo Create(LambdaExpression expression)
        {
            var body = expression.Body as MethodCallExpression;
            if (body == null)
            {
                throw new ArgumentException("Expression is not a MethodCallExpression");
            }

            return new MethodCallInfo(body.Method.Name, body.Arguments);
        }
    }
}
