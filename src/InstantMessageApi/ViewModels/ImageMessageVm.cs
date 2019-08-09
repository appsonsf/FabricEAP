using Newtonsoft.Json;
using System.Collections.Generic;

namespace InstantMessage.ViewModels
{
    public class ImageMessageVm : MessageVmBase
    {
        /// <summary>
        /// 上传图片返回的guid
        /// </summary>
        public string ImageGuid { get; set; }
        /// <summary>
        /// 图片的宽
        /// </summary>
        public int ImageWith { get; set; }
        /// <summary>
        /// 图片的高
        /// </summary>
        public int ImageHeight { get; set; }

        public string ImageName { get; set; }

        private Dictionary<string, object> GetContent()
        {
            var dic = new Dictionary<string, object>
            {
                { "ImageGuid", ImageGuid },
                { "ImageName", ImageName }
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
