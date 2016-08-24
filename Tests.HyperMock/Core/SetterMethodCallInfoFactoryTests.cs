namespace HyperMock.Universal.Tests.Core
{
    using System;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Support;
    using Universal.Core;

    [TestClass]
    public class SetterMethodCallInfoFactoryTests
    {
        private SetterMethodCallInfoFactory _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new SetterMethodCallInfoFactory();
        }

        [TestMethod]
        public void Create_ExpressionIsAMemberAccessExpressionToAPropertyWithASetter_ReturnsAMethodCallInfo()
        {
            var userController = new UserController(null);
            Expression<Func<int>> expression = () => userController.Version;

            var methodCallInfo = _subject.Create(expression);

            Assert.AreEqual("set_Version", methodCallInfo.Name);
        }

        [TestMethod]
        public void Create_ExpressionIsNotAMemberAccessExpression_ThrowsAnException()
        {
            Expression<Func<int>> expression = () => 1 + 2;

            Assert.ThrowsException<ArgumentException>(() => _subject.Create(expression));
        }

        [TestMethod]
        public void Create_ExpressionIsAMemberAccessExpressionToAPropertyWithoutASetter_ReturnsAMethodCallInfo()
        {
            Expression<Func<int>> expression = () => "1123".Length;

            Assert.ThrowsException<ArgumentException>(() => _subject.Create(expression));
        }
    }
}
