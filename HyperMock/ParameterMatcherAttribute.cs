using System;

namespace HyperMock.Universal
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ParameterMatcherAttribute : Attribute
    {
        public Type MatcherType { get; }

        public ParameterMatcherAttribute(Type matcherType)
        {
            MatcherType = matcherType;
        }
    }
}