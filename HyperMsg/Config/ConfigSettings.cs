using System.Configuration;

namespace HyperMsg.Config
{
    public class ConfigSettings : IConfigSettings
    {
        public string Address => ConfigurationManager.AppSettings["HyperMsg.Address"];
    }
}