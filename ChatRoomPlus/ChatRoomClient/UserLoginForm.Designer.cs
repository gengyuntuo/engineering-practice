namespace ChatRoomClient
{
    partial class UserLoginForm
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
            label1 = new Label();
            textBoxUserName = new TextBox();
            label2 = new Label();
            textBoxPassword = new TextBox();
            label3 = new Label();
            buttonLogin = new Button();
            buttonRegister = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("华文琥珀", 16.125F, FontStyle.Bold, GraphicsUnit.Point, 134);
            label1.Location = new Point(346, 44);
            label1.Name = "label1";
            label1.Size = new Size(107, 44);
            label1.TabIndex = 0;
            label1.Text = "登录";
            // 
            // textBoxUserName
            // 
            textBoxUserName.Location = new Point(299, 109);
            textBoxUserName.Name = "textBoxUserName";
            textBoxUserName.Size = new Size(271, 38);
            textBoxUserName.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(217, 112);
            label2.Name = "label2";
            label2.Size = new Size(62, 31);
            label2.TabIndex = 0;
            label2.Text = "账号";
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(299, 178);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.PasswordChar = '*';
            textBoxPassword.Size = new Size(271, 38);
            textBoxPassword.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(217, 181);
            label3.Name = "label3";
            label3.Size = new Size(62, 31);
            label3.TabIndex = 2;
            label3.Text = "密码";
            // 
            // buttonLogin
            // 
            buttonLogin.Location = new Point(217, 265);
            buttonLogin.Name = "buttonLogin";
            buttonLogin.Size = new Size(150, 46);
            buttonLogin.TabIndex = 3;
            buttonLogin.Text = "登录";
            buttonLogin.UseVisualStyleBackColor = true;
            // 
            // buttonRegister
            // 
            buttonRegister.Location = new Point(420, 265);
            buttonRegister.Name = "buttonRegister";
            buttonRegister.Size = new Size(150, 46);
            buttonRegister.TabIndex = 4;
            buttonRegister.Text = "注册";
            buttonRegister.UseVisualStyleBackColor = true;
            // 
            // UserLoginForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonRegister);
            Controls.Add(buttonLogin);
            Controls.Add(label3);
            Controls.Add(textBoxPassword);
            Controls.Add(label2);
            Controls.Add(textBoxUserName);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "UserLoginForm";
            Text = "登录";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBoxUserName;
        private Label label2;
        private TextBox textBoxPassword;
        private Label label3;
        private Button buttonLogin;
        private Button buttonRegister;
    }
}