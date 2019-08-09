using System;
using System.Collections.Generic;
using System.Text;

namespace InstantMessage.ViewModels
{
    public class CreateP2PConversationVm
    {
        /// <summary>
        /// 发起方的Id(单点登录返回的Sub)
        /// </summary>
        public Guid SenderId { get; set; }
        /// <summary>
        /// 接收方的Id(单点登录返回的Sub)
        /// </summary>
        public Guid RecieverId { get; set; }
    }
}
