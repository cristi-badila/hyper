namespace HyperMock.Universal.Tests.ExtensionMethods
{
    using System;
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
    }
}