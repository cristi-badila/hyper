using HyperMock;
using HyperMock.Verification;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Tests.HyperMock.Support;

namespace Tests.HyperMock
{
    [TestClass]
    public class MethodTest
    {
        [TestMethod]
        public void VerifyMatchesExpectedSingleVisit()
        {
            var proxy = Mock.Create<IUserService>();
            proxy.Setup(p => p.Delete("Homer"));

            var controller = new UserController(proxy);

            controller.Delete("Homer");

            proxy.Verify(p => p.Delete("Homer"), Occurred.Once());
        }

        [TestMethod]
        public void VerifyMatchesExpectedZeroVisits()
        {
            var proxy = Mock.Create<IUserService>();
            proxy.Setup(p => p.Delete("Homer"));
            proxy.Setup(p => p.Delete("Marge"));

            var controller = new UserController(proxy);

            controller.Delete("Homer");

            proxy.Verify(p => p.Delete("Marge"), Occurred.Never());
        }

        [TestMethod]
        public void VerifyMatchesExpectedAtLeastVisits()
        {
            var proxy = Mock.Create<IUserService>();
            proxy.Setup(p => p.Delete("Homer"));

            var controller = new UserController(proxy);

            controller.Delete("Homer");
            controller.Delete("Homer");
            controller.Delete("Homer");

            proxy.Verify(p => p.Delete("Homer"), Occurred.AtLeast(2));
        }

        [TestMethod]
        public void VerifyMatchesSingleVisitForAnyParameter()
        {
            var proxy = Mock.Create<IUserService>();
            proxy.Setup(p => p.Delete(Param.IsAny<string>()));

            var controller = new UserController(proxy);

            controller.Delete("Homer");

            proxy.Verify(p => p.Delete(Param.IsAny<string>()), Occurred.Once());
        }
    }
}