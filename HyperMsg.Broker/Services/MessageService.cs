using System;
using System.Collections.Generic;
using System.Linq;
using HyperMsg.Broker.Data.Entities;
using HyperMsg.Broker.Data.Repositories;
using HyperMsg.Contracts;
using HyperMsg.Messages;

namespace HyperMsg.Broker.Services
{
    /// <summary>
    /// Defines the message service for handling messages.
    /// </summary>
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public MessageResponse<IEnumerable<Message>> Get(string endPoint, string count)
        {
            try
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
                        EndPoint = entity.EndPoint,
                        RetryLimit = entity.RetryLimit,
                    };
                    messages.Add(message);
                }

                return new MessageResponse<IEnumerable<Message>> {Body = messages};
            }
            catch (Exception error)
            {
                return new MessageResponse<IEnumerable<Message>> {Error = error.Message};
            }
        }

        public MessageResponse Post(Message message)
        {
            try
            {
                var entity = new MessageEntity
                {
                    MessageId = message.Id,
                    Body = message.Body,
                    EndPoint = message.EndPoint,
                    RetryLimit = message.RetryLimit
                };
                _messageRepository.Add(entity);

                return new MessageResponse();
            }
            catch (Exception error)
            {
                return new MessageResponse {Error = error.Message};
            }
        }

        public MessageResponse Acknowledge(Acknowledgement acknowlege)
        {
            try
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

                return new MessageResponse();
            }
            catch (Exception error)
            {
                return new MessageResponse {Error = error.Message};
            }
        }
    }
}
