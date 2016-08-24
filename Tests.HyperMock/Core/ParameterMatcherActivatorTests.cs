namespace HyperMock.Universal.Tests.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using ParameterMatchers;
    using Support;
    using Universal.Core;

    [TestClass]
    public class ParameterMatcherActivatorTests
    {
        [TestMethod]
        public void CreateInstance_NoMatcherCtorArgumentsAreGiven_ReturnsAnInstanceOfTheMatcher()
        {
            var matcher = ParameterMatcherFactory.CreateInstance(typeof(AnyMatcher), new List<Expression>());

            Assert.IsInstanceOfType(matcher, typeof(AnyMatcher));
        }

        [TestMethod]
        public void CreateInstance_GivenCtorArgumentsContainALambdaExpression_ReturnsAnInstanceOfTheMatcherWithTheCompiledLambda()
        {
            var lambdaReturnValue = Guid.NewGuid();
            Expression<Func<Guid>> parameter = () => lambdaReturnValue;
            var matcher = ParameterMatcherFactory.CreateInstance(typeof(TestMatcher<string>), new[] { parameter });

            Assert.IsInstanceOfType(matcher, typeof(TestMatcher<string>));
            Assert.AreEqual(lambdaReturnValue, ((TestMatcher<string>)matcher).CtorParameter());
        }

        [TestMethod]
        public void CreateInstance_GivenCtorArgumentsContainAConstantExpression_ReturnsAnInstanceOfTheMatcherWithTheConstant()
        {
            var constantValue = Guid.NewGuid().ToString();
            var expression = Expression.Constant(constantValue);
            var matcher = ParameterMatcherFactory.CreateInstance(typeof(TestMatcher2), new[] { expression });

            Assert.IsInstanceOfType(matcher, typeof(TestMatcher2));
            Assert.AreEqual(constantValue, ((TestMatcher2)matcher).CtorParameter);
        }

        [TestMethod]
        public void CreateInstance_GivenCtorArgumentsContainAMemberExpression_ReturnsAnInstanceOfTheMatcherWithTheMemberGetter()
        {
            const string constantValue = "123456789";
            var propertyInfo = typeof(string).GetProperty("Length");
            var expression = Expression.MakeMemberAccess(Expression.Constant(constantValue), propertyInfo);
            var matcher = ParameterMatcherFactory.CreateInstance(typeof(TestMatcher3), new[] { expression });

            Assert.IsInstanceOfType(matcher, typeof(TestMatcher3));
            Assert.AreEqual(9, ((TestMatcher3)matcher).CtorParameter);
        }
    }
}