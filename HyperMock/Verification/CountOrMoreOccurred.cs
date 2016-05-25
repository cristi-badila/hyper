using HyperMock.Universal.Exceptions;

namespace HyperMock.Universal.Verification
{
    public class CountOrMoreOccurred : Occurred
    {
        public CountOrMoreOccurred(int count) : base(count)
        {

        }

        public override void Assert(int actualCount)
        {
            if (actualCount < Count)
                throw new VerificationException($"Verification mismatch: Expected at least {Count}; Actual {actualCount}");
        }
    }
}