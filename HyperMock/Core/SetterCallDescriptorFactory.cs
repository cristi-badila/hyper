namespace HyperMock.Universal.Core
{
    public class SetterCallDescriptorFactory : CallDescriptorFactory<SetterCallDescriptorFactory>
    {
        public SetterCallDescriptorFactory()
            : this(SetterMethodCallInfoFactory.Instance)
        {
        }

        public SetterCallDescriptorFactory(
            IMethodCallInfoFactory methodCallInfoFactory = null,
            IParameterMatcherFactory parameterMatcherFactory = null)
            : base(methodCallInfoFactory ?? SetterMethodCallInfoFactory.Instance, parameterMatcherFactory)
        {
        }
    }
}
