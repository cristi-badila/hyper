using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HyperMsg.Exceptions;
using HyperMsg.Messages;

namespace HyperMsg.Providers
{
    public class InMemoryMessageProvider : IMessageProvider
    {
        private readonly List<Message> _messages = new List<Message>();
        private bool _disposed;

        ~InMemoryMessageProvider()
        {
            Dispose(false);
        }

        public void Send<TMessage>(TMessage message) where TMessage : Message
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (string.IsNullOrWhiteSpace(message.EndPoint))
                throw new MessageException(ErrorResources.MessageEndpoint);

            _messages.Add(message);
        }

        public IEnumerable<TMessage> Receive<TMessage>(string endPoint, int count = 1) where TMessage : Message
        {
            if (string.IsNullOrWhiteSpace(endPoint))
                throw new ArgumentException(ErrorResources.InvalidEndpoint, nameof(endPoint));
            if (count < 1 || count > 100)
                throw new ArgumentOutOfRangeException(nameof(count), ErrorResources.MessageCountOutOfRange);

            return _messages.Where(m => m.EndPoint == endPoint).Take(count).Cast<TMessage>();
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
                _messages.Clear();
            }

            _disposed = true;
        }
    }
}
