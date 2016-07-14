using System.Linq;

namespace HyperMock.Universal.ExtensionMethods
{
    public static class ParameterCollectionExtensionMethods
    {
        public static bool Matches(this ParameterCollection expectedParams, object[] actualParams)
        {
            return expectedParams.Count == actualParams.Length &&
                   expectedParams.Where((parameter, index) => ParameterMatches(parameter, actualParams[index])).Count() == expectedParams.Count;
        }

        private static bool ParameterMatches(Parameter parameter, object args)
        {
            return parameter.Type == ParameterType.Anything || Equals(args, parameter.Value);
        }
    }
}