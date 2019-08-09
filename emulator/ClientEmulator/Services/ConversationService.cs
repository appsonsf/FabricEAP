using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClientEmulator.Config;
using InstantMessage;
using InstantMessage.ViewModels;
using Newtonsoft.Json;
using Notification;

namespace ClientEmulator.Services
{
    public partial class ConversationService
    {
        private readonly HttpClient _httpClient;
        public ConversationService(HttpClient httpClient)
        {
            //httpClient.BaseAddress = new Uri(URLConfig.API_Host);
            _httpClient = httpClient;
        }

        /// <summary>
        /// create a p2p conversation
        /// </summary>
        /// <returns></returns>
        public async Task<Guid> CreateConversationAsync(string senderId, string reciverId, string accessToken)
        {
            //HttpClient must be Mananged,may be overflow!
            this._httpClient.SetBearerToken(accessToken);
            var param = new CreateP2PConversationVm()
            {
                SenderId = new Guid(senderId),
                RecieverId = new Guid(reciverId)
            };

            var response = await this._httpClient.PostAsync(BizUrlConfig.Create_Conversation_POST,
                new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, "application/json"));
            var str = await response.Content.ReadAsStringAsync();
            str = str.Replace("\"", "");
            return new Guid(str);
        }

        /// <summary>
        /// send a text message
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="senderId"></param>
        /// <param name="text"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<SendMessageResult> SendTextAsync(string conversationId, string senderId, string text, string accessToken, ConversationType type)
        {
            this._httpClient.SetBearerToken(accessToken);
            var param = new TextMessageVm()
            {
                Content = text,
                ConversationId = new Guid(conversationId),
                ConversationType = type,
                SenderId = new Guid(senderId),
            };
            var response = await this._httpClient.PostAsync(BizUrlConfig.Send_Text_POST,
                new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, "application/json"));
            var response_model = JsonConvert.DeserializeObject<SendMessageResult>(await response.Content.ReadAsStringAsync());
            return response_model;
        }

        public async Task<List<ConversationVm>> GetNewMessageListAsync(List<MessageNotifyDto> parma, string accessToken)
        {
            this._httpClient.SetBearerToken(accessToken);
            var response = await this._httpClient.PostAsync(BizUrlConfig.GetNewMessageList_POST,
                new StringContent(JsonConvert.SerializeObject(parma), Encoding.UTF8, "application/json"));
            var str = await response.Content.ReadAsStringAsync();
            var response_model =
                JsonConvert.DeserializeObject<List<ConversationVm>>(str);
            return response_model;
        }

        public async Task<List<MessageVm>> GetMessageHistryAsync(MessageNotifyDto parma, string accessToken)
        {
            this._httpClient.SetBearerToken(accessToken);
            var response = await this._httpClient.PostAsync(BizUrlConfig.GetMessageHistory_POST,
                new StringContent(JsonConvert.SerializeObject(parma), Encoding.UTF8, "application/json"));
            var str = await response.Content.ReadAsStringAsync();
            var response_model =
                JsonConvert.DeserializeObject<List<MessageVm>>(str);
            return response_model;
        }

        public async Task<string> DeleteP2PConversation(Guid conversationId, string accessToken)
        {
            this._httpClient.SetBearerToken(accessToken);
            var response = await this._httpClient.DeleteAsync(string.Format(BizUrlConfig.DeleteP2pConversation_DELETE, conversationId.ToString()));
            if (response.IsSuccessStatusCode)
                return "OK";
            return "BadRequest";
        }
    }
}
