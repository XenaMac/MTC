namespace OctaHarness
{
    partial class Assists
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Assists));
            this.label1 = new System.Windows.Forms.Label();
            this.txtAssistID = new System.Windows.Forms.TextBox();
            this.cboIncidents = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboTruck = new System.Windows.Forms.ComboBox();
            this.btnPostAssist = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Assist ID";
            // 
            // txtAssistID
            // 
            this.txtAssistID.Location = new System.Drawing.Point(105, 10);
            this.txtAssistID.Name = "txtAssistID";
            this.txtAssistID.Size = new System.Drawing.Size(357, 20);
            this.txtAssistID.TabIndex = 1;
            // 
            // cboIncidents
            // 
            this.cboIncidents.FormattingEnabled = true;
            this.cboIncidents.Location = new System.Drawing.Point(105, 37);
            this.cboIncidents.Name = "cboIncidents";
            this.cboIncidents.Size = new System.Drawing.Size(357, 21);
            this.cboIncidents.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Incident";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Truck";
            // 
            // cboTruck
            // 
            this.cboTruck.FormattingEnabled = true;
            this.cboTruck.Location = new System.Drawing.Point(105, 64);
            this.cboTruck.Name = "cboTruck";
            this.cboTruck.Size = new System.Drawing.Size(357, 21);
            this.cboTruck.TabIndex = 4;
            // 
            // btnPostAssist
            // 
            /*
            this.btnPostAssist.Location = new System.Drawing.Point(387, 91);
            this.btnPostAssist.Name = "btnPostAssist";
            this.btnPostAssist.Size = new System.Drawing.Size(75, 23);
            this.btnPostAssist.TabIndex = 6;
            this.btnPostAssist.Text = "Post";
            this.btnPostAssist.UseVisualStyleBackColor = true;
            this.btnPostAssist.Click += new System.EventHandler(this.btnPostAssist_Click);*/
            // 
            // Assists
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 176);
            this.Controls.Add(this.btnPostAssist);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboTruck);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboIncidents);
            this.Controls.Add(this.txtAssistID);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Assists";
            this.Text = "Assists";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAssistID;
        private System.Windows.Forms.ComboBox cboIncidents;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboTruck;
        private System.Windows.Forms.Button btnPostAssist;
    }
}