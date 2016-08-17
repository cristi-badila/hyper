namespace HyperMock.Universal.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Support;

    [TestClass]
    public class MethodTest : TestBase<UserController>
    {
        [TestMethod]
        public void VerifyMatchesExpectedSingleVisit()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Delete("Homer"));

            Subject.Delete("Homer");

            proxy.Verify(p => p.Delete("Homer"), Occurred.Once());
        }

        [TestMethod]
        public void VerifyMatchesExpectedZeroVisits()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Delete("Homer"));
            proxy.Setup(p => p.Delete("Marge"));

            Subject.Delete("Homer");

            proxy.Verify(p => p.Delete("Marge"), Occurred.Never());
        }

        [TestMethod]
        public void VerifyMatchesExpectedAtLeastVisits()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Delete("Homer"));

            Subject.Delete("Homer");
            Subject.Delete("Homer");
            Subject.Delete("Homer");

            proxy.Verify(p => p.Delete("Homer"), Occurred.AtLeast(2));
        }

        [TestMethod]
        public void VerifyMatchesSingleVisitForAnyParameter()
        {
            var proxy = MockFor<IUserService>();

            Subject.Delete("Homer");

            proxy.Verify(p => p.Delete(It.IsAny<string>()), Occurred.Once());
        }

        [TestMethod]
        public async Task VerifyMatchesExpectedSingleVisitAsync()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.DeleteAsync("Homer")).Returns(Task.Delay(0));

            await Subject.DeleteAsync("Homer");

            proxy.Verify(p => p.DeleteAsync("Homer"), Occurred.Once());
        }

        [TestMethod]
        public void VerifyMatchesSingleVisitForPartialMatchParameter()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Delete(It.IsAny<string>()));

            Subject.Delete("Homer");

            proxy.Verify(p => p.Delete(It.Is<string>(s => s == "Homer")), Occurred.Once());
        }

        [TestMethod]
        public void SetupIsAbleToHandleEquivalentCollection()
        {
            var proxy = MockFor<IUserService>();
            var expectedList = new List<string> { "NameB", "NameC", "NameA" };
            proxy.Setup(p => p.ToggleEnabled(Collection.IsEquivalentTo(expectedList))).Returns(true);

            var actualList = new List<string> { "NameA", "NameB", "NameC" };
            Assert.IsTrue(proxy.Object.ToggleEnabled(actualList));
        }

        [TestMethod]
        public void VerifyIsAbleToHandleEquivalentCollection()
        {
            var actualList = new List<string> { "NameA", "NameB", "NameC" };
            Subject.ToggleEnabled(actualList);

            var proxy = MockFor<IUserService>();
            var expectedList = new List<string> { "NameB", "NameC", "NameA" };
            proxy.Verify(p => p.ToggleEnabled(Collection.IsEquivalentTo(expectedList)), Occurred.Once());
        }
    }
}