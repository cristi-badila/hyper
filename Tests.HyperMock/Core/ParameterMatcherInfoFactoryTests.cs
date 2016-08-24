namespace HyperMock.Universal.Tests.Core
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using ParameterMatchers;
    using Support;
    using Universal.Core;

    [TestClass]
    public class ParameterMatcherInfoFactoryTests
    {
        private ParameterMatcherInfoFactory _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new ParameterMatcherInfoFactory();
        }

        [TestMethod]
        public void Create_ArgumentIsAMethodCallExpressionWhichHasAParameterMatcherAttribute_ReturnsCorrectParameterMatcherInfo()
        {
            Expression<Func<bool>> expression = () => Equals(TestSyntax.GeneralMatcher());
            var methodCallExpression = (MethodCallExpression)expression.Body;
            var argumentExpression = methodCallExpression.Arguments.First();

            var parameterMatchers = _subject.Create(argumentExpression, expression);

            Assert.AreSame(parameterMatchers.MatcherType, typeof(GeneralMatcher));
            Assert.AreEqual(0, parameterMatchers.CtorArguments.Count);
        }

        [TestMethod]
        public void Create_ArgumentIsAGenericMethodCallExpressionWhichHasAParameterMatcherAttribute_ReturnsCorrectParameterMatcherInfo()
        {
            Expression<Func<bool>> expression = () => Equals(TestSyntax.GenericMatcher<bool>());
            var methodCallExpression = (MethodCallExpression)expression.Body;
            var argumentExpression = methodCallExpression.Arguments.First();

            var parameterMatchers = _subject.Create(argumentExpression, expression);

            Assert.AreSame(parameterMatchers.MatcherType, typeof(GenericMatcher<bool>));
            Assert.AreEqual(0, parameterMatchers.CtorArguments.Count);
        }

        [TestMethod]
        public void Create_ArgumentIsAMethodCallExpressionWhichHasAParameterMatcherAttributeWithCtorArguments_ReturnsCorrectParameterMatcherInfo()
        {
            const string testString = "1234567";
            Expression<Func<bool>> expression = () => Equals(TestSyntax.IntMatcher(testString.Length));
            var methodCallExpression = (MethodCallExpression)expression.Body;
            var argumentExpression = methodCallExpression.Arguments.First();

            var parameterMatchers = _subject.Create(argumentExpression, expression);

            Assert.AreSame(parameterMatchers.MatcherType, typeof(IntMatcher));
            Assert.AreEqual(1, parameterMatchers.CtorArguments.Count);
            Assert.AreEqual("\"1234567\".Length", parameterMatchers.CtorArguments.First().ToString());
        }

        [TestMethod]
        public void Create_ArgumentIsAFieldAccessExpression_ReturnsExcatMatcherAsTheMatcherInfo()
        {
            var fieldArgument = Guid.NewGuid();
            Expression<Func<bool>> expression = () => Equals(fieldArgument);
            var methodCallExpression = (MethodCallExpression)expression.Body;
            var argumentExpression = methodCallExpression.Arguments.First();

            var parameterMatchers = _subject.Create(argumentExpression, expression);

            Assert.AreSame(parameterMatchers.MatcherType, typeof(ExactMatcher));
            Assert.AreEqual(0, parameterMatchers.CtorArguments.Count);
        }

        [TestMethod]
        public void Create_ArgumentIsIsAMemberAccessExpression_ReturnsExcatMatcherAsTheMatcherInfo()
        {
            var fieldArgument = new DateTime(1, 1, 1, 0, 0, 1, DateTimeKind.Utc);
            Expression<Func<bool>> expression = () => Equals(fieldArgument.Ticks);
            var methodCallExpression = (MethodCallExpression)expression.Body;
            var argumentExpression = methodCallExpression.Arguments.First();

            var parameterMatchers = _subject.Create(argumentExpression, expression);

            Assert.AreSame(parameterMatchers.MatcherType, typeof(ExactMatcher));
            Assert.AreEqual(0, parameterMatchers.CtorArguments.Count);
        }

        [TestMethod]
        public void Create_ArgumentIsAMethodCallExpressionWihoutParameterMatcher_ReturnsExcatMatcherAsTheMatcherInfo()
        {
            var guid = Guid.NewGuid();
            Expression<Func<bool>> expression = () => Equals(guid.ToString());
            var methodCallExpression = (MethodCallExpression)expression.Body;
            var argumentExpression = methodCallExpression.Arguments.First();

            var parameterMatchers = _subject.Create(argumentExpression, expression);

            Assert.AreSame(parameterMatchers.MatcherType, typeof(ExactMatcher));
            Assert.AreEqual(0, parameterMatchers.CtorArguments.Count);
        }
    }
}
