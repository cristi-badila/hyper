using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using HyperMsg.Broker.Services;
using HyperMsg.Broker.Wcf;
using HyperMsg.Broker.Config;
using HyperMsg.Contracts;

namespace HyperMsg.Broker
{
    /// <summary>
    /// Provides the entry point for the broker to handle messages.
    /// </summary>
    public class BrokerManager : IBrokerManager
    {
        private readonly BrokerServiceHost _host;
        private bool _disposed;

        /// <summary>
        /// Initialises a new instance of the class.
        /// </summary>
        /// <param name="resolver">Dependency resolver</param>
        /// <param name="settings">Config settings</param>
        public BrokerManager(IDependencyResolver resolver, IConfigSettings settings)
        {
            _host = new BrokerServiceHost(resolver, typeof(MessageService), new Uri(settings.Address));
            _host.AddServiceEndpoint(typeof(IMessageService), new WebHttpBinding(), "");
            var behaviour = _host.Description.Behaviors.Find<ServiceDebugBehavior>();
            behaviour.HttpHelpPageEnabled = false;
        }

        ~BrokerManager()
        {
            Dispose(false);
        }

        /// <summary>
        /// Runs the broker by opening a host for handling requests.
        /// </summary>
        public void Run()
        {
            _host.Open();
        }

        /// <summary>
        /// Closes the host.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _host.Close();
            }

            _disposed = true;
        }
    }
}
