namespace HyperMock.Universal
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Exceptions;
    using ExtensionMethods;
    using ParameterMatchers;

    /// <summary>
    ///     Set of extensions for verifying that calls have occurred.
    /// </summary>
    public static class VerifyExtensions
    {
        /// <summary>
        ///     Verifies a method matching the expression was called the given number of times.
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
        ///     Verifies a method matching the expression was called the given number of times.
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
        ///     Verifies a method matching the expression was called the given number of times.
        /// </summary>
        /// <typeparam name="TMock">Mocked type</typeparam>
        /// <typeparam name="TLambda">The type of the expression to analyze</typeparam>
        /// <param name="mock">Mocked instance</param>
        /// <param name="expression">Expression containing the method call</param>
        /// <param name="occurred">Expected occurrence</param>
        public static void Verify<TMock, TLambda>(this Mock<TMock> mock, Expression<TLambda> expression, Occurred occurred)
            where TMock : class
        {
            var expressionInfo = expression.GetExpressionInfoForMethod();
            if (expressionInfo == null)
            {
                throw new UnknownExpressionException(expression);
            }

            var parameterMatchers = expressionInfo.GetParameterMatchers(expression.Parameters);
            occurred.Assert(mock.Dispatcher.RecordedCalls.Filter(expressionInfo.Name, parameterMatchers).Count());
        }

        /// <summary>
        ///     Verifies a read property matching the expression was called a given number of times.
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

            occurred.Assert(mock.Dispatcher.RecordedCalls.Filter(expressionInfo.Name).Count());
        }

        /// <summary>
        ///     Verifies a write property matching the expression was called a given number of times
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

            var parameterMatcher = new ExactMatcher(expectedValue);
            occurred.Assert(mock.Dispatcher.RecordedCalls.Filter(expressionInfo.Name, parameterMatcher).Count());
        }
    }
}