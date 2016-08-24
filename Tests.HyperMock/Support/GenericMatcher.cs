namespace HyperMock.Universal.Tests.Support
{
    using Universal.Core;

    public class GenericMatcher<TInput> : ParameterMatcher
    {
        public override bool Matches(object argument)
        {
            return !Equals(default(TInput), argument);
        }
    }
}
