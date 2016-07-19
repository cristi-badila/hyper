namespace HyperMock.Universal.ParameterMatchers
{
    using System;

    public class PartialMatcher<T> : Parameter
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