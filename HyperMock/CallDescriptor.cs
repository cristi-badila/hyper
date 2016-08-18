namespace HyperMock.Universal
{
    using System;
    using ExtensionMethods;

    public class CallDescriptor
    {
        public CallDescriptor()
        {
            Parameters = new ParameterMatchersCollection();
        }

        public string MemberName { get; set; }

        public ParameterMatchersCollection Parameters { get; set; }

        public object ReturnValue { get; set; }

        public Type ExceptionType { get; set; }
    }
}