namespace HyperMock.Universal
{
    /// <summary>
    ///     Provides return type behaviours to be added.
    /// </summary>
    /// <typeparam name="TMock">Mocked type</typeparam>
    /// <typeparam name="TReturn">Return type</typeparam>
    public class ReturnBehaviour<TMock, TReturn>
    {
        private readonly CallInfo _callInfo;

        internal ReturnBehaviour(CallInfo callInfo)
        {
            _callInfo = callInfo;
        }

        /// <summary>
        ///     The mocked type method or property returns this value.
        /// </summary>
        /// <param name="returnValue">Value to return</param>
        public void Returns(TReturn returnValue)
        {
            _callInfo.ReturnValue = returnValue;
        }

        /// <summary>
        ///     The mocked type method or parameter throws an excpetion.
        /// </summary>
        /// <typeparam name="TException">Exception type</typeparam>
        public void Throws<TException>()
        {
            _callInfo.ExceptionType = typeof(TException);
        }
    }
}