namespace HyperMock.Universal.Tests.Support
{
    using Universal.Core;

    public class IntMatcher : ParameterMatcher
    {
        public IntMatcher(int value)
        {
            CtorParameter = value;
        }

        public int CtorParameter { get; }

        public override bool Matches(object argument)
        {
            return CtorParameter % 2 == 0;
        }
    }
}
