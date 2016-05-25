using System;
using HyperMock.Universal;
using HyperMock.Universal.Exceptions;
using HyperMock.Universal.Verification;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Tests.HyperMock.Universal.Support;

namespace Tests.HyperMock.Universal
{
    [TestClass]
    public class FunctionTest
    {
        [TestMethod]
        public void ReturnsTrueForMatchingParameter()
        {
            var proxy = Mock.Create<IUserService>();
            proxy.Setup(p => p.Save("Homer")).Returns(true);

            var controller = new UserController(proxy);

            var result = controller.Save("Homer");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReturnsTrueForAnyParameter()
        {
            var proxy = Mock.Create<IUserService>();
            proxy.Setup(p => p.Save(Param.IsAny<string>())).Returns(true);

            var controller = new UserController(proxy);

            var result = controller.Save("Homer");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReturnsTrueForOverloadWithAnyParameter()
        {
            var proxy = Mock.Create<IUserService>();
            proxy.Setup(p => p.Save("Homer")).Returns(false); // Set to check overload works!
            proxy.Setup(p => p.Save("Homer", Param.IsAny<string>())).Returns(true);

            var controller = new UserController(proxy);

            var result = controller.SaveWithRole("Homer", "Manager");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReturnsTrueForMatchingParameterWithMultiSetups()
        {
            var proxy = Mock.Create<IUserService>();
            proxy.Setup(p => p.Save("Marge")).Returns(true);
            proxy.Setup(p => p.Save("Homer")).Returns(true);

            var controller = new UserController(proxy);

            var result = controller.Save("Homer");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UnmatchedThrowsException()
        {
            var proxy = Mock.Create<IUserService>();
            proxy.Setup(p => p.Save("Homer")).Returns(true);

            var controller = new UserController(proxy);

            Assert.ThrowsException<MockException>(() => controller.Save("Marge"));
        }

        [TestMethod]
        public void MatchThrowsException()
        {
            var proxy = Mock.Create<IUserService>();
            proxy.Setup(p => p.Save("Bart")).Throws<InvalidOperationException>();

            var controller = new UserController(proxy);

            Assert.ThrowsException<InvalidOperationException>(() => controller.Save("Bart"));
        }

        [TestMethod]
        public void VerifyMatchesExpectedSingleVisit()
        {
            var proxy = Mock.Create<IUserService>();
            proxy.Setup(p => p.Save("Bart")).Returns(true);

            var controller = new UserController(proxy);

            controller.Save("Bart");

            proxy.Verify(p => p.Save("Bart"), Occurred.Once());
        }

        [TestMethod]
        public void VerifyMatchesExpectedZeroVisits()
        {
            var proxy = Mock.Create<IUserService>();
            proxy.Setup(p => p.Save("Bart")).Returns(true);
            proxy.Setup(p => p.Save("Marge")).Returns(false);

            var controller = new UserController(proxy);

            controller.Save("Bart");

            proxy.Verify(p => p.Save("Marge"), Occurred.Never());
        }

        [TestMethod]
        public void VerifyMatchesExpectedAtLeastVisits()
        {
            var proxy = Mock.Create<IUserService>();
            proxy.Setup(p => p.Save("Bart")).Returns(true);

            var controller = new UserController(proxy);

            controller.Save("Bart");
            controller.Save("Bart");
            controller.Save("Bart");

            proxy.Verify(p => p.Save("Bart"), Occurred.AtLeast(2));
        }
    }
}