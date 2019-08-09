using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientEmulator.Config;
using ClientEmulator.Services.Models;
using InstantMessage;
using InstantMessage.ViewModels;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Notification;
using Guid = System.Guid;

namespace ClientEmulator.Services
{
    public class ChatHub
    {
        //消息
        private static readonly string _receiveMessageNotify_methodName = "ReceiveMessageNotify";
        private static readonly string _receiveEventNotify = "ReceiveEventNotify";

        private HubConnection _connection { get; set; }

        /// <summary>
        /// 新消息提醒
        /// </summary>
        public delegate void OnNewMessagesDelegate(List<ConversationVm> conversations);
        public event OnNewMessagesDelegate OnNewMessagesEvent;

        /// <summary>
        /// 事件提醒
        /// </summary>
        public delegate void OnNewEventDelate(List<EventNotifyDto> eventNotifyDto);
        public event OnNewEventDelate OnNewEventNotifyEvent;

        public delegate void OnNewMessageEventDelate(List<MessageNotifyDto> messageNotifyDto);

        public event OnNewMessageEventDelate OnNewMessageNotifyEvent;

        public event Action<string, string> OnSendMessageEvent;

        private readonly ConversationService _conversationService;
        public readonly UserInfo _userInfo;

        public static Guid ConversationId { get; set; }

        public static ConversationType ConversationType { get; set; }

        public static Guid LastMsgId { get; set; }

        public ChatHub(UserInfo userInfo)
        {
            _userInfo = userInfo;
            _conversationService = new ConversationService(new HttpClient() { BaseAddress = new Uri(URLConfig.API_Host) });
        }

        public async void ConnectAsync()
        {
            var connection = new HubConnectionBuilder()
                .WithUrl(URLConfig.Hub_Host, option =>
                {
                    option.Transports = HttpTransportType.ServerSentEvents | HttpTransportType.WebSockets |
                                        HttpTransportType.LongPolling;
                    option.AccessTokenProvider = () => Task.FromResult(_userInfo.AccessToken);
                })
                .Build();
            connection.On<List<MessageNotifyDto>>(_receiveMessageNotify_methodName, ReceiveMessageNotify);
            connection.On<List<EventNotifyDto>>(_receiveEventNotify, ReceiveEventNotify);
            _connection = connection;
            await connection.StartAsync();
        }

        public async void DisConnectAsync()
        {
            await _connection.StopAsync();
        }

        /// <summary>
        /// 必须下创建会话,然后再来初始化
        /// </summary>
        public async Task<string> CreateConversationAsync(string reciverId)
        {
            try
            {
                var conversationId =
                    await this._conversationService.CreateConversationAsync(_userInfo.UserId, reciverId,
                        _userInfo.AccessToken);
                ConversationId = conversationId;
                ConversationType = ConversationType.P2P;
                return ConversationId.ToString();
            }
            catch (Exception e)
            {
                return e.ToString();
            }

        }

        public async Task<string> CreateGroupAsync(UserInfo owner, UserInfo partner, string name, string remark)
        {
            try
            {
                var response = await this._conversationService.CreateGroupAsync(owner, partner, name, remark);
                ConversationId = response.Data;
                ConversationType = ConversationType.Group;
                return JsonConvert.SerializeObject(response);
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public async Task<string> SendTextAsync(string reciverId, string message)
        {
            try
            {
                var result = await this._conversationService.SendTextAsync(ConversationId.ToString(), _userInfo.UserId,
                    message, _userInfo.AccessToken, ConversationType);
                if (result.Success)
                    this.OnSendMessageEvent?.Invoke(reciverId, message);
                return JsonConvert.SerializeObject(result);
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public async Task<string> GetMessageHistoryAsync()
        {
            try
            {
                var history = await this._conversationService.GetMessageHistryAsync(new MessageNotifyDto()
                {
                    LatestMsgId = LastMsgId,
                    Target = NotifyTargetType.Conversation,
                    TargetId = ConversationId.ToString(),
                    TargetCategory = (int)ConversationType
                }, this._userInfo.AccessToken);
                return JsonConvert.SerializeObject(history);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> DeleteP2PConversationAsync()
        {
            try
            {
                var result = await this._conversationService.DeleteP2PConversation(ConversationId, _userInfo.AccessToken);
                return result;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> QuitGroupAsync()
        {
            try
            {
                var response = await this._conversationService.QuitGroup(this._userInfo, ConversationId);
                return JsonConvert.SerializeObject(response);
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public async Task<string> DeleteGroupAsync()
        {
            try
            {
                var response = await this._conversationService.DeleteGroup(this._userInfo, ConversationId);
                return JsonConvert.SerializeObject(response);
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }


        private async void ReceiveMessageNotify(List<MessageNotifyDto> messages)
        {
            if (messages == null || messages.Count == 0)
                return;
            try
            {
                this.OnNewMessageNotifyEvent?.Invoke(messages);
                var conversationVms =
                    await this._conversationService.GetNewMessageListAsync(messages, _userInfo.AccessToken);
                var msg = conversationVms.SelectMany(u => u.MessageList).ToList();
                if (msg.Count > 0)
                    LastMsgId = msg.Last().Id;
                this.OnNewMessagesEvent?.Invoke(conversationVms);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR");
            }
        }

        private async void ReceiveEventNotify(List<EventNotifyDto> events)
        {
            this.OnNewEventNotifyEvent?.Invoke(events);
        }
    }
}
