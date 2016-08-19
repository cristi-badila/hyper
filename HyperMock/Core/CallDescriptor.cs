namespace HyperMock.Universal.Core
{
    using System;

    public class CallDescriptor
    {
        public CallDescriptor()
        {
            ParameterMatchers = new ParameterMatchersList();
        }

        public string MemberName { get; set; }

        public ParameterMatchersList ParameterMatchers { get; set; }

        public object ReturnValue { get; set; }

        public Type ExceptionType { get; set; }
    }
}