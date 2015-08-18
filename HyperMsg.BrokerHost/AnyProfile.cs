using HyperIoC;
using HyperMsg.Broker;
using HyperMsg.Broker.Data;
using HyperMsg.Broker.Data.Repositories;
using HyperMsg.Broker.Services;

namespace HyperMsg.BrokerHost
{
    public class AnyProfile : FactoryProfile
    {
        public override void Construct(IFactoryBuilder builder)
        {
            builder.Add<IConfigSettings, ConfigSettings>().AsSingleton();
            builder.Add<IMessageService, MessageService>();
            builder.Add<IConnectionProvider, DatabaseManager>().AsSingleton();
            builder.Add<IBrokerManager, BrokerManager>().AsSingleton();
            builder.Add<IMessageRepository, MessageRepository>().AsSingleton();
        }
    }
}