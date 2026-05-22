namespace tasked_forms
{
    partial class Form1
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
            this.logo = new System.Windows.Forms.Label();
            this.signUpBtn = new System.Windows.Forms.Button();
            this.logInBtn = new System.Windows.Forms.Button();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.passwordlabel = new System.Windows.Forms.Label();
            this.usernametxtbox = new System.Windows.Forms.TextBox();
            this.agreementCheckBox = new System.Windows.Forms.CheckBox();
            this.forgotpassword = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.passwordtxtbox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.logopic = new System.Windows.Forms.PictureBox();
            this.dashedDivider = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logopic)).BeginInit();
            this.SuspendLayout();
            // 
            // logo
            // 
            this.logo.AutoSize = true;
            this.logo.Font = new System.Drawing.Font("Mignone", 55F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logo.ForeColor = System.Drawing.Color.Blue;
            this.logo.Location = new System.Drawing.Point(620, 92);
            this.logo.Margin = new System.Windows.Forms.Padding(0);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(407, 133);
            this.logo.TabIndex = 0;
            this.logo.Text = "tasked";
            this.logo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.logo.Click += new System.EventHandler(this.label1_Click);
            // 
            // signUpBtn
            // 
            this.signUpBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.signUpBtn.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.signUpBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.signUpBtn.Location = new System.Drawing.Point(628, 598);
            this.signUpBtn.Name = "signUpBtn";
            this.signUpBtn.Size = new System.Drawing.Size(384, 61);
            this.signUpBtn.TabIndex = 0;
            this.signUpBtn.Text = "I don\'t have an account";
            this.signUpBtn.UseVisualStyleBackColor = false;
            this.signUpBtn.Click += new System.EventHandler(this.signUpBtn_Click);
            // 
            // logInBtn
            // 
            this.logInBtn.BackColor = System.Drawing.Color.Blue;
            this.logInBtn.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.logInBtn.FlatAppearance.BorderSize = 2;
            this.logInBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.logInBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.logInBtn.Location = new System.Drawing.Point(628, 517);
            this.logInBtn.Name = "logInBtn";
            this.logInBtn.Size = new System.Drawing.Size(384, 61);
            this.logInBtn.TabIndex = 1;
            this.logInBtn.Text = "Log In";
            this.logInBtn.UseVisualStyleBackColor = false;
            this.logInBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(622, 242);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(111, 28);
            this.usernameLabel.TabIndex = 2;
            this.usernameLabel.Text = "Username";
            this.usernameLabel.Click += new System.EventHandler(this.usernameLabel_Click);
            // 
            // passwordlabel
            // 
            this.passwordlabel.AutoSize = true;
            this.passwordlabel.Location = new System.Drawing.Point(622, 344);
            this.passwordlabel.Name = "passwordlabel";
            this.passwordlabel.Size = new System.Drawing.Size(106, 28);
            this.passwordlabel.TabIndex = 3;
            this.passwordlabel.Text = "Password";
            this.passwordlabel.Click += new System.EventHandler(this.label3_Click);
            // 
            // usernametxtbox
            // 
            this.usernametxtbox.BackColor = System.Drawing.Color.White;
            this.usernametxtbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.usernametxtbox.Font = new System.Drawing.Font("Inter", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usernametxtbox.ForeColor = System.Drawing.Color.Blue;
            this.usernametxtbox.Location = new System.Drawing.Point(631, 273);
            this.usernametxtbox.Name = "usernametxtbox";
            this.usernametxtbox.Size = new System.Drawing.Size(352, 32);
            this.usernametxtbox.TabIndex = 3;
            this.usernametxtbox.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // agreementCheckBox
            // 
            this.agreementCheckBox.AutoSize = true;
            this.agreementCheckBox.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.agreementCheckBox.Location = new System.Drawing.Point(627, 692);
            this.agreementCheckBox.Name = "agreementCheckBox";
            this.agreementCheckBox.Size = new System.Drawing.Size(389, 32);
            this.agreementCheckBox.TabIndex = 7;
            this.agreementCheckBox.Text = "I have read the terms and conditions";
            this.agreementCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.agreementCheckBox.UseVisualStyleBackColor = true;
            this.agreementCheckBox.CheckedChanged += new System.EventHandler(this.agreementCheckBox_CheckedChanged);
            // 
            // forgotpassword
            // 
            this.forgotpassword.AutoSize = true;
            this.forgotpassword.DisabledLinkColor = System.Drawing.Color.DarkGray;
            this.forgotpassword.ForeColor = System.Drawing.Color.Gray;
            this.forgotpassword.LinkColor = System.Drawing.Color.Gray;
            this.forgotpassword.Location = new System.Drawing.Point(835, 434);
            this.forgotpassword.Name = "forgotpassword";
            this.forgotpassword.Size = new System.Drawing.Size(173, 28);
            this.forgotpassword.TabIndex = 8;
            this.forgotpassword.TabStop = true;
            this.forgotpassword.Text = "Forgot password";
            this.forgotpassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.forgotpassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.forgotpassword_LinkClicked);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Blue;
            this.label4.Font = new System.Drawing.Font("Inter", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(623, 312);
            this.label4.MaximumSize = new System.Drawing.Size(0, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(360, 3);
            this.label4.TabIndex = 9;
            this.label4.Text = "--------------------------------------------------";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Blue;
            this.label5.Font = new System.Drawing.Font("Inter", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(627, 414);
            this.label5.MaximumSize = new System.Drawing.Size(0, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(360, 3);
            this.label5.TabIndex = 11;
            this.label5.Text = "--------------------------------------------------";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // passwordtxtbox
            // 
            this.passwordtxtbox.BackColor = System.Drawing.Color.White;
            this.passwordtxtbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.passwordtxtbox.Font = new System.Drawing.Font("Inter", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordtxtbox.ForeColor = System.Drawing.Color.Blue;
            this.passwordtxtbox.Location = new System.Drawing.Point(631, 375);
            this.passwordtxtbox.Name = "passwordtxtbox";
            this.passwordtxtbox.Size = new System.Drawing.Size(352, 32);
            this.passwordtxtbox.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Blue;
            this.panel1.Controls.Add(this.logopic);
            this.panel1.Location = new System.Drawing.Point(2, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(379, 846);
            this.panel1.TabIndex = 26;
            // 
            // logopic
            // 
            this.logopic.BackColor = System.Drawing.Color.White;
            this.logopic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.logopic.Image = global::tasked_forms.Properties.Resources.Subtract_Cover_fnl;
            this.logopic.Location = new System.Drawing.Point(-3, -7);
            this.logopic.Name = "logopic";
            this.logopic.Size = new System.Drawing.Size(386, 853);
            this.logopic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logopic.TabIndex = 14;
            this.logopic.TabStop = false;
            this.logopic.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // dashedDivider
            // 
            this.dashedDivider.AutoSize = true;
            this.dashedDivider.Font = new System.Drawing.Font("Inter", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dashedDivider.ForeColor = System.Drawing.Color.Blue;
            this.dashedDivider.Location = new System.Drawing.Point(387, -102);
            this.dashedDivider.Name = "dashedDivider";
            this.dashedDivider.Size = new System.Drawing.Size(18, 980);
            this.dashedDivider.TabIndex = 27;
            this.dashedDivider.Text = "I\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\n" +
    "I\r\nI\r\nI\r\nI\r\nI\r\nI\r\nI\r\n\r\n";
            this.dashedDivider.Click += new System.EventHandler(this.dashedDivider_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1278, 844);
            this.Controls.Add(this.dashedDivider);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.passwordtxtbox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.forgotpassword);
            this.Controls.Add(this.agreementCheckBox);
            this.Controls.Add(this.usernametxtbox);
            this.Controls.Add(this.passwordlabel);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.logInBtn);
            this.Controls.Add(this.logo);
            this.Controls.Add(this.signUpBtn);
            this.Font = new System.Drawing.Font("Inter", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Blue;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logopic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label logo;
        private System.Windows.Forms.Button signUpBtn;
        private System.Windows.Forms.Button logInBtn;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.Label passwordlabel;
        private System.Windows.Forms.TextBox usernametxtbox;
        private System.Windows.Forms.CheckBox agreementCheckBox;
        private System.Windows.Forms.LinkLabel forgotpassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox passwordtxtbox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox logopic;
        private System.Windows.Forms.Label dashedDivider;
    }
}

