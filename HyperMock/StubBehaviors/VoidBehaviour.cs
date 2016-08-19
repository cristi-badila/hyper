namespace HyperMock.Universal.StubBehaviors
{
    using Core;

    /// <summary>
    ///     Provides void method behaviours to be added.
    /// </summary>
    public class VoidBehaviour
    {
        private readonly CallDescriptor _callDescriptor;

        public VoidBehaviour(CallDescriptor callDescriptor)
        {
            _callDescriptor = callDescriptor;
        }

        /// <summary>
        ///     The mocked type method or parameter throws an exception.
        /// </summary>
        /// <typeparam name="TException">Exception type</typeparam>
        public void Throws<TException>()
        {
            _callDescriptor.ExceptionType = typeof(TException);
        }
    }
}