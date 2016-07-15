using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using HyperMock.Universal.Exceptions;
using HyperMock.Universal.ExtensionMethods;

namespace HyperMock.Universal.Verification
{
    /// <summary>
    ///     Set of extensions for verifying behaviours have occurred.
    /// </summary>
    public static class VerifierExtensions
    {
        /// <summary>
        ///     Verifies a method matching the expression occurred the correct number of times.
        /// </summary>
        /// <typeparam name="TMock">Mocked type</typeparam>
        /// <param name="instance">Mocked instance</param>
        /// <param name="expression">Expression</param>
        /// <param name="occurred">Expected occurrence</param>
        public static void Verify<TMock>(
            this TMock instance, Expression<Action<TMock>> expression, Occurred occurred)
        {
            var dispatcher = GetDispatcher(instance);

            string name;
            ReadOnlyCollection<Expression> arguments;

            if (dispatcher.TryGetMethodNameAndArgs(expression, out name, out arguments))
            {
                var values = new List<object>();

                foreach (var argument in arguments)
                {
                    var lambda = Expression.Lambda(argument, expression.Parameters);
                    var compiledDelegate = lambda.Compile();
                    var value = compiledDelegate.DynamicInvoke(new object[1]);
                    values.Add(value);
                }

                var callInfo = dispatcher.FindByParameterMatch(name, values.ToArray());

                if (callInfo == null && occurred.Count > 0)
                    throw new VerificationException(
                        $"Unable to verify that the action occurred '{occurred.Count} " +
                        $"time{(occurred.Count == 1 ? "s" : "")}.");

                if (callInfo != null)
                    occurred.Assert(callInfo.Visited);
            }
        }

        /// <summary>
        ///     Verifies a function matching the expression occurred the correct number of times.
        /// </summary>
        /// <typeparam name="TMock">Mocked type</typeparam>
        /// <typeparam name="TReturn">Mocked expression return type</typeparam>
        /// <param name="instance">Mocked instance</param>
        /// <param name="expression">Expression</param>
        /// <param name="occurred">Expected occurance</param>
        public static void Verify<TMock, TReturn>(
            this TMock instance, Expression<Func<TMock, TReturn>> expression, Occurred occurred)
        {
            var dispatcher = GetDispatcher(instance);

            string name;
            ReadOnlyCollection<Expression> arguments;

            if (dispatcher.TryGetMethodNameAndArgs(expression, out name, out arguments))
            {
                var values = new List<object>();

                foreach (var argument in arguments)
                {
                    var lambda = Expression.Lambda(argument, expression.Parameters);
                    var compiledDelegate = lambda.Compile();
                    var value = compiledDelegate.DynamicInvoke(new object[1]);
                    values.Add(value);
                }

                var callInfo = dispatcher.FindByParameterMatch(name, values.ToArray());

                if (callInfo == null && occurred.Count > 0)
                    throw new VerificationException(
                        $"Unable to verify that the action occurred '{occurred.Count} " +
                        $"time{(occurred.Count == 1 ? "s" : "")}.");

                if (callInfo != null)
                    occurred.Assert(callInfo.Visited);
            }
        }

        /// <summary>
        ///     Verifies a read property matching the expression returns the expected value.
        /// </summary>
        /// <typeparam name="TMock">Mocked type</typeparam>
        /// <typeparam name="TReturn">Mocked expression return type</typeparam>
        /// <param name="instance">Mocked instance</param>
        /// <param name="expression">Expression</param>
        /// <param name="expectedValue">Expected return value</param>
        public static void VerifyGet<TMock, TReturn>(
            this TMock instance, Expression<Func<TMock, TReturn>> expression, TReturn expectedValue)
        {
            var dispatcher = GetDispatcher(instance);

            string name;

            if (dispatcher.TryGetReadPropertyNameAndArgs(expression, out name))
            {
                var callInfo = dispatcher.FindByReturnMatch(name, expectedValue);

                if (callInfo == null || callInfo.Visited == 0)
                    throw new VerificationException(
                        $"Unable to verify that the value '{expectedValue}' was returned on the property.");
            }
        }

        /// <summary>
        ///     Verifies a write property matching the expression sets the expected value.
        /// </summary>
        /// <typeparam name="TMock">Mocked type</typeparam>
        /// <typeparam name="TReturn">Mocked expression return type</typeparam>
        /// <param name="instance">Mocked instance</param>
        /// <param name="expression">Expression</param>
        /// <param name="expectedValue">Expected set value</param>
        public static void VerifySet<TMock, TReturn>(
            this TMock instance, Expression<Func<TMock, TReturn>> expression, TReturn expectedValue)
        {
            var dispatcher = GetDispatcher(instance);

            string name;

            if (dispatcher.TryGetWritePropertyNameAndArgs(expression, out name))
            {
                var callInfo = dispatcher.FindByParameterMatch(name, new object[] {expectedValue});

                if (callInfo == null || callInfo.Visited == 0)
                    throw new VerificationException(
                        $"Unable to verify that the value '{expectedValue}' was set on the property.");
            }
        }

        public static CallInfo FindByParameterMatch(this MockProxyDispatcher mockProxyDispatcher, string name, object[] args)
        {
            var callInfoListForName = mockProxyDispatcher.RegisteredCallInfoList.Where(ci => ci.Name == name).ToList();

            return callInfoListForName.FirstOrDefault(ci => ci.IsMatchFor(args));
        }

        public static CallInfo FindByReturnMatch(this MockProxyDispatcher mockProxyDispatcher, string name, object returnValue)
        {
            return mockProxyDispatcher.RegisteredCallInfoList.FirstOrDefault(ci => ci.Name == name && ci.ReturnValue == returnValue);
        }

        private static MockProxyDispatcher GetDispatcher<TMock>(TMock instance)
        {
            var dispatcher = instance as MockProxyDispatcher;

            if (dispatcher == null)
                throw new MockException("Unable to get the dispatcher from the instance.");

            return dispatcher;
        }
    }
}