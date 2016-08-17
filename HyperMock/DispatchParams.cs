namespace HyperMock.Universal
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class DispatchParams
    {
        public DispatchParams()
        {
            Arguments = new Expression[0];
        }

        public DispatchParams(string name, IEnumerable<Expression> arguments)
            : this()
        {
            Name = name;
            Arguments = arguments == null ? Arguments : arguments.ToArray();
        }

        public string Name { get; set; }

        public Expression[] Arguments { get; set; }
    }
}