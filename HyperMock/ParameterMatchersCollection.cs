namespace HyperMock.Universal
{
    using System.Collections.Generic;

    public class ParameterMatchersCollection : List<ParameterMatcher>
    {
        public ParameterMatchersCollection()
        {
        }

        public ParameterMatchersCollection(IEnumerable<ParameterMatcher> collection)
            : base(collection)
        {
        }
    }
}