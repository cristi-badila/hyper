using System.Configuration;

namespace HyperMsg.Controller
{
    /// <summary>
    /// Provides the config settings.
    /// </summary>
    public class ConfigSettings : IConfigSettings
    {
        public string DatabasePath => ConfigurationManager.AppSettings["DatabasePath"];
    }
}
