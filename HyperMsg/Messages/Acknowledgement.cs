using System;
using System.Runtime.Serialization;

namespace HyperMsg.Messages
{
    /// <summary>
    /// Defines an acknowledgement message that is used to acknowledge receipt.
    /// </summary>
    [DataContract]
    public class Acknowledgement
    {
        /// <summary>
        /// Gets or sets a flag indicating if the list of messages has been abandoned. This will increment the retry
        /// for each message.
        /// </summary>
        [DataMember]
        public bool IsAbandoned { get; set; }

        /// <summary>
        /// Gets or sets the list of messages Ids to acknowledge.
        /// </summary>
        [DataMember]
        public Guid[] MessageIds { get; set; }
    }
}