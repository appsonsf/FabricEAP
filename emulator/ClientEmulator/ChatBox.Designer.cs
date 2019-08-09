using System.Windows.Forms;

namespace ClientEmulator
{
    partial class ChatBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chat_messages = new System.Windows.Forms.RichTextBox();
            this.txt_message = new System.Windows.Forms.TextBox();
            this.btn_sendText = new System.Windows.Forms.Button();
            this.btn_connect = new System.Windows.Forms.Button();
            this.btn_disconnect = new System.Windows.Forms.Button();
            this.btn_creategroup = new System.Windows.Forms.Button();
            this.btn_quitGruop = new System.Windows.Forms.Button();
            this.btn_deleteGroup = new System.Windows.Forms.Button();
            this.btn_CreateConversation = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lab_convarsationType = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lab_conversationId = new System.Windows.Forms.Label();
            this.btn_todo = new System.Windows.Forms.Button();
            this.btn_complete = new System.Windows.Forms.Button();
            this.txt_component = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_messageHistory = new System.Windows.Forms.Button();
            this.btn_deleteP2pConversation = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chat_messages
            // 
            this.chat_messages.Location = new System.Drawing.Point(6, 30);
            this.chat_messages.Name = "chat_messages";
            this.chat_messages.Size = new System.Drawing.Size(1102, 320);
            this.chat_messages.TabIndex = 0;
            this.chat_messages.Text = "";
            // 
            // txt_message
            // 
            this.txt_message.Location = new System.Drawing.Point(8, 355);
            this.txt_message.Name = "txt_message";
            this.txt_message.Size = new System.Drawing.Size(458, 25);
            this.txt_message.TabIndex = 1;
            // 
            // btn_sendText
            // 
            this.btn_sendText.Location = new System.Drawing.Point(473, 356);
            this.btn_sendText.Name = "btn_sendText";
            this.btn_sendText.Size = new System.Drawing.Size(75, 23);
            this.btn_sendText.TabIndex = 2;
            this.btn_sendText.Text = "SendText";
            this.btn_sendText.UseVisualStyleBackColor = true;
            this.btn_sendText.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_connect
            // 
            this.btn_connect.Location = new System.Drawing.Point(7, 4);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(75, 23);
            this.btn_connect.TabIndex = 3;
            this.btn_connect.Text = "Connect";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // btn_disconnect
            // 
            this.btn_disconnect.Enabled = false;
            this.btn_disconnect.Location = new System.Drawing.Point(88, 4);
            this.btn_disconnect.Name = "btn_disconnect";
            this.btn_disconnect.Size = new System.Drawing.Size(75, 23);
            this.btn_disconnect.TabIndex = 4;
            this.btn_disconnect.Text = "disconnect";
            this.btn_disconnect.UseVisualStyleBackColor = true;
            this.btn_disconnect.Click += new System.EventHandler(this.btn_disconnect_Click);
            // 
            // btn_creategroup
            // 
            this.btn_creategroup.Location = new System.Drawing.Point(786, 354);
            this.btn_creategroup.Name = "btn_creategroup";
            this.btn_creategroup.Size = new System.Drawing.Size(107, 23);
            this.btn_creategroup.TabIndex = 5;
            this.btn_creategroup.Text = "CreateGroup";
            this.btn_creategroup.UseVisualStyleBackColor = true;
            this.btn_creategroup.Click += new System.EventHandler(this.btn_creategroup_Click);
            // 
            // btn_quitGruop
            // 
            this.btn_quitGruop.Location = new System.Drawing.Point(900, 354);
            this.btn_quitGruop.Name = "btn_quitGruop";
            this.btn_quitGruop.Size = new System.Drawing.Size(87, 23);
            this.btn_quitGruop.TabIndex = 6;
            this.btn_quitGruop.Text = "QuitGruop";
            this.btn_quitGruop.UseVisualStyleBackColor = true;
            this.btn_quitGruop.Click += new System.EventHandler(this.btn_quitGruop_Click);
            // 
            // btn_deleteGroup
            // 
            this.btn_deleteGroup.Location = new System.Drawing.Point(994, 354);
            this.btn_deleteGroup.Name = "btn_deleteGroup";
            this.btn_deleteGroup.Size = new System.Drawing.Size(111, 23);
            this.btn_deleteGroup.TabIndex = 7;
            this.btn_deleteGroup.Text = "DeleteGroup";
            this.btn_deleteGroup.UseVisualStyleBackColor = true;
            this.btn_deleteGroup.Click += new System.EventHandler(this.btn_deleteGroup_Click);
            // 
            // btn_CreateConversation
            // 
            this.btn_CreateConversation.Location = new System.Drawing.Point(614, 354);
            this.btn_CreateConversation.Name = "btn_CreateConversation";
            this.btn_CreateConversation.Size = new System.Drawing.Size(166, 23);
            this.btn_CreateConversation.TabIndex = 8;
            this.btn_CreateConversation.Text = "CreateP2PConversation";
            this.btn_CreateConversation.UseVisualStyleBackColor = true;
            this.btn_CreateConversation.Click += new System.EventHandler(this.btn_CreateConversation_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(170, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "当前会话类型:";
            // 
            // lab_convarsationType
            // 
            this.lab_convarsationType.AutoSize = true;
            this.lab_convarsationType.Location = new System.Drawing.Point(281, 8);
            this.lab_convarsationType.Name = "lab_convarsationType";
            this.lab_convarsationType.Size = new System.Drawing.Size(97, 15);
            this.lab_convarsationType.TabIndex = 10;
            this.lab_convarsationType.Text = "还未创建会话";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(421, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "ConversationId:";
            // 
            // lab_conversationId
            // 
            this.lab_conversationId.AutoSize = true;
            this.lab_conversationId.Location = new System.Drawing.Point(555, 8);
            this.lab_conversationId.Name = "lab_conversationId";
            this.lab_conversationId.Size = new System.Drawing.Size(0, 15);
            this.lab_conversationId.TabIndex = 12;
            // 
            // btn_todo
            // 
            this.btn_todo.Location = new System.Drawing.Point(614, 401);
            this.btn_todo.Name = "btn_todo";
            this.btn_todo.Size = new System.Drawing.Size(166, 23);
            this.btn_todo.TabIndex = 13;
            this.btn_todo.Text = "PullTodo";
            this.btn_todo.UseVisualStyleBackColor = true;
            this.btn_todo.Click += new System.EventHandler(this.btn_todo_Click);
            // 
            // btn_complete
            // 
            this.btn_complete.Location = new System.Drawing.Point(787, 401);
            this.btn_complete.Name = "btn_complete";
            this.btn_complete.Size = new System.Drawing.Size(106, 23);
            this.btn_complete.TabIndex = 14;
            this.btn_complete.Text = "PULL_DoneTask";
            this.btn_complete.UseVisualStyleBackColor = true;
            this.btn_complete.Click += new System.EventHandler(this.btn_complete_Click);
            // 
            // txt_component
            // 
            this.txt_component.Location = new System.Drawing.Point(109, 382);
            this.txt_component.Name = "txt_component";
            this.txt_component.Size = new System.Drawing.Size(357, 25);
            this.txt_component.TabIndex = 15;
            this.txt_component.Text = "ad8d496c-da1b-4f9b-8a96-d2e0f3a63812";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 388);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 15);
            this.label3.TabIndex = 16;
            this.label3.Text = "ComponentId";
            // 
            // btn_messageHistory
            // 
            this.btn_messageHistory.Location = new System.Drawing.Point(614, 377);
            this.btn_messageHistory.Name = "btn_messageHistory";
            this.btn_messageHistory.Size = new System.Drawing.Size(166, 23);
            this.btn_messageHistory.TabIndex = 17;
            this.btn_messageHistory.Text = "GetMessageHistroy";
            this.btn_messageHistory.UseVisualStyleBackColor = true;
            this.btn_messageHistory.Click += new System.EventHandler(this.btn_messageHistory_Click);
            // 
            // btn_deleteP2pConversation
            // 
            this.btn_deleteP2pConversation.Location = new System.Drawing.Point(787, 377);
            this.btn_deleteP2pConversation.Name = "btn_deleteP2pConversation";
            this.btn_deleteP2pConversation.Size = new System.Drawing.Size(200, 23);
            this.btn_deleteP2pConversation.TabIndex = 18;
            this.btn_deleteP2pConversation.Text = "DeleteP2PConversation";
            this.btn_deleteP2pConversation.UseVisualStyleBackColor = true;
            this.btn_deleteP2pConversation.Click += new System.EventHandler(this.btn_deleteP2pConversation_Click);
            // 
            // ChatBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_deleteP2pConversation);
            this.Controls.Add(this.btn_messageHistory);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_component);
            this.Controls.Add(this.btn_complete);
            this.Controls.Add(this.btn_todo);
            this.Controls.Add(this.lab_conversationId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lab_convarsationType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_CreateConversation);
            this.Controls.Add(this.btn_deleteGroup);
            this.Controls.Add(this.btn_quitGruop);
            this.Controls.Add(this.btn_creategroup);
            this.Controls.Add(this.btn_disconnect);
            this.Controls.Add(this.btn_connect);
            this.Controls.Add(this.btn_sendText);
            this.Controls.Add(this.txt_message);
            this.Controls.Add(this.chat_messages);
            this.Name = "ChatBox";
            this.Size = new System.Drawing.Size(1111, 429);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox chat_messages;
        private System.Windows.Forms.TextBox txt_message;
        private System.Windows.Forms.Button btn_sendText;
        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.Button btn_disconnect;
        private System.Windows.Forms.Button btn_creategroup;
        private System.Windows.Forms.Button btn_quitGruop;
        private System.Windows.Forms.Button btn_deleteGroup;
        private System.Windows.Forms.Button btn_CreateConversation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lab_convarsationType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lab_conversationId;
        private Button btn_todo;
        private Button btn_complete;
        private TextBox txt_component;
        private Label label3;
        private Button btn_messageHistory;
        private Button btn_deleteP2pConversation;
    }
}
