using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientEmulator.Config;
using ClientEmulator.Services;
using ClientEmulator.Services.Models;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;

namespace ClientEmulator
{
    public partial class IMChatForm : Form
    {

        public IMChatForm()
        {
            InitializeComponent();
            this.loginControl1.OnUserLoginSuccessEvent += LoginControl_OnUserLoginSuccessEvent;
            this.loginControl2.OnUserLoginSuccessEvent += LoginControl_OnUserLoginSuccessEvent;

            {
                this.loginControl1.txtUserName.Text = "003139";
                this.loginControl1.txtPassword.Text = "a123456";
                this.loginControl2.txtUserName.Text = "yangrugang";
                this.loginControl2.txtPassword.Text = "a123456";
            }
        }

        private void LoginControl_OnUserLoginSuccessEvent(object sender, UserInfo userinfo)
        {
            var tabPage = new TabPage();
            tabPage.Location = new Point(4, 25);
            tabPage.Padding = new Padding(3);
            tabPage.Size = new Size(953, 412);
            tabPage.TabIndex = 0;
            tabPage.Text = $"sender:{userinfo.UserName}";
            tabPage.UseVisualStyleBackColor = true;
            tabControl1.Controls.Add(tabPage);
            var chatBox = new ChatBox(userinfo);
            tabPage.Controls.Add(chatBox);
            ChatBox.Chatboxs.Add(chatBox);
        }

        //Apply URL Config
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLogin_host.Text)
            || string.IsNullOrEmpty(txtApi_host.Text)
            || string.IsNullOrEmpty(txtHub_host.Text))
            {
                MessageBox.Show("Ensure Address!", "ERROR");
                return;
            }

            URLConfig.API_Host = txtApi_host.Text;
            URLConfig.Login_Host = txtLogin_host.Text;
            URLConfig.Hub_Host = txtHub_host.Text;
            btnApply.Enabled = false;
            txtLogin_host.Enabled = false;
            txtApi_host.Enabled = false;
            txtHub_host.Enabled = false;
            userGroupConfig.Enabled = true;
        }
    }
}
