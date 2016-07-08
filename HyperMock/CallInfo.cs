using System;
using HyperMock.Universal.ExtensionMethods;

namespace HyperMock.Universal
{
    internal class CallInfo
    {
        internal CallInfo()
        {
            Parameters = new ParameterCollection();
        }

        internal string Name { get; set; }

        internal ParameterCollection Parameters { get; set; }

        internal object ReturnValue { get; set; }

        internal Type ExceptionType { get; set; }

        internal int Visited { get; set; }

        internal bool IsMatchFor(params object[] args)
        {
            return Parameters.Matches(args);
        }
    }
}