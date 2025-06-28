namespace ChatRoomClient
{
    partial class FriendAddForm
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
            buttonAddFriend = new Button();
            textBoxFriendUsername = new TextBox();
            label2 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // buttonAddFriend
            // 
            buttonAddFriend.Location = new Point(333, 258);
            buttonAddFriend.Name = "buttonAddFriend";
            buttonAddFriend.Size = new Size(150, 46);
            buttonAddFriend.TabIndex = 0;
            buttonAddFriend.Text = "添加好友";
            buttonAddFriend.UseVisualStyleBackColor = true;
            // 
            // textBoxFriendUsername
            // 
            textBoxFriendUsername.Location = new Point(324, 132);
            textBoxFriendUsername.Name = "textBoxFriendUsername";
            textBoxFriendUsername.Size = new Size(262, 38);
            textBoxFriendUsername.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(208, 135);
            label2.Name = "label2";
            label2.Size = new Size(110, 31);
            label2.TabIndex = 4;
            label2.Text = "好友账号";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("华文琥珀", 16.125F, FontStyle.Bold, GraphicsUnit.Point, 134);
            label3.Location = new Point(301, 43);
            label3.Name = "label3";
            label3.Size = new Size(195, 44);
            label3.TabIndex = 7;
            label3.Text = "添加用户";
            // 
            // FriendAddForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label3);
            Controls.Add(textBoxFriendUsername);
            Controls.Add(label2);
            Controls.Add(buttonAddFriend);
            Name = "FriendAddForm";
            Text = "添加好友";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonAddFriend;
        private TextBox textBoxFriendUsername;
        private Label label2;
        private Label label3;
    }
}