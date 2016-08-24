namespace HyperMock.Universal.Core
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class ParameterMatchersList : ReadOnlyCollection<ParameterMatcher>
    {
        public ParameterMatchersList(ParameterMatcher parameterMatcher)
            : base(new List<ParameterMatcher> { parameterMatcher })
        {
        }


        public ParameterMatchersList(IEnumerable<ParameterMatcher> collection)
            : base(collection.ToList())
        {
        }

        public ParameterMatchersList(IList<ParameterMatcher> collection)
            : base(collection)
        {
        }
    }
}