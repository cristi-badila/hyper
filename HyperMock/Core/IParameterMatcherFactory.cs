namespace HyperMock.Universal.Core
{
    using System.Linq.Expressions;

    public interface IParameterMatcherFactory
    {
        ParameterMatcher Create(LambdaExpression expression);
    }
}