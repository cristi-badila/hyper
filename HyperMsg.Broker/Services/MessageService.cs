using System.Collections.Generic;
using System.Linq;
using HyperMsg.Broker.Data.Entities;
using HyperMsg.Broker.Data.Repositories;
using HyperMsg.Contracts;
using HyperMsg.Messages;

namespace HyperMsg.Broker.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public IEnumerable<Message> Get(string endPoint, string count)
        {
            int messageCount;

            if (!int.TryParse(count, out messageCount) || messageCount < 1) messageCount = 1;
            if (messageCount > 100) messageCount = 100;

            var entities = _messageRepository.Get(endPoint, messageCount);

            var messages = new List<Message>();

            foreach (var entity in entities)
            {
                var message = new BrokeredMessage
                {
                    Id = entity.MessageId,
                    Body = entity.Body,
                    EndPoint = entity.EndPoint
                };
                messages.Add(message);
            }

            return messages;
        }

        public void Post(Message message)
        {
            var entity = new MessageEntity
            {
                MessageId = message.Id,
                Body = message.Body,
                EndPoint = message.EndPoint
            };
            _messageRepository.Add(entity);
        }

        public void Acknowledge(Acknowledgement acknowlege)
        {
            if (acknowlege.IsAbandoned)
            {
                var entities = _messageRepository.Get(acknowlege.MessageIds).ToList();
                entities.ForEach(me => me.RetryCount++);
                _messageRepository.UpdateRetry(entities.ToArray());
            }
            else
            {
                _messageRepository.Remove(acknowlege.MessageIds);
            }
        }
    }
}
