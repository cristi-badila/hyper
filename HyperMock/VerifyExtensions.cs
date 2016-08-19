namespace HyperMock.Universal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Exceptions;
    using ExtensionMethods;
    using ParameterMatchers;

    /// <summary>
    ///     Set of extensions for verifying behaviours have occurred.
    /// </summary>
    public static class VerifyExtensions
    {
        /// <summary>
        ///     Verifies a method matching the expression occurred the correct number of times.
        /// </summary>
        /// <typeparam name="TMock">Mocked type</typeparam>
        /// <param name="mock">Mocked instance</param>
        /// <param name="expression">Expression</param>
        /// <param name="occurred">Expected occurrence</param>
        public static void Verify<TMock>(
            this Mock<TMock> mock, Expression<Action<TMock>> expression, Occurred occurred)
            where TMock : class
        {
            mock.Verify<TMock, Action<TMock>>(expression, occurred);
        }

        /// <summary>
        ///     Verifies a function matching the expression occurred the correct number of times.
        /// </summary>
        /// <typeparam name="TMock">Mocked type</typeparam>
        /// <typeparam name="TReturn">Mocked expression return type</typeparam>
        /// <param name="mock">Mocked instance</param>
        /// <param name="expression">Expression</param>
        /// <param name="occurred">Expected occurrence</param>
        public static void Verify<TMock, TReturn>(
            this Mock<TMock> mock, Expression<Func<TMock, TReturn>> expression, Occurred occurred)
            where TMock : class
        {
            mock.Verify<TMock, Func<TMock, TReturn>>(expression, occurred);
        }

        /// <summary>
        ///     Verifies a read property matching the expression returns the expected value.
        /// </summary>
        /// <typeparam name="TMock">Mocked type</typeparam>
        /// <typeparam name="TReturn">Mocked expression return type</typeparam>
        /// <param name="mock">Mocked instance</param>
        /// <param name="expression">Expression</param>
        /// <param name="occurred">Occurrence matcher</param>
        public static void VerifyGet<TMock, TReturn>(
            this Mock<TMock> mock, Expression<Func<TMock, TReturn>> expression, Occurred occurred)
            where TMock : class
        {
            var expressionInfo = expression.GetExpressionInfoForGet();
            if (expressionInfo == null)
            {
                throw new UnknownExpressionException(expression);
            }

            mock.AssertCallOccurance(occurred, expressionInfo.Name, new ParameterMatchersCollection());
        }

        /// <summary>
        ///     Verifies a write property matching the expression sets the expected value.
        /// </summary>
        /// <typeparam name="TMock">Mocked type</typeparam>
        /// <typeparam name="TReturn">Mocked expression return type</typeparam>
        /// <param name="mock">Mocked instance</param>
        /// <param name="expression">Expression</param>
        /// <param name="expectedValue">Expected set value</param>
        /// <param name="occurred">Expected occurrence count</param>
        public static void VerifySet<TMock, TReturn>(
            this Mock<TMock> mock, Expression<Func<TMock, TReturn>> expression, TReturn expectedValue, Occurred occurred = null)
            where TMock : class
        {
            occurred = occurred ?? Occurred.AtLeast(1);
            var expressionInfo = expression.GetExpressionInfoForSet();
            if (expressionInfo == null)
            {
                throw new UnknownExpressionException(expression);
            }

            var parameterMatchers = new ParameterMatchersCollection
            {
                new ExactMatcher(expectedValue)
            };
            mock.AssertCallOccurance(occurred, expressionInfo.Name, parameterMatchers);
        }

        public static void Verify<TMock, TLambda>(this Mock<TMock> mock, Expression<TLambda> expression, Occurred occurred)
            where TMock : class
        {
            var expressionInfo = expression.GetExpressionInfoForMethod();
            if (expressionInfo == null)
            {
                throw new UnknownExpressionException(expression);
            }

            var parameters = new ParameterMatchersCollection(expressionInfo.Arguments
                    .Select(argument => Expression.Lambda(argument, expression.Parameters))
                    .Select(LambdaExtensionMethods.GetParameterMatcher));
            mock.AssertCallOccurance(occurred, expressionInfo.Name, parameters);
        }

        public static IEnumerable<CallRecording> FindCalls(
            this MockProxyDispatcher mockProxyDispatcher,
            string memberName,
            ParameterMatchersCollection parameterMatchers)
        {
            var filteredCalls = mockProxyDispatcher.RecordedCalls.Where(ci => ci.MemberName == memberName);
            if (parameterMatchers.Any())
            {
                filteredCalls = filteredCalls.Where(callRecording => parameterMatchers.Match(callRecording.Arguments));
            }

            return filteredCalls;
        }

        private static void AssertCallOccurance(this IMock mock, Occurred occurred, string memberName, ParameterMatchersCollection parameterMatchers)
        {
            occurred.Assert(mock.Dispatcher.FindCalls(memberName, parameterMatchers).Count());
        }
    }
}