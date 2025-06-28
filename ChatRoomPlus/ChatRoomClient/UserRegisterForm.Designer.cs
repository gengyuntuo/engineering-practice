namespace ChatRoomClient
{
    partial class UserRegisterForm
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
            label2 = new Label();
            buttonRegister = new Button();
            textBoxUsername = new TextBox();
            textBoxPassword = new TextBox();
            label3 = new Label();
            textBoxRepeatPassword = new TextBox();
            label4 = new Label();
            label1 = new Label();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(196, 105);
            label2.Name = "label2";
            label2.Size = new Size(110, 31);
            label2.TabIndex = 1;
            label2.Text = "账号名称";
            // 
            // buttonRegister
            // 
            buttonRegister.Location = new Point(312, 314);
            buttonRegister.Name = "buttonRegister";
            buttonRegister.Size = new Size(150, 46);
            buttonRegister.TabIndex = 2;
            buttonRegister.Text = "注册";
            buttonRegister.UseVisualStyleBackColor = true;
            // 
            // textBoxUsername
            // 
            textBoxUsername.Location = new Point(312, 98);
            textBoxUsername.Name = "textBoxUsername";
            textBoxUsername.Size = new Size(276, 38);
            textBoxUsername.TabIndex = 3;
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(312, 168);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.PasswordChar = '*';
            textBoxPassword.Size = new Size(276, 38);
            textBoxPassword.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(196, 171);
            label3.Name = "label3";
            label3.Size = new Size(110, 31);
            label3.TabIndex = 4;
            label3.Text = "账号密码";
            // 
            // textBoxRepeatPassword
            // 
            textBoxRepeatPassword.Location = new Point(312, 244);
            textBoxRepeatPassword.Name = "textBoxRepeatPassword";
            textBoxRepeatPassword.PasswordChar = '*';
            textBoxRepeatPassword.Size = new Size(276, 38);
            textBoxRepeatPassword.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(196, 247);
            label4.Name = "label4";
            label4.Size = new Size(110, 31);
            label4.TabIndex = 6;
            label4.Text = "重复密码";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("华文琥珀", 16.125F, FontStyle.Bold, GraphicsUnit.Point, 134);
            label1.Location = new Point(289, 31);
            label1.Name = "label1";
            label1.Size = new Size(195, 44);
            label1.TabIndex = 8;
            label1.Text = "用户注册";
            // 
            // UserRegisterForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(textBoxRepeatPassword);
            Controls.Add(label4);
            Controls.Add(textBoxPassword);
            Controls.Add(label3);
            Controls.Add(textBoxUsername);
            Controls.Add(buttonRegister);
            Controls.Add(label2);
            Name = "UserRegisterForm";
            Text = "注册";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private Button buttonRegister;
        private TextBox textBoxUsername;
        private TextBox textBoxPassword;
        private Label label3;
        private TextBox textBoxRepeatPassword;
        private Label label4;
        private Label label1;
    }
}