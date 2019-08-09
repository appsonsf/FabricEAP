using ServiceFabricContrib;
using System;
using System.Runtime.Serialization;

namespace InstantMessage.Entities
{
    [Serializable]
    [DataContract]
    public struct ConversationMsg
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public Guid ConversationId { get; set; }

        [DataMember]
        public Guid SenderId { get; set; }

        [DataMember]
        public DateTimeOffset Time { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public ConversationMsgType Type { get; set; }

        public static ConversationMsg Create(Guid conversationId, Guid senderId, string content, ConversationMsgType type)
        {
            var msg = new ConversationMsg
            {
                Content = content,
                ConversationId = conversationId,
                Id = Guid.NewGuid(),
                SenderId = senderId,
                Time = DateTimeOffset.UtcNow,
                Type = type
            };
            return msg;
        }
    }
}