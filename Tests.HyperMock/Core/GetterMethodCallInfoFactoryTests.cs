namespace HyperMock.Universal.Tests.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Support;
    using Universal.Core;

    [TestClass]
    public class GetterMethodCallInfoFactoryTests
    {
        private GetterMethodCallInfoFactory _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new GetterMethodCallInfoFactory();
        }

        [TestMethod]
        public void Create_ExpressionIsAMemberAccessExpressionToAPropertyWithAGetter_ReturnsAMethodCallInfo()
        {
            Expression<Func<int>> expression = () => "123".Length;

            var methodCallInfo = _subject.Create(expression);

            Assert.AreEqual("get_Length", methodCallInfo.Name);
        }

        [TestMethod]
        public void Create_ExpressionIsNotAMemberAccessExpression_ThrowsAnException()
        {
            Expression<Func<int>> expression = () => 1 + 2;

            Assert.ThrowsException<ArgumentException>(() => _subject.Create(expression));
        }

        [TestMethod]
        public void Create_ExpressionIsAMemberAccessExpressionToAPropertyWithoutAGetter_ReturnsAMethodCallInfo()
        {
            var userController = new UserController(null);
            var propertyInfo = userController.GetType().GetProperty("UserKey");
            var memeberAccessExpression = Expression.MakeMemberAccess(Expression.Constant(userController), propertyInfo);
            var expression = Expression.Lambda(memeberAccessExpression, new List<ParameterExpression>());

            Assert.ThrowsException<ArgumentException>(() => _subject.Create(expression));
        }
    }
}
