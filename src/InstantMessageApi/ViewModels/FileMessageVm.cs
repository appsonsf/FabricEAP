using Newtonsoft.Json;
using System.Collections.Generic;

namespace InstantMessage.ViewModels
{
    public class FileMessageVm : MessageVmBase
    {
        /// <summary>
        /// 文件上传后返回的guid
        /// </summary>
        public string FileGuid { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }

        private Dictionary<string, object> GetContent()
        {
            var dic = new Dictionary<string, object>
            {
                { "FileGuid", FileGuid },
                { "FileName", FileName },
                { "FileSize", FileSize }
        };
            return dic;
        }

        /// <summary>
        /// 获取消息内容  ConversationMsg 中的Content 
        /// </summary>
        /// <returns></returns>
        public string GetJsonContent()
        {
            return JsonConvert.SerializeObject(GetContent());
        }
    }
}
