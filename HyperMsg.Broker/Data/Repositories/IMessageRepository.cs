using System;
using System.Collections.Generic;
using HyperMsg.Broker.Data.Entities;

namespace HyperMsg.Broker.Data.Repositories
{
    public interface IMessageRepository
    {
        IEnumerable<MessageEntity> Get(params Guid[] messageIds);
        IEnumerable<MessageEntity> Get(string endpoint, int count);
        void Add(MessageEntity messageEntity);
        void Remove(params Guid[] messageIds);
        void Update(params MessageEntity[] entities);
    }
}