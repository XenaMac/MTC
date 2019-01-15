namespace OctaHarness
{
    partial class TruckList
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
            this.components = new System.ComponentModel.Container();
            this.gvTruckList = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gvTruckList)).BeginInit();
            this.SuspendLayout();
            // 
            // gvTruckList
            // 
            this.gvTruckList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvTruckList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvTruckList.Location = new System.Drawing.Point(0, 0);
            this.gvTruckList.Name = "gvTruckList";
            this.gvTruckList.Size = new System.Drawing.Size(997, 349);
            this.gvTruckList.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // TruckList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 349);
            this.Controls.Add(this.gvTruckList);
            this.Name = "TruckList";
            this.Text = "TruckList";
            ((System.ComponentModel.ISupportInitialize)(this.gvTruckList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gvTruckList;
        private System.Windows.Forms.Timer timer1;
    }
}