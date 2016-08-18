namespace HyperMock.Universal.ExtensionMethods
{
    using System.Linq;

    public static class ParameterCollectionExtensionMethods
    {
        public static bool Match(this ParameterMatchersCollection expectedParams, object[] actualParams)
        {
            return expectedParams.Count == actualParams.Length &&
                   expectedParams.Where((parameter, index) => parameter.Matches(actualParams[index])).Count() == expectedParams.Count;
        }
    }
}