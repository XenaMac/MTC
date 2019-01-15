namespace OctaHarness
{
    partial class WAZEMessage
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
            this.ddlServiceAddress = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSendWAZE = new System.Windows.Forms.Button();
            this.txtUUID = new System.Windows.Forms.TextBox();
            this.txtAlert = new System.Windows.Forms.TextBox();
            this.txtLat = new System.Windows.Forms.TextBox();
            this.txtLon = new System.Windows.Forms.TextBox();
            this.txtNThumbsUp = new System.Windows.Forms.TextBox();
            this.txtConfidence = new System.Windows.Forms.TextBox();
            this.txtReliability = new System.Windows.Forms.TextBox();
            this.txtStreet = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Lat = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.ddlServiceRef = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ddlServiceAddress
            // 
            this.ddlServiceAddress.FormattingEnabled = true;
            this.ddlServiceAddress.Items.AddRange(new object[] {
            "38.124.164.213",
            "127.0.0.1",
            "38.124.164.212",
            "38.124.164.211"});
            this.ddlServiceAddress.Location = new System.Drawing.Point(105, 12);
            this.ddlServiceAddress.Name = "ddlServiceAddress";
            this.ddlServiceAddress.Size = new System.Drawing.Size(174, 21);
            this.ddlServiceAddress.TabIndex = 81;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 80;
            this.label2.Text = "Service Address";
            // 
            // btnSendWAZE
            // 
            this.btnSendWAZE.Location = new System.Drawing.Point(204, 274);
            this.btnSendWAZE.Name = "btnSendWAZE";
            this.btnSendWAZE.Size = new System.Drawing.Size(75, 23);
            this.btnSendWAZE.TabIndex = 82;
            this.btnSendWAZE.Text = "Send WAZE";
            this.btnSendWAZE.UseVisualStyleBackColor = true;
            this.btnSendWAZE.Click += new System.EventHandler(this.btnSendWAZE_Click);
            // 
            // txtUUID
            // 
            this.txtUUID.Location = new System.Drawing.Point(105, 66);
            this.txtUUID.Name = "txtUUID";
            this.txtUUID.Size = new System.Drawing.Size(174, 20);
            this.txtUUID.TabIndex = 83;
            // 
            // txtAlert
            // 
            this.txtAlert.Location = new System.Drawing.Point(105, 92);
            this.txtAlert.Name = "txtAlert";
            this.txtAlert.Size = new System.Drawing.Size(174, 20);
            this.txtAlert.TabIndex = 84;
            this.txtAlert.Text = "alert";
            // 
            // txtLat
            // 
            this.txtLat.Location = new System.Drawing.Point(105, 118);
            this.txtLat.Name = "txtLat";
            this.txtLat.Size = new System.Drawing.Size(174, 20);
            this.txtLat.TabIndex = 85;
            this.txtLat.Text = "35.096695";
            // 
            // txtLon
            // 
            this.txtLon.Location = new System.Drawing.Point(105, 144);
            this.txtLon.Name = "txtLon";
            this.txtLon.Size = new System.Drawing.Size(174, 20);
            this.txtLon.TabIndex = 86;
            this.txtLon.Text = "-106.566354";
            // 
            // txtNThumbsUp
            // 
            this.txtNThumbsUp.Location = new System.Drawing.Point(105, 170);
            this.txtNThumbsUp.Name = "txtNThumbsUp";
            this.txtNThumbsUp.Size = new System.Drawing.Size(174, 20);
            this.txtNThumbsUp.TabIndex = 87;
            this.txtNThumbsUp.Text = "1";
            // 
            // txtConfidence
            // 
            this.txtConfidence.Location = new System.Drawing.Point(105, 196);
            this.txtConfidence.Name = "txtConfidence";
            this.txtConfidence.Size = new System.Drawing.Size(174, 20);
            this.txtConfidence.TabIndex = 88;
            this.txtConfidence.Text = "5";
            // 
            // txtReliability
            // 
            this.txtReliability.Location = new System.Drawing.Point(105, 222);
            this.txtReliability.Name = "txtReliability";
            this.txtReliability.Size = new System.Drawing.Size(174, 20);
            this.txtReliability.TabIndex = 89;
            this.txtReliability.Text = "5";
            // 
            // txtStreet
            // 
            this.txtStreet.Location = new System.Drawing.Point(105, 248);
            this.txtStreet.Name = "txtStreet";
            this.txtStreet.Size = new System.Drawing.Size(174, 20);
            this.txtStreet.TabIndex = 90;
            this.txtStreet.Text = "1600 Pennsylvania Ave";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 91;
            this.label1.Text = "UUID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 92;
            this.label3.Text = "Title";
            // 
            // Lat
            // 
            this.Lat.AutoSize = true;
            this.Lat.Location = new System.Drawing.Point(12, 121);
            this.Lat.Name = "Lat";
            this.Lat.Size = new System.Drawing.Size(22, 13);
            this.Lat.TabIndex = 93;
            this.Lat.Text = "Lat";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 13);
            this.label5.TabIndex = 94;
            this.label5.Text = "Lon";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 173);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 95;
            this.label6.Text = "nThumbsUp";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 199);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 13);
            this.label7.TabIndex = 96;
            this.label7.Text = "Confidence";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 225);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 97;
            this.label8.Text = "Reliability";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 251);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 13);
            this.label9.TabIndex = 98;
            this.label9.Text = "Street";
            // 
            // ddlServiceRef
            // 
            this.ddlServiceRef.FormattingEnabled = true;
            this.ddlServiceRef.Items.AddRange(new object[] {
            "38.124.164.213:9017/TowTruckService.svc",
            "localhost:9017/TowTruckService.svc",
            "38.124.164.212:9017/TowTruckService.svc",
            "38.124.164.211:9017/TowTruckService.svc"});
            this.ddlServiceRef.Location = new System.Drawing.Point(105, 39);
            this.ddlServiceRef.Name = "ddlServiceRef";
            this.ddlServiceRef.Size = new System.Drawing.Size(174, 21);
            this.ddlServiceRef.TabIndex = 100;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(13, 42);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(24, 13);
            this.label12.TabIndex = 99;
            this.label12.Text = "Ref";
            // 
            // WAZEMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 315);
            this.Controls.Add(this.ddlServiceRef);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Lat);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtStreet);
            this.Controls.Add(this.txtReliability);
            this.Controls.Add(this.txtConfidence);
            this.Controls.Add(this.txtNThumbsUp);
            this.Controls.Add(this.txtLon);
            this.Controls.Add(this.txtLat);
            this.Controls.Add(this.txtAlert);
            this.Controls.Add(this.txtUUID);
            this.Controls.Add(this.btnSendWAZE);
            this.Controls.Add(this.ddlServiceAddress);
            this.Controls.Add(this.label2);
            this.Name = "WAZEMessage";
            this.Text = "WAZEMessage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ddlServiceAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSendWAZE;
        private System.Windows.Forms.TextBox txtUUID;
        private System.Windows.Forms.TextBox txtAlert;
        private System.Windows.Forms.TextBox txtLat;
        private System.Windows.Forms.TextBox txtLon;
        private System.Windows.Forms.TextBox txtNThumbsUp;
        private System.Windows.Forms.TextBox txtConfidence;
        private System.Windows.Forms.TextBox txtReliability;
        private System.Windows.Forms.TextBox txtStreet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label Lat;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox ddlServiceRef;
        private System.Windows.Forms.Label label12;
    }
}