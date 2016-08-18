namespace HyperMock.Universal
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class DispatchParams
    {
        public DispatchParams()
        {
            Arguments = new List<Expression>();
        }

        public DispatchParams(string name, IEnumerable<Expression> arguments)
            : this()
        {
            Name = name;
            Arguments = arguments == null ? Arguments : arguments.ToList();
        }

        public string Name { get; set; }

        public List<Expression> Arguments { get; set; }
    }
}