namespace HyperMock.Universal.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class MethodCallInfo
    {
        public MethodCallInfo()
        {
            Arguments = new List<Expression>();
        }

        public MethodCallInfo(string name, IEnumerable<Expression> arguments = null)
            : this()
        {
            Name = name;
            Arguments = arguments == null ? Arguments : arguments.ToList();
        }

        public string Name { get; }

        public IReadOnlyCollection<Expression> Arguments { get; }
    }
}