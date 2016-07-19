namespace HyperMock.Universal.Tests
{
    using Exceptions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Support;
    using Verification;

    [TestClass]
    public class PropertyTest : TestBase<UserController>
    {
        [TestMethod]
        public void CanReadMockProperty()
        {
            MockFor<IUserService>().SetupGet(p => p.Help).Returns("Some help");

            var result = Subject.GetHelp();

            Assert.AreEqual(result, "Some help");
        }

        [TestMethod]
        public void VerifyReadProperty()
        {
            MockFor<IUserService>().SetupGet(p => p.Help).Returns("Some help");

            Subject.GetHelp();

            MockFor<IUserService>().VerifyGet(p => p.Help, "Some help");
        }

        [TestMethod]
        public void VerifyThrowsExceptionInvalidReadPropertyValue()
        {
            MockFor<IUserService>().SetupGet(p => p.Help).Returns("Some help");

            Subject.GetHelp();

            Assert.ThrowsException<VerificationException>(
                () => MockFor<IUserService>().VerifyGet(p => p.CurrentRole, "No help"));
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
        public void VerifyThrowsExceptionInvalidWritePropertyValue()
        {
            var proxy = MockFor<IUserService>();
            proxy.SetupSet(p => p.CurrentRole, "Manager");

            Subject.SetCurrentRole("Manager");

            Assert.ThrowsException<VerificationException>(() => proxy.VerifySet(p => p.CurrentRole, "Supervisor"));
        }
    }
}