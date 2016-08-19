namespace HyperMock.Universal.Tests
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Support;
    using Syntax;

    [TestClass]
    public class FunctionTest : TestBase<UserController>
    {
        [TestMethod]
        public void ReturnsTrueForMatchingParameter()
        {
            MockFor<IUserService>().Setup(p => p.Save("Homer")).Returns(true);

            var result = Subject.Save("Homer");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReturnsTrueForAnyParameter()
        {
            MockFor<IUserService>().Setup(p => p.Save(It.IsAny<string>())).Returns(true);

            var result = Subject.Save("Homer");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReturnsTrueForOverloadWithAnyParameter()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Save("Homer")).Returns(false); // Set to check overload works!
            proxy.Setup(p => p.Save("Homer", It.IsAny<string>())).Returns(true);

            var result = Subject.SaveWithRole("Homer", "Manager");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReturnsTrueForMatchingParameterWithMultiSetups()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Save("Marge")).Returns(true);
            proxy.Setup(p => p.Save("Homer")).Returns(true);

            var result = Subject.Save("Homer");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UnmatchedReturnsDefaultValue()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Save("Homer")).Returns(true);

            Assert.IsFalse(Subject.Save("Marge"));
        }

        [TestMethod]
        public void MatchThrowsException()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Save("Bart")).Throws<InvalidOperationException>();

            Assert.ThrowsException<InvalidOperationException>(() => Subject.Save("Bart"));
        }

        [TestMethod]
        public void VerifyMatchesExpectedSingleVisit()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Save("Bart")).Returns(true);

            Subject.Save("Bart");

            proxy.Verify(p => p.Save("Bart"), Occurred.Once());
        }

        [TestMethod]
        public void VerifyMatchesExpectedZeroVisits()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Save("Bart")).Returns(true);
            proxy.Setup(p => p.Save("Marge")).Returns(false);

            Subject.Save("Bart");

            proxy.Verify(p => p.Save("Marge"), Occurred.Never());
        }

        [TestMethod]
        public void VerifyMatchesExpectedAtLeastVisits()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Save("Bart")).Returns(true);

            Subject.Save("Bart");
            Subject.Save("Bart");
            Subject.Save("Bart");

            proxy.Verify(p => p.Save("Bart"), Occurred.AtLeast(2));
        }

        [TestMethod]
        public async Task ReturnsTrueForMatchingParameterAsync()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.SaveAsync("Homer")).Returns(Task.Run(() => true));

            var result = await Subject.SaveAsync("Homer");

            Assert.IsTrue(result);
        }
    }
}