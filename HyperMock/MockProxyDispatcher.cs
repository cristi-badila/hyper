using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HyperMock.Universal.Exceptions;

namespace HyperMock.Universal
{
    /// <summary>
    ///     Instance of the dispatcher proxy. This acts like an interceptor for the mocking framework.
    /// </summary>
    public class MockProxyDispatcher : DispatchProxy
    {
        private readonly List<CallInfo> _callInfoList = new List<CallInfo>();

        public IList<CallInfo> RegisteredCallInfoList => _callInfoList;

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var name = targetMethod.Name;
            var callInfoListForName = _callInfoList.Where(ci => ci.Name == name).ToList();
            var matchedCallInfo = callInfoListForName.FirstOrDefault(ci => ci.IsMatchFor(args));
            if (matchedCallInfo == null)
            {
                throw new MockException(targetMethod, args);
            }

            if (matchedCallInfo.ExceptionType != null)
            {
                var exception = (Exception)Activator.CreateInstance(matchedCallInfo.ExceptionType);
                matchedCallInfo.Visited++;
                throw exception;
            }

            matchedCallInfo.Visited++;

            return matchedCallInfo.ReturnValue;
        }
    }
}