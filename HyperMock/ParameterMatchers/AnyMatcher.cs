namespace HyperMock.Universal.ParameterMatchers
{
    public class AnyMatcher : ParameterMatcher
    {
        public override bool Matches(object argument)
        {
            return true;
        }
    }
}