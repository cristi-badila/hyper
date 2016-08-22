namespace HyperMock.Universal.Tests.Support
{
    using Universal.Core;

    public class TestMatcher2 : ParameterMatcher
    {
        public TestMatcher2(string value)
        {
            CtorParameter = value;
        }

        public string CtorParameter { get; }

        public override bool Matches(object argument)
        {
            return !Equals(CtorParameter, argument);
        }
    }
}
