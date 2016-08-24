namespace HyperMock.Universal.Tests.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Exceptions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Mocks;
    using Support;
    using Universal.Core;

    [TestClass]
    public class CallDescriptorFactoryTests
    {
        private CallDescriptorFactory _subject;
        private MockMethodCallInfoFactory _mockMethodCallInfoFactory;
        private Expression<Func<int>> _sampleExpression;
        private MockParameterMatcherFactory _mockParameterMatcherFactory;

        [TestInitialize]
        public void Setup()
        {
            _mockMethodCallInfoFactory = new MockMethodCallInfoFactory
            {
                ReturnValue = new MethodCallInfo(string.Empty)
            };
            _mockParameterMatcherFactory = new MockParameterMatcherFactory();
            _subject = new CallDescriptorFactory(_mockMethodCallInfoFactory, _mockParameterMatcherFactory);
            _sampleExpression = () => 1;
        }

        [TestMethod]
        public void Create_Always_TriesToGetAMethodCallInfoForTheExpression()
        {
            _subject.Create(_sampleExpression);

            Assert.AreEqual(1, _mockMethodCallInfoFactory.CallCount);
        }

        [TestMethod]
        public void Create_CannotGetAMethodCallInfoForTheExpression_ThrowsException()
        {
            _mockMethodCallInfoFactory.ExceptionToThrow = new ArgumentException();

            Assert.ThrowsException<UnknownCallExpressionException>(() => _subject.Create(_sampleExpression));
        }

        [TestMethod]
        public void Create_CanGetAMethodCallInfoForTheExpression_CreatesAParmeterMatcherForEachMethodCallArgument()
        {
            Expression<Func<int>> expression1 = () => "123".Length;
            Expression<Func<int>> expression2 = () => "1234".Length;
            var argumentExpressions = new List<Expression> { expression1, expression2 };
            _mockMethodCallInfoFactory.ReturnValue = new MethodCallInfo(string.Empty, argumentExpressions);

            _subject.Create(_sampleExpression);

            Assert.AreEqual(2, _mockParameterMatcherFactory.CallCount);
        }

        [TestMethod]
        public void Create_CanGetAMethodCallInfoForTheExpressionButCannotCreateParameterMatchers_ThrowsException()
        {
            Expression<Func<int>> expression = () => "123".Length;
            var argumentExpressions = new List<Expression> { expression };
            _mockMethodCallInfoFactory.ReturnValue = new MethodCallInfo(string.Empty, argumentExpressions);
            _mockParameterMatcherFactory.ExceptionToThrow = new UnsupportedArgumentExpressionException(expression);

            Assert.ThrowsException<UnknownCallExpressionException>(() => _subject.Create(_sampleExpression));
        }

        [TestMethod]
        public void Create_CanGetAMethodCallInfoForTheExpressionAndCanCreateParameterMatchers_ReturnsTheDescriptor()
        {
            Expression<Func<int>> expression = () => "123".Length;
            var argumentExpressions = new List<Expression> { expression };
            _mockMethodCallInfoFactory.ReturnValue = new MethodCallInfo("someMethod", argumentExpressions);
            var parameterMatcher = new TestMatcher2(string.Empty);
            _mockParameterMatcherFactory.ReturnValue = parameterMatcher;

            var callDescriptor = _subject.Create(_sampleExpression);

            Assert.AreEqual("someMethod", callDescriptor.MemberName);
            Assert.AreEqual(1, callDescriptor.ParameterMatchers.Count);
            Assert.AreSame(parameterMatcher, callDescriptor.ParameterMatchers.First());
        }
    }
}