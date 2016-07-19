namespace HyperMock.Universal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

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
            var matchedCallInfo = _callInfoList.Where(ci => ci.Name == name).FirstOrDefault(ci => ci.IsMatchFor(args));
            if (matchedCallInfo == null)
            {
                return GetDefault(targetMethod.ReturnType);
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

        private static object GetDefault(Type type)
        {
            return type.GetTypeInfo().IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}