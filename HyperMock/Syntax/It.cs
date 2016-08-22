namespace HyperMock.Universal.Syntax
{
    using System;
    using Core;
    using ParameterMatchers;

    public static class It
    {
        [ParameterMatcher(typeof(AnyMatcher))]
        public static object IsAny()
        {
            return default(object);
        }

        [ParameterMatcher(typeof(AnyMatcher<>))]
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