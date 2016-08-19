namespace HyperMock.Universal
{
    using System;
    using ExtensionMethods;

    public class CallDescriptor
    {
        public CallDescriptor()
        {
            ParameterMatchers = new ParameterMatchersCollection();
        }

        public string MemberName { get; set; }

        public ParameterMatchersCollection ParameterMatchers { get; set; }

        public object ReturnValue { get; set; }

        public Type ExceptionType { get; set; }
    }
}