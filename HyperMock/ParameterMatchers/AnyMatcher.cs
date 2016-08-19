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
}