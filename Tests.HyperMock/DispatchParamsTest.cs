namespace HyperMock.Universal.Tests
{
    using System;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class DispatchParamsTest
    {
        [TestMethod]
        public void Constructor_WithoutParams_HasNonEmptyArguments()
        {
            var dispatchParams = new DispatchParams();

            Assert.IsNotNull(dispatchParams.Arguments);
        }

        [TestMethod]
        public void Constructor_WithNameAndNotNullArguments_SetsTheGivenValuesOnItself()
        {
            Expression<Func<string, int>> tesExpression = @string => 42;
            var arguments = new Expression[] { tesExpression };

            var dispatchParams = new DispatchParams("testName", arguments);

            Assert.AreEqual("testName", dispatchParams.Name);
            CollectionAssert.AreEquivalent(arguments, dispatchParams.Arguments);
        }

        [TestMethod]
        public void Constructor_WithNameAndNullArguments_SetsTheGivenNameOnItselfAndHasNonNullArguments()
        {
            var dispatchParams = new DispatchParams("testName", null);

            Assert.AreEqual("testName", dispatchParams.Name);
            Assert.IsNotNull(dispatchParams.Arguments);
        }
    }
}