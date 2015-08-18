namespace HyperMsg
{
    /// <summary>
    /// Defines a standard message.
    /// </summary>
    /// <typeparam name="TBody">Body to transport</typeparam>
    public class StandardMessage<TBody> : Message<TBody>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="body">Body to transport</param>
        public StandardMessage(TBody body) : base(body)
        {
            
        }
    }
}