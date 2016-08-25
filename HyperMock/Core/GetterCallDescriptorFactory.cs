namespace HyperMock.Universal.Core
{
    public class GetterCallDescriptorFactory : CallDescriptorFactory<GetterCallDescriptorFactory>
    {
        public GetterCallDescriptorFactory()
            : this(GetterMethodCallInfoFactory.Instance)
        {
        }

        public GetterCallDescriptorFactory(
            IMethodCallInfoFactory methodCallInfoFactory = null,
            IParameterMatcherFactory parameterMatcherFactory = null)
            : base(methodCallInfoFactory ?? GetterMethodCallInfoFactory.Instance, parameterMatcherFactory)
        {
        }
    }
}