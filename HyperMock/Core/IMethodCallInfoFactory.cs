namespace HyperMock.Universal.Core
{
    using System.Linq.Expressions;

    public interface IMethodCallInfoFactory
    {
        MethodCallInfo Create(LambdaExpression expression);
    }
}