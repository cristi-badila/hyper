namespace HyperMock.Universal
{
    public abstract class ParameterMatcher
    {
        public abstract bool Matches(object argument);
    }
}