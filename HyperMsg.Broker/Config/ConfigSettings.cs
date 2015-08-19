using System.Configuration;

namespace HyperMsg.Broker.Config
{
    public class ConfigSettings : IConfigSettings
    {
        public string DatabasePath => ConfigurationManager.AppSettings["HyperMsg.DatabasePath"];
        public string Address => ConfigurationManager.AppSettings["HyperMsg.Address"];
    }
}