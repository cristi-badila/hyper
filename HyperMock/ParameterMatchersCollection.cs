using System.Collections.Generic;

namespace HyperMock.Universal
{
    public class ParameterMatchersCollection : List<Parameter>
    {
        public ParameterMatchersCollection()
        {            
        }

        public ParameterMatchersCollection(IEnumerable<Parameter> collection) : base(collection)
        {
        }
    }
}