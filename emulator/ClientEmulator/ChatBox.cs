using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientEmulator.Config;
using ClientEmulator.Services;
using ClientEmulator.Services.Models;
using InstantMessage;
using InstantMessage.ViewModels;
using Newtonsoft.Json;

namespace ClientEmulator
{
    public partial class ChatBox : UserControl
    {
        public ChatHub _chatHub { get; set; }
        private readonly BizService _bizService;

        public static List<ChatBox> Chatboxs { get; set; } = new List<ChatBox>();

        public event Action<ConversationType, string> OnChatCreatedEvent;

        public ChatBox(UserInfo userInfo)
        {
            _chatHub = new ChatHub(userInfo);
            _bizService = new BizService(new HttpClient() { BaseAddress = new Uri(URLConfig.API_Host) });
            InitializeComponent();
            InitMessageBoxEvent();

        }

        private void InitMessageBoxEvent()
        {
            this._chatHub.OnSendMessageEvent += _chatHub_OnSendMessageEvent;
            this._chatHub.OnNewMessagesEvent += _chatHub_OnNewMessagesEvent;
            this._chatHub.OnNewMessageNotifyEvent += _chatHub_OnNewMessageNotifyEvent;
            this._chatHub.OnNewEventNotifyEvent += _chatHub_OnNewEventNotifyEvent;
            this.OnChatCreatedEvent += ChatBox_OnChatCreatedEvent;
            this._bizService.OnSuccessEvent += _bizService_OnSuccessEvent;
            this._bizService.OnErrorOrExceptionEvent += _bizService_OnErrorOrExceptionEvent;
        }

        private async void _bizService_OnErrorOrExceptionEvent(HttpResponseMessage obj)
        {
            var error = await obj.Content.ReadAsStringAsync();
            AddMessage("BizSystem:" + error);
        }

        private void _bizService_OnSuccessEvent(string obj)
        {
            AddMessage("BizSystem:" + obj);
        }

        private void _chatHub_OnNewMessageNotifyEvent(List<Notification.MessageNotifyDto> messageNotifyDto)
        {
            AddMessage("Recived-MessageNotifyDto:" + JsonConvert.SerializeObject(messageNotifyDto));
        }

        private void ChatBox_OnChatCreatedEvent(ConversationType type, string id)
        {
            this.lab_convarsationType.Text = type.ToString();
            this.lab_conversationId.Text = id;
        }

        private void _chatHub_OnNewEventNotifyEvent(List<Notification.EventNotifyDto> eventNotifyDto)
        {
            AddMessage("RecivedEventNotify:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + JsonConvert.SerializeObject(eventNotifyDto));
        }

        //发送消息
        private void _chatHub_OnSendMessageEvent(string reciverId, string message)
        {
            AddMessage("Send:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + message);
        }

        //收到消息
        private void _chatHub_OnNewMessagesEvent(List<ConversationVm> conversations)
        {
            var messages = conversations.SelectMany(u => u.MessageList).ToList();
            foreach (var message in messages)
            {
                AddMessage("Recived:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + JsonConvert.SerializeObject(message));
            }

        }

        public void DisplayConversationInfo(ConversationType type, string id)
        {
            this.lab_conversationId.Text = id;
            this.lab_convarsationType.Text = type.ToString();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var message = txt_message.Text;
            if (string.IsNullOrEmpty(message))
                return;
            if (UserInfo.LoginedUsers.Count <= 1)
            {
                MessageBox.Show("需要再登录一枚用户", "ERROR");
                return;
            }
            var otherUser = UserInfo.LoginedUsers.FirstOrDefault(u => u.UserId != this._chatHub._userInfo.UserId);
            try
            {
                var result = await this._chatHub.SendTextAsync(otherUser.UserId, message);
                AddMessage(result);
            }
            catch (Exception ex)
            {
                AddMessage(ex.ToString());
            }

        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            try
            {
                this._chatHub.ConnectAsync();
                btn_connect.Enabled = false;
                btn_disconnect.Enabled = true;
            }
            catch (Exception ex)
            {
                AddMessage(ex.ToString());
            }

        }

        private void btn_disconnect_Click(object sender, EventArgs e)
        {
            try
            {
                this._chatHub.DisConnectAsync();
                btn_connect.Enabled = true;
                btn_disconnect.Enabled = false;
            }
            catch (Exception ex)
            {
                AddMessage(ex.ToString());
            }

        }

        private async void btn_creategroup_Click(object sender, EventArgs e)
        {
            var otherUser = UserInfo.LoginedUsers.FirstOrDefault(u => u.UserId != this._chatHub._userInfo.UserId);
            var result = await this._chatHub.CreateGroupAsync(this._chatHub._userInfo, otherUser, "测试群组名称", "备注信息");
            foreach (var chatBox in Chatboxs)
            {
                chatBox.OnChatCreatedEvent?.Invoke(ConversationType.Group, result);
            }
            AddMessage(result);
        }

        private async void btn_quitGruop_Click(object sender, EventArgs e)
        {
            var response = await this._chatHub.QuitGroupAsync();
            AddMessage(response);
        }

        private async void btn_deleteGroup_Click(object sender, EventArgs e)
        {
            var response = await this._chatHub.DeleteGroupAsync();
            foreach (var chatBox in Chatboxs)
            {
                chatBox.DisplayConversationInfo(ChatHub.ConversationType, ChatHub.ConversationId.ToString());
            }
            AddMessage(response);
        }

        private async void btn_CreateConversation_Click(object sender, EventArgs e)
        {
            if (UserInfo.LoginedUsers.Count <= 1)
            {
                MessageBox.Show("需要再登录一枚用户", "ERROR");
                return;
            }
            var otherUser = UserInfo.LoginedUsers.FirstOrDefault(u => u.UserId != this._chatHub._userInfo.UserId);
            var convId = await this._chatHub.CreateConversationAsync(otherUser.UserId);
            AddMessage("ConversationId:P2P:" + convId);
            foreach (var chatBox in Chatboxs)
            {
                chatBox.OnChatCreatedEvent?.Invoke(ConversationType.P2P, convId);
            }

        }

        private void AddMessage(string message)
        {
            this.chat_messages.Text += message + Environment.NewLine + Environment.NewLine;
        }

        private async void btn_todo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txt_component.Text))
            {
                MessageBox.Show("ComponentId不能为空");
                return;
            }
            await this._bizService.GetPendingList(this._chatHub._userInfo.AccessToken, this.txt_component.Text);
        }

        private async void btn_complete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txt_component.Text))
            {
                MessageBox.Show("ComponentId不能为空");
                return;
            }
            await this._bizService.GetDoneList(this._chatHub._userInfo.AccessToken, this.txt_component.Text);
        }

        private async void btn_messageHistory_Click(object sender, EventArgs e)
        {
            var result = await this._chatHub.GetMessageHistoryAsync();
            AddMessage("History:" + result);
        }

        private async void btn_deleteP2pConversation_Click(object sender, EventArgs e)
        {
            if (ChatHub.ConversationType != ConversationType.P2P)
            {
                MessageBox.Show("当前不是P2p模式,请转变成p2p模式！");
                return;
            }

            var s = await this._chatHub.DeleteP2PConversationAsync();
            AddMessage("DELETE P2P Conversation:" + s);
        }
    }
}
