using HyperMsg.Broker.Data.Entities;
using HyperMsg.Broker.Data.Repositories;
using Newtonsoft.Json;

namespace HyperMsg.Broker.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public void Post(Message message)
        {
            if (message.Persistent)
            {
                var entity = new MessageEntity
                {
                    MessageId = message.Id,
                    Body = JsonConvert.SerializeObject(message)
                };
                _messageRepository.Add(entity);
            }
        }
    }
}
