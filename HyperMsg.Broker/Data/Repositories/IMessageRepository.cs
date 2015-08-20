using System;
using System.Collections.Generic;
using HyperMsg.Broker.Data.Entities;

namespace HyperMsg.Broker.Data.Repositories
{
    /// <summary>
    /// Defines the members of the IMessageRepository interface.
    /// </summary>
    public interface IMessageRepository
    {
        /// <summary>
        /// Gets a list of messages for the selected Ids.
        /// </summary>
        /// <param name="messageIds">Message Ids</param>
        /// <returns>List of matching messages</returns>
        IEnumerable<MessageEntity> Get(params Guid[] messageIds);

        /// <summary>
        /// Gets a selected number of messages for the end point.
        /// </summary>
        /// <param name="endpoint">End point to get messages for</param>
        /// <param name="count">Number to retrieve - may return upto that amount</param>
        /// <returns>List of messages</returns>
        IEnumerable<MessageEntity> Get(string endpoint, int count);

        /// <summary>
        /// Adds a message.
        /// </summary>
        /// <param name="messageEntity">Message to add</param>
        void Add(MessageEntity messageEntity);

        /// <summary>
        /// Removes selected messages.
        /// </summary>
        /// <param name="messageIds">List of message Ids</param>
        void Remove(params Guid[] messageIds);

        /// <summary>
        /// Updates the retry count for the selected messages. If any of the messages have reached their retry limit
        /// then these are automatically dead lettered.
        /// </summary>
        /// <param name="entities">List of message to update retry for</param>
        void UpdateRetry(params MessageEntity[] entities);

        /// <summary>
        /// Gets a list of dead letters.
        /// </summary>
        /// <returns>List of dead lettered messages</returns>
        IEnumerable<MessageEntity> DeadLetters();
    }
}