namespace HyperMock.Universal.Core
{
    using System.Linq.Expressions;

    public interface IParameterMatcherInfoFactory
    {
        ParameterMatcherInfo Create(LambdaExpression parrentExpression);
    }
}