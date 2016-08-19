namespace HyperMock.Universal.Tests.Core
{
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Universal.Core;

    [TestClass]
    public class CallRecordingTests
    {
        [TestMethod]
        public void Constructor_Always_SetsGivenValuesOnTheCreatedInstance()
        {
            var callArguments = new object[0];

            var callRecording = new CallRecording("testName", callArguments);

            Assert.AreEqual("testName", callRecording.MemberName);
            Assert.AreSame(callArguments, callRecording.Arguments);
        }
    }
}
