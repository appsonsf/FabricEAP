namespace ClientEmulator
{
    partial class IMChatForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtHub_host = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.txtApi_host = new System.Windows.Forms.TextBox();
            this.txtLogin_host = new System.Windows.Forms.TextBox();
            this.Label_APIHost = new System.Windows.Forms.Label();
            this.Label_Host = new System.Windows.Forms.Label();
            this.userGroupConfig = new System.Windows.Forms.GroupBox();
            this.groupuser2 = new System.Windows.Forms.GroupBox();
            this.loginControl2 = new ClientEmulator.LoginControl();
            this.groupuser1 = new System.Windows.Forms.GroupBox();
            this.loginControl1 = new ClientEmulator.LoginControl();
            this.btn_CreateCovnertGroup = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.groupBox1.SuspendLayout();
            this.userGroupConfig.SuspendLayout();
            this.groupuser2.SuspendLayout();
            this.groupuser1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtHub_host);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.btnApply);
            this.groupBox1.Controls.Add(this.txtApi_host);
            this.groupBox1.Controls.Add(this.txtLogin_host);
            this.groupBox1.Controls.Add(this.Label_APIHost);
            this.groupBox1.Controls.Add(this.Label_Host);
            this.groupBox1.Location = new System.Drawing.Point(10, 11);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox1.Size = new System.Drawing.Size(221, 147);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "BaseConfig";
            // 
            // txtHub_host
            // 
            this.txtHub_host.Location = new System.Drawing.Point(75, 93);
            this.txtHub_host.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtHub_host.Name = "txtHub_host";
            this.txtHub_host.Size = new System.Drawing.Size(138, 20);
            this.txtHub_host.TabIndex = 6;
            this.txtHub_host.Text = "http://localhost:10124/notifyHub";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 96);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Hub_Host";
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(156, 120);
            this.btnApply.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(56, 20);
            this.btnApply.TabIndex = 4;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtApi_host
            // 
            this.txtApi_host.Location = new System.Drawing.Point(75, 65);
            this.txtApi_host.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtApi_host.Name = "txtApi_host";
            this.txtApi_host.Size = new System.Drawing.Size(138, 20);
            this.txtApi_host.TabIndex = 3;
            this.txtApi_host.Text = "http://localhost:10121";
            // 
            // txtLogin_host
            // 
            this.txtLogin_host.Location = new System.Drawing.Point(75, 38);
            this.txtLogin_host.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtLogin_host.Name = "txtLogin_host";
            this.txtLogin_host.Size = new System.Drawing.Size(138, 20);
            this.txtLogin_host.TabIndex = 2;
            this.txtLogin_host.Text = "http://login.test.scrbg.com";
            // 
            // Label_APIHost
            // 
            this.Label_APIHost.AutoSize = true;
            this.Label_APIHost.Location = new System.Drawing.Point(4, 65);
            this.Label_APIHost.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label_APIHost.Name = "Label_APIHost";
            this.Label_APIHost.Size = new System.Drawing.Size(52, 13);
            this.Label_APIHost.TabIndex = 1;
            this.Label_APIHost.Text = "API_Host";
            // 
            // Label_Host
            // 
            this.Label_Host.AutoSize = true;
            this.Label_Host.Location = new System.Drawing.Point(4, 38);
            this.Label_Host.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label_Host.Name = "Label_Host";
            this.Label_Host.Size = new System.Drawing.Size(61, 13);
            this.Label_Host.TabIndex = 0;
            this.Label_Host.Text = "Login_Host";
            // 
            // userGroupConfig
            // 
            this.userGroupConfig.Controls.Add(this.groupuser2);
            this.userGroupConfig.Controls.Add(this.groupuser1);
            this.userGroupConfig.Enabled = false;
            this.userGroupConfig.Location = new System.Drawing.Point(10, 183);
            this.userGroupConfig.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.userGroupConfig.Name = "userGroupConfig";
            this.userGroupConfig.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.userGroupConfig.Size = new System.Drawing.Size(253, 288);
            this.userGroupConfig.TabIndex = 1;
            this.userGroupConfig.TabStop = false;
            this.userGroupConfig.Text = "UserConfig";
            // 
            // groupuser2
            // 
            this.groupuser2.Controls.Add(this.loginControl2);
            this.groupuser2.Location = new System.Drawing.Point(6, 148);
            this.groupuser2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupuser2.Name = "groupuser2";
            this.groupuser2.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupuser2.Size = new System.Drawing.Size(242, 128);
            this.groupuser2.TabIndex = 1;
            this.groupuser2.TabStop = false;
            this.groupuser2.Text = "User2";
            // 
            // loginControl2
            // 
            this.loginControl2.Location = new System.Drawing.Point(5, 13);
            this.loginControl2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.loginControl2.Name = "loginControl2";
            this.loginControl2.Size = new System.Drawing.Size(222, 110);
            this.loginControl2.TabIndex = 0;
            // 
            // groupuser1
            // 
            this.groupuser1.Controls.Add(this.loginControl1);
            this.groupuser1.Location = new System.Drawing.Point(7, 15);
            this.groupuser1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupuser1.Name = "groupuser1";
            this.groupuser1.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupuser1.Size = new System.Drawing.Size(242, 128);
            this.groupuser1.TabIndex = 0;
            this.groupuser1.TabStop = false;
            this.groupuser1.Text = "User1";
            // 
            // loginControl1
            // 
            this.loginControl1.Location = new System.Drawing.Point(0, 12);
            this.loginControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.loginControl1.Name = "loginControl1";
            this.loginControl1.Size = new System.Drawing.Size(226, 111);
            this.loginControl1.TabIndex = 0;
            // 
            // btn_CreateCovnertGroup
            // 
            this.btn_CreateCovnertGroup.Enabled = false;
            this.btn_CreateCovnertGroup.Location = new System.Drawing.Point(269, 20);
            this.btn_CreateCovnertGroup.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_CreateCovnertGroup.Name = "btn_CreateCovnertGroup";
            this.btn_CreateCovnertGroup.Size = new System.Drawing.Size(158, 27);
            this.btn_CreateCovnertGroup.TabIndex = 3;
            this.btn_CreateCovnertGroup.Text = "CreateGroup";
            this.btn_CreateCovnertGroup.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Location = new System.Drawing.Point(269, 52);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(854, 419);
            this.tabControl1.TabIndex = 4;
            // 
            // IMChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1132, 483);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btn_CreateCovnertGroup);
            this.Controls.Add(this.userGroupConfig);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "IMChatForm";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.userGroupConfig.ResumeLayout(false);
            this.groupuser2.ResumeLayout(false);
            this.groupuser1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label Label_APIHost;
        private System.Windows.Forms.Label Label_Host;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.TextBox txtApi_host;
        private System.Windows.Forms.TextBox txtLogin_host;
        private System.Windows.Forms.GroupBox userGroupConfig;
        private System.Windows.Forms.GroupBox groupuser2;
        private System.Windows.Forms.GroupBox groupuser1;
        private System.Windows.Forms.TextBox txtHub_host;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_CreateCovnertGroup;
        private LoginControl loginControl2;
        private LoginControl loginControl1;
        private System.Windows.Forms.TabControl tabControl1;
    }
}

