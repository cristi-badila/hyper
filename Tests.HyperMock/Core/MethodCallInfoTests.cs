namespace HyperMock.Universal.Tests.Core
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Universal.Core;

    [TestClass]
    public class MethodCallInfoTests
    {
        [TestMethod]
        public void Constructor_WithoutParams_HasNonEmptyArguments()
        {
            var methodCallInfo = new MethodCallInfo(string.Empty);

            Assert.IsNotNull(methodCallInfo.Arguments);
        }

        [TestMethod]
        public void Constructor_WithNameAndNotNullArguments_SetsTheGivenValuesOnItself()
        {
            Expression<Func<string, int>> tesExpression = @string => 42;
            var arguments = new Expression[] { tesExpression };

            var methodCallInfo = new MethodCallInfo("testName", arguments);

            Assert.AreEqual("testName", methodCallInfo.Name);
            Assert.IsTrue(arguments.SequenceEqual(methodCallInfo.Arguments));
        }

        [TestMethod]
        public void Constructor_WithNameAndNullArguments_SetsTheGivenNameOnItselfAndHasNonNullArguments()
        {
            var methodCallInfo = new MethodCallInfo("testName");

            Assert.AreEqual("testName", methodCallInfo.Name);
            Assert.IsNotNull(methodCallInfo.Arguments);
        }
    }
}