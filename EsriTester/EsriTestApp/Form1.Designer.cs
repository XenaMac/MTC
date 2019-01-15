namespace EsriTestApp
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblBeatSeg = new System.Windows.Forms.Label();
            this.cboBeatSegs = new System.Windows.Forms.ComboBox();
            this.btnCheckIntersection = new System.Windows.Forms.Button();
            this.lblInside = new System.Windows.Forms.Label();
            this.txtLon = new System.Windows.Forms.TextBox();
            this.lblLon = new System.Windows.Forms.Label();
            this.txtLat = new System.Windows.Forms.TextBox();
            this.lblLat = new System.Windows.Forms.Label();
            this.txtLwkid = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtWkid = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtYmax = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtXmax = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtYmin = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtXmin = new System.Windows.Forms.TextBox();
            this.lblXmin = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gMapControl1 = new GMap.NET.WindowsForms.GMapControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.txtBeatSeg = new System.Windows.Forms.ToolStripTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.lblSegID = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(716, 481);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblSegID);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.lblBeatSeg);
            this.tabPage1.Controls.Add(this.cboBeatSegs);
            this.tabPage1.Controls.Add(this.btnCheckIntersection);
            this.tabPage1.Controls.Add(this.lblInside);
            this.tabPage1.Controls.Add(this.txtLon);
            this.tabPage1.Controls.Add(this.lblLon);
            this.tabPage1.Controls.Add(this.txtLat);
            this.tabPage1.Controls.Add(this.lblLat);
            this.tabPage1.Controls.Add(this.txtLwkid);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.txtWkid);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.txtYmax);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.txtXmax);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.txtYmin);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.txtXmin);
            this.tabPage1.Controls.Add(this.lblXmin);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(708, 455);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Load Data";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblBeatSeg
            // 
            this.lblBeatSeg.AutoSize = true;
            this.lblBeatSeg.Location = new System.Drawing.Point(15, 274);
            this.lblBeatSeg.Name = "lblBeatSeg";
            this.lblBeatSeg.Size = new System.Drawing.Size(51, 13);
            this.lblBeatSeg.TabIndex = 34;
            this.lblBeatSeg.Text = "Beat Seg";
            // 
            // cboBeatSegs
            // 
            this.cboBeatSegs.FormattingEnabled = true;
            this.cboBeatSegs.Location = new System.Drawing.Point(74, 271);
            this.cboBeatSegs.Name = "cboBeatSegs";
            this.cboBeatSegs.Size = new System.Drawing.Size(198, 21);
            this.cboBeatSegs.TabIndex = 33;
            // 
            // btnCheckIntersection
            // 
            this.btnCheckIntersection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckIntersection.Location = new System.Drawing.Point(74, 327);
            this.btnCheckIntersection.Name = "btnCheckIntersection";
            this.btnCheckIntersection.Size = new System.Drawing.Size(198, 23);
            this.btnCheckIntersection.TabIndex = 32;
            this.btnCheckIntersection.Text = "Check Intersection";
            this.btnCheckIntersection.UseVisualStyleBackColor = true;
            this.btnCheckIntersection.Click += new System.EventHandler(this.btnCheckIntersection_Click);
            // 
            // lblInside
            // 
            this.lblInside.AutoSize = true;
            this.lblInside.Location = new System.Drawing.Point(71, 301);
            this.lblInside.Name = "lblInside";
            this.lblInside.Size = new System.Drawing.Size(174, 13);
            this.lblInside.TabIndex = 31;
            this.lblInside.Text = "Check if Lat/Lon is inside boundary";
            // 
            // txtLon
            // 
            this.txtLon.Location = new System.Drawing.Point(74, 244);
            this.txtLon.Name = "txtLon";
            this.txtLon.Size = new System.Drawing.Size(198, 20);
            this.txtLon.TabIndex = 30;
            this.txtLon.Text = "-106.6506";
            // 
            // lblLon
            // 
            this.lblLon.AutoSize = true;
            this.lblLon.Location = new System.Drawing.Point(15, 247);
            this.lblLon.Name = "lblLon";
            this.lblLon.Size = new System.Drawing.Size(25, 13);
            this.lblLon.TabIndex = 29;
            this.lblLon.Text = "Lon";
            // 
            // txtLat
            // 
            this.txtLat.Location = new System.Drawing.Point(74, 218);
            this.txtLat.Name = "txtLat";
            this.txtLat.Size = new System.Drawing.Size(198, 20);
            this.txtLat.TabIndex = 28;
            this.txtLat.Text = "35.0844";
            // 
            // lblLat
            // 
            this.lblLat.AutoSize = true;
            this.lblLat.Location = new System.Drawing.Point(15, 221);
            this.lblLat.Name = "lblLat";
            this.lblLat.Size = new System.Drawing.Size(22, 13);
            this.lblLat.TabIndex = 27;
            this.lblLat.Text = "Lat";
            // 
            // txtLwkid
            // 
            this.txtLwkid.Location = new System.Drawing.Point(74, 164);
            this.txtLwkid.Name = "txtLwkid";
            this.txtLwkid.Size = new System.Drawing.Size(198, 20);
            this.txtLwkid.TabIndex = 26;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 167);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "lWkid";
            // 
            // txtWkid
            // 
            this.txtWkid.Location = new System.Drawing.Point(74, 138);
            this.txtWkid.Name = "txtWkid";
            this.txtWkid.Size = new System.Drawing.Size(198, 20);
            this.txtWkid.TabIndex = 24;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 141);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "wkid";
            // 
            // txtYmax
            // 
            this.txtYmax.Location = new System.Drawing.Point(74, 112);
            this.txtYmax.Name = "txtYmax";
            this.txtYmax.Size = new System.Drawing.Size(198, 20);
            this.txtYmax.TabIndex = 22;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "ymax";
            // 
            // txtXmax
            // 
            this.txtXmax.Location = new System.Drawing.Point(74, 86);
            this.txtXmax.Name = "txtXmax";
            this.txtXmax.Size = new System.Drawing.Size(198, 20);
            this.txtXmax.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "xmax";
            // 
            // txtYmin
            // 
            this.txtYmin.Location = new System.Drawing.Point(74, 60);
            this.txtYmin.Name = "txtYmin";
            this.txtYmin.Size = new System.Drawing.Size(198, 20);
            this.txtYmin.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "ymin";
            // 
            // txtXmin
            // 
            this.txtXmin.Location = new System.Drawing.Point(74, 34);
            this.txtXmin.Name = "txtXmin";
            this.txtXmin.Size = new System.Drawing.Size(198, 20);
            this.txtXmin.TabIndex = 16;
            // 
            // lblXmin
            // 
            this.lblXmin.AutoSize = true;
            this.lblXmin.Location = new System.Drawing.Point(12, 37);
            this.lblXmin.Name = "lblXmin";
            this.lblXmin.Size = new System.Drawing.Size(28, 13);
            this.lblXmin.TabIndex = 15;
            this.lblXmin.Text = "xmin";
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(702, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Read FS";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gMapControl1);
            this.tabPage2.Controls.Add(this.menuStrip1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(708, 455);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "View Data";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gMapControl1
            // 
            this.gMapControl1.Bearing = 0F;
            this.gMapControl1.CanDragMap = true;
            this.gMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gMapControl1.GrayScaleMode = false;
            this.gMapControl1.LevelsKeepInMemmory = 5;
            this.gMapControl1.Location = new System.Drawing.Point(3, 30);
            this.gMapControl1.MarkersEnabled = true;
            this.gMapControl1.MaxZoom = 18;
            this.gMapControl1.MinZoom = 2;
            this.gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gMapControl1.Name = "gMapControl1";
            this.gMapControl1.NegativeMode = false;
            this.gMapControl1.PolygonsEnabled = true;
            this.gMapControl1.RetryLoadTile = 0;
            this.gMapControl1.RoutesEnabled = true;
            this.gMapControl1.ShowTileGridLines = false;
            this.gMapControl1.Size = new System.Drawing.Size(702, 422);
            this.gMapControl1.TabIndex = 0;
            this.gMapControl1.Zoom = 0D;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtBeatSeg});
            this.menuStrip1.Location = new System.Drawing.Point(3, 3);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(702, 27);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // txtBeatSeg
            // 
            this.txtBeatSeg.Name = "txtBeatSeg";
            this.txtBeatSeg.Size = new System.Drawing.Size(100, 23);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(74, 357);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(198, 23);
            this.button2.TabIndex = 35;
            this.button2.Text = "Find Seg";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lblSegID
            // 
            this.lblSegID.AutoSize = true;
            this.lblSegID.Location = new System.Drawing.Point(278, 362);
            this.lblSegID.Name = "lblSegID";
            this.lblSegID.Size = new System.Drawing.Size(75, 13);
            this.lblSegID.TabIndex = 36;
            this.lblSegID.Text = "Click to check";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 481);
            this.Controls.Add(this.tabControl1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox txtLwkid;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtWkid;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtYmax;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtXmax;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtYmin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtXmin;
        private System.Windows.Forms.Label lblXmin;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabPage2;
        private GMap.NET.WindowsForms.GMapControl gMapControl1;
        private System.Windows.Forms.TextBox txtLat;
        private System.Windows.Forms.Label lblLat;
        private System.Windows.Forms.TextBox txtLon;
        private System.Windows.Forms.Label lblLon;
        private System.Windows.Forms.Button btnCheckIntersection;
        private System.Windows.Forms.Label lblInside;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripTextBox txtBeatSeg;
        private System.Windows.Forms.Label lblBeatSeg;
        private System.Windows.Forms.ComboBox cboBeatSegs;
        private System.Windows.Forms.Label lblSegID;
        private System.Windows.Forms.Button button2;

    }
}

