using System.Runtime.Serialization;

namespace HyperMsg.Messages
{
    /// <summary>
    /// Defines a standard message response.
    /// </summary>
    [DataContract]
    public class MessageResponse
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        [DataMember]
        public string Error { get; set; }

        /// <summary>
        /// Gets a flag indicating if an error exists.
        /// </summary>
        public bool HasError => !string.IsNullOrWhiteSpace(Error);
    }

    /// <summary>
    /// Defines a standard message response with a body.
    /// </summary>
    [DataContract]
    public class MessageResponse<TBody> : MessageResponse
    {
        /// <summary>
        /// Gets or sets the body of the message.
        /// </summary>
        [DataMember]
        public TBody Body { get; set; }
    }
}
