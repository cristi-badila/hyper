namespace HyperMock.Universal.Core
{
    using System.Collections.Generic;

    public class ParameterMatchersList : List<ParameterMatcher>
    {
        public ParameterMatchersList()
        {
        }

        public ParameterMatchersList(IEnumerable<ParameterMatcher> collection)
            : base(collection)
        {
        }
    }
}