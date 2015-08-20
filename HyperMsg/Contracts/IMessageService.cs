using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using HyperMsg.Messages;

namespace HyperMsg.Contracts
{
    /// <summary>
    /// Defines the members of the IMessageService interface.
    /// </summary>
    [ServiceContract]
    public interface IMessageService
    {
        /// <summary>
        /// Gets a set of messages. If the count is invalid or less than 1 then it will return 0 or 1 messages.
        /// </summary>
        /// <remarks>
        /// This operates an receive and delete mechanism meaning that once it is returned to the client, it will be deleted
        /// from the underlying store. If delivery fails it will still remain in the store. This uses an acknowledgment mechanism
        /// so when the client receives the message it will issue an acknowledgement.
        /// </remarks>
        /// <param name="endPoint">Endpoint to get the messages for</param>
        /// <param name="count">Number of messages to retrieve</param>
        /// <returns>Retrieves up to n messages</returns>
        [OperationContract]
        [WebGet(UriTemplate = "api/messages/{endPoint}/{count}")]
        MessageResponse<IEnumerable<Message>> Get(string endPoint, string count);

        /// <summary>
        /// Posts a message for subscribers.
        /// </summary>
        /// <param name="message">Message to post</param>
        [OperationContract]
        [WebInvoke(UriTemplate="api/messages")]
        MessageResponse Post(Message message);

        /// <summary>
        /// Acknowledges the list of messages by removing them from the store.
        /// </summary>
        /// <param name="acknowlege">List of messages to acknowledge</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "api/messages/acknowledge")]
        MessageResponse Acknowledge(Acknowledgement acknowlege);
    }
}
