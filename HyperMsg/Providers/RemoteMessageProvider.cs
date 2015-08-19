using System;
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

        /// <summary>
        /// Sends the message to the message service endpoint.
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <param name="message">Message to send</param>
        public void Send<TMessage>(TMessage message) where TMessage : Message
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (string.IsNullOrWhiteSpace(message.EndPoint))
                throw new MessageException("Message does not contain a valid endpoint.");

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
