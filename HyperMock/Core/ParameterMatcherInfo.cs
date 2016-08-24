namespace HyperMock.Universal.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;

    public class ParameterMatcherInfo
    {
        public ParameterMatcherInfo(Type matcherType, IEnumerable<Expression> ctorArguments = null)
        {
            MatcherType = matcherType;
            CtorArguments = new ReadOnlyCollection<Expression>((ctorArguments ?? new Expression[0]).ToList());
        }

        public Type MatcherType { get; }

        public IReadOnlyCollection<Expression> CtorArguments { get; }
    }
}