using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace HyperMsg.Messages
{
    /// <summary>
    /// Base class for messages.
    /// </summary>
    [DataContract]
    [KnownType(typeof(BrokeredMessage))]
    public abstract class Message
    {
        protected Message()
        {
            Id = System.Guid.NewGuid();
            RetryLimit = 5;
        }

        /// <summary>
        /// Gets the Unique Id for the message.
        /// </summary>
        [DataMember]
        public System.Guid Id { get; internal set; }

        /// <summary>
        /// Gets the message body to transport.
        /// </summary>
        [DataMember]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the endpoint of the message. This is where any registered subscribers will look for messages.
        /// </summary>
        [DataMember]
        public string EndPoint { get; set; }

        /// <summary>
        /// Gets or set the number of retries if a message before dead lettering it. Default value is 5.
        /// </summary>
        [DataMember]
        public int RetryLimit { get; set; }

        /// <summary>
        /// Sets the body of the message.
        /// </summary>
        /// <typeparam name="TBody">Bodt type</typeparam>
        /// <param name="body">Body to set</param>
        public void SetBody<TBody>(TBody body)
        {
            Body = JsonConvert.SerializeObject(body);
        }

        /// <summary>
        /// Gets the body of the message.
        /// </summary>
        /// <typeparam name="TBody">Body type</typeparam>
        /// <returns>Body</returns>
        public TBody GetBody<TBody>()
        {
            return JsonConvert.DeserializeObject<TBody>(Body);
        }
    }
}
