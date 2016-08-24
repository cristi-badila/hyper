namespace HyperMock.Universal.Syntax
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Core;
    using ParameterMatchers;
    using StubBehaviors;

    public static class MockExtensionMethods
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
            var callDescriptorFactory = GetMethodCallDescriptorFactory();
            var callDescriptor = callDescriptorFactory.Create(expression);
            occurred.Assert(mock.Dispatcher.RecordedCalls.Filter(callDescriptor).Count());
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
            var callDescriptorFactory = GetGetterCallDescriptorFactory();
            var callDescriptor = callDescriptorFactory.Create(expression);
            occurred.Assert(mock.Dispatcher.RecordedCalls.Filter(callDescriptor.MemberName).Count());
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
            var callDescriptorFactory = GetSetterCallDescriptorFactory();
            var callDescriptor = callDescriptorFactory.Create(expression);
            occurred.Assert(mock.Dispatcher.RecordedCalls.Filter(callDescriptor.MemberName, new ExactMatcher(expectedValue)).Count());
        }

        /// <summary>
        ///     Setup of a method with no return.
        /// </summary>
        /// <typeparam name="TMock">Mocked type</typeparam>
        /// <param name="instance">Mocked instance</param>
        /// <param name="expression">Method expression</param>
        /// <returns>Method behavior</returns>
        public static VoidBehaviour Setup<TMock>(
            this Mock<TMock> instance, Expression<Action<TMock>> expression)
            where TMock : class
        {
            return new VoidBehaviour(instance.Setup<TMock, Action<TMock>>(expression));
        }

        /// <summary>
        ///     Setup of a function with a return value.
        /// </summary>
        /// <typeparam name="TMock">Mocked type</typeparam>
        /// <typeparam name="TReturn">Return type</typeparam>
        /// <param name="instance">Mocked instance</param>
        /// <param name="expression">Function expression</param>
        /// <returns>Function behaviours</returns>
        public static ReturnBehaviour<TReturn> Setup<TMock, TReturn>(
            this Mock<TMock> instance, Expression<Func<TMock, TReturn>> expression)
            where TMock : class
        {
            return new ReturnBehaviour<TReturn>(instance.Setup<TMock, Func<TMock, TReturn>>(expression));
        }

        /// <summary>
        ///     Setup of a method call with a default return behavior.
        /// </summary>
        /// <typeparam name="TMock">Mocked type</typeparam>
        /// <typeparam name="TLambda">The expression containing the method call</typeparam>
        /// <param name="mock">Mocked instance</param>
        /// <param name="expression">Function expression</param>
        /// <returns>Function behaviours</returns>
        public static CallDescriptor Setup<TMock, TLambda>(this Mock<TMock> mock, Expression<TLambda> expression)
            where TMock : class
        {
            var callDescriptorFactory = GetMethodCallDescriptorFactory();
            var callDescriptor = callDescriptorFactory.Create(expression);
            mock.Dispatcher.KnownCallDescriptors.Add(callDescriptor);

            return callDescriptor;
        }

        /// <summary>
        ///     Setup of a property read.
        /// </summary>
        /// <typeparam name="TMock">Mocked type</typeparam>
        /// <typeparam name="TReturn">Return type</typeparam>
        /// <param name="mock">Mocked instance</param>
        /// <param name="expression">Read property expression</param>
        /// <returns>Read property behaviours</returns>
        public static ReturnBehaviour<TReturn> SetupGet<TMock, TReturn>(
            this Mock<TMock> mock, Expression<Func<TMock, TReturn>> expression)
            where TMock : class
        {
            var callDescriptorFactory = GetGetterCallDescriptorFactory();
            var callDescriptor = callDescriptorFactory.Create(expression);
            mock.Dispatcher.KnownCallDescriptors.Add(callDescriptor);

            return new ReturnBehaviour<TReturn>(callDescriptor);
        }

        /// <summary>
        ///     Setup of a property write.
        /// </summary>
        /// <typeparam name="TMock">Mocked type</typeparam>
        /// <typeparam name="TReturn">Return type</typeparam>
        /// <param name="mock">Mocked instance</param>
        /// <param name="expression">Write property expression</param>
        /// <returns>Write property behaviours</returns>
        public static VoidBehaviour SetupSet<TMock, TReturn>(
            this Mock<TMock> mock, Expression<Func<TMock, TReturn>> expression)
            where TMock : class
        {
            var callDescriptorFactory = GetSetterCallDescriptorFactory();
            var callDescriptor = callDescriptorFactory.Create(expression);
            mock.Dispatcher.KnownCallDescriptors.Add(callDescriptor);

            return new VoidBehaviour(callDescriptor);
        }

        private static CallDescriptorFactory GetMethodCallDescriptorFactory()
        {
            return new CallDescriptorFactory(
                new MethodCallInfoFactory(),
                new ParameterMatcherFactory(new ParameterMatcherInfoFactory()));
        }

        private static CallDescriptorFactory GetGetterCallDescriptorFactory()
        {
            return new CallDescriptorFactory(
                new GetterMethodCallInfoFactory(),
                new ParameterMatcherFactory(new ParameterMatcherInfoFactory()));
        }

        private static CallDescriptorFactory GetSetterCallDescriptorFactory()
        {
            return new CallDescriptorFactory(
                new SetterMethodCallInfoFactory(),
                new ParameterMatcherFactory(new ParameterMatcherInfoFactory()));
        }
    }
}
