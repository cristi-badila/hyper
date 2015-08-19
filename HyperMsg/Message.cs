using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace HyperMsg
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
        }

        /// <summary>
        /// Gets the Unique Id for the message.
        /// </summary>
        [DataMember]
        public System.Guid Id { get; private set; }

        /// <summary>
        /// Gets or sets a flag indicating if the message is persistent.
        /// </summary>
        [DataMember]
        public bool Persistent { get; set; }

        /// <summary>
        /// Gets the message body to transport.
        /// </summary>
        [DataMember]
        public string Body { get; private set; }

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
