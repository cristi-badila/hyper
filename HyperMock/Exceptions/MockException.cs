using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HyperMock.Universal.Exceptions
{
    public class MockException : Exception
    {
        private const string ParameterSeparator = ", ";
        private const string NullParameter = "null";

        public MockException(string message) : base(message)
        {
        }

        public MockException(MemberInfo targetMethod, IReadOnlyCollection<object> args)
            : base(GetMessage(targetMethod, args))
        {
        }

        private static string GetMessage(MemberInfo targetMethod, IReadOnlyCollection<object> args)
        {
            var name = targetMethod.Name;

            if (name.StartsWith("get_")) name = name.Remove(0, 4);
            if (name.StartsWith("set_")) name = name.Remove(0, 4);
            if (args.Count == 0)
            {
                return name;
            }

            var parameterValues = string.Join(ParameterSeparator, args.Select(p => p ?? NullParameter));
            return $"{name}({parameterValues})";
        }
    }
}