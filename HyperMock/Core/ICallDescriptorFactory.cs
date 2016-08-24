namespace HyperMock.Universal.Core
{
    using System.Linq.Expressions;

    public interface ICallDescriptorFactory
    {
        CallDescriptor Create<TDelegate>(Expression<TDelegate> expression);
    }
}