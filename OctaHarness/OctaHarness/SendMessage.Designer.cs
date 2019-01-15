namespace OctaHarness
{
    partial class SendMessage
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
            this.label1 = new System.Windows.Forms.Label();
            this.cboTrucks = new System.Windows.Forms.ComboBox();
            this.cboUsers = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRefreshTrucks = new System.Windows.Forms.Button();
            this.btnRefreshUsers = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.chkExpectResponse = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Truck";
            // 
            // cboTrucks
            // 
            this.cboTrucks.FormattingEnabled = true;
            this.cboTrucks.Location = new System.Drawing.Point(87, 10);
            this.cboTrucks.Name = "cboTrucks";
            this.cboTrucks.Size = new System.Drawing.Size(158, 21);
            this.cboTrucks.TabIndex = 1;
            // 
            // cboUsers
            // 
            this.cboUsers.FormattingEnabled = true;
            this.cboUsers.Location = new System.Drawing.Point(87, 38);
            this.cboUsers.Name = "cboUsers";
            this.cboUsers.Size = new System.Drawing.Size(158, 21);
            this.cboUsers.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Select User";
            // 
            // btnRefreshTrucks
            // 
            this.btnRefreshTrucks.Location = new System.Drawing.Point(251, 8);
            this.btnRefreshTrucks.Name = "btnRefreshTrucks";
            this.btnRefreshTrucks.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshTrucks.TabIndex = 4;
            this.btnRefreshTrucks.Text = "Refresh";
            this.btnRefreshTrucks.UseVisualStyleBackColor = true;
            this.btnRefreshTrucks.Click += new System.EventHandler(this.btnRefreshTrucks_Click);
            // 
            // btnRefreshUsers
            // 
            this.btnRefreshUsers.Location = new System.Drawing.Point(251, 36);
            this.btnRefreshUsers.Name = "btnRefreshUsers";
            this.btnRefreshUsers.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshUsers.TabIndex = 5;
            this.btnRefreshUsers.Text = "Refresh";
            this.btnRefreshUsers.UseVisualStyleBackColor = true;
            this.btnRefreshUsers.Click += new System.EventHandler(this.btnRefreshUsers_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(87, 66);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(239, 163);
            this.txtMessage.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Message";
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.Location = new System.Drawing.Point(249, 235);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(75, 23);
            this.btnSendMessage.TabIndex = 8;
            this.btnSendMessage.Text = "Send";
            this.btnSendMessage.UseVisualStyleBackColor = true;
            this.btnSendMessage.Click += new System.EventHandler(this.btnSendMessage_Click);
            // 
            // chkExpectResponse
            // 
            this.chkExpectResponse.AutoSize = true;
            this.chkExpectResponse.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkExpectResponse.Location = new System.Drawing.Point(12, 239);
            this.chkExpectResponse.Name = "chkExpectResponse";
            this.chkExpectResponse.Size = new System.Drawing.Size(105, 17);
            this.chkExpectResponse.TabIndex = 9;
            this.chkExpectResponse.Text = "Expect Reponse";
            this.chkExpectResponse.UseVisualStyleBackColor = true;
            // 
            // SendMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 266);
            this.Controls.Add(this.chkExpectResponse);
            this.Controls.Add(this.btnSendMessage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnRefreshUsers);
            this.Controls.Add(this.btnRefreshTrucks);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboUsers);
            this.Controls.Add(this.cboTrucks);
            this.Controls.Add(this.label1);
            this.Name = "SendMessage";
            this.Text = "SendMessage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboTrucks;
        private System.Windows.Forms.ComboBox cboUsers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRefreshTrucks;
        private System.Windows.Forms.Button btnRefreshUsers;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSendMessage;
        private System.Windows.Forms.CheckBox chkExpectResponse;
    }
}