namespace CybersecurityChatbot.WinFormsApp
{
    partial class MainForm
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
            lblTitle = new Label();
            txtAscii = new TextBox();
            rtbChat = new RichTextBox();
            txtName = new TextBox();
            btnSetName = new Button();
            lblMemory = new Label();
            txtInput = new TextBox();
            btnSend = new Button();
            btnClear = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(163, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(441, 38);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Cybersecurity Awareness Assistant\r\n";
            // 
            // txtAscii
            // 
            txtAscii.Location = new Point(213, 50);
            txtAscii.Multiline = true;
            txtAscii.Name = "txtAscii";
            txtAscii.ReadOnly = true;
            txtAscii.ScrollBars = ScrollBars.Vertical;
            txtAscii.Size = new Size(358, 143);
            txtAscii.TabIndex = 1;
            // 
            // rtbChat
            // 
            rtbChat.Location = new Point(163, 199);
            rtbChat.Name = "rtbChat";
            rtbChat.ReadOnly = true;
            rtbChat.Size = new Size(441, 181);
            rtbChat.TabIndex = 2;
            rtbChat.Text = "";
            // 
            // txtName
            // 
            txtName.Location = new Point(163, 386);
            txtName.Name = "txtName";
            txtName.Size = new Size(322, 27);
            txtName.TabIndex = 3;
            // 
            // btnSetName
            // 
            btnSetName.Location = new Point(491, 386);
            btnSetName.Name = "btnSetName";
            btnSetName.Size = new Size(97, 27);
            btnSetName.TabIndex = 4;
            btnSetName.Text = "Save Name";
            btnSetName.UseVisualStyleBackColor = true;
            // 
            // lblMemory
            // 
            lblMemory.AutoSize = true;
            lblMemory.Location = new Point(163, 426);
            lblMemory.Name = "lblMemory";
            lblMemory.Size = new Size(188, 20);
            lblMemory.TabIndex = 5;
            lblMemory.Text = "Memory: nothing saved yet";
            // 
            // txtInput
            // 
            txtInput.Location = new Point(163, 508);
            txtInput.Name = "txtInput";
            txtInput.Size = new Size(322, 27);
            txtInput.TabIndex = 6;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(494, 508);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(94, 29);
            btnSend.TabIndex = 7;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(610, 507);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(94, 29);
            btnClear.TabIndex = 8;
            btnClear.Text = "Clear Chat";
            btnClear.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(810, 572);
            Controls.Add(btnClear);
            Controls.Add(btnSend);
            Controls.Add(txtInput);
            Controls.Add(lblMemory);
            Controls.Add(btnSetName);
            Controls.Add(txtName);
            Controls.Add(rtbChat);
            Controls.Add(txtAscii);
            Controls.Add(lblTitle);
            Name = "MainForm";
            Text = "Cybersecurity Awareness Assistant";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private TextBox txtAscii;
        private RichTextBox rtbChat;
        private TextBox txtName;
        private Button btnSetName;
        private Label lblMemory;
        private TextBox txtInput;
        private Button btnSend;
        private Button btnClear;
    }
}
