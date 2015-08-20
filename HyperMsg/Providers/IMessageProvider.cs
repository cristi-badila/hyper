using System;
using System.Collections.Generic;
using HyperMsg.Messages;

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
        /// Recieves up to n messages for the specified end point. This does not delete them from the underlying
        /// store. You need call Complete to acknowledge the message. If you do not them potentially you could receive
        /// the message again for processing. Your code will need to idempotent.
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <param name="endPoint">End point name</param>
        /// <param name="count">Max number of messages to get</param>
        /// <returns>List of messages for the end point</returns>
        IEnumerable<TMessage> Receive<TMessage>(string endPoint, int count = 1) where TMessage : Message;

        /// <summary>
        /// Recieves up to n messages for the specified end point. This deletes them immediately from the underlying
        /// store as soon as it receives them.
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <param name="endPoint">End point name</param>
        /// <param name="count">Max number of messages to get</param>
        /// <returns>List of messages for the end point</returns>
        IEnumerable<TMessage> ReceiveAndDelete<TMessage>(string endPoint, int count = 1) where TMessage : Message;

        /// <summary>
        /// Abandons the selected messages.
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <param name="messages">List of messages</param>
        void Abandon<TMessage>(params TMessage[] messages) where TMessage : Message;

        /// <summary>
        /// Completes the processing of messages by acknowledging them.
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <param name="messages">List of messages</param>
        void Complete<TMessage>(params TMessage[] messages) where TMessage : Message;
    }
}