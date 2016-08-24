namespace HyperMock.Universal.Tests.Core
{
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Universal.Core;

    [TestClass]
    public class ParameterMatcherInfoTests
    {
        [TestMethod]
        public void Ctor_NoArgumentsAreGiven_SetsCtorArgumentsToAnEmptyCollection()
        {
            var matcherInfo = new ParameterMatcherInfo(typeof(object));

            Assert.AreEqual(0, matcherInfo.CtorArguments.Count);
        }
    }
}