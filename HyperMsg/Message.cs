using System;

namespace HyperMsg
{
    /// <summary>
    /// Base class for messages.
    /// </summary>
    public abstract class Message<TBody>
    {
        protected Message(TBody body)
        {
            Id = Guid.NewGuid();
            Body = body;
        }

        /// <summary>
        /// Gets the Unique Id for the message.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the message body to transport.
        /// </summary>
        public TBody Body { get; private set; }

        /// <summary>
        /// Gets or sets a flag indicating if the message is persistent.
        /// </summary>
        public bool Persistent { get; set; }
    }
}
