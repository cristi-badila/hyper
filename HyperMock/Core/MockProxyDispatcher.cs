﻿namespace HyperMock.Universal.Core
{
    using System;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    ///     Instance of the dispatcher proxy. This acts like an interceptor for the mocking framework.
    /// </summary>
    public class MockProxyDispatcher : DispatchProxy
    {
        public CallDescriptors KnownCallDescriptors { get; } = new CallDescriptors();

        public CallRecordings RecordedCalls { get; } = new CallRecordings();

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var name = targetMethod.Name;
            var matchedCallInfo = KnownCallDescriptors.LastOrDefault(ci => ci.MemberName == name && ci.ParameterMatchers.Match(args));
            RecordCall(targetMethod, args);
            return matchedCallInfo == null
                ? HandleUnmatchedCall(targetMethod)
                : HandleMatchedCall(matchedCallInfo);
        }

        private static object HandleUnmatchedCall(MethodInfo targetMethod)
        {
            return GetDefaultValueByType(targetMethod.ReturnType);
        }

        private static object HandleMatchedCall(CallDescriptor matchedCallDescriptor)
        {
            if (matchedCallDescriptor.ExceptionType == null)
            {
                return matchedCallDescriptor.ReturnValue;
            }

            throw (Exception)Activator.CreateInstance(matchedCallDescriptor.ExceptionType);
        }

        private static object GetDefaultValueByType(Type type)
        {
            return type != typeof(void) && type.GetTypeInfo().IsValueType
                ? Activator.CreateInstance(type)
                : null;
        }

        private void RecordCall(MemberInfo targetMethod, object[] args)
        {
            RecordedCalls.Add(new CallRecording(targetMethod.Name, args));
        }
    }
}