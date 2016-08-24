namespace HyperMock.Universal.Tests.Core
{
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Universal.Core;

    [TestClass]
    public class CallDescriptorTests
    {
        [TestMethod]
        public void Constructor_ByDefault_InitializesParameterMatchers()
        {
            var callDescriptor = new CallDescriptor(string.Empty);

            Assert.IsNotNull(callDescriptor.ParameterMatchers);
        }
    }
}