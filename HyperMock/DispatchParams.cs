using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace HyperMock.Universal
{
    public class DispatchParams
    {
        public DispatchParams()
        {
            Arguments = new Expression[0];
        }

        public DispatchParams(string name, ReadOnlyCollection<Expression> arguments)
        {
            Name = name;
            Arguments = arguments.ToArray();
        }

        public string Name { get; set; }

        public Expression[] Arguments { get; set; }
    }
}