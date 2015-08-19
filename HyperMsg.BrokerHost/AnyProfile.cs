using HyperIoC;
using HyperMsg.Broker;
using HyperMsg.Broker.Config;
using HyperMsg.Broker.Data;
using HyperMsg.Broker.Data.Repositories;
using HyperMsg.Broker.Services;
using HyperMsg.Contracts;

namespace HyperMsg.BrokerHost
{
    public class AnyProfile : FactoryProfile
    {
        public override void Construct(IFactoryBuilder builder)
        {
            builder.Add<IDependencyResolver, DependencyResolver>().AsSingleton();
            builder.Add<IConfigSettings, ConfigSettings>().AsSingleton();
            builder.Add<IMessageService, MessageService>();
            builder.Add<IDatabaseFactory, DatabaseFactory>().AsSingleton();
            builder.Add<IBrokerManager, BrokerManager>().AsSingleton();
            builder.Add<IMessageRepository, MessageRepository>().AsSingleton();
        }
    }
}