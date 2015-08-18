using System;

namespace HyperMsg.Controller.Data.Entities
{
    internal class MessageEntity
    {
        internal int Id { get; set; }
        internal Guid MessageId { get; set; }
        internal string Body { get; set; }
    }
}