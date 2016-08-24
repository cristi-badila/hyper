namespace HyperMock.Universal.Tests.Core
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Universal.Core;

    [TestClass]
    public class MethodCallInfoFactoryTests
    {
        private MethodCallInfoFactory _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new MethodCallInfoFactory();
        }

        [TestMethod]
        public void Create_ExpressionIsAMethodCallExpression_ReturnsAPropertMethodCallInfo()
        {
            Expression<Func<string>> expression = () => 1.ToString("D");

            var methodCallInfo = _subject.Create(expression);

            Assert.AreEqual("ToString", methodCallInfo.Name);
            Assert.AreEqual(1, methodCallInfo.Arguments.Count);
            Assert.IsInstanceOfType(methodCallInfo.Arguments.First(), typeof(ConstantExpression));
        }

        [TestMethod]
        public void Create_ExpressionIsNotAMethodCallExpression_ThrowsException()
        {
            Expression<Func<int>> expression = () => 1 + 3;

            Assert.ThrowsException<ArgumentException>(() => _subject.Create(expression));
        }
    }
}