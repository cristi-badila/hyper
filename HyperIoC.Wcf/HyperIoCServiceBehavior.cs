using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace HyperIoC.Wcf
{
    /// <summary>
    /// Defines HyperIoCServiceBehavior class.
    /// </summary>
    internal class HyperIoCServiceBehavior : IServiceBehavior
    {
        private readonly IFactoryResolver _resolver;

        /// <summary>
        /// Initialises a new instance of the class.
        /// </summary>
        internal HyperIoCServiceBehavior(IFactoryResolver resolver)
        {
            _resolver = resolver;
        }

        /// <summary>
        /// Adds binding parameters. Not used.
        /// </summary>
        public void AddBindingParameters(
            ServiceDescription serviceDescription, 
            ServiceHostBase serviceHostBase, 
            Collection<ServiceEndpoint> endpoints, 
            BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Adds the dispatch behaviour. This is how the host resolves the service to farm for the request. It uses
        /// a custom provides that then uses the IoC framework to create the service from its contract.
        /// </summary>
        /// <param name="serviceDescription">Service description</param>
        /// <param name="serviceHostBase">Service host</param>
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
                        new HyperIoCInstanceProvider(_resolver, serviceEndPoint.Contract.ContractType);
                }
            }
        }

        /// <summary>
        /// Validates the behaviour. Not used.
        /// </summary>
        /// <param name="serviceDescription">Service description</param>
        /// <param name="serviceHostBase">Service host</param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}
