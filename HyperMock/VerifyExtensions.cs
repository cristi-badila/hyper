namespace HyperMock.Universal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Exceptions;
    using ExtensionMethods;

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
            var dispatchParams = expression.GetDispatchParamsForGet();
            if (dispatchParams == null)
            {
                throw new UnknownDispatchParamsException(expression);
            }

            mock.AssertCallOccurance(occurred, dispatchParams);
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
            var dispatchParams = expression.GetDispatchParamsForSet();
            if (dispatchParams == null)
            {
                throw new UnknownDispatchParamsException(expression);
            }

            mock.AssertCallOccurance(occurred, dispatchParams, new object[] { expectedValue });
        }

        public static void Verify<TMock, TLambda>(this Mock<TMock> mock, Expression<TLambda> expression, Occurred occurred)
            where TMock : class
        {
            var dispatchParams = expression.GetDispatchParamsForMethod();
            if (dispatchParams == null)
            {
                throw new UnknownDispatchParamsException(expression);
            }

            var actualParams = GetActualParams(expression, dispatchParams.Arguments);
            mock.AssertCallOccurance(occurred, dispatchParams, actualParams);
        }

        public static CallInfo FindCall(this MockProxyDispatcher mockProxyDispatcher, string name, object[] args = null)
        {
            var filteredCalls = mockProxyDispatcher.RegisteredCallInfoList.Where(ci => ci.Name == name);
            if (args != null)
            {
                filteredCalls = filteredCalls.Where(ci => ci.IsMatchFor(args));
            }

            return filteredCalls.FirstOrDefault();
        }

        private static void AssertCallOccurance(this IMock mock, Occurred occurred, DispatchParams dispatchParams, object[] actualParams = null)
        {
            var callInfo = mock.Dispatcher.FindCall(dispatchParams.Name, actualParams);
            if (callInfo == null)
            {
                throw new NoMatchingCallFoundException(dispatchParams.Name);
            }

            occurred.Assert(callInfo.Visited);
        }

        private static object[] GetActualParams<TLambda>(Expression<TLambda> expression, IEnumerable<Expression> expressionArguments)
        {
            var actualParams = expressionArguments.Select(
                argument => Expression.Lambda(argument, expression.Parameters))
                .Select(lambda => lambda.Compile())
                .Select(compiledDelegate => compiledDelegate.DynamicInvoke(new object[1]))
                .ToArray();
            return actualParams;
        }
    }
}