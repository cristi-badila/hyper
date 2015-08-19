using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using HyperMsg.Config;
using HyperMsg.Contracts;
using HyperMsg.Exceptions;

namespace HyperMsg.Providers
{
    public class RemoteMessageProvider : IMessageProvider
    {
        private readonly ChannelFactory<IMessageService> _channelFactory;
        private bool _disposed;

        public RemoteMessageProvider(IConfigSettings settings)
        {
            _channelFactory = new ChannelFactory<IMessageService>(new WebHttpBinding(), settings.Address);
            _channelFactory.Endpoint.Behaviors.Add(new WebHttpBehavior());
        }

        ~RemoteMessageProvider()
        {
            Dispose(false);
        }

        public void Send<TMessage>(TMessage message) where TMessage : Message
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (string.IsNullOrWhiteSpace(message.EndPoint))
                throw new MessageException("Message does not contain a valid endpoint.");

            var channel = _channelFactory.CreateChannel();
            channel.Post(message);
            CloseChannel(channel);
        }

        public IEnumerable<TMessage> Receive<TMessage>(string endPoint, int count = 1) where TMessage : Message
        {
            var channel = _channelFactory.CreateChannel();
            var messages = channel.Get(endPoint, count.ToString()).ToList();
            CloseChannel(channel);
            return messages.Cast<TMessage>();
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
