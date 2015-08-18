namespace HyperMsg
{
    /// <summary>
    /// Base class for messages.
    /// </summary>
    public abstract class Message
    {
        protected Message()
        {
            Id = System.Guid.NewGuid();
        }

        /// <summary>
        /// Gets the Unique Id for the message.
        /// </summary>
        public System.Guid Id { get; private set; }

        /// <summary>
        /// Gets or sets a flag indicating if the message is persistent.
        /// </summary>
        public bool Persistent { get; set; }
    }

    /// <summary>
    /// Base class for messages.
    /// </summary>
    public abstract class Message<TBody> : Message
    {
        protected Message(TBody body)
        {
            Body = body;
        }

        /// <summary>
        /// Gets the message body to transport.
        /// </summary>
        public TBody Body { get; private set; }
    }
}
