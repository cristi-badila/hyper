namespace HyperMock.Universal.Tests.Support
{
    using System;
    using Universal.Core;

    public class TestMatcher<TInput> : ParameterMatcher
    {
        public TestMatcher(Func<Guid> ctorParameter)
        {
            CtorParameter = ctorParameter;
        }

        public Func<Guid> CtorParameter { get; private set; }

        public override bool Matches(object argument)
        {
            return !Equals(default(TInput), argument);
        }
    }
}
