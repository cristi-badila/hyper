using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using HyperMsg.Config;
using HyperMsg.Contracts;
using HyperMsg.Exceptions;
using HyperMsg.Messages;

namespace HyperMsg.Providers
{
    /// <summary>
    /// Defines a message provider that calls a remote service as defined by the configuration settings address property.
    /// </summary>
    public class RemoteMessageProvider : IMessageProvider
    {
        private readonly ChannelFactory<IMessageService> _channelFactory;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="settings">Config settings</param>
        public RemoteMessageProvider(IConfigSettings settings)
        {
            _channelFactory = new ChannelFactory<IMessageService>(new WebHttpBinding(), settings.Address);
            _channelFactory.Endpoint.Behaviors.Add(
                new WebHttpBehavior {FaultExceptionEnabled = false, HelpEnabled = false});
        }

        ~RemoteMessageProvider()
        {
            Dispose(false);
        }

        public void Send<TMessage>(TMessage message) where TMessage : Message
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (string.IsNullOrWhiteSpace(message.EndPoint))
                throw new MessageException(ErrorResources.MessageEndpoint);

            var channel = _channelFactory.CreateChannel();
            channel.Post(message);
            CloseChannel(channel);
        }

        public IEnumerable<TMessage> Receive<TMessage>(string endPoint, int count = 1) where TMessage : Message
        {
            if (string.IsNullOrWhiteSpace(endPoint))
                throw new ArgumentException(ErrorResources.InvalidEndpoint, nameof(endPoint));
            if (count < 1 || count > 100)
                throw new ArgumentOutOfRangeException(nameof(count), ErrorResources.MessageCountOutOfRange);

            var channel = _channelFactory.CreateChannel();
            var response = channel.Get(endPoint, count.ToString());

            if (response.HasError)
                throw new Exception(response.Error);

            var messages = response.Body.Cast<TMessage>().ToList();
            CloseChannel(channel);

            return messages;
        }

        public IEnumerable<TMessage> ReceiveAndDelete<TMessage>(string endPoint, int count = 1) where TMessage : Message
        {
            var messages = Receive<TMessage>(endPoint, count).ToList();

            Complete(messages.ToArray());

            return messages;
        }

        public void Abandon<TMessage>(params TMessage[] messages) where TMessage : Message
        {
            var channel = _channelFactory.CreateChannel();
            channel.Acknowledge(new Acknowledgement {IsAbandoned = true, MessageIds = messages.Select(m => m.Id).ToArray()});
            CloseChannel(channel);
        }

        public void Complete<TMessage>(params TMessage[] messages) where TMessage : Message
        {
            var channel = _channelFactory.CreateChannel();
            channel.Acknowledge(new Acknowledgement { MessageIds = messages.Select(m => m.Id).ToArray() });
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
