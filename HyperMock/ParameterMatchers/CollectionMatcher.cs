namespace HyperMock.Universal.ParameterMatchers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class CollectionMatcher : ParameterMatcher
    {
        private readonly IEnumerable _enumerable;
        private IList<object> _list;

        public CollectionMatcher(IEnumerable enumerable)
        {
            _enumerable = enumerable;
        }

        private IList<object> ExpectedCollection => _list ?? (_list = _enumerable.Cast<object>().ToList());

        public override bool Matches(object argument)
        {
            IList<object> actualCollection;
            return TryGetList(argument, out actualCollection)
                   && actualCollection.Count == ExpectedCollection.Count
                   && actualCollection.All(item => ExpectedCollection.Contains(item))
                   && ExpectedCollection.All(item => actualCollection.Contains(item));
        }

        private static bool TryGetList(object argument, out IList<object> result)
        {
            result = null;
            var enumerable = argument as IEnumerable;
            if (enumerable == null)
            {
                return false;
            }

            result = enumerable.Cast<object>().ToList();
            return true;
        }
    }
}