using System;
using System.ServiceModel.Web;

namespace HyperMsg.Broker.Wcf
{
    internal class BrokerServiceHost : WebServiceHost
    {
        private readonly IDependencyResolver _resolver;

        public BrokerServiceHost(IDependencyResolver resolver, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            _resolver = resolver;
        }

        protected override void OnOpening()
        {
            base.OnOpening();

            if (Description.Behaviors.Find<BrokerServiceBehavior>() == null)
            {
                Description.Behaviors.Add(new BrokerServiceBehavior(_resolver));
            }
        }
    }
}