namespace HyperMock.Universal.Verification
{
    public abstract class Occurred
    {
        protected Occurred(int count)
        {
            Count = count;
        }

        public int Count { get; }

        public static Occurred Once()
        {
            return new ExactOccurred(1);
        }

        public static Occurred Never()
        {
            return new ExactOccurred(0);
        }

        public static Occurred Exactly(int count)
        {
            return new ExactOccurred(count);
        }

        public static Occurred AtLeast(int count)
        {
            return new CountOrMoreOccurred(count);
        }

        public abstract void Assert(int actualCount);
    }
}