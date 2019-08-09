using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace InstantMessage.Entities
{
    [Serializable]
    [DataContract]
    public struct Conversation
    {
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// 此属性仅用于P2P
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public ConversationType Type { get; set; }

        [DataMember]
        public List<Guid> Participants { get; set; }

        [DataMember]
        public DateTimeOffset Updated { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }
    }
}
