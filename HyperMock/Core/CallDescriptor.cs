using System.Collections.Generic;

namespace HyperMock.Universal.Core
{
    using System;

    public class CallDescriptor
    {
        public CallDescriptor(string memberName, ParameterMatchersList parameterMatchers = null)
        {
            MemberName = memberName;
            ParameterMatchers = parameterMatchers ?? new ParameterMatchersList(new List<ParameterMatcher>());
        }

        public string MemberName { get; }

        public ParameterMatchersList ParameterMatchers { get; }

        public object ReturnValue { get; set; }

        public Type ExceptionType { get; set; }
    }
}