namespace ChatRoomClient
{
    partial class ChatRoomClientForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            buttonConnectServer = new Button();
            textBoxServerAddress = new TextBox();
            label1 = new Label();
            groupBox = new GroupBox();
            richTextBoxMessageEditer = new RichTextBox();
            buttonSendMessage = new Button();
            buttonClearContent = new Button();
            richTextBoxChatContent = new RichTextBox();
            statusStrip = new StatusStrip();
            groupBox.SuspendLayout();
            SuspendLayout();
            // 
            // buttonConnectServer
            // 
            buttonConnectServer.Location = new Point(395, 10);
            buttonConnectServer.Name = "buttonConnectServer";
            buttonConnectServer.Size = new Size(150, 46);
            buttonConnectServer.TabIndex = 5;
            buttonConnectServer.Text = "连接服务器";
            buttonConnectServer.UseVisualStyleBackColor = true;
            // 
            // textBoxServerAddress
            // 
            textBoxServerAddress.Location = new Point(189, 15);
            textBoxServerAddress.Name = "textBoxServerAddress";
            textBoxServerAddress.Size = new Size(200, 38);
            textBoxServerAddress.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(21, 18);
            label1.Name = "label1";
            label1.Size = new Size(162, 31);
            label1.TabIndex = 3;
            label1.Text = "服务器IP地址:";
            // 
            // groupBox
            // 
            groupBox.Controls.Add(richTextBoxMessageEditer);
            groupBox.Controls.Add(buttonSendMessage);
            groupBox.Controls.Add(buttonClearContent);
            groupBox.Controls.Add(richTextBoxChatContent);
            groupBox.Location = new Point(12, 62);
            groupBox.Name = "groupBox";
            groupBox.Size = new Size(630, 690);
            groupBox.TabIndex = 6;
            groupBox.TabStop = false;
            groupBox.Text = "聊天区";
            // 
            // richTextBoxMessageEditer
            // 
            richTextBoxMessageEditer.Location = new Point(9, 475);
            richTextBoxMessageEditer.Name = "richTextBoxMessageEditer";
            richTextBoxMessageEditer.Size = new Size(615, 157);
            richTextBoxMessageEditer.TabIndex = 10;
            richTextBoxMessageEditer.Text = "";
            // 
            // buttonSendMessage
            // 
            buttonSendMessage.Location = new Point(474, 638);
            buttonSendMessage.Name = "buttonSendMessage";
            buttonSendMessage.Size = new Size(150, 46);
            buttonSendMessage.TabIndex = 9;
            buttonSendMessage.Text = "发送";
            buttonSendMessage.UseVisualStyleBackColor = true;
            // 
            // buttonClearContent
            // 
            buttonClearContent.Location = new Point(474, 423);
            buttonClearContent.Name = "buttonClearContent";
            buttonClearContent.Size = new Size(150, 46);
            buttonClearContent.TabIndex = 8;
            buttonClearContent.Text = "清空";
            buttonClearContent.UseVisualStyleBackColor = true;
            // 
            // richTextBoxChatContent
            // 
            richTextBoxChatContent.Location = new Point(9, 37);
            richTextBoxChatContent.Name = "richTextBoxChatContent";
            richTextBoxChatContent.ReadOnly = true;
            richTextBoxChatContent.Size = new Size(615, 380);
            richTextBoxChatContent.TabIndex = 0;
            richTextBoxChatContent.Text = "";
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(32, 32);
            statusStrip.Location = new Point(0, 767);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(654, 22);
            statusStrip.TabIndex = 7;
            statusStrip.Text = "statusStrip1";
            // 
            // ChatRoomClientForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(654, 789);
            Controls.Add(statusStrip);
            Controls.Add(groupBox);
            Controls.Add(buttonConnectServer);
            Controls.Add(textBoxServerAddress);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "ChatRoomClientForm";
            Text = "ChatRoom Client";
            groupBox.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonConnectServer;
        private TextBox textBoxServerAddress;
        private Label label1;
        private GroupBox groupBox;
        private StatusStrip statusStrip;
        private RichTextBox richTextBoxChatContent;
        private Button buttonSendMessage;
        private Button buttonClearContent;
        private RichTextBox richTextBoxMessageEditer;
    }
}
