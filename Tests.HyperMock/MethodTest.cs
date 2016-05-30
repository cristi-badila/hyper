using System.Threading.Tasks;
using HyperMock.Universal;
using HyperMock.Universal.Verification;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Tests.HyperMock.Universal.Support;

namespace Tests.HyperMock.Universal
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

        [TestMethod]
        public async Task VerifyMatchesExpectedSingleVisitAsync()
        {
            var proxy = Mock.Create<IUserService>();
            proxy.Setup(p => p.DeleteAsync("Homer")).Returns(Task.Delay(0));

            var controller = new UserController(proxy);

            await controller.DeleteAsync("Homer");

            proxy.Verify(p => p.DeleteAsync("Homer"), Occurred.Once());
        }
    }
}