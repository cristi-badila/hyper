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
        /// <param name="expectedValue">Expected return value</param>
        public static void VerifyGet<TMock, TReturn>(
            this Mock<TMock> mock, Expression<Func<TMock, TReturn>> expression, TReturn expectedValue)
            where TMock : class
        {
            string name;
            if (!expression.TryGetReadPropertyName(out name))
            {
                throw new UnknownDispatchParamsException(expression);
            }

            var callInfo = mock.Dispatcher.FindByReturnMatch(name, expectedValue);
            if (callInfo == null || callInfo.Visited == 0)
            {
                throw new VerificationException(
                    $"Unable to verify that the value '{expectedValue}' was returned on the property.");
            }
        }

        /// <summary>
        ///     Verifies a write property matching the expression sets the expected value.
        /// </summary>
        /// <typeparam name="TMock">Mocked type</typeparam>
        /// <typeparam name="TReturn">Mocked expression return type</typeparam>
        /// <param name="mock">Mocked instance</param>
        /// <param name="expression">Expression</param>
        /// <param name="expectedValue">Expected set value</param>
        public static void VerifySet<TMock, TReturn>(
            this Mock<TMock> mock, Expression<Func<TMock, TReturn>> expression, TReturn expectedValue)
            where TMock : class
        {
            string name;

            if (!expression.TryGetWritePropertyName(out name))
            {
                throw new UnknownDispatchParamsException(expression);
            }

            var callInfo = mock.Dispatcher.FindByParameterMatch(name, new object[] { expectedValue });

            if (callInfo == null || callInfo.Visited == 0)
            {
                throw new VerificationException(
                    $"Unable to verify that the value '{expectedValue}' was set on the property.");
            }
        }

        public static void Verify<TMock, TLambda>(this Mock<TMock> mock, Expression<TLambda> expression, Occurred occurred)
            where TMock : class
        {
            DispatchParams dispatchParams;
            if (!expression.TryGetDispatchParams(out dispatchParams))
            {
                throw new UnknownDispatchParamsException(expression);
            }

            var actualParams = GetActualParams(expression, dispatchParams.Arguments);
            var callInfo = mock.Dispatcher.FindByParameterMatch(dispatchParams.Name, actualParams);

            if (callInfo == null && occurred.Count > 0)
            {
                throw new VerificationException(
                    $"Unable to verify that the action occurred '{occurred.Count} " +
                    $"time{(occurred.Count == 1 ? "s" : string.Empty)}.");
            }

            if (callInfo != null)
            {
                occurred.Assert(callInfo.Visited);
            }
        }

        public static CallInfo FindByParameterMatch(this MockProxyDispatcher mockProxyDispatcher, string name, object[] args)
        {
            return mockProxyDispatcher.RegisteredCallInfoList.FirstOrDefault(ci => ci.Name == name && ci.IsMatchFor(args));
        }

        public static CallInfo FindByReturnMatch(this MockProxyDispatcher mockProxyDispatcher, string name, object returnValue)
        {
            return mockProxyDispatcher.RegisteredCallInfoList.FirstOrDefault(ci => ci.Name == name && ci.ReturnValue == returnValue);
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