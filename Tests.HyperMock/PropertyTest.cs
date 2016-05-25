using HyperMock.Universal;
using HyperMock.Universal.Exceptions;
using HyperMock.Universal.Verification;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Tests.HyperMock.Universal.Support;

namespace Tests.HyperMock.Universal
{
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
            Subject.SetCurrentRole("Manager");

            MockFor<IUserService>().VerifySet(p => p.CurrentRole, "Manager");
        }

        [TestMethod]
        public void VerifyThrowsExceptionInvalidWritePropertyValue()
        {
            Subject.SetCurrentRole("Manager");

            Assert.ThrowsException<VerificationException>(
                () => MockFor<IUserService>().VerifySet(p => p.CurrentRole, "Supervisor"));
        }
    }
}