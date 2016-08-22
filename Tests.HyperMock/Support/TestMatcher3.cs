namespace HyperMock.Universal.Tests.Support
{
    using Universal.Core;

    public class TestMatcher3 : ParameterMatcher
    {
        public TestMatcher3(int value)
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
