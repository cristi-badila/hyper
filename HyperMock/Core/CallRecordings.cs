namespace HyperMock.Universal.Core
{
    using System.Collections.Generic;
    using System.Linq;

    public class CallRecordings : List<CallRecording>
    {
        public IEnumerable<CallRecording> Filter(CallDescriptor callDescriptor)
        {
            return this.Where(callRecording =>
                callRecording.MemberName == callDescriptor.MemberName &&
                callDescriptor.ParameterMatchers.Match(callRecording.Arguments));
        }

        public IEnumerable<CallRecording> Filter(string memberName, ParameterMatcher parameterMatcher = null)
        {
            var matchers = parameterMatcher == null
                ? new ParameterMatchersList()
                : new ParameterMatchersList(parameterMatcher);
            return Filter(new CallDescriptor(memberName, matchers));
        }
    }
}