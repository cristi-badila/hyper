namespace HyperMsg.Config
{
    public interface IConfigSettings
    {
        /// <summary>
        /// Gets the address of the service for posting messages.
        /// </summary>
        string Address { get; }    
    }
}
