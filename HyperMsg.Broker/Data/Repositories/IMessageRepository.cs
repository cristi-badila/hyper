using System;
using System.Collections.Generic;
using HyperMsg.Broker.Data.Entities;

namespace HyperMsg.Broker.Data.Repositories
{
    public interface IMessageRepository
    {
        MessageEntity Get(Guid id);
        IEnumerable<MessageEntity> Get(int count);
        void Add(MessageEntity messageEntity);
        void Remove(Guid id);
    }
}