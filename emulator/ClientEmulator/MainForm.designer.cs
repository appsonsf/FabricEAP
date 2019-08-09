namespace ClientEmulator
{
    partial class MainForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.txtBaseAddress = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtAccessToken = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAuthority = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnSendNotifyMsg = new System.Windows.Forms.Button();
            this.txtNotifyMsgUserIds = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtNotifyMsgContent = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtNotifyMsgCategory = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtNotifyBizSystemId = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnConnectRabbitMq = new System.Windows.Forms.Button();
            this.txtRabbitMqPassword = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtRabbitMqUsername = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtRabbitMqAddress = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnTempToken = new System.Windows.Forms.Button();
            this.txtTempToken = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBaseAddress = new System.Windows.Forms.TextBox();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.TextFileId = new System.Windows.Forms.TextBox();
            this.TextDownloadMsg = new System.Windows.Forms.Label();
            this.TextUploadMsg = new System.Windows.Forms.Label();
            this.buttonUpload = new System.Windows.Forms.Button();
            this.buttonChoose = new System.Windows.Forms.Button();
            this.textGroupId = new System.Windows.Forms.TextBox();
            this.textLocation = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btnAttachment_UploadFile = new System.Windows.Forms.Button();
            this.btnAttachment_SelectFile = new System.Windows.Forms.Button();
            this.txtAttachment_FilePath = new System.Windows.Forms.TextBox();
            this.txtAttachment_ContextId = new System.Windows.Forms.TextBox();
            this.lblAttachment_ContextId = new System.Windows.Forms.Label();
            this.lblAttachment_FilePath = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.txtBaseAddress);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtAccessToken);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnLogin);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtAuthority);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtUsername);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(800, 128);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "登录";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(710, 22);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(68, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "IMChat";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtBaseAddress
            // 
            this.txtBaseAddress.Location = new System.Drawing.Point(109, 83);
            this.txtBaseAddress.Margin = new System.Windows.Forms.Padding(2);
            this.txtBaseAddress.MinimumSize = new System.Drawing.Size(302, 4);
            this.txtBaseAddress.Name = "txtBaseAddress";
            this.txtBaseAddress.Size = new System.Drawing.Size(670, 20);
            this.txtBaseAddress.TabIndex = 9;
            this.txtBaseAddress.Text = "http://localhost:10121/api/v1";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(36, 86);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(69, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "BaseAddress";
            // 
            // txtAccessToken
            // 
            this.txtAccessToken.Location = new System.Drawing.Point(109, 58);
            this.txtAccessToken.Name = "txtAccessToken";
            this.txtAccessToken.ReadOnly = true;
            this.txtAccessToken.Size = new System.Drawing.Size(670, 20);
            this.txtAccessToken.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "AccessToken：";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(630, 22);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_ClickAsync);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(512, 23);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(100, 20);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Text = "secret";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(477, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "密码";
            // 
            // txtAuthority
            // 
            this.txtAuthority.Location = new System.Drawing.Point(63, 22);
            this.txtAuthority.Name = "txtAuthority";
            this.txtAuthority.Size = new System.Drawing.Size(234, 20);
            this.txtAuthority.TabIndex = 1;
            this.txtAuthority.Text = "http://login.test.scrbg.com";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "地址";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(356, 23);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(100, 20);
            this.txtUsername.TabIndex = 1;
            this.txtUsername.Text = "user1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(312, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 128);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 322);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 296);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "发送业务消息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnSendNotifyMsg);
            this.groupBox3.Controls.Add(this.txtNotifyMsgUserIds);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.txtNotifyMsgContent);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.txtNotifyMsgCategory);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.txtNotifyBizSystemId);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Location = new System.Drawing.Point(390, 18);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(365, 125);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "BizSystemNotifyMsg";
            // 
            // btnSendNotifyMsg
            // 
            this.btnSendNotifyMsg.Location = new System.Drawing.Point(212, 17);
            this.btnSendNotifyMsg.Name = "btnSendNotifyMsg";
            this.btnSendNotifyMsg.Size = new System.Drawing.Size(147, 23);
            this.btnSendNotifyMsg.TabIndex = 4;
            this.btnSendNotifyMsg.Text = "SendNotifyMsg";
            this.btnSendNotifyMsg.UseVisualStyleBackColor = true;
            this.btnSendNotifyMsg.Click += new System.EventHandler(this.BtnSendNotifyMsg_Click);
            // 
            // txtNotifyMsgUserIds
            // 
            this.txtNotifyMsgUserIds.Location = new System.Drawing.Point(69, 97);
            this.txtNotifyMsgUserIds.Name = "txtNotifyMsgUserIds";
            this.txtNotifyMsgUserIds.Size = new System.Drawing.Size(290, 20);
            this.txtNotifyMsgUserIds.TabIndex = 3;
            this.txtNotifyMsgUserIds.Text = "aef621bb-bf4a-4a20-9b62-a8fd22c4aff1";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(8, 100);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(43, 13);
            this.label17.TabIndex = 2;
            this.label17.Text = "UserIds";
            // 
            // txtNotifyMsgContent
            // 
            this.txtNotifyMsgContent.Location = new System.Drawing.Point(69, 71);
            this.txtNotifyMsgContent.Name = "txtNotifyMsgContent";
            this.txtNotifyMsgContent.Size = new System.Drawing.Size(290, 20);
            this.txtNotifyMsgContent.TabIndex = 3;
            this.txtNotifyMsgContent.Text = "you have a todo item";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(8, 74);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(44, 13);
            this.label16.TabIndex = 2;
            this.label16.Text = "Content";
            // 
            // txtNotifyMsgCategory
            // 
            this.txtNotifyMsgCategory.Location = new System.Drawing.Point(69, 45);
            this.txtNotifyMsgCategory.Name = "txtNotifyMsgCategory";
            this.txtNotifyMsgCategory.Size = new System.Drawing.Size(137, 20);
            this.txtNotifyMsgCategory.TabIndex = 3;
            this.txtNotifyMsgCategory.Text = "1";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(8, 48);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(49, 13);
            this.label15.TabIndex = 2;
            this.label15.Text = "Category";
            // 
            // txtNotifyBizSystemId
            // 
            this.txtNotifyBizSystemId.Location = new System.Drawing.Point(69, 19);
            this.txtNotifyBizSystemId.Name = "txtNotifyBizSystemId";
            this.txtNotifyBizSystemId.Size = new System.Drawing.Size(137, 20);
            this.txtNotifyBizSystemId.TabIndex = 3;
            this.txtNotifyBizSystemId.Text = "oa";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(8, 22);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(50, 13);
            this.label14.TabIndex = 2;
            this.label14.Text = "SystemId";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnConnectRabbitMq);
            this.groupBox2.Controls.Add(this.txtRabbitMqPassword);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.txtRabbitMqUsername);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.txtRabbitMqAddress);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Location = new System.Drawing.Point(13, 18);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(358, 125);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RabbitMQ";
            // 
            // btnConnectRabbitMq
            // 
            this.btnConnectRabbitMq.Location = new System.Drawing.Point(261, 95);
            this.btnConnectRabbitMq.Name = "btnConnectRabbitMq";
            this.btnConnectRabbitMq.Size = new System.Drawing.Size(75, 23);
            this.btnConnectRabbitMq.TabIndex = 2;
            this.btnConnectRabbitMq.Text = "Connect";
            this.btnConnectRabbitMq.UseVisualStyleBackColor = true;
            this.btnConnectRabbitMq.Click += new System.EventHandler(this.BtnConnectRabbitMq_Click);
            // 
            // txtRabbitMqPassword
            // 
            this.txtRabbitMqPassword.Location = new System.Drawing.Point(74, 68);
            this.txtRabbitMqPassword.Name = "txtRabbitMqPassword";
            this.txtRabbitMqPassword.Size = new System.Drawing.Size(263, 20);
            this.txtRabbitMqPassword.TabIndex = 1;
            this.txtRabbitMqPassword.Text = "guest";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(13, 71);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Password";
            // 
            // txtRabbitMqUsername
            // 
            this.txtRabbitMqUsername.Location = new System.Drawing.Point(74, 42);
            this.txtRabbitMqUsername.Name = "txtRabbitMqUsername";
            this.txtRabbitMqUsername.Size = new System.Drawing.Size(263, 20);
            this.txtRabbitMqUsername.TabIndex = 1;
            this.txtRabbitMqUsername.Text = "guest";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(13, 45);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(55, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Username";
            // 
            // txtRabbitMqAddress
            // 
            this.txtRabbitMqAddress.Location = new System.Drawing.Point(74, 16);
            this.txtRabbitMqAddress.Name = "txtRabbitMqAddress";
            this.txtRabbitMqAddress.Size = new System.Drawing.Size(263, 20);
            this.txtRabbitMqAddress.TabIndex = 1;
            this.txtRabbitMqAddress.Text = "rabbitmq://localhost";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 19);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(45, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Address";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnTempToken);
            this.tabPage2.Controls.Add(this.txtTempToken);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 296);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "获取临时密码";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnTempToken
            // 
            this.btnTempToken.Location = new System.Drawing.Point(405, 15);
            this.btnTempToken.Name = "btnTempToken";
            this.btnTempToken.Size = new System.Drawing.Size(120, 23);
            this.btnTempToken.TabIndex = 7;
            this.btnTempToken.Text = "请求TempToken";
            this.btnTempToken.UseVisualStyleBackColor = true;
            this.btnTempToken.Click += new System.EventHandler(this.btnTempToken_Click);
            // 
            // txtTempToken
            // 
            this.txtTempToken.Location = new System.Drawing.Point(92, 17);
            this.txtTempToken.Name = "txtTempToken";
            this.txtTempToken.ReadOnly = true;
            this.txtTempToken.Size = new System.Drawing.Size(291, 20);
            this.txtTempToken.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "临时密码";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBaseAddress);
            this.tabPage3.Controls.Add(this.buttonDownload);
            this.tabPage3.Controls.Add(this.TextFileId);
            this.tabPage3.Controls.Add(this.TextDownloadMsg);
            this.tabPage3.Controls.Add(this.TextUploadMsg);
            this.tabPage3.Controls.Add(this.buttonUpload);
            this.tabPage3.Controls.Add(this.buttonChoose);
            this.tabPage3.Controls.Add(this.textGroupId);
            this.tabPage3.Controls.Add(this.textLocation);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage3.Size = new System.Drawing.Size(792, 296);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "群文件";
            this.tabPage3.UseVisualStyleBackColor = true;
            this.tabPage3.Click += new System.EventHandler(this.tabPage3_Click);
            // 
            // textBaseAddress
            // 
            this.textBaseAddress.Location = new System.Drawing.Point(74, 12);
            this.textBaseAddress.Margin = new System.Windows.Forms.Padding(2);
            this.textBaseAddress.MinimumSize = new System.Drawing.Size(302, 4);
            this.textBaseAddress.Name = "textBaseAddress";
            this.textBaseAddress.Size = new System.Drawing.Size(302, 20);
            this.textBaseAddress.TabIndex = 8;
            // 
            // buttonDownload
            // 
            this.buttonDownload.Location = new System.Drawing.Point(390, 140);
            this.buttonDownload.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(206, 28);
            this.buttonDownload.TabIndex = 7;
            this.buttonDownload.Text = "Download";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // TextFileId
            // 
            this.TextFileId.Location = new System.Drawing.Point(72, 146);
            this.TextFileId.Margin = new System.Windows.Forms.Padding(2);
            this.TextFileId.MinimumSize = new System.Drawing.Size(302, 4);
            this.TextFileId.Name = "TextFileId";
            this.TextFileId.Size = new System.Drawing.Size(302, 20);
            this.TextFileId.TabIndex = 6;
            // 
            // TextDownloadMsg
            // 
            this.TextDownloadMsg.AutoSize = true;
            this.TextDownloadMsg.Location = new System.Drawing.Point(13, 176);
            this.TextDownloadMsg.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TextDownloadMsg.MinimumSize = new System.Drawing.Size(400, 31);
            this.TextDownloadMsg.Name = "TextDownloadMsg";
            this.TextDownloadMsg.Size = new System.Drawing.Size(400, 31);
            this.TextDownloadMsg.TabIndex = 5;
            // 
            // TextUploadMsg
            // 
            this.TextUploadMsg.AutoSize = true;
            this.TextUploadMsg.Location = new System.Drawing.Point(13, 90);
            this.TextUploadMsg.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TextUploadMsg.MinimumSize = new System.Drawing.Size(400, 31);
            this.TextUploadMsg.Name = "TextUploadMsg";
            this.TextUploadMsg.Size = new System.Drawing.Size(400, 31);
            this.TextUploadMsg.TabIndex = 5;
            // 
            // buttonUpload
            // 
            this.buttonUpload.Location = new System.Drawing.Point(498, 58);
            this.buttonUpload.Margin = new System.Windows.Forms.Padding(2);
            this.buttonUpload.Name = "buttonUpload";
            this.buttonUpload.Size = new System.Drawing.Size(97, 28);
            this.buttonUpload.TabIndex = 4;
            this.buttonUpload.Text = "上传";
            this.buttonUpload.UseVisualStyleBackColor = true;
            this.buttonUpload.Click += new System.EventHandler(this.buttonUpload_Click);
            // 
            // buttonChoose
            // 
            this.buttonChoose.Location = new System.Drawing.Point(390, 58);
            this.buttonChoose.Margin = new System.Windows.Forms.Padding(2);
            this.buttonChoose.Name = "buttonChoose";
            this.buttonChoose.Size = new System.Drawing.Size(82, 28);
            this.buttonChoose.TabIndex = 3;
            this.buttonChoose.Text = "选择文件";
            this.buttonChoose.UseVisualStyleBackColor = true;
            this.buttonChoose.Click += new System.EventHandler(this.buttonChoose_Click);
            // 
            // textGroupId
            // 
            this.textGroupId.Location = new System.Drawing.Point(72, 63);
            this.textGroupId.Margin = new System.Windows.Forms.Padding(2);
            this.textGroupId.MinimumSize = new System.Drawing.Size(302, 4);
            this.textGroupId.Name = "textGroupId";
            this.textGroupId.Size = new System.Drawing.Size(302, 20);
            this.textGroupId.TabIndex = 2;
            // 
            // textLocation
            // 
            this.textLocation.AutoSize = true;
            this.textLocation.Location = new System.Drawing.Point(74, 40);
            this.textLocation.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.textLocation.MinimumSize = new System.Drawing.Size(300, 0);
            this.textLocation.Name = "textLocation";
            this.textLocation.Size = new System.Drawing.Size(300, 13);
            this.textLocation.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 148);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "文件Id";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 65);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "群组Id";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 12);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "地址：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 40);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "文件地址：";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.btnAttachment_UploadFile);
            this.tabPage4.Controls.Add(this.btnAttachment_SelectFile);
            this.tabPage4.Controls.Add(this.txtAttachment_FilePath);
            this.tabPage4.Controls.Add(this.txtAttachment_ContextId);
            this.tabPage4.Controls.Add(this.lblAttachment_ContextId);
            this.tabPage4.Controls.Add(this.lblAttachment_FilePath);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(792, 296);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "附件";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // btnAttachment_UploadFile
            // 
            this.btnAttachment_UploadFile.Location = new System.Drawing.Point(124, 101);
            this.btnAttachment_UploadFile.Margin = new System.Windows.Forms.Padding(2);
            this.btnAttachment_UploadFile.Name = "btnAttachment_UploadFile";
            this.btnAttachment_UploadFile.Size = new System.Drawing.Size(97, 28);
            this.btnAttachment_UploadFile.TabIndex = 9;
            this.btnAttachment_UploadFile.Text = "上传";
            this.btnAttachment_UploadFile.UseVisualStyleBackColor = true;
            this.btnAttachment_UploadFile.Click += new System.EventHandler(this.BtnAttachment_UploadFile_Click);
            // 
            // btnAttachment_SelectFile
            // 
            this.btnAttachment_SelectFile.Location = new System.Drawing.Point(16, 101);
            this.btnAttachment_SelectFile.Margin = new System.Windows.Forms.Padding(2);
            this.btnAttachment_SelectFile.Name = "btnAttachment_SelectFile";
            this.btnAttachment_SelectFile.Size = new System.Drawing.Size(82, 28);
            this.btnAttachment_SelectFile.TabIndex = 8;
            this.btnAttachment_SelectFile.Text = "选择文件";
            this.btnAttachment_SelectFile.UseVisualStyleBackColor = true;
            this.btnAttachment_SelectFile.Click += new System.EventHandler(this.BtnAttachment_SelectFile_Click);
            // 
            // txtAttachment_FilePath
            // 
            this.txtAttachment_FilePath.Location = new System.Drawing.Point(84, 67);
            this.txtAttachment_FilePath.Margin = new System.Windows.Forms.Padding(2);
            this.txtAttachment_FilePath.MinimumSize = new System.Drawing.Size(302, 4);
            this.txtAttachment_FilePath.Name = "txtAttachment_FilePath";
            this.txtAttachment_FilePath.Size = new System.Drawing.Size(302, 20);
            this.txtAttachment_FilePath.TabIndex = 7;
            // 
            // txtAttachment_ContextId
            // 
            this.txtAttachment_ContextId.Location = new System.Drawing.Point(84, 34);
            this.txtAttachment_ContextId.Margin = new System.Windows.Forms.Padding(2);
            this.txtAttachment_ContextId.MinimumSize = new System.Drawing.Size(302, 4);
            this.txtAttachment_ContextId.Name = "txtAttachment_ContextId";
            this.txtAttachment_ContextId.Size = new System.Drawing.Size(302, 20);
            this.txtAttachment_ContextId.TabIndex = 7;
            // 
            // lblAttachment_ContextId
            // 
            this.lblAttachment_ContextId.AutoSize = true;
            this.lblAttachment_ContextId.Location = new System.Drawing.Point(11, 37);
            this.lblAttachment_ContextId.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAttachment_ContextId.Name = "lblAttachment_ContextId";
            this.lblAttachment_ContextId.Size = new System.Drawing.Size(52, 13);
            this.lblAttachment_ContextId.TabIndex = 5;
            this.lblAttachment_ContextId.Text = "上下文Id";
            // 
            // lblAttachment_FilePath
            // 
            this.lblAttachment_FilePath.AutoSize = true;
            this.lblAttachment_FilePath.Location = new System.Drawing.Point(13, 70);
            this.lblAttachment_FilePath.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAttachment_FilePath.Name = "lblAttachment_FilePath";
            this.lblAttachment_FilePath.Size = new System.Drawing.Size(61, 13);
            this.lblAttachment_FilePath.TabIndex = 6;
            this.lblAttachment_FilePath.Text = "文件地址：";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtAuthority;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnTempToken;
        private System.Windows.Forms.TextBox txtTempToken;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAccessToken;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label textLocation;
        private System.Windows.Forms.TextBox textGroupId;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button buttonChoose;
        private System.Windows.Forms.Button buttonUpload;
        private System.Windows.Forms.Label TextUploadMsg;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TextFileId;
        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.Label TextDownloadMsg;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBaseAddress;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button btnAttachment_UploadFile;
        private System.Windows.Forms.Button btnAttachment_SelectFile;
        private System.Windows.Forms.TextBox txtAttachment_FilePath;
        private System.Windows.Forms.TextBox txtAttachment_ContextId;
        private System.Windows.Forms.Label lblAttachment_ContextId;
        private System.Windows.Forms.Label lblAttachment_FilePath;
        private System.Windows.Forms.TextBox txtBaseAddress;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnConnectRabbitMq;
        private System.Windows.Forms.TextBox txtRabbitMqPassword;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtRabbitMqUsername;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtRabbitMqAddress;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnSendNotifyMsg;
        private System.Windows.Forms.TextBox txtNotifyMsgUserIds;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtNotifyMsgContent;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtNotifyMsgCategory;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtNotifyBizSystemId;
        private System.Windows.Forms.Label label14;
    }
}

