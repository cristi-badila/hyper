using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using HyperMsg.Broker.Services;

namespace HyperMsg.Broker
{
    /// <summary>
    /// Provides the entry point for the broker to handle messages.
    /// </summary>
    public class BrokerManager : IBrokerManager
    {
        private bool _disposed;
        private WebServiceHost _host;

        ~BrokerManager()
        {
            Dispose(false);
        }

        /// <summary>
        /// Initialises the broker.
        /// </summary>
        /// <returns>Self</returns>
        public IBrokerManager Init()
        {
            _host = new WebServiceHost(typeof(MessageService), new Uri("http://localhost:8000"));
            _host.AddServiceEndpoint(typeof(IMessageService), new WebHttpBinding(), "");
            var behaviour = _host.Description.Behaviors.Find<ServiceDebugBehavior>();
            behaviour.HttpHelpPageEnabled = false;

            return this;
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
