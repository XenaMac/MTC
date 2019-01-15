namespace MTCPlaybackGMap
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
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView2 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView3 = new DevExpress.XtraCharts.LineSeriesView();
            this.scVertical = new System.Windows.Forms.SplitContainer();
            this.cboPlaybackSpeed = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.label14 = new System.Windows.Forms.Label();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblOnPatrol = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pbOnPatrol = new System.Windows.Forms.PictureBox();
            this.btnExportData = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.cboBeats = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cboContractors = new System.Windows.Forms.ComboBox();
            this.btnStopPlayback = new System.Windows.Forms.Button();
            this.btnPlayback = new System.Windows.Forms.Button();
            this.btnLoadData = new System.Windows.Forms.Button();
            this.gvStatusData = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.label7 = new System.Windows.Forms.Label();
            this.cboDrivers = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboCallsigns = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboTrucks = new System.Windows.Forms.ComboBox();
            this.btnPreLoad = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tEndTime = new System.Windows.Forms.DateTimePicker();
            this.dEndDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tStartTime = new System.Windows.Forms.DateTimePicker();
            this.dStartDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMapQuery = new System.Windows.Forms.Label();
            this.scHorizontal1 = new System.Windows.Forms.SplitContainer();
            this.gMapControl1 = new GMap.NET.WindowsForms.GMapControl();
            this.scHorizontal2 = new System.Windows.Forms.SplitContainer();
            this.gvData = new DevExpress.XtraGrid.GridControl();
            this.playBackRowBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.coltimeStamp = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTruckNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDriver = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCallSign = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colContractor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSchedule = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBeat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBeatSegment = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSpeed = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLon = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colHeading = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colHasAlarms = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAlarmInfo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colrunID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chartControl1 = new DevExpress.XtraCharts.ChartControl();
            this.tmrPlayback = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.scVertical)).BeginInit();
            this.scVertical.Panel1.SuspendLayout();
            this.scVertical.Panel2.SuspendLayout();
            this.scVertical.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOnPatrol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvStatusData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scHorizontal1)).BeginInit();
            this.scHorizontal1.Panel1.SuspendLayout();
            this.scHorizontal1.Panel2.SuspendLayout();
            this.scHorizontal1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scHorizontal2)).BeginInit();
            this.scHorizontal2.Panel1.SuspendLayout();
            this.scHorizontal2.Panel2.SuspendLayout();
            this.scHorizontal2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playBackRowBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView3)).BeginInit();
            this.SuspendLayout();
            // 
            // scVertical
            // 
            this.scVertical.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scVertical.Location = new System.Drawing.Point(0, 0);
            this.scVertical.Name = "scVertical";
            // 
            // scVertical.Panel1
            // 
            this.scVertical.Panel1.Controls.Add(this.cboPlaybackSpeed);
            this.scVertical.Panel1.Controls.Add(this.label16);
            this.scVertical.Panel1.Controls.Add(this.label15);
            this.scVertical.Panel1.Controls.Add(this.pictureBox6);
            this.scVertical.Panel1.Controls.Add(this.label14);
            this.scVertical.Panel1.Controls.Add(this.pictureBox5);
            this.scVertical.Panel1.Controls.Add(this.label13);
            this.scVertical.Panel1.Controls.Add(this.label12);
            this.scVertical.Panel1.Controls.Add(this.pictureBox4);
            this.scVertical.Panel1.Controls.Add(this.label11);
            this.scVertical.Panel1.Controls.Add(this.label10);
            this.scVertical.Panel1.Controls.Add(this.lblOnPatrol);
            this.scVertical.Panel1.Controls.Add(this.pictureBox3);
            this.scVertical.Panel1.Controls.Add(this.pictureBox2);
            this.scVertical.Panel1.Controls.Add(this.pictureBox1);
            this.scVertical.Panel1.Controls.Add(this.pbOnPatrol);
            this.scVertical.Panel1.Controls.Add(this.btnExportData);
            this.scVertical.Panel1.Controls.Add(this.label9);
            this.scVertical.Panel1.Controls.Add(this.cboBeats);
            this.scVertical.Panel1.Controls.Add(this.label8);
            this.scVertical.Panel1.Controls.Add(this.cboContractors);
            this.scVertical.Panel1.Controls.Add(this.btnStopPlayback);
            this.scVertical.Panel1.Controls.Add(this.btnPlayback);
            this.scVertical.Panel1.Controls.Add(this.btnLoadData);
            this.scVertical.Panel1.Controls.Add(this.gvStatusData);
            this.scVertical.Panel1.Controls.Add(this.label7);
            this.scVertical.Panel1.Controls.Add(this.cboDrivers);
            this.scVertical.Panel1.Controls.Add(this.label6);
            this.scVertical.Panel1.Controls.Add(this.cboCallsigns);
            this.scVertical.Panel1.Controls.Add(this.label5);
            this.scVertical.Panel1.Controls.Add(this.cboTrucks);
            this.scVertical.Panel1.Controls.Add(this.btnPreLoad);
            this.scVertical.Panel1.Controls.Add(this.label3);
            this.scVertical.Panel1.Controls.Add(this.tEndTime);
            this.scVertical.Panel1.Controls.Add(this.dEndDate);
            this.scVertical.Panel1.Controls.Add(this.label4);
            this.scVertical.Panel1.Controls.Add(this.label2);
            this.scVertical.Panel1.Controls.Add(this.tStartTime);
            this.scVertical.Panel1.Controls.Add(this.dStartDate);
            this.scVertical.Panel1.Controls.Add(this.label1);
            this.scVertical.Panel1.Controls.Add(this.lblMapQuery);
            // 
            // scVertical.Panel2
            // 
            this.scVertical.Panel2.Controls.Add(this.scHorizontal1);
            this.scVertical.Size = new System.Drawing.Size(1271, 769);
            this.scVertical.SplitterDistance = 289;
            this.scVertical.TabIndex = 0;
            // 
            // cboPlaybackSpeed
            // 
            this.cboPlaybackSpeed.FormattingEnabled = true;
            this.cboPlaybackSpeed.Items.AddRange(new object[] {
            "1x",
            "2x",
            "5x",
            "10x",
            "50x"});
            this.cboPlaybackSpeed.Location = new System.Drawing.Point(109, 356);
            this.cboPlaybackSpeed.Name = "cboPlaybackSpeed";
            this.cboPlaybackSpeed.Size = new System.Drawing.Size(167, 21);
            this.cboPlaybackSpeed.TabIndex = 47;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(18, 359);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(85, 13);
            this.label16.TabIndex = 46;
            this.label16.Text = "Playback Speed";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(156, 536);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(45, 13);
            this.label15.TabIndex = 45;
            this.label15.Text = "On Tow";
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox6.Image")));
            this.pictureBox6.Location = new System.Drawing.Point(129, 532);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(21, 22);
            this.pictureBox6.TabIndex = 44;
            this.pictureBox6.TabStop = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(156, 508);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(60, 13);
            this.label14.TabIndex = 43;
            this.label14.Text = "Logged On";
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(129, 504);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(21, 22);
            this.pictureBox5.TabIndex = 42;
            this.pictureBox5.TabStop = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(156, 476);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(62, 13);
            this.label13.TabIndex = 41;
            this.label13.Text = "On Incident";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(45, 532);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(52, 13);
            this.label12.TabIndex = 40;
            this.label12.Text = "En Route";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(18, 528);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(21, 22);
            this.pictureBox4.TabIndex = 39;
            this.pictureBox4.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(45, 563);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(185, 13);
            this.label11.TabIndex = 38;
            this.label11.Text = "Roll In, Roll Out, On Break, On Lunch";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(45, 504);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 13);
            this.label10.TabIndex = 37;
            this.label10.Text = "No Comms";
            // 
            // lblOnPatrol
            // 
            this.lblOnPatrol.AutoSize = true;
            this.lblOnPatrol.Location = new System.Drawing.Point(45, 477);
            this.lblOnPatrol.Name = "lblOnPatrol";
            this.lblOnPatrol.Size = new System.Drawing.Size(51, 13);
            this.lblOnPatrol.TabIndex = 36;
            this.lblOnPatrol.Text = "On Patrol";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(129, 472);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(21, 22);
            this.pictureBox3.TabIndex = 35;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(18, 559);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(21, 22);
            this.pictureBox2.TabIndex = 34;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(18, 500);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(21, 22);
            this.pictureBox1.TabIndex = 33;
            this.pictureBox1.TabStop = false;
            // 
            // pbOnPatrol
            // 
            this.pbOnPatrol.Image = ((System.Drawing.Image)(resources.GetObject("pbOnPatrol.Image")));
            this.pbOnPatrol.Location = new System.Drawing.Point(18, 472);
            this.pbOnPatrol.Name = "pbOnPatrol";
            this.pbOnPatrol.Size = new System.Drawing.Size(21, 22);
            this.pbOnPatrol.TabIndex = 32;
            this.pbOnPatrol.TabStop = false;
            // 
            // btnExportData
            // 
            this.btnExportData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportData.Location = new System.Drawing.Point(18, 441);
            this.btnExportData.Name = "btnExportData";
            this.btnExportData.Size = new System.Drawing.Size(258, 23);
            this.btnExportData.TabIndex = 31;
            this.btnExportData.Text = "Export Data";
            this.btnExportData.UseVisualStyleBackColor = true;
            this.btnExportData.Click += new System.EventHandler(this.btnExportData_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 217);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 13);
            this.label9.TabIndex = 30;
            this.label9.Text = "Select Beat";
            this.label9.Visible = false;
            // 
            // cboBeats
            // 
            this.cboBeats.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboBeats.FormattingEnabled = true;
            this.cboBeats.Location = new System.Drawing.Point(120, 214);
            this.cboBeats.Name = "cboBeats";
            this.cboBeats.Size = new System.Drawing.Size(156, 21);
            this.cboBeats.TabIndex = 29;
            this.cboBeats.Visible = false;
            this.cboBeats.SelectedIndexChanged += new System.EventHandler(this.cboBeats_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 190);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "Select Contractor";
            this.label8.Visible = false;
            // 
            // cboContractors
            // 
            this.cboContractors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboContractors.FormattingEnabled = true;
            this.cboContractors.Location = new System.Drawing.Point(120, 187);
            this.cboContractors.Name = "cboContractors";
            this.cboContractors.Size = new System.Drawing.Size(156, 21);
            this.cboContractors.TabIndex = 27;
            this.cboContractors.Visible = false;
            this.cboContractors.SelectedIndexChanged += new System.EventHandler(this.cboContractors_SelectedIndexChanged);
            // 
            // btnStopPlayback
            // 
            this.btnStopPlayback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopPlayback.Location = new System.Drawing.Point(18, 412);
            this.btnStopPlayback.Name = "btnStopPlayback";
            this.btnStopPlayback.Size = new System.Drawing.Size(258, 23);
            this.btnStopPlayback.TabIndex = 26;
            this.btnStopPlayback.Text = "Stop Playback";
            this.btnStopPlayback.UseVisualStyleBackColor = true;
            this.btnStopPlayback.Click += new System.EventHandler(this.btnStopPlayback_Click);
            // 
            // btnPlayback
            // 
            this.btnPlayback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlayback.Location = new System.Drawing.Point(18, 383);
            this.btnPlayback.Name = "btnPlayback";
            this.btnPlayback.Size = new System.Drawing.Size(258, 23);
            this.btnPlayback.TabIndex = 25;
            this.btnPlayback.Text = "Play Data";
            this.btnPlayback.UseVisualStyleBackColor = true;
            this.btnPlayback.Click += new System.EventHandler(this.btnPlayback_Click);
            // 
            // btnLoadData
            // 
            this.btnLoadData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadData.Location = new System.Drawing.Point(18, 329);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(258, 23);
            this.btnLoadData.TabIndex = 24;
            this.btnLoadData.Text = "Load Data";
            this.btnLoadData.UseVisualStyleBackColor = true;
            this.btnLoadData.Click += new System.EventHandler(this.btnLoadData_Click);
            // 
            // gvStatusData
            // 
            this.gvStatusData.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gvStatusData.Location = new System.Drawing.Point(0, 587);
            this.gvStatusData.MainView = this.gridView2;
            this.gvStatusData.Name = "gvStatusData";
            this.gvStatusData.Size = new System.Drawing.Size(289, 182);
            this.gvStatusData.TabIndex = 23;
            this.gvStatusData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gvStatusData;
            this.gridView2.Name = "gridView2";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 298);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Select Driver";
            // 
            // cboDrivers
            // 
            this.cboDrivers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboDrivers.FormattingEnabled = true;
            this.cboDrivers.Location = new System.Drawing.Point(120, 295);
            this.cboDrivers.Name = "cboDrivers";
            this.cboDrivers.Size = new System.Drawing.Size(156, 21);
            this.cboDrivers.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 271);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Select Callsign";
            // 
            // cboCallsigns
            // 
            this.cboCallsigns.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboCallsigns.FormattingEnabled = true;
            this.cboCallsigns.Location = new System.Drawing.Point(120, 268);
            this.cboCallsigns.Name = "cboCallsigns";
            this.cboCallsigns.Size = new System.Drawing.Size(156, 21);
            this.cboCallsigns.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 244);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Select Truck";
            // 
            // cboTrucks
            // 
            this.cboTrucks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboTrucks.FormattingEnabled = true;
            this.cboTrucks.Location = new System.Drawing.Point(120, 241);
            this.cboTrucks.Name = "cboTrucks";
            this.cboTrucks.Size = new System.Drawing.Size(156, 21);
            this.cboTrucks.TabIndex = 10;
            // 
            // btnPreLoad
            // 
            this.btnPreLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreLoad.Location = new System.Drawing.Point(18, 155);
            this.btnPreLoad.Name = "btnPreLoad";
            this.btnPreLoad.Size = new System.Drawing.Size(258, 23);
            this.btnPreLoad.TabIndex = 9;
            this.btnPreLoad.Text = "Pre Load";
            this.btnPreLoad.UseVisualStyleBackColor = true;
            this.btnPreLoad.Click += new System.EventHandler(this.btnPreLoad_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "End Time";
            // 
            // tEndTime
            // 
            this.tEndTime.Location = new System.Drawing.Point(76, 129);
            this.tEndTime.Name = "tEndTime";
            this.tEndTime.Size = new System.Drawing.Size(200, 20);
            this.tEndTime.TabIndex = 7;
            // 
            // dEndDate
            // 
            this.dEndDate.Location = new System.Drawing.Point(76, 103);
            this.dEndDate.Name = "dEndDate";
            this.dEndDate.Size = new System.Drawing.Size(200, 20);
            this.dEndDate.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "End Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Start Time";
            // 
            // tStartTime
            // 
            this.tStartTime.Location = new System.Drawing.Point(76, 77);
            this.tStartTime.Name = "tStartTime";
            this.tStartTime.Size = new System.Drawing.Size(200, 20);
            this.tStartTime.TabIndex = 3;
            // 
            // dStartDate
            // 
            this.dStartDate.Location = new System.Drawing.Point(76, 51);
            this.dStartDate.Name = "dStartDate";
            this.dStartDate.Size = new System.Drawing.Size(200, 20);
            this.dStartDate.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Start Date";
            // 
            // lblMapQuery
            // 
            this.lblMapQuery.AutoSize = true;
            this.lblMapQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMapQuery.Location = new System.Drawing.Point(13, 13);
            this.lblMapQuery.Name = "lblMapQuery";
            this.lblMapQuery.Size = new System.Drawing.Size(128, 26);
            this.lblMapQuery.TabIndex = 0;
            this.lblMapQuery.Text = "Map Query";
            // 
            // scHorizontal1
            // 
            this.scHorizontal1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scHorizontal1.Location = new System.Drawing.Point(0, 0);
            this.scHorizontal1.Name = "scHorizontal1";
            this.scHorizontal1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scHorizontal1.Panel1
            // 
            this.scHorizontal1.Panel1.Controls.Add(this.gMapControl1);
            // 
            // scHorizontal1.Panel2
            // 
            this.scHorizontal1.Panel2.Controls.Add(this.scHorizontal2);
            this.scHorizontal1.Size = new System.Drawing.Size(978, 769);
            this.scHorizontal1.SplitterDistance = 463;
            this.scHorizontal1.TabIndex = 0;
            // 
            // gMapControl1
            // 
            this.gMapControl1.Bearing = 0F;
            this.gMapControl1.CanDragMap = true;
            this.gMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gMapControl1.EmptyTileColor = System.Drawing.Color.Navy;
            this.gMapControl1.GrayScaleMode = false;
            this.gMapControl1.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
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
            this.gMapControl1.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gMapControl1.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gMapControl1.ShowTileGridLines = false;
            this.gMapControl1.Size = new System.Drawing.Size(978, 463);
            this.gMapControl1.TabIndex = 0;
            this.gMapControl1.Zoom = 15D;
            // 
            // scHorizontal2
            // 
            this.scHorizontal2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scHorizontal2.Location = new System.Drawing.Point(0, 0);
            this.scHorizontal2.Name = "scHorizontal2";
            this.scHorizontal2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scHorizontal2.Panel1
            // 
            this.scHorizontal2.Panel1.Controls.Add(this.gvData);
            // 
            // scHorizontal2.Panel2
            // 
            this.scHorizontal2.Panel2.Controls.Add(this.chartControl1);
            this.scHorizontal2.Size = new System.Drawing.Size(978, 302);
            this.scHorizontal2.SplitterDistance = 182;
            this.scHorizontal2.TabIndex = 0;
            // 
            // gvData
            // 
            this.gvData.DataSource = this.playBackRowBindingSource;
            this.gvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvData.Location = new System.Drawing.Point(0, 0);
            this.gvData.MainView = this.gridView1;
            this.gvData.Name = "gvData";
            this.gvData.Size = new System.Drawing.Size(978, 182);
            this.gvData.TabIndex = 0;
            this.gvData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // playBackRowBindingSource
            // 
            this.playBackRowBindingSource.DataSource = typeof(MTCPlaybackGMap.playBackRow);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.coltimeStamp,
            this.colTruckNumber,
            this.colDriver,
            this.colCallSign,
            this.colContractor,
            this.colStatus,
            this.colSchedule,
            this.colBeat,
            this.colBeatSegment,
            this.colSpeed,
            this.colLat,
            this.colLon,
            this.colHeading,
            this.colHasAlarms,
            this.colAlarmInfo,
            this.colrunID});
            this.gridView1.GridControl = this.gvData;
            this.gridView1.Name = "gridView1";
            // 
            // coltimeStamp
            // 
            this.coltimeStamp.DisplayFormat.FormatString = "HH:mm:ss";
            this.coltimeStamp.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.coltimeStamp.FieldName = "timeStamp";
            this.coltimeStamp.Name = "coltimeStamp";
            this.coltimeStamp.Visible = true;
            this.coltimeStamp.VisibleIndex = 0;
            // 
            // colTruckNumber
            // 
            this.colTruckNumber.FieldName = "TruckNumber";
            this.colTruckNumber.Name = "colTruckNumber";
            this.colTruckNumber.Visible = true;
            this.colTruckNumber.VisibleIndex = 1;
            // 
            // colDriver
            // 
            this.colDriver.FieldName = "Driver";
            this.colDriver.Name = "colDriver";
            this.colDriver.Visible = true;
            this.colDriver.VisibleIndex = 2;
            // 
            // colCallSign
            // 
            this.colCallSign.FieldName = "CallSign";
            this.colCallSign.Name = "colCallSign";
            this.colCallSign.Visible = true;
            this.colCallSign.VisibleIndex = 3;
            // 
            // colContractor
            // 
            this.colContractor.FieldName = "Contractor";
            this.colContractor.Name = "colContractor";
            this.colContractor.Visible = true;
            this.colContractor.VisibleIndex = 4;
            // 
            // colStatus
            // 
            this.colStatus.FieldName = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.Visible = true;
            this.colStatus.VisibleIndex = 5;
            // 
            // colSchedule
            // 
            this.colSchedule.FieldName = "Schedule";
            this.colSchedule.Name = "colSchedule";
            this.colSchedule.Visible = true;
            this.colSchedule.VisibleIndex = 6;
            // 
            // colBeat
            // 
            this.colBeat.FieldName = "Beat";
            this.colBeat.Name = "colBeat";
            this.colBeat.Visible = true;
            this.colBeat.VisibleIndex = 7;
            // 
            // colBeatSegment
            // 
            this.colBeatSegment.FieldName = "BeatSegment";
            this.colBeatSegment.Name = "colBeatSegment";
            this.colBeatSegment.Visible = true;
            this.colBeatSegment.VisibleIndex = 8;
            // 
            // colSpeed
            // 
            this.colSpeed.FieldName = "Speed";
            this.colSpeed.Name = "colSpeed";
            this.colSpeed.Visible = true;
            this.colSpeed.VisibleIndex = 9;
            // 
            // colLat
            // 
            this.colLat.FieldName = "Lat";
            this.colLat.Name = "colLat";
            this.colLat.Visible = true;
            this.colLat.VisibleIndex = 10;
            // 
            // colLon
            // 
            this.colLon.FieldName = "Lon";
            this.colLon.Name = "colLon";
            this.colLon.Visible = true;
            this.colLon.VisibleIndex = 11;
            // 
            // colHeading
            // 
            this.colHeading.FieldName = "Heading";
            this.colHeading.Name = "colHeading";
            this.colHeading.Visible = true;
            this.colHeading.VisibleIndex = 12;
            // 
            // colHasAlarms
            // 
            this.colHasAlarms.FieldName = "HasAlarms";
            this.colHasAlarms.Name = "colHasAlarms";
            this.colHasAlarms.Visible = true;
            this.colHasAlarms.VisibleIndex = 13;
            // 
            // colAlarmInfo
            // 
            this.colAlarmInfo.FieldName = "AlarmInfo";
            this.colAlarmInfo.Name = "colAlarmInfo";
            this.colAlarmInfo.Visible = true;
            this.colAlarmInfo.VisibleIndex = 14;
            // 
            // colrunID
            // 
            this.colrunID.FieldName = "runID";
            this.colrunID.Name = "colrunID";
            // 
            // chartControl1
            // 
            this.chartControl1.DataBindings = null;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            this.chartControl1.Diagram = xyDiagram1;
            this.chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartControl1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            this.chartControl1.Location = new System.Drawing.Point(0, 0);
            this.chartControl1.Name = "chartControl1";
            series1.Name = "Series 1";
            series1.View = lineSeriesView1;
            series2.Name = "Series 2";
            series2.View = lineSeriesView2;
            this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2};
            this.chartControl1.SeriesTemplate.View = lineSeriesView3;
            this.chartControl1.Size = new System.Drawing.Size(978, 116);
            this.chartControl1.TabIndex = 0;
            // 
            // tmrPlayback
            // 
            this.tmrPlayback.Enabled = true;
            this.tmrPlayback.Interval = 10000;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1271, 769);
            this.Controls.Add(this.scVertical);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "MTC Playback Tool - Provided by LATA";
            this.scVertical.Panel1.ResumeLayout(false);
            this.scVertical.Panel1.PerformLayout();
            this.scVertical.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scVertical)).EndInit();
            this.scVertical.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOnPatrol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvStatusData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.scHorizontal1.Panel1.ResumeLayout(false);
            this.scHorizontal1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scHorizontal1)).EndInit();
            this.scHorizontal1.ResumeLayout(false);
            this.scHorizontal2.Panel1.ResumeLayout(false);
            this.scHorizontal2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scHorizontal2)).EndInit();
            this.scHorizontal2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playBackRowBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scVertical;
        private System.Windows.Forms.SplitContainer scHorizontal1;
        private GMap.NET.WindowsForms.GMapControl gMapControl1;
        private System.Windows.Forms.SplitContainer scHorizontal2;
        private DevExpress.XtraCharts.ChartControl chartControl1;
        private System.Windows.Forms.DateTimePicker tStartTime;
        private System.Windows.Forms.DateTimePicker dStartDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMapQuery;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker tEndTime;
        private System.Windows.Forms.DateTimePicker dEndDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboTrucks;
        private System.Windows.Forms.Button btnPreLoad;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboDrivers;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboCallsigns;
        private DevExpress.XtraGrid.GridControl gvData;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.GridControl gvStatusData;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private System.Windows.Forms.Button btnPlayback;
        private System.Windows.Forms.Button btnLoadData;
        private System.Windows.Forms.Timer tmrPlayback;
        private System.Windows.Forms.Button btnStopPlayback;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboBeats;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboContractors;
        private System.Windows.Forms.BindingSource playBackRowBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn coltimeStamp;
        private DevExpress.XtraGrid.Columns.GridColumn colTruckNumber;
        private DevExpress.XtraGrid.Columns.GridColumn colDriver;
        private DevExpress.XtraGrid.Columns.GridColumn colCallSign;
        private DevExpress.XtraGrid.Columns.GridColumn colContractor;
        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
        private DevExpress.XtraGrid.Columns.GridColumn colSchedule;
        private DevExpress.XtraGrid.Columns.GridColumn colBeat;
        private DevExpress.XtraGrid.Columns.GridColumn colBeatSegment;
        private DevExpress.XtraGrid.Columns.GridColumn colSpeed;
        private DevExpress.XtraGrid.Columns.GridColumn colLat;
        private DevExpress.XtraGrid.Columns.GridColumn colLon;
        private DevExpress.XtraGrid.Columns.GridColumn colHeading;
        private DevExpress.XtraGrid.Columns.GridColumn colHasAlarms;
        private DevExpress.XtraGrid.Columns.GridColumn colAlarmInfo;
        private DevExpress.XtraGrid.Columns.GridColumn colrunID;
        private System.Windows.Forms.Button btnExportData;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblOnPatrol;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pbOnPatrol;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.ComboBox cboPlaybackSpeed;
        private System.Windows.Forms.Label label16;
    }
}

