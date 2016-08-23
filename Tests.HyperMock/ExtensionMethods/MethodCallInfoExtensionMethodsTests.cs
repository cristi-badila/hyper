namespace HyperMock.Universal.Tests.ExtensionMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Exceptions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using ParameterMatchers;
    using Support;
    using Syntax;
    using Universal.Core;
    using Universal.ExtensionMethods;

    [TestClass]
    public class MethodCallInfoExtensionMethodsTests
    {
        [TestMethod]
        public void GetParameterMatchers_ExpressionInfoHasNoArguments_ReturnsAnEmptyParameterMatchersList()
        {
            var methodCallInfo = new MethodCallInfo();

            var parameterMatchers = methodCallInfo.GetParameterMatchers(new List<ParameterExpression>()).ToList();

            Assert.AreEqual(0, parameterMatchers.Count);
        }

        [TestMethod]
        public void GetParameterMatchers_ExpressionInfoHasOneArgument_ReturnsAParameterMatchersListWithTheParameterMatcher()
        {
            Expression<Func<bool>> expression = () => Equals(It.IsAny());
            var methodCallExpression = (MethodCallExpression)expression.Body;
            var argumentExpression = methodCallExpression.Arguments.First();
            var methodCallInfo = new MethodCallInfo(string.Empty, new[] { argumentExpression });

            var parameterMatchers = methodCallInfo.GetParameterMatchers(expression.Parameters).ToList();

            Assert.AreEqual(1, parameterMatchers.Count);
            Assert.IsInstanceOfType(parameterMatchers.First(), typeof(AnyMatcher));
        }

        [TestMethod]
        public void GetParameterMatchers_ExpressionInfoGenericArgumentMatcher_ReturnsAParameterMatchersListWithTheParameterMatchers()
        {
            Expression<Func<bool>> expression = () => Equals(It.IsAny<Guid>());
            var methodCallExpression = (MethodCallExpression)expression.Body;
            var argumentExpression = methodCallExpression.Arguments.First();
            var methodCallInfo = new MethodCallInfo(string.Empty, new[] { argumentExpression });

            var parameterMatchers = methodCallInfo.GetParameterMatchers(expression.Parameters).ToList();

            Assert.AreEqual(1, parameterMatchers.Count);
            Assert.IsInstanceOfType(parameterMatchers.First(), typeof(AnyMatcher<Guid>));
        }

        [TestMethod]
        public void GetParameterMatchers_ExpressionInfoHasAFieldArgument_ReturnsAParameterMatchersListWithTheParameterMatchers()
        {
            var fieldArgument = Guid.NewGuid();

            Expression<Func<bool>> expression = () => Equals(fieldArgument);
            var methodCallExpression = (MethodCallExpression)expression.Body;
            var argumentExpression = methodCallExpression.Arguments.First();
            var methodCallInfo = new MethodCallInfo(string.Empty, new[] { argumentExpression });

            var parameterMatchers = methodCallInfo.GetParameterMatchers(expression.Parameters).ToList();

            Assert.AreEqual(1, parameterMatchers.Count);
            var parameterMatcher = parameterMatchers.First() as ExactMatcher;
            Assert.IsNotNull(parameterMatcher);
            Assert.AreEqual(fieldArgument, parameterMatcher.Value);
        }

        [TestMethod]
        public void GetParameterMatchers_ExpressionInfoHasAMemberAccessArgument_ReturnsAParameterMatchersListWithTheParameterMatchers()
        {
            var fieldArgument = new DateTime(1, 1, 1, 0, 0, 1, DateTimeKind.Utc);

            Expression<Func<bool>> expression = () => Equals(fieldArgument.Ticks);
            var methodCallExpression = (MethodCallExpression)expression.Body;
            var argumentExpression = methodCallExpression.Arguments.First();
            var methodCallInfo = new MethodCallInfo(string.Empty, new[] { argumentExpression });

            var parameterMatchers = methodCallInfo.GetParameterMatchers(expression.Parameters).ToList();

            Assert.AreEqual(1, parameterMatchers.Count);
            var parameterMatcher = parameterMatchers.First() as ExactMatcher;
            Assert.IsNotNull(parameterMatcher);
            Assert.AreEqual(10000000L, parameterMatcher.Value);
        }

        [TestMethod]
        public void GetParameterMatchers_ExpressionInfoHasArgumentWhichIsAMethodCallWihoutParameterMatcher_ReturnsCorrectMatchers()
        {
            var guid = Guid.NewGuid();

            Expression<Func<bool>> expression = () => Equals(guid.ToString());
            var methodCallExpression = (MethodCallExpression)expression.Body;
            var argumentExpression = methodCallExpression.Arguments.First();
            var methodCallInfo = new MethodCallInfo(string.Empty, new[] { argumentExpression });

            var parameterMatchers = methodCallInfo.GetParameterMatchers(expression.Parameters).ToList();

            Assert.AreEqual(1, parameterMatchers.Count);
            var parameterMatcher = parameterMatchers.First() as ExactMatcher;
            Assert.IsNotNull(parameterMatcher);
            Assert.AreEqual(guid.ToString(), parameterMatcher.Value);
        }

        [TestMethod]
        public void GetParameterMatchers_ExpressionInfoHasArgumentWhichDependsOnParameter_ThrowsException()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            Expression<Func<UserController, bool>> expression = p1 => p1.Equals(p1.GetHelp());
            var methodCallExpression = (MethodCallExpression)expression.Body;
            var argumentExpression = methodCallExpression.Arguments.First();
            var methodCallInfo = new MethodCallInfo(string.Empty, new[] { argumentExpression });

            Assert.ThrowsException<InvalidParameterExpressionException>(
                () => methodCallInfo.GetParameterMatchers(expression.Parameters).ToList(),
                "A parameter expression was encountered which could not be evaluated: \"p1.GetHelp()\"");
        }

        [TestMethod]
        public void GetParameterMatchers_ExpressionInfoHasMoreThenOneArgument_ReturnsAParameterMatchersListWithTheParameterMatchers()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            Expression<Func<Guid, string, bool>> expression = (param1, param2) => Equals(It.IsAny(), It.Is<string>(_ => false));
            var methodCallExpression = (MethodCallExpression)expression.Body;
            var argumentExpression1 = methodCallExpression.Arguments.First();
            var argumentExpression2 = methodCallExpression.Arguments.Last();
            var methodCallInfo = new MethodCallInfo(string.Empty, new[] { argumentExpression1, argumentExpression2 });

            var parameterMatchers = methodCallInfo.GetParameterMatchers(expression.Parameters).ToList();

            Assert.AreEqual(2, parameterMatchers.Count);
            Assert.IsInstanceOfType(parameterMatchers.First(), typeof(AnyMatcher));
            Assert.IsInstanceOfType(parameterMatchers.Last(), typeof(PartialMatcher<string>));
        }
    }
}
