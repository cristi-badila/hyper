namespace HyperMock.Universal.ParameterMatchers
{
    public class AnyMatcher : Parameter
    {
        public override bool Matches(object argument)
        {
            return true;
        }
    }
}