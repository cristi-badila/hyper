using System;
using System.Collections.Generic;

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

        /// <summary>
        /// Recieves up to n messages for the specified end point.
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <param name="endPoint">End point name</param>
        /// <param name="count">Max number of messages to get</param>
        /// <returns>List of messages for the end point</returns>
        IEnumerable<TMessage> Receive<TMessage>(string endPoint, int count = 1) where TMessage : Message;
    }
}