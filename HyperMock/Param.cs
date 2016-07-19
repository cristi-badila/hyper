﻿namespace HyperMock.Universal
{
    using System;
    using ParameterMatchers;

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