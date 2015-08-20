using System;

namespace HyperMsg.Broker.Data.Entities
{
    public class MessageEntity
    {
        public int Id { get; set; }
        public Guid MessageId { get; set; }
        public string Body { get; set; }
        public string EndPoint { get; set; }
        public short RetryLimit { get; set; }
        public short RetryCount { get; set; }
    }
}