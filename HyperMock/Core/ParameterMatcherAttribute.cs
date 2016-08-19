namespace HyperMock.Universal.Core
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class ParameterMatcherAttribute : Attribute
    {
        public ParameterMatcherAttribute(Type matcherType)
        {
            MatcherType = matcherType;
        }

        public Type MatcherType { get; }
    }
}