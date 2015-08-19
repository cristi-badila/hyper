using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace HyperMsg.Broker.Wcf
{
    internal class BrokerServiceBehavior : IServiceBehavior
    {
        private readonly IDependencyResolver _resolver;

        internal BrokerServiceBehavior(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        public void AddBindingParameters(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                var currentChannelDispatcher = channelDispatcher as ChannelDispatcher;

                if (currentChannelDispatcher == null) continue;

                foreach (var endPointDispatcher in currentChannelDispatcher.Endpoints)
                {
                    if (endPointDispatcher.ContractName == "IMetadataExchange") { continue; }

                    var serviceEndPoint = serviceDescription.Endpoints.FirstOrDefault();

                    if (serviceEndPoint == null) continue;

                    endPointDispatcher.DispatchRuntime.InstanceProvider =
                        new BrokerInstanceProvider(_resolver, serviceEndPoint.Contract.ContractType);
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}