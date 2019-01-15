namespace OctaHarness
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
            this.txtMessageStatus = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSendAbePacket = new System.Windows.Forms.Button();
            this.txtMLon = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMLat = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMaxSpeed = new System.Windows.Forms.TextBox();
            this.lblDriverID = new System.Windows.Forms.Label();
            this.btnSendPacket = new System.Windows.Forms.Button();
            this.txtLon = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtLat = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.txtSpeed = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtHead = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtIPAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.btnSendState = new System.Windows.Forms.Button();
            this.btnParseKML = new System.Windows.Forms.Button();
            this.btnStopPlay = new System.Windows.Forms.Button();
            this.btnClearStatus = new System.Windows.Forms.Button();
            this.btnPlayCSV = new System.Windows.Forms.Button();
            this.btnStopCSV = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.btnCurrentTrucks = new System.Windows.Forms.Button();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.btnAlarmCheck = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnGetAlarms = new System.Windows.Forms.Button();
            this.txtCell = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtReceived = new System.Windows.Forms.TextBox();
            this.ddlServiceAddress = new System.Windows.Forms.ComboBox();
            this.ddlServiceRef = new System.Windows.Forms.ComboBox();
            this.btnAssistRequest = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.cboLocations = new System.Windows.Forms.ComboBox();
            this.txtIPHistory = new System.Windows.Forms.TextBox();
            this.btnSendIPHistory = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnInjectWAZE = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtMessageStatus
            // 
            this.txtMessageStatus.Location = new System.Drawing.Point(288, 31);
            this.txtMessageStatus.Multiline = true;
            this.txtMessageStatus.Name = "txtMessageStatus";
            this.txtMessageStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMessageStatus.Size = new System.Drawing.Size(241, 312);
            this.txtMessageStatus.TabIndex = 23;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(288, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "Sent";
            // 
            // btnSendAbePacket
            // 
            this.btnSendAbePacket.Location = new System.Drawing.Point(17, 372);
            this.btnSendAbePacket.Name = "btnSendAbePacket";
            this.btnSendAbePacket.Size = new System.Drawing.Size(265, 23);
            this.btnSendAbePacket.TabIndex = 53;
            this.btnSendAbePacket.Text = "Send Abe Packet";
            this.btnSendAbePacket.UseVisualStyleBackColor = true;
            this.btnSendAbePacket.Click += new System.EventHandler(this.btnSendAbePacket_Click);
            // 
            // txtMLon
            // 
            this.txtMLon.Location = new System.Drawing.Point(174, 184);
            this.txtMLon.Name = "txtMLon";
            this.txtMLon.Size = new System.Drawing.Size(107, 20);
            this.txtMLon.TabIndex = 52;
            this.txtMLon.Text = "-106.571552";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(134, 187);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 51;
            this.label6.Text = "MLon";
            // 
            // txtMLat
            // 
            this.txtMLat.Location = new System.Drawing.Point(49, 184);
            this.txtMLat.Name = "txtMLat";
            this.txtMLat.Size = new System.Drawing.Size(79, 20);
            this.txtMLat.TabIndex = 50;
            this.txtMLat.Text = "35.100391";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 187);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(31, 13);
            this.label11.TabIndex = 49;
            this.label11.Text = "MLat";
            // 
            // txtMaxSpeed
            // 
            this.txtMaxSpeed.Location = new System.Drawing.Point(107, 158);
            this.txtMaxSpeed.Name = "txtMaxSpeed";
            this.txtMaxSpeed.Size = new System.Drawing.Size(175, 20);
            this.txtMaxSpeed.TabIndex = 48;
            this.txtMaxSpeed.Text = "65";
            // 
            // lblDriverID
            // 
            this.lblDriverID.AutoSize = true;
            this.lblDriverID.Location = new System.Drawing.Point(15, 161);
            this.lblDriverID.Name = "lblDriverID";
            this.lblDriverID.Size = new System.Drawing.Size(61, 13);
            this.lblDriverID.TabIndex = 47;
            this.lblDriverID.Text = "Max Speed";
            // 
            // btnSendPacket
            // 
            this.btnSendPacket.Location = new System.Drawing.Point(16, 401);
            this.btnSendPacket.Name = "btnSendPacket";
            this.btnSendPacket.Size = new System.Drawing.Size(126, 23);
            this.btnSendPacket.TabIndex = 46;
            this.btnSendPacket.Text = "&Send GPS Packet";
            this.btnSendPacket.UseVisualStyleBackColor = true;
            this.btnSendPacket.Click += new System.EventHandler(this.btnSendPacket_Click_1);
            // 
            // txtLon
            // 
            this.txtLon.Location = new System.Drawing.Point(174, 90);
            this.txtLon.Name = "txtLon";
            this.txtLon.Size = new System.Drawing.Size(108, 20);
            this.txtLon.TabIndex = 45;
            this.txtLon.Text = "-106.571552";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(143, 93);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(25, 13);
            this.label10.TabIndex = 44;
            this.label10.Text = "Lon";
            // 
            // txtLat
            // 
            this.txtLat.Location = new System.Drawing.Point(49, 90);
            this.txtLat.Name = "txtLat";
            this.txtLat.Size = new System.Drawing.Size(88, 20);
            this.txtLat.TabIndex = 43;
            this.txtLat.Text = "35.100391";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 93);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(22, 13);
            this.label9.TabIndex = 42;
            this.label9.Text = "Lat";
            // 
            // txtTime
            // 
            this.txtTime.Location = new System.Drawing.Point(107, 210);
            this.txtTime.Name = "txtTime";
            this.txtTime.ReadOnly = true;
            this.txtTime.Size = new System.Drawing.Size(175, 20);
            this.txtTime.TabIndex = 41;
            // 
            // txtSpeed
            // 
            this.txtSpeed.Location = new System.Drawing.Point(107, 64);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.Size = new System.Drawing.Size(175, 20);
            this.txtSpeed.TabIndex = 40;
            this.txtSpeed.Text = "55";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 39;
            this.label7.Text = "Speed";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 213);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 38;
            this.label5.Text = "Time";
            // 
            // txtHead
            // 
            this.txtHead.Location = new System.Drawing.Point(107, 38);
            this.txtHead.Name = "txtHead";
            this.txtHead.Size = new System.Drawing.Size(175, 20);
            this.txtHead.TabIndex = 37;
            this.txtHead.Text = "331";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 36;
            this.label4.Text = "Head";
            // 
            // txtIPAddress
            // 
            this.txtIPAddress.Location = new System.Drawing.Point(107, 12);
            this.txtIPAddress.Name = "txtIPAddress";
            this.txtIPAddress.ReadOnly = true;
            this.txtIPAddress.Size = new System.Drawing.Size(175, 20);
            this.txtIPAddress.TabIndex = 33;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "IP Address";
            // 
            // timer1
            // 
            this.timer1.Interval = 60000;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 239);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 56;
            this.label2.Text = "Service Address";
            // 
            // btnSendState
            // 
            this.btnSendState.Location = new System.Drawing.Point(17, 343);
            this.btnSendState.Name = "btnSendState";
            this.btnSendState.Size = new System.Drawing.Size(265, 23);
            this.btnSendState.TabIndex = 58;
            this.btnSendState.Text = "Send State";
            this.btnSendState.UseVisualStyleBackColor = true;
            this.btnSendState.Click += new System.EventHandler(this.btnSendState_Click);
            // 
            // btnParseKML
            // 
            this.btnParseKML.Location = new System.Drawing.Point(291, 345);
            this.btnParseKML.Name = "btnParseKML";
            this.btnParseKML.Size = new System.Drawing.Size(75, 23);
            this.btnParseKML.TabIndex = 60;
            this.btnParseKML.Text = "Play KML";
            this.btnParseKML.UseVisualStyleBackColor = true;
            this.btnParseKML.Click += new System.EventHandler(this.btnParseKML_Click);
            // 
            // btnStopPlay
            // 
            this.btnStopPlay.Location = new System.Drawing.Point(372, 345);
            this.btnStopPlay.Name = "btnStopPlay";
            this.btnStopPlay.Size = new System.Drawing.Size(75, 23);
            this.btnStopPlay.TabIndex = 61;
            this.btnStopPlay.Text = "Stop Play";
            this.btnStopPlay.UseVisualStyleBackColor = true;
            this.btnStopPlay.Click += new System.EventHandler(this.btnStopPlay_Click);
            // 
            // btnClearStatus
            // 
            this.btnClearStatus.Location = new System.Drawing.Point(454, 345);
            this.btnClearStatus.Name = "btnClearStatus";
            this.btnClearStatus.Size = new System.Drawing.Size(75, 23);
            this.btnClearStatus.TabIndex = 62;
            this.btnClearStatus.Text = "Clear Status";
            this.btnClearStatus.UseVisualStyleBackColor = true;
            this.btnClearStatus.Click += new System.EventHandler(this.btnClearStatus_Click);
            // 
            // btnPlayCSV
            // 
            this.btnPlayCSV.Location = new System.Drawing.Point(535, 345);
            this.btnPlayCSV.Name = "btnPlayCSV";
            this.btnPlayCSV.Size = new System.Drawing.Size(75, 23);
            this.btnPlayCSV.TabIndex = 63;
            this.btnPlayCSV.Text = "Play CSV";
            this.btnPlayCSV.UseVisualStyleBackColor = true;
            this.btnPlayCSV.Click += new System.EventHandler(this.btnPlayCSV_Click);
            // 
            // btnStopCSV
            // 
            this.btnStopCSV.Location = new System.Drawing.Point(616, 345);
            this.btnStopCSV.Name = "btnStopCSV";
            this.btnStopCSV.Size = new System.Drawing.Size(75, 23);
            this.btnStopCSV.TabIndex = 64;
            this.btnStopCSV.Text = "Stop CSV";
            this.btnStopCSV.UseVisualStyleBackColor = true;
            this.btnStopCSV.Click += new System.EventHandler(this.btnStopCSV_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(15, 265);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(24, 13);
            this.label12.TabIndex = 68;
            this.label12.Text = "Ref";
            // 
            // btnCurrentTrucks
            // 
            this.btnCurrentTrucks.Location = new System.Drawing.Point(536, 370);
            this.btnCurrentTrucks.Name = "btnCurrentTrucks";
            this.btnCurrentTrucks.Size = new System.Drawing.Size(155, 23);
            this.btnCurrentTrucks.TabIndex = 70;
            this.btnCurrentTrucks.Text = "Current Trucks";
            this.btnCurrentTrucks.UseVisualStyleBackColor = true;
            this.btnCurrentTrucks.Click += new System.EventHandler(this.btnCurrentTrucks_Click);
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.Location = new System.Drawing.Point(697, 370);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(97, 23);
            this.btnSendMessage.TabIndex = 71;
            this.btnSendMessage.Text = "Send Message";
            this.btnSendMessage.UseVisualStyleBackColor = true;
            this.btnSendMessage.Click += new System.EventHandler(this.btnSendMessage_Click);
            // 
            // btnAlarmCheck
            // 
            this.btnAlarmCheck.Location = new System.Drawing.Point(697, 345);
            this.btnAlarmCheck.Name = "btnAlarmCheck";
            this.btnAlarmCheck.Size = new System.Drawing.Size(75, 23);
            this.btnAlarmCheck.TabIndex = 72;
            this.btnAlarmCheck.Text = "Alarm Chk";
            this.btnAlarmCheck.UseVisualStyleBackColor = true;
            this.btnAlarmCheck.Click += new System.EventHandler(this.btnAlarmCheck_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(155, 401);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 23);
            this.button1.TabIndex = 73;
            this.button1.Text = "FWD GPS Packet";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnGetAlarms
            // 
            this.btnGetAlarms.Location = new System.Drawing.Point(291, 401);
            this.btnGetAlarms.Name = "btnGetAlarms";
            this.btnGetAlarms.Size = new System.Drawing.Size(115, 23);
            this.btnGetAlarms.TabIndex = 74;
            this.btnGetAlarms.Text = "Get Alarms";
            this.btnGetAlarms.UseVisualStyleBackColor = true;
            this.btnGetAlarms.Click += new System.EventHandler(this.btnGetAlarms_Click);
            // 
            // txtCell
            // 
            this.txtCell.Location = new System.Drawing.Point(45, 289);
            this.txtCell.Name = "txtCell";
            this.txtCell.Size = new System.Drawing.Size(236, 20);
            this.txtCell.TabIndex = 75;
            this.txtCell.Text = "77|1.131|0.250|0.621";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(15, 292);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(24, 13);
            this.label13.TabIndex = 76;
            this.label13.Text = "Cell";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(536, 12);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 13);
            this.label14.TabIndex = 78;
            this.label14.Text = "Received";
            // 
            // txtReceived
            // 
            this.txtReceived.Location = new System.Drawing.Point(536, 31);
            this.txtReceived.Multiline = true;
            this.txtReceived.Name = "txtReceived";
            this.txtReceived.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReceived.Size = new System.Drawing.Size(241, 312);
            this.txtReceived.TabIndex = 77;
            // 
            // ddlServiceAddress
            // 
            this.ddlServiceAddress.FormattingEnabled = true;
            this.ddlServiceAddress.Items.AddRange(new object[] {
            "38.124.164.213",
            "127.0.0.1",
            "38.124.164.212",
            "38.124.164.211"});
            this.ddlServiceAddress.Location = new System.Drawing.Point(107, 236);
            this.ddlServiceAddress.Name = "ddlServiceAddress";
            this.ddlServiceAddress.Size = new System.Drawing.Size(174, 21);
            this.ddlServiceAddress.TabIndex = 79;
            // 
            // ddlServiceRef
            // 
            this.ddlServiceRef.FormattingEnabled = true;
            this.ddlServiceRef.Items.AddRange(new object[] {
            "38.124.164.213:9017/TowTruckService.svc",
            "localhost:9017/TowTruckService.svc",
            "38.124.164.212:9017/TowTruckService.svc",
            "38.124.164.211:9017/TowTruckService.svc"});
            this.ddlServiceRef.Location = new System.Drawing.Point(45, 262);
            this.ddlServiceRef.Name = "ddlServiceRef";
            this.ddlServiceRef.Size = new System.Drawing.Size(236, 21);
            this.ddlServiceRef.TabIndex = 80;
            // 
            // btnAssistRequest
            // 
            this.btnAssistRequest.Location = new System.Drawing.Point(291, 370);
            this.btnAssistRequest.Name = "btnAssistRequest";
            this.btnAssistRequest.Size = new System.Drawing.Size(238, 23);
            this.btnAssistRequest.TabIndex = 81;
            this.btnAssistRequest.Text = "Assist Request";
            this.btnAssistRequest.UseVisualStyleBackColor = true;
            this.btnAssistRequest.Click += new System.EventHandler(this.btnAssistRequest_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(15, 126);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(77, 13);
            this.label15.TabIndex = 82;
            this.label15.Text = "Special Places";
            // 
            // cboLocations
            // 
            this.cboLocations.FormattingEnabled = true;
            this.cboLocations.Items.AddRange(new object[] {
            "Use Lat/Lon",
            "In ABQ Yard",
            "In ABQ Drop Site",
            "ABQ On Patrol",
            "ABQ OFF BEAT",
            "Good San Fran B4",
            "Bad San Fran Drop Area",
            "Bad San Fran EX Seg",
            "Bad San Fran Location",
            "Super Segment",
            "Beat 8 Valid",
            "Beat 10 Valid",
            "Beat 10 Invalid"});
            this.cboLocations.Location = new System.Drawing.Point(98, 123);
            this.cboLocations.Name = "cboLocations";
            this.cboLocations.Size = new System.Drawing.Size(183, 21);
            this.cboLocations.TabIndex = 83;
            this.cboLocations.SelectedIndexChanged += new System.EventHandler(this.cboLocations_SelectedIndexChanged);
            // 
            // txtIPHistory
            // 
            this.txtIPHistory.Location = new System.Drawing.Point(74, 315);
            this.txtIPHistory.Name = "txtIPHistory";
            this.txtIPHistory.Size = new System.Drawing.Size(126, 20);
            this.txtIPHistory.TabIndex = 84;
            this.txtIPHistory.Text = "127.0.0.255";
            // 
            // btnSendIPHistory
            // 
            this.btnSendIPHistory.Location = new System.Drawing.Point(206, 313);
            this.btnSendIPHistory.Name = "btnSendIPHistory";
            this.btnSendIPHistory.Size = new System.Drawing.Size(75, 23);
            this.btnSendIPHistory.TabIndex = 85;
            this.btnSendIPHistory.Text = "Send IP";
            this.btnSendIPHistory.UseVisualStyleBackColor = true;
            this.btnSendIPHistory.Click += new System.EventHandler(this.btnSendIPHistory_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 318);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 86;
            this.label3.Text = "IPHistory";
            // 
            // btnInjectWAZE
            // 
            this.btnInjectWAZE.Location = new System.Drawing.Point(413, 401);
            this.btnInjectWAZE.Name = "btnInjectWAZE";
            this.btnInjectWAZE.Size = new System.Drawing.Size(116, 23);
            this.btnInjectWAZE.TabIndex = 87;
            this.btnInjectWAZE.Text = "Inject WAZE";
            this.btnInjectWAZE.UseVisualStyleBackColor = true;
            this.btnInjectWAZE.Click += new System.EventHandler(this.btnInjectWAZE_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 443);
            this.Controls.Add(this.btnInjectWAZE);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSendIPHistory);
            this.Controls.Add(this.txtIPHistory);
            this.Controls.Add(this.cboLocations);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.btnAssistRequest);
            this.Controls.Add(this.ddlServiceRef);
            this.Controls.Add(this.ddlServiceAddress);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtReceived);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtCell);
            this.Controls.Add(this.btnGetAlarms);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnAlarmCheck);
            this.Controls.Add(this.btnSendMessage);
            this.Controls.Add(this.btnCurrentTrucks);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.btnStopCSV);
            this.Controls.Add(this.btnPlayCSV);
            this.Controls.Add(this.btnClearStatus);
            this.Controls.Add(this.btnStopPlay);
            this.Controls.Add(this.btnParseKML);
            this.Controls.Add(this.btnSendState);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSendAbePacket);
            this.Controls.Add(this.txtMLon);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtMLat);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtMaxSpeed);
            this.Controls.Add(this.lblDriverID);
            this.Controls.Add(this.btnSendPacket);
            this.Controls.Add(this.txtLon);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtLat);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.txtSpeed);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtHead);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtIPAddress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtMessageStatus);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "OCTA Test Harness";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMessageStatus;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnSendAbePacket;
        private System.Windows.Forms.TextBox txtMLon;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMLat;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtMaxSpeed;
        private System.Windows.Forms.Label lblDriverID;
        private System.Windows.Forms.Button btnSendPacket;
        private System.Windows.Forms.TextBox txtLon;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtLat;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.TextBox txtSpeed;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtHead;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtIPAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSendState;
        private System.Windows.Forms.Button btnParseKML;
        private System.Windows.Forms.Button btnStopPlay;
        private System.Windows.Forms.Button btnClearStatus;
        private System.Windows.Forms.Button btnPlayCSV;
        private System.Windows.Forms.Button btnStopCSV;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnCurrentTrucks;
        private System.Windows.Forms.Button btnSendMessage;
        private System.Windows.Forms.Button btnAlarmCheck;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnGetAlarms;
        private System.Windows.Forms.TextBox txtCell;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtReceived;
        private System.Windows.Forms.ComboBox ddlServiceAddress;
        private System.Windows.Forms.ComboBox ddlServiceRef;
        private System.Windows.Forms.Button btnAssistRequest;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cboLocations;
        private System.Windows.Forms.TextBox txtIPHistory;
        private System.Windows.Forms.Button btnSendIPHistory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnInjectWAZE;
    }
}

