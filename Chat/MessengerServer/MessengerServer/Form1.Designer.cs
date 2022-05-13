namespace MessengerServer
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
            this.ipTextBox = new System.Windows.Forms.TextBox();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.userGroupBox = new System.Windows.Forms.GroupBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.chatLogTextBox = new System.Windows.Forms.TextBox();
            this.messageInputTextBox = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.userGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ipTextBox
            // 
            this.ipTextBox.Location = new System.Drawing.Point(95, 19);
            this.ipTextBox.Name = "ipTextBox";
            this.ipTextBox.Size = new System.Drawing.Size(212, 20);
            this.ipTextBox.TabIndex = 0;
            // 
            // portTextBox
            // 
            this.portTextBox.Location = new System.Drawing.Point(95, 45);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(212, 20);
            this.portTextBox.TabIndex = 1;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(95, 72);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(212, 20);
            this.nameTextBox.TabIndex = 3;
            // 
            // userGroupBox
            // 
            this.userGroupBox.Controls.Add(this.textBox8);
            this.userGroupBox.Controls.Add(this.textBox7);
            this.userGroupBox.Controls.Add(this.textBox6);
            this.userGroupBox.Controls.Add(this.portTextBox);
            this.userGroupBox.Controls.Add(this.nameTextBox);
            this.userGroupBox.Controls.Add(this.ipTextBox);
            this.userGroupBox.Location = new System.Drawing.Point(12, 12);
            this.userGroupBox.Name = "userGroupBox";
            this.userGroupBox.Size = new System.Drawing.Size(319, 107);
            this.userGroupBox.TabIndex = 4;
            this.userGroupBox.TabStop = false;
            this.userGroupBox.Text = "User";
            // 
            // textBox8
            // 
            this.textBox8.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox8.Location = new System.Drawing.Point(21, 75);
            this.textBox8.Name = "textBox8";
            this.textBox8.ReadOnly = true;
            this.textBox8.Size = new System.Drawing.Size(68, 13);
            this.textBox8.TabIndex = 6;
            this.textBox8.Text = "Name";
            this.textBox8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox7
            // 
            this.textBox7.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox7.Location = new System.Drawing.Point(21, 47);
            this.textBox7.Name = "textBox7";
            this.textBox7.ReadOnly = true;
            this.textBox7.Size = new System.Drawing.Size(68, 13);
            this.textBox7.TabIndex = 5;
            this.textBox7.Text = "Port";
            this.textBox7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox6
            // 
            this.textBox6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox6.Location = new System.Drawing.Point(21, 22);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(68, 13);
            this.textBox6.TabIndex = 4;
            this.textBox6.Text = "IP";
            this.textBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(372, 54);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(85, 23);
            this.connectButton.TabIndex = 5;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.ConnectClick);
            // 
            // chatLogTextBox
            // 
            this.chatLogTextBox.Location = new System.Drawing.Point(13, 143);
            this.chatLogTextBox.Multiline = true;
            this.chatLogTextBox.Name = "chatLogTextBox";
            this.chatLogTextBox.ReadOnly = true;
            this.chatLogTextBox.Size = new System.Drawing.Size(460, 204);
            this.chatLogTextBox.TabIndex = 6;
            // 
            // messageInputTextBox
            // 
            this.messageInputTextBox.Location = new System.Drawing.Point(13, 364);
            this.messageInputTextBox.Name = "messageInputTextBox";
            this.messageInputTextBox.Size = new System.Drawing.Size(345, 20);
            this.messageInputTextBox.TabIndex = 7;
            this.messageInputTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SendKeyPressEnter);
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(382, 362);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(75, 23);
            this.sendButton.TabIndex = 8;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.SendClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 402);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.messageInputTextBox);
            this.Controls.Add(this.chatLogTextBox);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.userGroupBox);
            this.Name = "Form1";
            this.Text = "Messenger Server";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.userGroupBox.ResumeLayout(false);
            this.userGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ipTextBox;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.GroupBox userGroupBox;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.TextBox chatLogTextBox;
        private System.Windows.Forms.TextBox messageInputTextBox;
        private System.Windows.Forms.Button sendButton;
    }
}

