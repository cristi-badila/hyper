using System;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;

namespace HyperMsg.Broker.Wcf
{
    internal class BrokerInstanceProvider : IInstanceProvider
    {
        private readonly IDependencyResolver _resolver;
        private readonly Type _contractType;

        internal BrokerInstanceProvider(IDependencyResolver resolver, Type contractType)
        {
            _resolver = resolver;
            _contractType = contractType;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return GetInstance(instanceContext, null);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        public object GetInstance(InstanceContext instanceContext, System.ServiceModel.Channels.Message message)
        {
            var instance = _resolver.Get(_contractType);
            return instance;
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {

        }
    }
}
