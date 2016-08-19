namespace HyperMock.Universal.Tests.IntegrationTests
{
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Support;
    using Syntax;

    [TestClass]
    public class PropertyTests : TestBase<UserController>
    {
        [TestMethod]
        public void CanReadMockProperty()
        {
            MockFor<IUserService>().SetupGet(p => p.Help).Returns("Some help");

            var result = Subject.GetHelp();

            Assert.AreEqual("Some help", result);
        }

        [TestMethod]
        public void VerifyReadProperty()
        {
            var proxy = MockFor<IUserService>();
            proxy.SetupGet(p => p.Help).Returns("Some help");

            Subject.GetHelp();

            proxy.VerifyGet(p => p.Help, Occurred.Once());
        }

        [TestMethod]
        public void VerifyWriteProperty()
        {
            var proxy = MockFor<IUserService>();
            proxy.SetupSet(p => p.CurrentRole);

            Subject.SetCurrentRole("Manager");

            proxy.VerifySet(p => p.CurrentRole, "Manager");
        }

        [TestMethod]
        public void VerifyWritePropertyWithNotUsedValueWorks()
        {
            var proxy = MockFor<IUserService>();
            proxy.SetupSet(p => p.CurrentRole);

            Subject.SetCurrentRole("Manager");

            proxy.VerifySet(p => p.CurrentRole, "Supervisor", Occurred.Never());
        }
    }
}