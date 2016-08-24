namespace HyperMock.Universal.Tests.Core
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Support;
    using Universal.Core;

    [TestClass]
    public class ParameterMatchersListTests
    {
        private ParameterMatchersList _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new ParameterMatchersList();
        }

        [TestMethod]
        public void Match_GivenListOfParamsHasMoreItemsThenTheMatchers_ReturnsFalse()
        {
            Assert.IsFalse(_subject.Match(new List<object> { new object() }));
        }

        [TestMethod]
        public void Match_GivenListOfParamsHasLessItemsThenTheMatchers_ReturnsFalse()
        {
            _subject = new ParameterMatchersList(new ParameterMatcher[] { new GeneralMatcher() });

            Assert.IsFalse(_subject.Match(new List<object>()));
        }

        [TestMethod]
        public void Match_GivenListOfParamsHasSameCountOfItemsAsTheMatchersButMatchersDoNotMatch_ReturnsFalse()
        {
            _subject = new ParameterMatchersList(new ParameterMatcher[] { new StringMatcher("test") });

            Assert.IsFalse(_subject.Match(new List<object> { "test1" }));
        }

        [TestMethod]
        public void Match_GivenListOfParamsHasSameCountOfItemsAsTheMatchersAndMatchersMatch_ReturnsTrue()
        {
            _subject = new ParameterMatchersList(new ParameterMatcher[]
            {
                new StringMatcher("test"),
                new IntMatcher(3)
            });

            Assert.IsTrue(_subject.Match(new List<object> { "test", 3 }));
        }

        [TestMethod]
        public void Match_GivenListOfParamsHasNoItemsAndNoMatchersAreGiven_ReturnsTrue()
        {
            Assert.IsTrue(_subject.Match(new List<object>()));
        }

    }
}