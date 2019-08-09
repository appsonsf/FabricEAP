using InstantMessage.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstantMessage.ViewModels
{
    public class MessageVm
    {
        /// <summary>
        /// 会话id
        /// </summary>
        public Guid ConversationId { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTimeOffset SendTime { get; set; }
        /// <summary>
        /// 消息Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 发送者id
        /// </summary>
        public Guid SenderId { get; set; }
        /// <summary>
        /// 发送者姓名
        /// </summary>
        public string SenderName { get; set; }
        /// <summary>
        /// 发送者性别
        /// </summary>
        public int SenderGender { get; set; }
        /// <summary>
        /// 消息内容 json
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public ConversationMsgType Type { get; set; }

        public static MessageVm Create(ConversationMsg msg)
        {
            var model = new MessageVm
            {
                Id = msg.Id,
                SendTime = msg.Time,
                Type = msg.Type,
                SenderId = msg.SenderId,
                ConversationId = msg.ConversationId
            };
            switch (msg.Type)
            {
                case ConversationMsgType.Text:
                    model.Message = JsonConvert.SerializeObject(new TextMessageVm { Content = msg.Content });
                    break;
                default:
                    model.Message = msg.Content;
                    break;
            }
            return model;
        }

        public MessageVm SetSenderInfo(string senderName, int senderGender)
        {
            SenderName = senderName;
            SenderGender = senderGender;
            return this;
        }
    }
}
