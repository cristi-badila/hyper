namespace HyperMock.Universal.Tests.ExtensionMethods
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Exceptions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Support;
    using Universal.ExtensionMethods;

    [TestClass]
    public class LambdaExtensionMethodsTests
    {
        [TestMethod]
        public void GetExactValueMatcher_ExpressionIsAMemberAccess_CompilesTheExpressionAndReturnsTheResultOfItsInvocation()
        {
            var value = Guid.NewGuid();
            Expression<Func<Guid>> expression = () => value;

            var exactValueMatcher = expression.GetExactValueMatcher();

            Assert.AreEqual(value, exactValueMatcher.Value);
        }

        [TestMethod]
        public void GetExactValueMatcher_ExpressionIsAConstantExpression_CompilesTheExpressionAndReturnsTheResultOfItsInvocation()
        {
            Expression<Func<string>> expression = () => "123";

            var exactValueMatcher = expression.GetExactValueMatcher();

            Assert.AreEqual("123", exactValueMatcher.Value);
        }

        [TestMethod]
        public void GetExactValueMatcher_ExpressionIsAMethodCallExpression_CompilesTheExpressionAndReturnsTheResultOfItsInvocation()
        {
            var value = Guid.NewGuid();
            Expression<Func<string>> expression = () => value.ToString();

            var exactValueMatcher = expression.GetExactValueMatcher();

            Assert.AreEqual(value.ToString(), exactValueMatcher.Value);
        }

        [TestMethod]
        public void GetExactValueMatcher_ExpressionIsAParameterExpression_ThrowsAnInvalidParameterExpressionException()
        {
            Expression<Func<UserController, string>> expression = p1 => p1.ToString();

            Assert.ThrowsException<InvalidParameterExpressionException>(() => expression.GetExactValueMatcher());
        }

        [TestMethod]
        public void GetExpressionInfoForMethod_ExpressionIsMethodCall_ReturnsCorrespondingExpressionInfo()
        {
            Expression<Func<bool>> expression = () => 1.Equals(2);

            var expressionInfo = expression.GetExpressionInfoForMethod();

            Assert.AreEqual("Equals", expressionInfo.Name);
            Assert.AreEqual(1, expressionInfo.Arguments.Count);
            Assert.AreEqual("2", expressionInfo.Arguments.First().ToString());
        }

        [TestMethod]
        public void GetExpressionInfoForMethod_ExpressionIsNotAMethodCall_ReturnsNull()
        {
            Expression<Func<bool>> expression = () => 1 == 2;

            var expressionInfo = expression.GetExpressionInfoForMethod();

            Assert.IsNull(expressionInfo);
        }

        [TestMethod]
        public void GetExpressionInfoForGet_ExpressionIsAPropertyAccess_ReturnsCorrespondingExpressionInfo()
        {
            const string value = "123";
            Expression<Func<int>> expression = () => value.Length;

            var expressionInfo = expression.GetExpressionInfoForGet();

            Assert.AreEqual("get_Length", expressionInfo.Name);
            Assert.AreEqual(0, expressionInfo.Arguments.Count);
        }

        [TestMethod]
        public void GetExpressionInfoForGet_ExpressionIsNotAPropertyAccess_ReturnsNull()
        {
            const string value = "123";
            Expression<Func<string>> expression = () => value;

            var expressionInfo = expression.GetExpressionInfoForGet();

            Assert.IsNull(expressionInfo);
        }

        [TestMethod]
        public void GetExpressionInfoForSet_ExpressionIsAPropertyAccessForAWritableProperty_ReturnsCorrespondingExpressionInfo()
        {
            var userController = new UserController(null);
            Expression<Func<int>> expression = () => userController.Version;

            var expressionInfo = expression.GetExpressionInfoForSet();

            Assert.AreEqual("set_Version", expressionInfo.Name);
            Assert.AreEqual(0, expressionInfo.Arguments.Count);
        }

        [TestMethod]
        public void GetExpressionInfoForSet_ExpressionIsAPropertyAccessForANonWritableProperty_ReturnsNull()
        {
            const string value = "123";
            Expression<Func<int>> expression = () => value.Length;

            var expressionInfo = expression.GetExpressionInfoForSet();

            Assert.IsNull(expressionInfo);
        }

        [TestMethod]
        public void GetExpressionInfoForSet_ExpressionIsNotAPropertyAccess_ReturnsNull()
        {
            const string value = "123";
            Expression<Func<string>> expression = () => value;

            var expressionInfo = expression.GetExpressionInfoForSet();

            Assert.IsNull(expressionInfo);
        }
    }
}