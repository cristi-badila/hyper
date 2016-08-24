namespace HyperMock.Universal.Core
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Exceptions;

    public class CallDescriptorFactory : ICallDescriptorFactory
    {
        private readonly IMethodCallInfoFactory _methodCallInfoFactory;
        private readonly IParameterMatcherFactory _parameterMatcherFactory;

        public CallDescriptorFactory(IMethodCallInfoFactory methodCallInfoFactory, IParameterMatcherFactory parameterMatcherFactory)
        {
            _methodCallInfoFactory = methodCallInfoFactory;
            _parameterMatcherFactory = parameterMatcherFactory;
        }

        public CallDescriptor Create<TDelegate>(Expression<TDelegate> expression)
        {
            MethodCallInfo methodCallInfo;
            ParameterMatchersList parameterMatchers;
            try
            {
                methodCallInfo = _methodCallInfoFactory.Create(expression);
                parameterMatchers = new ParameterMatchersList(methodCallInfo.Arguments
                    .Select(argumentExpression => Expression.Lambda(argumentExpression, expression.Parameters))
                    .Select(argumentLambda => _parameterMatcherFactory.Create(argumentLambda)).ToList());
            }
            catch (Exception exception)
                when (exception is ArgumentException ||
                      exception is UnsupportedArgumentExpressionException)
            {
                throw new UnknownCallExpressionException(expression, exception);
            }

            return new CallDescriptor(methodCallInfo.Name, parameterMatchers);
        }
    }
}