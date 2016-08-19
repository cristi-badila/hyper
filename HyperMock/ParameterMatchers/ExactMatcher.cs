namespace HyperMock.Universal.ParameterMatchers
{
    using Core;

    public class ExactMatcher : ParameterMatcher
    {
        public ExactMatcher(object value)
        {
            Value = value;
        }

        public object Value { get; }

        public override bool Matches(object argument)
        {
            return Equals(argument, Value);
        }
    }
}