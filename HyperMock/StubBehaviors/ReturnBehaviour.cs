namespace HyperMock.Universal.StubBehaviors
{
    /// <summary>
    ///     Provides return type behaviours to be added.
    /// </summary>
    /// <typeparam name="TReturn">Return type</typeparam>
    public class ReturnBehaviour<TReturn>
    {
        private readonly CallDescriptor _callDescriptor;

        public ReturnBehaviour(CallDescriptor callDescriptor)
        {
            _callDescriptor = callDescriptor;
        }

        /// <summary>
        ///     The mocked type method or property returns this value.
        /// </summary>
        /// <param name="returnValue">Value to return</param>
        public void Returns(TReturn returnValue)
        {
            _callDescriptor.ReturnValue = returnValue;
        }

        /// <summary>
        ///     The mocked type method or parameter throws an excpetion.
        /// </summary>
        /// <typeparam name="TException">Exception type</typeparam>
        public void Throws<TException>()
        {
            _callDescriptor.ExceptionType = typeof(TException);
        }
    }
}