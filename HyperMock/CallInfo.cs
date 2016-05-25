using System;
using System.Linq;

namespace HyperMock
{
    internal class CallInfo
    {
        internal CallInfo()
        {
            Parameters = new Parameter[0];
        }

        internal string Name { get; set; }
        internal Parameter[] Parameters { get; set; }
        internal object ReturnValue { get; set; }
        internal Type ExceptionType { get; set; }
        internal int Visited { get; set; }

        internal bool IsMatchFor(params object[] args)
        {
            if (args.Length == 0 && args.Length == Parameters.Count()) return true;

            if (args.Length == Parameters.Count())
            {
                for (var i = 0; i < Parameters.Count(); i++)
                {
                    if (Parameters[i].Type == ParameterType.Anything) continue;
                    if (!Equals(args[i], Parameters[i].Value)) return false;
                }

                return true;
            }

            return false;
        }
    }
}