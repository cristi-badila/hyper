namespace HyperMock.Universal.Tests.Support
{
    using Universal.Core;

    public class StringMatcher : ParameterMatcher
    {
        public StringMatcher(string value)
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
