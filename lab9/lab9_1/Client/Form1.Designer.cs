namespace Client
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.HostTextBox = new System.Windows.Forms.TextBox();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.DisconnectButton = new System.Windows.Forms.Button();
            this.SendButton = new System.Windows.Forms.Button();
            this.MessageTextBox = new System.Windows.Forms.TextBox();
            this.LogListBox = new System.Windows.Forms.ListBox();
            this.PortClient = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // HostTextBox
            // 
            this.HostTextBox.Location = new System.Drawing.Point(592, 38);
            this.HostTextBox.Name = "HostTextBox";
            this.HostTextBox.Size = new System.Drawing.Size(179, 31);
            this.HostTextBox.TabIndex = 0;
            this.HostTextBox.Text = "Host";
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(24, 32);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(151, 48);
            this.ConnectButton.TabIndex = 2;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click_1);
            // 
            // DisconnectButton
            // 
            this.DisconnectButton.Location = new System.Drawing.Point(198, 32);
            this.DisconnectButton.Name = "DisconnectButton";
            this.DisconnectButton.Size = new System.Drawing.Size(151, 48);
            this.DisconnectButton.TabIndex = 3;
            this.DisconnectButton.Text = "Disconnect";
            this.DisconnectButton.UseVisualStyleBackColor = true;
            this.DisconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click_1);
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(592, 155);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(179, 48);
            this.SendButton.TabIndex = 4;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click_1);
            // 
            // MessageTextBox
            // 
            this.MessageTextBox.Location = new System.Drawing.Point(24, 103);
            this.MessageTextBox.Multiline = true;
            this.MessageTextBox.Name = "MessageTextBox";
            this.MessageTextBox.Size = new System.Drawing.Size(529, 100);
            this.MessageTextBox.TabIndex = 5;
            this.MessageTextBox.Text = "MessageTextBox";
            // 
            // LogListBox
            // 
            this.LogListBox.FormattingEnabled = true;
            this.LogListBox.ItemHeight = 25;
            this.LogListBox.Location = new System.Drawing.Point(24, 230);
            this.LogListBox.Name = "LogListBox";
            this.LogListBox.Size = new System.Drawing.Size(764, 179);
            this.LogListBox.TabIndex = 6;
            // 
            // PortClient
            // 
            this.PortClient.Location = new System.Drawing.Point(592, 85);
            this.PortClient.Name = "PortClient";
            this.PortClient.Size = new System.Drawing.Size(179, 31);
            this.PortClient.TabIndex = 7;
            this.PortClient.Text = " Port";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(379, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "label1";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(592, 116);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(179, 33);
            this.comboBox1.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PortClient);
            this.Controls.Add(this.LogListBox);
            this.Controls.Add(this.MessageTextBox);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.DisconnectButton);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.HostTextBox);
            this.Name = "Form1";
            this.Text = "Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox HostTextBox;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Button DisconnectButton;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.TextBox MessageTextBox;
        public System.Windows.Forms.ListBox LogListBox;
        private System.Windows.Forms.TextBox PortClient;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}

