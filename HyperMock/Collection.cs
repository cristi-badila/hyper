namespace HyperMock.Universal
{
    using System.Collections;
    using ParameterMatchers;

    public static class Collection
    {
        [ParameterMatcher(typeof(CollectionMatcher))]
        public static TCollection IsEquivalentTo<TCollection>(TCollection expectedList)
            where TCollection : IEnumerable
        {
            return default(TCollection);
        }
    }
}