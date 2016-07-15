namespace HyperMock.Universal.ParameterMatchers
{
    public class ExactMatcher : Parameter
    {
        public object Value { get; }

        public ExactMatcher(object value)
        {
            Value = value;
        }

        public override bool Matches(object argument)
        {
            return Equals(argument, Value);
        }
    }
}