namespace HyperMock.Universal.Tests.IntegrationTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Support;
    using Syntax;

    [TestClass]
    public class VerifyTests : TestBase<UserController>
    {
        [TestMethod]
        public void Verify_SpyWasCalledOnce_AssertsCount()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Save("Bart")).Returns(true);

            Subject.Save("Bart");

            proxy.Verify(p => p.Save("Bart"), Occurred.Once());
        }

        [TestMethod]
        public void Verify_SpyWasNeverCalled_AssertsCount()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Save("Bart")).Returns(true);
            proxy.Setup(p => p.Save("Marge")).Returns(false);

            Subject.Save("Bart");

            proxy.Verify(p => p.Save("Bart"), Occurred.Once());
            proxy.Verify(p => p.Save("Marge"), Occurred.Never());
        }

        [TestMethod]
        public void VerifyWithAtLeastMatcher_SpyWasCalled_AssertsCount()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Save("Bart")).Returns(true);

            Subject.Save("Bart");
            Subject.Save("Bart");
            Subject.Save("Bart");

            proxy.Verify(p => p.Save("Bart"), Occurred.AtLeast(2));
        }

        [TestMethod]
        public void VerifyWithIsAnyMatcher_SpyWasCalled_AssertsCallCount()
        {
            var proxy = MockFor<IUserService>();

            Subject.Delete("Homer");

            proxy.Verify(p => p.Delete(It.IsAny<string>()), Occurred.Once());
        }

        [TestMethod]
        public void VerifyWithPartialMathcer_SpyWasCalledASingleTime_AssertsCallCount()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Delete(It.IsAny<string>()));

            Subject.Delete("Homer");

            proxy.Verify(p => p.Delete(It.Is<string>(s => s == "Homer")), Occurred.Once());
        }

        [TestMethod]
        public void VerifyWithPartialMatcher_SpyWasCalledMultipleTimes_AssertsCorrectCount()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Save("Bart")).Returns(true);

            Subject.Save("Bart");
            Subject.Save("Cart");
            Subject.Save("Dart");
            Subject.Save("Frat");

            proxy.Verify(p => p.Save(It.Is<string>(name => name.EndsWith("art"))), Occurred.Exactly(3));
        }

        [TestMethod]
        public void VerifyWithCollectionMatcher_SpyWasCalledWithMatchingParameter_AssertsCallCount()
        {
            var actualList = new List<string> { "NameA", "NameB", "NameC" };
            Subject.ToggleEnabled(actualList);

            var proxy = MockFor<IUserService>();
            var expectedList = new List<string> { "NameB", "NameC", "NameA" };
            proxy.Verify(p => p.ToggleEnabled(Collection.IsEquivalentTo(expectedList)), Occurred.Once());
        }

        [TestMethod]
        public async Task Verify_SpyIsAsyncMethodAndWasCalledWithMatchingParameter_ReturnsGivenValue()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.DeleteAsync("Homer")).Returns(Task.Delay(0));

            await Subject.DeleteAsync("Homer");

            proxy.Verify(p => p.DeleteAsync("Homer"), Occurred.Once());
        }

        [TestMethod]
        public void VerifyWithReadProperty_Always_AssertsCallCount()
        {
            var proxy = MockFor<IUserService>();
            proxy.SetupGet(p => p.Help).Returns("Some help");

            Subject.GetHelp();

            proxy.VerifyGet(p => p.Help, Occurred.Once());
        }

        [TestMethod]
        public void VerifyWithWritePropertyAndNoOccurrenceMatcher_PropertyWasSetASingleTime_AssertsPropertyWasSet()
        {
            var proxy = MockFor<IUserService>();
            proxy.SetupSet(p => p.CurrentRole);

            Subject.SetCurrentRole("Manager");

            proxy.VerifySet(p => p.CurrentRole, "Manager");
        }

        [TestMethod]
        public void VerifyWithWritePropertyAndNoOccurrenceMatcher_PropertyWasNotSet_AssertsPropertyWasNotSet()
        {
            var proxy = MockFor<IUserService>();
            proxy.SetupSet(p => p.CurrentRole);

            Subject.SetCurrentRole("Manager");

            proxy.VerifySet(p => p.CurrentRole, "Test", Occurred.Never());
        }

        [TestMethod]
        public void VerifyWithWriteProperty_PropertyWasSetMultipleTimes_AssertsCount()
        {
            var proxy = MockFor<IUserService>();
            proxy.SetupSet(p => p.CurrentRole);

            Subject.SetCurrentRole("Manager");
            Subject.SetCurrentRole("Test");
            Subject.SetCurrentRole("Manager");

            proxy.VerifySet(p => p.CurrentRole, "Manager", Occurred.Exactly(2));
        }
    }
}
