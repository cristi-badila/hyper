using System.Linq;

namespace HyperMock.Universal.ExtensionMethods
{
    public static class ParameterCollectionExtensionMethods
    {
        public static bool Matches(this ParameterCollection expectedParams, object[] actualParams)
        {
            return expectedParams.Count == actualParams.Length &&
                   expectedParams.Where((parameter, index) => parameter.Matches(actualParams[index])).Count() == expectedParams.Count;
        }
    }
}