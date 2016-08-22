namespace HyperMock.Universal.ParameterMatchers
{
    using Core;

    public class AnyMatcher : ParameterMatcher
    {
        public override bool Matches(object argument)
        {
            return true;
        }
    }

#pragma warning disable SA1402 // File may only contain a single class
    public class AnyMatcher<T> : ParameterMatcher
#pragma warning restore SA1402 // File may only contain a single class
    {
        public override bool Matches(object argument)
        {
            return argument is T;
        }
    }
}