namespace HyperMock.Universal.Tests.Core
{
    using System;
    using System.Linq.Expressions;
    using Exceptions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Mocks;
    using ParameterMatchers;
    using Support;
    using Universal.Core;

    [TestClass]
    public class ParameterMatcherFactoryTests
    {
        private ParameterMatcherFactory _subject;
        private MockParameterMatcherInfoFactory _mockParameterMatcherInfoFactory;

        [TestInitialize]
        public void Setup()
        {
            _mockParameterMatcherInfoFactory = new MockParameterMatcherInfoFactory
            {
                ReturnValue = new ParameterMatcherInfo(typeof(GeneralMatcher))
            };
            _subject = new ParameterMatcherFactory(_mockParameterMatcherInfoFactory);
        }

        [TestMethod]
        public void Create_Always_CreatesAMatcherInfoInstanceForTheGivenExpression()
        {
            Expression<Func<int>> expression = () => 0;

            _subject.Create(expression);

            Assert.AreEqual(1, _mockParameterMatcherInfoFactory.CallCount);
        }

        [TestMethod]
        public void Create_MatcherTypeIsExactMatcherAndExpressionIsAMemberExpression_ReturnsAnInstanceOfTheMatcherWithTheCompiledLambda()
        {
            var returnValue = Guid.NewGuid();
            Expression<Func<Guid>> expression = () => returnValue;
            SetupExactMatcher();

            var matcher = _subject.Create(expression);

            Assert.IsInstanceOfType(matcher, typeof(ExactMatcher));
            Assert.AreEqual(returnValue, ((ExactMatcher)matcher).Value);
        }

        [TestMethod]
        public void Create_MatcherTypeIsExactMatcherAndExpressionIsAConstantExpression_ReturnsAnInstanceOfTheMatcherWithTheConstant()
        {
            Expression<Func<string>> expression = () => "123";
            SetupExactMatcher();

            var matcher = _subject.Create(expression);

            Assert.IsInstanceOfType(matcher, typeof(ExactMatcher));
            Assert.AreEqual("123", ((ExactMatcher)matcher).Value);
        }

        [TestMethod]
        public void Create_MatcherTypeIsExactMatcherAndExpressionIsAMemberCallExpression_ReturnsAnInstanceOfTheMatcherWithTheResult()
        {
            Expression<Func<int>> expression = () => "123".Length;
            SetupExactMatcher();

            var matcher = _subject.Create(expression);

            Assert.IsInstanceOfType(matcher, typeof(ExactMatcher));
            Assert.AreEqual(3, ((ExactMatcher)matcher).Value);
        }

        [TestMethod]
        public void Create_MatcherTypeIsExactMatcherAndIsAMethodCallExpression_ReturnsAnInstanceOfTheMatcherWithTheResult()
        {
            var guid = Guid.NewGuid();
            Expression<Func<string>> expression = () => guid.ToString();
            SetupExactMatcher();

            var matcher = _subject.Create(expression);

            Assert.IsInstanceOfType(matcher, typeof(ExactMatcher));
            Assert.AreEqual(guid.ToString(), ((ExactMatcher)matcher).Value);
        }

        [TestMethod]
        public void Create_MatcherTypeIsNotExactMatcherAndHasNoConstructorArguments_ReturnsAParameterMatcherWithTheCorrectMatcher()
        {
            _mockParameterMatcherInfoFactory.ReturnValue = new ParameterMatcherInfo(typeof(GeneralMatcher), new Expression[0]);
            Expression<Func<int>> argumentExpression = () => 0;

            var matcher = _subject.Create(argumentExpression);

            Assert.IsInstanceOfType(matcher, typeof(GeneralMatcher));
        }

        [TestMethod]
        public void Create_MatcherIsNotExactMatcherAndExpressionHasAnArgument_ReturnsAnInstanceOfTheParameterMatcherAndTheGivenArgument()
        {
            Expression<Func<string>> expression = () => TestSyntax.StringMatcher("someString");
            _mockParameterMatcherInfoFactory.ReturnValue = new ParameterMatcherInfo(
                typeof(StringMatcher),
                new Expression[] { Expression.Constant("someString") });

            var matcher = _subject.Create(expression);

            Assert.IsInstanceOfType(matcher, typeof(StringMatcher));
            Assert.AreEqual("someString", ((StringMatcher)matcher).CtorParameter);
        }

        [TestMethod]
        public void Create_MatcherTypeIsExactMatcherAndExpressionIsAParameterExpression_ThrowsException()
        {
            Expression<Func<UserController, string>> expression = p1 => p1.GetHelp();
            SetupExactMatcher();

            Assert.ThrowsException<UnsupportedArgumentExpressionException>(
                () => _subject.Create(expression),
                "Unsupported argument expression:: \"p1.GetHelp()\"");
        }

        [TestMethod]
        public void Create_MatcherTypeDoesNotInheritFromParameterMatcher_ThrowsException()
        {
            Expression<Func<UserController, string>> expression = p1 => p1.GetHelp();
            _mockParameterMatcherInfoFactory.ReturnValue = new ParameterMatcherInfo(typeof(object), new Expression[0]);

            Assert.ThrowsException<InvalidParameterMatcherException>(() => _subject.Create(expression));
        }

        private void SetupExactMatcher()
        {
            _mockParameterMatcherInfoFactory.ReturnValue = new ParameterMatcherInfo(typeof(ExactMatcher), new Expression[0]);
        }
    }
}