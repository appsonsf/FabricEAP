using Newtonsoft.Json;
using System.Collections.Generic;

namespace InstantMessage.ViewModels
{
    public class VoiceMessageVm : MessageVmBase
    {
        /// <summary>
        /// 语音上传后返回的guid
        /// </summary>
        public string VoiceGuid { get; set; }
        /// <summary>
        /// 语音长度 单位s
        /// </summary>
        public double Duration { get; set; }

        public string FileName { get; set; }

        private Dictionary<string, object> GetContent()
        {
            var dic = new Dictionary<string, object>
            {
                { "VoiceGuid", VoiceGuid },
                { "Duration", Duration },
                { "FileName", FileName }
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
