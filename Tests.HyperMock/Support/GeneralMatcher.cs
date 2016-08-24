namespace HyperMock.Universal.Tests.Support
{
    using Universal.Core;

    public class GeneralMatcher : ParameterMatcher
    {
        public override bool Matches(object argument)
        {
            return false;
        }
    }
}