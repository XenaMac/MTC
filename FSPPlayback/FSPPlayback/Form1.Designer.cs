namespace FSPPlayback
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gMapControl1 = new GMap.NET.WindowsForms.GMapControl();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.cboBeats = new System.Windows.Forms.ComboBox();
            this.lblSelectBeat = new System.Windows.Forms.Label();
            this.cboPlaybackSpeed = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStopPlayback = new System.Windows.Forms.Button();
            this.btnStartPlayback = new System.Windows.Forms.Button();
            this.txtEndHour = new System.Windows.Forms.TextBox();
            this.txtStartHour = new System.Windows.Forms.TextBox();
            this.chkLogon = new System.Windows.Forms.CheckBox();
            this.cboTrucks = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLoadTrucks = new System.Windows.Forms.Button();
            this.lblStart = new System.Windows.Forms.Label();
            this.lblEnd = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.btnLoadPlayback = new System.Windows.Forms.Button();
            this.gvData = new System.Windows.Forms.DataGridView();
            this.tmrPlayback = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkLeaveTrail = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gMapControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1188, 628);
            this.splitContainer1.SplitterDistance = 417;
            this.splitContainer1.TabIndex = 0;
            // 
            // gMapControl1
            // 
            this.gMapControl1.Bearing = 0F;
            this.gMapControl1.CanDragMap = true;
            this.gMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gMapControl1.GrayScaleMode = false;
            this.gMapControl1.LevelsKeepInMemmory = 5;
            this.gMapControl1.Location = new System.Drawing.Point(0, 0);
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
            this.gMapControl1.Size = new System.Drawing.Size(1188, 417);
            this.gMapControl1.TabIndex = 0;
            this.gMapControl1.Zoom = 15D;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.chkLeaveTrail);
            this.splitContainer2.Panel1.Controls.Add(this.cboBeats);
            this.splitContainer2.Panel1.Controls.Add(this.lblSelectBeat);
            this.splitContainer2.Panel1.Controls.Add(this.cboPlaybackSpeed);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            this.splitContainer2.Panel1.Controls.Add(this.btnStopPlayback);
            this.splitContainer2.Panel1.Controls.Add(this.btnStartPlayback);
            this.splitContainer2.Panel1.Controls.Add(this.txtEndHour);
            this.splitContainer2.Panel1.Controls.Add(this.txtStartHour);
            this.splitContainer2.Panel1.Controls.Add(this.chkLogon);
            this.splitContainer2.Panel1.Controls.Add(this.cboTrucks);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            this.splitContainer2.Panel1.Controls.Add(this.btnLoadTrucks);
            this.splitContainer2.Panel1.Controls.Add(this.lblStart);
            this.splitContainer2.Panel1.Controls.Add(this.lblEnd);
            this.splitContainer2.Panel1.Controls.Add(this.dtpStart);
            this.splitContainer2.Panel1.Controls.Add(this.dtpEnd);
            this.splitContainer2.Panel1.Controls.Add(this.btnLoadPlayback);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.gvData);
            this.splitContainer2.Size = new System.Drawing.Size(1188, 207);
            this.splitContainer2.SplitterDistance = 346;
            this.splitContainer2.TabIndex = 16;
            // 
            // cboBeats
            // 
            this.cboBeats.FormattingEnabled = true;
            this.cboBeats.Location = new System.Drawing.Point(243, 83);
            this.cboBeats.Name = "cboBeats";
            this.cboBeats.Size = new System.Drawing.Size(91, 21);
            this.cboBeats.TabIndex = 32;
            this.toolTip1.SetToolTip(this.cboBeats, "Select data by beat, ignored if truck is selected");
            // 
            // lblSelectBeat
            // 
            this.lblSelectBeat.AutoSize = true;
            this.lblSelectBeat.Location = new System.Drawing.Point(175, 86);
            this.lblSelectBeat.Name = "lblSelectBeat";
            this.lblSelectBeat.Size = new System.Drawing.Size(62, 13);
            this.lblSelectBeat.TabIndex = 31;
            this.lblSelectBeat.Text = "Select Beat";
            // 
            // cboPlaybackSpeed
            // 
            this.cboPlaybackSpeed.FormattingEnabled = true;
            this.cboPlaybackSpeed.Items.AddRange(new object[] {
            "1x",
            "2x",
            "3x",
            "4x",
            "5x",
            "10x",
            "20x",
            "50x"});
            this.cboPlaybackSpeed.Location = new System.Drawing.Point(266, 109);
            this.cboPlaybackSpeed.Name = "cboPlaybackSpeed";
            this.cboPlaybackSpeed.Size = new System.Drawing.Size(68, 21);
            this.cboPlaybackSpeed.TabIndex = 30;
            this.cboPlaybackSpeed.Text = "Select";
            this.toolTip1.SetToolTip(this.cboPlaybackSpeed, "Increase the speed of the playback");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(175, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Playback Speed";
            // 
            // btnStopPlayback
            // 
            this.btnStopPlayback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopPlayback.Location = new System.Drawing.Point(175, 179);
            this.btnStopPlayback.Name = "btnStopPlayback";
            this.btnStopPlayback.Size = new System.Drawing.Size(159, 23);
            this.btnStopPlayback.TabIndex = 28;
            this.btnStopPlayback.Text = "Stop Playback";
            this.toolTip1.SetToolTip(this.btnStopPlayback, "Stop the playback at the current record, you can restart from the same point");
            this.btnStopPlayback.UseVisualStyleBackColor = true;
            this.btnStopPlayback.Click += new System.EventHandler(this.btnStopPlayback_Click_1);
            // 
            // btnStartPlayback
            // 
            this.btnStartPlayback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartPlayback.Location = new System.Drawing.Point(12, 179);
            this.btnStartPlayback.Name = "btnStartPlayback";
            this.btnStartPlayback.Size = new System.Drawing.Size(157, 23);
            this.btnStartPlayback.TabIndex = 27;
            this.btnStartPlayback.Text = "Start Playback";
            this.toolTip1.SetToolTip(this.btnStartPlayback, "Start playback on either on first record or selected record.");
            this.btnStartPlayback.UseVisualStyleBackColor = true;
            this.btnStartPlayback.Click += new System.EventHandler(this.btnStartPlayback_Click_1);
            // 
            // txtEndHour
            // 
            this.txtEndHour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEndHour.Location = new System.Drawing.Point(284, 30);
            this.txtEndHour.Name = "txtEndHour";
            this.txtEndHour.Size = new System.Drawing.Size(50, 20);
            this.txtEndHour.TabIndex = 26;
            this.txtEndHour.Text = "23:59:59";
            this.toolTip1.SetToolTip(this.txtEndHour, "Ending Time (24 hour time)");
            // 
            // txtStartHour
            // 
            this.txtStartHour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStartHour.Location = new System.Drawing.Point(283, 4);
            this.txtStartHour.Name = "txtStartHour";
            this.txtStartHour.Size = new System.Drawing.Size(51, 20);
            this.txtStartHour.TabIndex = 25;
            this.txtStartHour.Text = "00:00:00";
            this.toolTip1.SetToolTip(this.txtStartHour, "Starting Time (24 hour time)");
            // 
            // chkLogon
            // 
            this.chkLogon.AutoSize = true;
            this.chkLogon.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkLogon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkLogon.Location = new System.Drawing.Point(12, 110);
            this.chkLogon.Name = "chkLogon";
            this.chkLogon.Size = new System.Drawing.Size(157, 17);
            this.chkLogon.TabIndex = 23;
            this.chkLogon.Text = "Only show logged on events";
            this.chkLogon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.chkLogon, "Only disply data where the driver is logged on");
            this.chkLogon.UseVisualStyleBackColor = true;
            // 
            // cboTrucks
            // 
            this.cboTrucks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboTrucks.FormattingEnabled = true;
            this.cboTrucks.Location = new System.Drawing.Point(77, 83);
            this.cboTrucks.Name = "cboTrucks";
            this.cboTrucks.Size = new System.Drawing.Size(92, 21);
            this.cboTrucks.TabIndex = 22;
            this.toolTip1.SetToolTip(this.cboTrucks, "If truck is selected, beat is ignored");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Select Truck";
            // 
            // btnLoadTrucks
            // 
            this.btnLoadTrucks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadTrucks.Location = new System.Drawing.Point(12, 53);
            this.btnLoadTrucks.Name = "btnLoadTrucks";
            this.btnLoadTrucks.Size = new System.Drawing.Size(322, 23);
            this.btnLoadTrucks.TabIndex = 20;
            this.btnLoadTrucks.Text = "Load Trucks and Beats";
            this.toolTip1.SetToolTip(this.btnLoadTrucks, "Load up data in the selected start and end ranges");
            this.btnLoadTrucks.UseVisualStyleBackColor = true;
            this.btnLoadTrucks.Click += new System.EventHandler(this.btnLoadTrucks_Click_1);
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point(9, 10);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(62, 13);
            this.lblStart.TabIndex = 16;
            this.lblStart.Text = "Select Start";
            // 
            // lblEnd
            // 
            this.lblEnd.AutoSize = true;
            this.lblEnd.Location = new System.Drawing.Point(9, 36);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(59, 13);
            this.lblEnd.TabIndex = 19;
            this.lblEnd.Text = "Select End";
            // 
            // dtpStart
            // 
            this.dtpStart.Location = new System.Drawing.Point(77, 4);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(200, 20);
            this.dtpStart.TabIndex = 17;
            this.toolTip1.SetToolTip(this.dtpStart, "Starting Date");
            // 
            // dtpEnd
            // 
            this.dtpEnd.Location = new System.Drawing.Point(77, 30);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(200, 20);
            this.dtpEnd.TabIndex = 18;
            this.toolTip1.SetToolTip(this.dtpEnd, "Ending Date");
            // 
            // btnLoadPlayback
            // 
            this.btnLoadPlayback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadPlayback.Location = new System.Drawing.Point(12, 133);
            this.btnLoadPlayback.Name = "btnLoadPlayback";
            this.btnLoadPlayback.Size = new System.Drawing.Size(322, 23);
            this.btnLoadPlayback.TabIndex = 24;
            this.btnLoadPlayback.Text = "Load Playback";
            this.toolTip1.SetToolTip(this.btnLoadPlayback, "Load up the selected playback data for the selected date range and selected truck" +
        " or beat");
            this.btnLoadPlayback.UseVisualStyleBackColor = true;
            this.btnLoadPlayback.Click += new System.EventHandler(this.btnLoadPlayback_Click_1);
            // 
            // gvData
            // 
            this.gvData.CausesValidation = false;
            this.gvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvData.Location = new System.Drawing.Point(0, 0);
            this.gvData.Name = "gvData";
            this.gvData.Size = new System.Drawing.Size(838, 207);
            this.gvData.TabIndex = 10;
            // 
            // tmrPlayback
            // 
            this.tmrPlayback.Interval = 30000;
            this.tmrPlayback.Tick += new System.EventHandler(this.tmrPlayback_Tick);
            // 
            // chkLeaveTrail
            // 
            this.chkLeaveTrail.AutoSize = true;
            this.chkLeaveTrail.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkLeaveTrail.Location = new System.Drawing.Point(13, 160);
            this.chkLeaveTrail.Name = "chkLeaveTrail";
            this.chkLeaveTrail.Size = new System.Drawing.Size(79, 17);
            this.chkLeaveTrail.TabIndex = 33;
            this.chkLeaveTrail.Text = "Leave Trail";
            this.chkLeaveTrail.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1188, 628);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "FSP Playback";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private GMap.NET.WindowsForms.GMapControl gMapControl1;
        private System.Windows.Forms.Timer tmrPlayback;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ComboBox cboPlaybackSpeed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStopPlayback;
        private System.Windows.Forms.Button btnStartPlayback;
        private System.Windows.Forms.TextBox txtEndHour;
        private System.Windows.Forms.TextBox txtStartHour;
        private System.Windows.Forms.CheckBox chkLogon;
        private System.Windows.Forms.ComboBox cboTrucks;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLoadTrucks;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Button btnLoadPlayback;
        private System.Windows.Forms.DataGridView gvData;
        private System.Windows.Forms.ComboBox cboBeats;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblSelectBeat;
        private System.Windows.Forms.CheckBox chkLeaveTrail;
    }
}

