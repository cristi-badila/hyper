namespace HyperMock.Universal.Tests.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Support;
    using Syntax;

    [TestClass]
    public class SetupTests : TestBase<UserController>
    {
        [TestMethod]
        public void SetupWithExactMatcher_CalledWithExpectedParameter_ReturnsGivenValue()
        {
            MockFor<IUserService>().Setup(p => p.Save("Homer")).Returns(true);

            var result = Subject.Save("Homer");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SetupWithFuzzyParameterMatcher_CalledWithParameterMatchingExpectation_ReturnsGivenValue()
        {
            MockFor<IUserService>().Setup(p => p.Save(It.Is<string>(name => name.EndsWith("r")))).Returns(true);

            var result = Subject.Save("Homer");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Setup_CalledWithUnmatchedArgument_ReturnsDefaultValue()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Save("Homer")).Returns(true);

            Assert.IsFalse(Subject.Save("Marge"));
        }

        [TestMethod]
        public void SetupWithException_CalledWithMatchedArgument_ThrowsGivenException()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Save("Bart")).Throws<InvalidOperationException>();

            Assert.ThrowsException<InvalidOperationException>(() => Subject.Save("Bart"));
        }

        [TestMethod]
        public async Task SetupForAsyncMethod_ArgumentMatches_ReturnsGivenValue()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.SaveAsync("Homer")).Returns(Task.Run(() => true));

            var result = await Subject.SaveAsync("Homer");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SetupWithCollectionMatcher_CalledWithMatchingParameter_ReturnsGivenValue()
        {
            var mockUserService = MockFor<IUserService>();
            var expectedList = new List<string> { "NameB", "NameC", "NameA" };
            mockUserService.Setup(p => p.ToggleEnabled(Collection.IsEquivalentTo(expectedList))).Returns(true);

            var actualList = new List<string> { "NameA", "NameB", "NameC" };
            Assert.IsTrue(mockUserService.Object.ToggleEnabled(actualList));
        }

        [TestMethod]
        public void SetupWithCollectionMatcher_CalledWithNonMatchingParameter_ReturnsDefaultValue()
        {
            var mockUserService = MockFor<IUserService>();
            var expectedList = new List<string> { "NameB", "NameC", "NameA" };
            mockUserService.Setup(p => p.ToggleEnabled(Collection.IsEquivalentTo(expectedList))).Returns(true);

            var actualList = new List<string> { "NameA", "NameB", "t" };
            Assert.IsFalse(mockUserService.Object.ToggleEnabled(actualList));
        }

        [TestMethod]
        public void SetupWithProperty_Always_ReturnsGivenValue()
        {
            MockFor<IUserService>().SetupGet(p => p.Help).Returns("Some help");

            var result = Subject.GetHelp();

            Assert.AreEqual("Some help", result);
        }

        [TestMethod]
        public void MultipleSetupsForOverloadedMethod_CalledWithMatchingParameter_ReturnsCorrectValue()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Save("Homer")).Returns(false);

            // save on the next line is the overloaded variant of the tested save method
            proxy.Setup(p => p.Save("Homer", It.IsAny<string>())).Returns(true);

            var result = Subject.Save("Homer");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MultipleSetupsWithSameMatchers_CalledWithMatchingParameter_ReturnsTheLastConfiguredValue()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Save("Homer")).Returns(false);
            proxy.Setup(p => p.Save("Homer")).Returns(true);

            var result = Subject.Save("Homer");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MultipleSetupsWithDifferentMatchers_CalledWithMatchingParameter_ReturnsValueForSetupWhichItMatched()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Save("Homer")).Returns(false);
            proxy.Setup(p => p.Save("Test")).Returns(true);

            Assert.IsFalse(Subject.Save("Homer"));
            Assert.IsTrue(Subject.Save("Test"));
        }

        [TestMethod]
        public void MultipleSetupsWithDifferentMatchers_CalledWithParameterThatMatchesMultipleSetups_ReturnsTheValueForTheLastMatchingSetup()
        {
            var proxy = MockFor<IUserService>();
            proxy.Setup(p => p.Save(It.IsAny<string>())).Returns(false);
            proxy.Setup(p => p.Save("Test")).Returns(true);

            // the actual call should not match this setup and as such ignore it
            proxy.Setup(p => p.Save(It.Is<string>(value => value != "Test"))).Returns(false);

            var result = Subject.Save("Test");

            Assert.IsTrue(result);
        }
    }
}