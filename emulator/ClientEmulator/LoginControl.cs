using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientEmulator.Services;
using ClientEmulator.Services.Models;

namespace ClientEmulator
{
    public partial class LoginControl : UserControl
    {
        private readonly AuthenticationService _authenticationService;

        public event Action<object, UserInfo> OnUserLoginSuccessEvent;

        public LoginControl()
        {
            InitializeComponent();
            this._authenticationService = new AuthenticationService();
        }

        private async void btn_login_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var userName = txtUserName.Text;
            var password = txtPassword.Text;
            if (string.IsNullOrEmpty(userName) ||
                string.IsNullOrEmpty(password))
            {
                MessageBox.Show("请输入正确的用户名和密码", "ERROR");
                return;
            }
            button.Enabled = false;
            button.Text = "正在登录...";
            try
            {
                var userinfo = await this._authenticationService.LoginAsync(userName.Trim(), password.Trim());
                UserInfo.LoginedUsers.Add(userinfo);
                txtUserName.Enabled = false;
                txtPassword.Enabled = false;
                txtToken.Text = userinfo.AccessToken;
                button.Text = "Login";
                lab_Status.Text = "Success";
                lab_Status.BackColor = Color.MediumSeaGreen;
                OnUserLoginSuccessEvent?.Invoke(this, userinfo);
            }
            catch (Exception ex)
            {
                lab_Status.Text = "ERROR";
                lab_Status.BackColor = Color.Red;
                button.Enabled = true;
                button.Text = "Login";
                txtUserName.Enabled = true;
                txtPassword.Enabled = true;
                MessageBox.Show(ex.ToString(), "ERROR");
            }
        }
    }
}
