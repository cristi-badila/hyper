namespace HyperMock.Universal.OccuredMatchers
{
    using Exceptions;
    using Syntax;

    public class CountOrMoreOccurred : Occurred
    {
        public CountOrMoreOccurred(int count)
            : base(count)
        {
        }

        public override void Assert(int actualCount)
        {
            if (actualCount < Count)
            {
                throw new VerificationException(
                    $"Verification mismatch: Expected at least {Count} call(s); Actual {actualCount}");
            }
        }
    }
}