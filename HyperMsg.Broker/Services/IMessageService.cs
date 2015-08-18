using System.ServiceModel;
using System.ServiceModel.Web;

namespace HyperMsg.Broker.Services
{
    /// <summary>
    /// Defines the members of the IMessageService interface.
    /// </summary>
    [ServiceContract]
    public interface IMessageService
    {
        /// <summary>
        /// Posts a message for subscribers.
        /// </summary>
        /// <param name="message">Message to post</param>
        [OperationContract]
        [WebInvoke(UriTemplate="api/messages")]
        void Post(Message message);
    }
}
