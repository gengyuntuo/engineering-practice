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
            dataGridView = new DataGridView();
            label = new Label();
            textBoxUserSearch = new TextBox();
            buttonUserSearch = new Button();
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            groupBox = new GroupBox();
            checkBoxOnlyOnlinUser = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            statusStrip.SuspendLayout();
            groupBox.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView
            // 
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Location = new Point(23, 84);
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersWidth = 82;
            dataGridView.Size = new Size(1124, 548);
            dataGridView.TabIndex = 12;
            // 
            // label
            // 
            label.AutoSize = true;
            label.Location = new Point(19, 40);
            label.Name = "label";
            label.Size = new Size(226, 31);
            label.TabIndex = 11;
            label.Text = "用户名称 | 用户账号";
            // 
            // textBoxUserSearch
            // 
            textBoxUserSearch.Location = new Point(251, 37);
            textBoxUserSearch.Name = "textBoxUserSearch";
            textBoxUserSearch.Size = new Size(333, 38);
            textBoxUserSearch.TabIndex = 9;
            // 
            // buttonUserSearch
            // 
            buttonUserSearch.Location = new Point(601, 32);
            buttonUserSearch.Name = "buttonUserSearch";
            buttonUserSearch.Size = new Size(150, 46);
            buttonUserSearch.TabIndex = 8;
            buttonUserSearch.Text = "搜索用户";
            buttonUserSearch.UseVisualStyleBackColor = true;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(32, 32);
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
            statusStrip.Location = new Point(0, 674);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1237, 41);
            statusStrip.TabIndex = 13;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(86, 31);
            toolStripStatusLabel.Text = "已启动";
            // 
            // groupBox
            // 
            groupBox.Controls.Add(checkBoxOnlyOnlinUser);
            groupBox.Controls.Add(label);
            groupBox.Controls.Add(textBoxUserSearch);
            groupBox.Controls.Add(dataGridView);
            groupBox.Controls.Add(buttonUserSearch);
            groupBox.Location = new Point(29, 24);
            groupBox.Name = "groupBox";
            groupBox.Size = new Size(1169, 638);
            groupBox.TabIndex = 14;
            groupBox.TabStop = false;
            groupBox.Text = "用户列表";
            // 
            // checkBoxOnlyOnlinUser
            // 
            checkBoxOnlyOnlinUser.AutoSize = true;
            checkBoxOnlyOnlinUser.Location = new Point(773, 39);
            checkBoxOnlyOnlinUser.Name = "checkBoxOnlyOnlinUser";
            checkBoxOnlyOnlinUser.Size = new Size(166, 35);
            checkBoxOnlyOnlinUser.TabIndex = 13;
            checkBoxOnlyOnlinUser.Text = "仅在线用户";
            checkBoxOnlyOnlinUser.UseVisualStyleBackColor = true;
            // 
            // ChatRoomServerForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1237, 715);
            Controls.Add(groupBox);
            Controls.Add(statusStrip);
            Name = "ChatRoomServerForm";
            Text = "聊天室程序服务端";
            Load += ChatRoomServerForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            groupBox.ResumeLayout(false);
            groupBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView;
        private Label label;
        private TextBox textBoxUserSearch;
        private Button buttonUserSearch;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLabel;
        private GroupBox groupBox;
        private CheckBox checkBoxOnlyOnlinUser;
    }
}
