namespace HyperMsg.Controller
{
    /// <summary>
    /// Defines the members of the IConfigSettings interface.
    /// </summary>
    public interface IConfigSettings
    {
        /// <summary>
        /// Gets the path to where the database is located.
        /// </summary>
        string DatabasePath { get; }
    }
}