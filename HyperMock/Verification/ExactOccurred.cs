using HyperMock.Exceptions;

namespace HyperMock.Verification
{
    public class ExactOccurred : Occurred
    {
        public ExactOccurred(int count) : base(count)
        {

        }

        public override void Assert(int actualCount)
        {
            if (Count != actualCount)
                throw new VerificationException($"Verification mismatch: Expected {Count}; Actual {actualCount}");
        }
    }
}