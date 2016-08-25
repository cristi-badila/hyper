namespace HyperMock.Universal.Core
{
    public class MethodCallDescriptorFactory : CallDescriptorFactory<MethodCallDescriptorFactory>
    {
        public MethodCallDescriptorFactory()
            : this(MethodCallInfoFactory.Instance)
        {
        }

        public MethodCallDescriptorFactory(
            IMethodCallInfoFactory methodCallInfoFactory = null,
            IParameterMatcherFactory parameterMatcherFactory = null)
            : base(methodCallInfoFactory ?? MethodCallInfoFactory.Instance, parameterMatcherFactory)
        {
        }
    }
}
