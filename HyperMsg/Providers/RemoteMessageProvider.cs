using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using HyperMsg.Contracts;

namespace HyperMsg.Providers
{
    public class RemoteMessageProvider : IMessageProvider
    {
        private readonly ChannelFactory<IMessageService> _channelFactory;
        private bool _disposed;

        public RemoteMessageProvider()
        {
            _channelFactory = new ChannelFactory<IMessageService>(new WebHttpBinding(), "http://localhost:8000");
            _channelFactory.Endpoint.Behaviors.Add(new WebHttpBehavior());
        }

        ~RemoteMessageProvider()
        {
            Dispose(false);
        }

        public void Send<TMessage>(TMessage message) where TMessage : Message
        {
            var channel = _channelFactory.CreateChannel();
            channel.Post(message);
            CloseChannel(channel);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _channelFactory.Close();
            }

            _disposed = true;
        }

        private static void CloseChannel(IMessageService service)
        {
            // ReSharper disable SuspiciousTypeConversion.Global
            var clientChannel = service as IClientChannel;
            // ReSharper restore SuspiciousTypeConversion.Global
            clientChannel?.Close();
        }
    }
}
