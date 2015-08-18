using System.Configuration;

namespace HyperMsg.Broker
{
    public class ConfigSettings : IConfigSettings
    {
        public string DatabasePath => ConfigurationManager.AppSettings["DatabasePath"];
    }
}