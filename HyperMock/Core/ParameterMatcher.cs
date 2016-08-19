namespace HyperMock.Universal.Core
{
    public abstract class ParameterMatcher
    {
        public abstract bool Matches(object argument);
    }
}