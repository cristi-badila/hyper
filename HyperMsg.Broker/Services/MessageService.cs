using System.Collections.Generic;
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

        public IEnumerable<Message> Get(string endpoint, string count)
        {
            int recordCount;

            if (!int.TryParse(count, out recordCount) || recordCount < 1) recordCount = 1;

            var entities = _messageRepository.Get(endpoint, recordCount);

            var messages = new List<Message>();

            foreach (var entity in entities)
            {
                var message = new BrokeredMessage
                {
                    Id = entity.MessageId,
                    Body = entity.Body,
                    EndPoint = entity.EndPoint,
                    Persistent = entity.Persistent
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
                EndPoint = message.EndPoint,
                Persistent = message.Persistent
            };
            _messageRepository.Add(entity);
        }

        public void Acknowledge(Acknowledgement acknowlege)
        {
            _messageRepository.Remove(acknowlege.MessageIds);
        }
    }
}
