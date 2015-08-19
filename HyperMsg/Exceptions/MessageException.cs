using System;

namespace HyperMsg.Exceptions
{
    /// <summary>
    /// Defines a message exception for the framework.
    /// </summary>
    public class MessageException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the class.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner exception (optional)</param>
        public MessageException(string message, Exception innerException = null) : base(message, innerException)
        {
            
        }
    }
}
