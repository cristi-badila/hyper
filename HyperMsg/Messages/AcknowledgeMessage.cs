using System;
using System.Runtime.Serialization;

namespace HyperMsg.Messages
{
    /// <summary>
    /// Defines an acknowledgement message that is used to acknowledge receipt.
    /// </summary>
    [DataContract]
    public class AcknowledgeMessage
    {
        /// <summary>
        /// Gets or sets the list of messages Ids to acknowledge.
        /// </summary>
        [DataMember]
        public Guid[] MessageIds { get; set; }
    }
}