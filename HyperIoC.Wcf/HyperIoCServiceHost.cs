using System;
using System.ServiceModel;

namespace HyperIoC.Wcf
{
    /// <summary>
    /// Defines HyperIoCServiceHost class. This is responsible for setting up the service behavior.
    /// </summary>
    internal class HyperIoCServiceHost : ServiceHost
    {
        private readonly IFactoryResolver _resolver;

        /// <summary>
        /// Initialises a new instance of the class
        /// </summary>
        public HyperIoCServiceHost(IFactoryResolver resolver, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            _resolver = resolver;
        }

        /// <summary>
        /// Handles the opening event to add the service behaviour to the host description.
        /// </summary>
        protected override void OnOpening()
        {
            base.OnOpening();

            if (Description.Behaviors.Find<HyperIoCServiceBehavior>() == null)
            {
                Description.Behaviors.Add(new HyperIoCServiceBehavior(_resolver));
            }
        }
    }
}
