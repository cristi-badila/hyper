namespace HyperMock.Universal
{
    using System.Collections.Generic;
    using System.Linq;
    using ExtensionMethods;

    public class CallRecordings : List<CallRecording>
    {
        public IEnumerable<CallRecording> Filter(string memberName, ParameterMatcher parameterMatcher = null)
        {
            var matchers = parameterMatcher == null
                ? new ParameterMatcher[0]
                : new[] { parameterMatcher };
            return Filter(memberName, matchers);
        }

        public IEnumerable<CallRecording> Filter(string memberName, ICollection<ParameterMatcher> parameterMatchers)
        {
            var filteredCalls = this.Where(ci => ci.MemberName == memberName);
            if (parameterMatchers.Any())
            {
                filteredCalls = filteredCalls.Where(callRecording => parameterMatchers.Match(callRecording.Arguments));
            }

            return filteredCalls;
        }
    }
}