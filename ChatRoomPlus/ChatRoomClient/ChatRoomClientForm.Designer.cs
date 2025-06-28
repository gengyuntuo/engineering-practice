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
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            listViewMyFriends = new ListView();
            label1 = new Label();
            buttonAddFriend = new Button();
            labelFriendInfo = new Label();
            richTextBoxMessageMonitor = new RichTextBox();
            richTextBoxMessageEditor = new RichTextBox();
            buttonSendMessage = new Button();
            menuStrip = new MenuStrip();
            toolStripMenuItem = new ToolStripMenuItem();
            ToolStripMenuItemSignOut = new ToolStripMenuItem();
            statusStrip.SuspendLayout();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(32, 32);
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
            statusStrip.Location = new Point(0, 724);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1133, 41);
            statusStrip.TabIndex = 0;
            statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(62, 31);
            toolStripStatusLabel.Text = "就绪";
            // 
            // listViewMyFriends
            // 
            listViewMyFriends.Location = new Point(32, 103);
            listViewMyFriends.Name = "listViewMyFriends";
            listViewMyFriends.Size = new Size(262, 605);
            listViewMyFriends.TabIndex = 1;
            listViewMyFriends.UseCompatibleStateImageBehavior = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 134);
            label1.Location = new Point(32, 57);
            label1.Name = "label1";
            label1.Size = new Size(129, 37);
            label1.TabIndex = 2;
            label1.Text = "我的好友";
            // 
            // buttonAddFriend
            // 
            buttonAddFriend.Location = new Point(167, 54);
            buttonAddFriend.Name = "buttonAddFriend";
            buttonAddFriend.Size = new Size(126, 46);
            buttonAddFriend.TabIndex = 3;
            buttonAddFriend.Text = "添加好友";
            buttonAddFriend.UseVisualStyleBackColor = true;
            // 
            // labelFriendInfo
            // 
            labelFriendInfo.AutoSize = true;
            labelFriendInfo.Font = new Font("Microsoft YaHei UI", 13.875F, FontStyle.Bold, GraphicsUnit.Point, 134);
            labelFriendInfo.Location = new Point(315, 25);
            labelFriendInfo.Name = "labelFriendInfo";
            labelFriendInfo.Size = new Size(0, 50);
            labelFriendInfo.TabIndex = 2;
            // 
            // richTextBoxMessageMonitor
            // 
            richTextBoxMessageMonitor.Location = new Point(315, 103);
            richTextBoxMessageMonitor.Name = "richTextBoxMessageMonitor";
            richTextBoxMessageMonitor.ReadOnly = true;
            richTextBoxMessageMonitor.Size = new Size(773, 442);
            richTextBoxMessageMonitor.TabIndex = 4;
            richTextBoxMessageMonitor.Text = "";
            // 
            // richTextBoxMessageEditor
            // 
            richTextBoxMessageEditor.Location = new Point(315, 551);
            richTextBoxMessageEditor.Name = "richTextBoxMessageEditor";
            richTextBoxMessageEditor.Size = new Size(674, 157);
            richTextBoxMessageEditor.TabIndex = 5;
            richTextBoxMessageEditor.Text = "";
            // 
            // buttonSendMessage
            // 
            buttonSendMessage.Location = new Point(995, 551);
            buttonSendMessage.Name = "buttonSendMessage";
            buttonSendMessage.Size = new Size(93, 157);
            buttonSendMessage.TabIndex = 6;
            buttonSendMessage.Text = "发送";
            buttonSendMessage.UseVisualStyleBackColor = true;
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(32, 32);
            menuStrip.Items.AddRange(new ToolStripItem[] { toolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(1133, 39);
            menuStrip.TabIndex = 7;
            menuStrip.Text = "menuStrip1";
            // 
            // toolStripMenuItem
            // 
            toolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ToolStripMenuItemSignOut });
            toolStripMenuItem.Name = "toolStripMenuItem";
            toolStripMenuItem.Size = new Size(82, 35);
            toolStripMenuItem.Text = "菜单";
            // 
            // ToolStripMenuItemSignOut
            // 
            ToolStripMenuItemSignOut.Name = "ToolStripMenuItemSignOut";
            ToolStripMenuItemSignOut.Size = new Size(243, 44);
            ToolStripMenuItemSignOut.Text = "退出登录";
            ToolStripMenuItemSignOut.Click += ToolStripMenuItemSignOut_Click;
            // 
            // ChatRoomClientForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1133, 765);
            Controls.Add(buttonSendMessage);
            Controls.Add(richTextBoxMessageEditor);
            Controls.Add(richTextBoxMessageMonitor);
            Controls.Add(buttonAddFriend);
            Controls.Add(labelFriendInfo);
            Controls.Add(label1);
            Controls.Add(listViewMyFriends);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Name = "ChatRoomClientForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "网络聊天室";
            Load += ChatRoomClientForm_Load;
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusStrip;
        private ListView listViewMyFriends;
        private Label label1;
        private Button buttonAddFriend;
        private Label labelFriendInfo;
        private RichTextBox richTextBoxMessageMonitor;
        private RichTextBox richTextBoxMessageEditor;
        private Button buttonSendMessage;
        private ToolStripStatusLabel toolStripStatusLabel;
        private MenuStrip menuStrip;
        private ToolStripMenuItem toolStripMenuItem;
        private ToolStripMenuItem ToolStripMenuItemSignOut;
    }
}
