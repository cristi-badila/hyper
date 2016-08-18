namespace HyperMock.Universal
{
    using System;
    using System.Linq;
    using System.Reflection;
    using ExtensionMethods;

    /// <summary>
    ///     Instance of the dispatcher proxy. This acts like an interceptor for the mocking framework.
    /// </summary>
    public class MockProxyDispatcher : DispatchProxy
    {
        public CallDescriptors RegisteredCalls { get; } = new CallDescriptors();

        public CallRecordings RecordedCalls { get; } = new CallRecordings();

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var name = targetMethod.Name;
            var matchedCallInfo = RegisteredCalls.Where(ci => ci.MemberName == name).FirstOrDefault(ci => ci.Parameters.Match(args));
            RecordCall(targetMethod, args);
            return matchedCallInfo == null
                ? HandleUnmatchedCall(targetMethod)
                : HandleMatchedCall(matchedCallInfo);
        }

        private static object HandleUnmatchedCall(MethodInfo targetMethod)
        {
            return GetDefault(targetMethod.ReturnType);
        }

        private static object HandleMatchedCall(CallDescriptor matchedCallDescriptor)
        {
            if (matchedCallDescriptor.ExceptionType == null)
            {
                return matchedCallDescriptor.ReturnValue;
            }

            throw (Exception)Activator.CreateInstance(matchedCallDescriptor.ExceptionType);
        }

        private static object GetDefault(Type type)
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