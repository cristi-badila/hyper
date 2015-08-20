using System;

namespace HyperMsg.Broker
{
    /// <summary>
    /// Defines the members of the IBrokerManager interface.
    /// </summary>
    public interface IBrokerManager : IDisposable
    {
        /// <summary>
        /// Starts the broker ready to receive messages. You will need to keep the broker manager available
        /// for the lifetime of your host. Remember to call dispose before exiting.
        /// </summary>
        void Run();
    }
}