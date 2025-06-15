
namespace ChatRoomServer
{
    partial class ChatRoomServerForm
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
            label1 = new Label();
            textBoxServerAddress = new TextBox();
            buttonStartServer = new Button();
            label2 = new Label();
            statusStrip = new StatusStrip();
            listBoxOnlineClient = new ListBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 24);
            label1.Name = "label1";
            label1.Size = new Size(162, 31);
            label1.TabIndex = 0;
            label1.Text = "服务器IP地址:";
            // 
            // textBoxServerAddress
            // 
            textBoxServerAddress.Location = new Point(191, 21);
            textBoxServerAddress.Name = "textBoxServerAddress";
            textBoxServerAddress.Size = new Size(200, 38);
            textBoxServerAddress.TabIndex = 1;
            // 
            // buttonStartServer
            // 
            buttonStartServer.Location = new Point(397, 16);
            buttonStartServer.Name = "buttonStartServer";
            buttonStartServer.Size = new Size(150, 46);
            buttonStartServer.TabIndex = 2;
            buttonStartServer.Text = "启动服务";
            buttonStartServer.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(23, 78);
            label2.Name = "label2";
            label2.Size = new Size(134, 31);
            label2.TabIndex = 3;
            label2.Text = "在线客户端";
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(32, 32);
            statusStrip.Location = new Point(0, 767);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(574, 22);
            statusStrip.TabIndex = 4;
            statusStrip.Text = "statusStrip1";
            // 
            // listBoxOnlineClient
            // 
            listBoxOnlineClient.FormattingEnabled = true;
            listBoxOnlineClient.Location = new Point(23, 121);
            listBoxOnlineClient.Name = "listBoxOnlineClient";
            listBoxOnlineClient.Size = new Size(524, 624);
            listBoxOnlineClient.TabIndex = 5;
            // 
            // ChatRoomServerForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(574, 789);
            Controls.Add(listBoxOnlineClient);
            Controls.Add(statusStrip);
            Controls.Add(label2);
            Controls.Add(buttonStartServer);
            Controls.Add(textBoxServerAddress);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "ChatRoomServerForm";
            Text = "ChatRoom Server";
            ResumeLayout(false);
            PerformLayout();
        }

        private void buttonStartServer_Click_1(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private Label label1;
        private TextBox textBoxServerAddress;
        private Button buttonStartServer;
        private Label label2;
        private StatusStrip statusStrip;
        private ListBox listBoxOnlineClient;
    }
}
