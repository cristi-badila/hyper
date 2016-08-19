namespace HyperMock.Universal.ParameterMatchers
{
    using System;
    using Core;

    public class PartialMatcher<T> : ParameterMatcher
        where T : class
    {
        private readonly Func<T, bool> _comparer;

        public PartialMatcher(Func<T, bool> comparer)
        {
            _comparer = comparer;
        }

        public override bool Matches(object argument)
        {
            return _comparer.Invoke(argument as T);
        }
    }
}