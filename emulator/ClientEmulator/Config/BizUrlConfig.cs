using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientEmulator.Config
{
    public class BizUrlConfig
    {
        /// <summary>
        /// the url of create_conversation
        /// </summary>
        public const string Create_Conversation_POST = "/api/v1/im/p2p";

        /// <summary>
        /// the url of send_text
        /// </summary>
        public const string Send_Text_POST = "api/v1/im/message/sendText";

        /// <summary>
        /// the url of get_new_messages
        /// </summary>
        public const string GetNewMessageList_POST = "/api/v1/im/message/getNew";

        public const string GetMessageHistory_POST = "/api/v1/im/message/getHistory";
        public const string DeleteP2pConversation_DELETE = "/api/v1/im/p2p/{0}";

        public const string CreateGroup_POST = "/api/v1/group";
        public const string QuitGroup_PUT = "/api/v1/group/{0}/quit";
        public const string DeleteGroup_DELETE = "/api/v1/group/{0}";


        public const string PUll_TODO_GET = "/api/v1/appcom/todo/Pending";
        public const string PUll_DONE_GET = "/api/v1/appcom/todo/Done";

    }
}
