using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

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
        /// <param name="endpoint">Endpoint to get the messages for</param>
        /// <param name="count">Number of messages to retrieve</param>
        /// <returns>Retrieves up to n messages</returns>
        [OperationContract]
        [WebGet(UriTemplate = "api/messages/{endPoint}/{count}")]
        IEnumerable<Message> Get(string endpoint, string count);

        /// <summary>
        /// Posts a message for subscribers.
        /// </summary>
        /// <param name="message">Message to post</param>
        [OperationContract]
        [WebInvoke(UriTemplate="api/messages")]
        void Post(Message message);
    }
}
