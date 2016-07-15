using System;
using HyperMock.Universal.ParameterMatchers;

namespace HyperMock.Universal
{
    public static class Param
    {
        [ParameterMatcher(typeof(AnyMatcher))]
        public static T IsAny<T>()
        {
            return default(T);
        }

        [ParameterMatcher(typeof(PartialMatcher<>))]
        public static T Is<T>(Func<T, bool> comparer)
        {
            return default(T);
        }
    }
}