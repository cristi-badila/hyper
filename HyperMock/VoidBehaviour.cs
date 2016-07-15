namespace HyperMock.Universal
{
    /// <summary>
    ///     Provides void method behaviours to be added.
    /// </summary>
    public class VoidBehaviour
    {
        private readonly CallInfo _callInfo;

        public VoidBehaviour(CallInfo callInfo)
        {
            _callInfo = callInfo;
        }

        /// <summary>
        ///     The mocked type method or parameter throws an exception.
        /// </summary>
        /// <typeparam name="TException">Exception type</typeparam>
        public void Throws<TException>()
        {
            _callInfo.ExceptionType = typeof(TException);
        }
    }
}