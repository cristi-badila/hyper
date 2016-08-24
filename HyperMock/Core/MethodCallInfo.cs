namespace HyperMock.Universal.Core
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq.Expressions;

    public class MethodCallInfo
    {
        public MethodCallInfo(string name, IList<Expression> arguments = null)
        {
            Name = name;
            Arguments = new ReadOnlyCollection<Expression>(arguments ?? new List<Expression>());
        }

        public string Name { get; }

        public IReadOnlyCollection<Expression> Arguments { get; }
    }
}