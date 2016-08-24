namespace HyperMock.Universal.Tests.Support
{
    using Universal.Core;

    public class TestSyntax
    {
        [ParameterMatcher(typeof(GeneralMatcher))]
        public static object GeneralMatcher()
        {
            return null;
        }

        [ParameterMatcher(typeof(GenericMatcher<>))]
        public static T GenericMatcher<T>()
        {
            return default(T);
        }

        [ParameterMatcher(typeof(StringMatcher))]
        public static string StringMatcher(string testValue)
        {
            return string.Empty;
        }

        [ParameterMatcher(typeof(IntMatcher))]
        public static int IntMatcher(int param1)
        {
            return 0;
        }
    }
}