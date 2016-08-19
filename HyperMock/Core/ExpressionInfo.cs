namespace HyperMock.Universal.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class ExpressionInfo
    {
        public ExpressionInfo()
        {
            Arguments = new List<Expression>();
        }

        public ExpressionInfo(string name, IEnumerable<Expression> arguments)
            : this()
        {
            Name = name;
            Arguments = arguments == null ? Arguments : arguments.ToList();
        }

        public string Name { get; set; }

        public List<Expression> Arguments { get; set; }
    }
}