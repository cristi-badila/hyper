namespace HyperMock.Universal.Tests.Core
{
    using System;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Universal.Core;

    [TestClass]
    public class ExpressionInfoTests
    {
        [TestMethod]
        public void Constructor_WithoutParams_HasNonEmptyArguments()
        {
            var expressionInfo = new ExpressionInfo();

            Assert.IsNotNull(expressionInfo.Arguments);
        }

        [TestMethod]
        public void Constructor_WithNameAndNotNullArguments_SetsTheGivenValuesOnItself()
        {
            Expression<Func<string, int>> tesExpression = @string => 42;
            var arguments = new Expression[] { tesExpression };

            var expressionInfo = new ExpressionInfo("testName", arguments);

            Assert.AreEqual("testName", expressionInfo.Name);
            CollectionAssert.AreEquivalent(arguments, expressionInfo.Arguments);
        }

        [TestMethod]
        public void Constructor_WithNameAndNullArguments_SetsTheGivenNameOnItselfAndHasNonNullArguments()
        {
            var expressionInfo = new ExpressionInfo("testName", null);

            Assert.AreEqual("testName", expressionInfo.Name);
            Assert.IsNotNull(expressionInfo.Arguments);
        }
    }
}