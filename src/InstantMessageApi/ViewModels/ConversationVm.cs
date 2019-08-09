using InstantMessage.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstantMessage.ViewModels
{
    public class ConversationVm
    {
        /// <summary>
        /// 会话id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 会话topic
        /// </summary>
        public string Topic { get; set; }
        /// <summary>
        /// 会话头像
        /// </summary>
        //NOTE 通过Group接口获取 public string Image { get; set; }

        /// <summary>
        /// 会话类型
        /// </summary>
        public ConversationType ConversationType { get; set; }

        /// <summary>
        /// 会话消息
        /// </summary>
        public List<MessageVm> MessageList { get; set; }


        public static ConversationVm Create(Conversation conversation)
        {

            return new ConversationVm
            {
                Id = conversation.Id,
                ConversationType = conversation.Type,
                MessageList = new List<MessageVm>()
            };
        }
    }
}
