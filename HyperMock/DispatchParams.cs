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
        {
            Name = name;
            Arguments = arguments.ToArray();
        }

        public string Name { get; set; }

        public Expression[] Arguments { get; set; }
    }
}