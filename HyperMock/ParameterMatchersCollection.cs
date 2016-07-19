namespace HyperMock.Universal
{
    using System.Collections.Generic;

    public class ParameterMatchersCollection : List<Parameter>
    {
        public ParameterMatchersCollection()
        {
        }

        public ParameterMatchersCollection(IEnumerable<Parameter> collection)
            : base(collection)
        {
        }
    }
}