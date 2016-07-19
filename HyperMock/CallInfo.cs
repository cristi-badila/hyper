using System;
using HyperMock.Universal.ExtensionMethods;

namespace HyperMock.Universal
{
    public class CallInfo
    {
        public CallInfo()
        {
            Parameters = new ParameterMatchersCollection();
        }

        public string Name { get; set; }

        public ParameterMatchersCollection Parameters { get; set; }

        public object ReturnValue { get; set; }

        public Type ExceptionType { get; set; }

        public int Visited { get; set; }

        public bool IsMatchFor(params object[] args)
        {
            return Parameters.Matches(args);
        }
    }
}