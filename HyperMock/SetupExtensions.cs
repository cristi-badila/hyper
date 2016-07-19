namespace HyperMock.Universal
{
    using System;
    using System.Linq.Expressions;
    using Setup;
    using StubBehaviors;

    /// <summary>
    ///     Set of extensions on the proxy for setting up method and property behaviours.
    /// </summary>
    public static class SetupExtensions
    {
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
            return new VoidBehaviour(instance.AddHandling(expression));
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
            return new ReturnBehaviour<TReturn>(instance.AddHandling(expression));
        }

        /// <summary>
        ///     Setup of a property read.
        /// </summary>
        /// <typeparam name="TMock">Mocked type</typeparam>
        /// <typeparam name="TReturn">Return type</typeparam>
        /// <param name="instance">Mocked instance</param>
        /// <param name="expression">Read property expression</param>
        /// <returns>Read property behaviours</returns>
        public static ReturnBehaviour<TReturn> SetupGet<TMock, TReturn>(
            this Mock<TMock> instance, Expression<Func<TMock, TReturn>> expression)
            where TMock : class
        {
            return new ReturnBehaviour<TReturn>(instance.AddHandlingForPropertyGet(expression));
        }

        /// <summary>
        ///     Setup of a property write.
        /// </summary>
        /// <typeparam name="TMock">Mocked type</typeparam>
        /// <typeparam name="TReturn">Return type</typeparam>
        /// <param name="instance">Mocked instance</param>
        /// <param name="expression">Write property expression</param>
        /// <returns>Write property behaviours</returns>
        public static VoidBehaviour SetupSet<TMock, TReturn>(
            this Mock<TMock> instance, Expression<Func<TMock, TReturn>> expression)
            where TMock : class
        {
            return new VoidBehaviour(instance.AddHandlingForPropertySet(expression));
        }

        /// <summary>
        ///     Setup of a property write.
        /// </summary>
        /// <typeparam name="TMock">Mocked type</typeparam>
        /// <typeparam name="TReturn">Return type</typeparam>
        /// <param name="instance">Mocked instance</param>
        /// <param name="expression">Write property expression</param>
        /// <param name="value">The expected value to be set</param>
        /// <returns>Write property behaviours</returns>
        public static VoidBehaviour SetupSet<TMock, TReturn>(
            this Mock<TMock> instance, Expression<Func<TMock, TReturn>> expression, TReturn value)
            where TMock : class
        {
            return new VoidBehaviour(instance.AddHandlingForPropertySet(expression, value));
        }
    }
}