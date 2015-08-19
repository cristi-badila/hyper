using System;

namespace HyperMsg.Providers
{
    /// <summary>
    /// Defines the members of the IMessageProvider interface.
    /// </summary>
    public interface IMessageProvider : IDisposable
    {
        /// <summary>
        /// Sends a message to the broker for handling.
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <param name="message">Message to send</param>
        void Send<TMessage>(TMessage message) where TMessage : Message;
    }
}