using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Web.Script.Serialization;
using Microsoft.SqlServer.Types;
using System.Xml.Serialization;
using System.IO;
using System.Threading;
using System.Timers;

namespace OctaHarness
{
    public partial class Form1 : Form
    {
        string IPAddr = "";
        ReceiveUDP myUDP = new ReceiveUDP();
        Thread runKMLThread;
        Thread runCSVThread;
        Thread runMTCThread;
        private bool alarmCheckStarted = false;
        private System.Windows.Forms.Timer tmrAlarms;
        UDPListener udp;
        
        public Form1()
        {
            InitializeComponent();
            txtTime.Text = DateTime.Now.ToString();
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    IPAddr = ip.ToString();
                }
            }
            txtIPAddress.Text = IPAddr;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            runKMLThread = new Thread(new ThreadStart(RunKMLThread));
            runKMLThread.IsBackground = true;
            runCSVThread = new Thread(new ThreadStart(RunCSVThread));
            runCSVThread.IsBackground = true;
            udp = new UDPListener();
            udp.gotMessage += new UDPListener.MessageReceived(udp_gotMessage);
            cboLocations.SelectedIndex = 0;
            cboLocations.SelectedIndexChanged += new EventHandler(cboLocations_SelectedIndexChanged);
        }

        void cboLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboLocations.Text)
            {
                case "Use Lat/Lon":
                    txtLat.Text = "35.100391";
                    txtLon.Text = "-106.571552";
                    break;
                case "In ABQ Yard":
                    txtLat.Text = "35.102863";
                    txtLon.Text = "-106.577075";
                    break;
                case "In ABQ Drop Site":
                    txtLat.Text = "35.105215";
                    txtLon.Text = "-106.549558";
                    break;
                case "ABQ On Patrol":
                    txtLat.Text = "35.096721";
                    txtLon.Text = "-106.566481";
                    break;
                case "ABQ OFF BEAT":
                    txtLat.Text = "35.095073";
                    txtLon.Text = "-106.572100";
                    break;
                case "Good San Fran B4":
                    txtLat.Text = "37.687310";
                    txtLon.Text = "-122.132880";
                    break;
                case "Bad San Fran Drop Area":
                    txtLat.Text = "37.690754";
                    txtLon.Text = "-122.130118";
                    break;
                case "Bad San Fran EX Seg":
                    txtLat.Text = "37.690364";
                    txtLon.Text = "-122.098323";
                    break;
                case "Bad San Fran Location":
                    txtLat.Text = "37.686737";
                    txtLon.Text = "-122.127455";
                    break;
                case "Super Segment":
                    txtLat.Text = "37.690364";
                    txtLon.Text = "-122.098323";
                    break;
                case "Beat 8 Valid":
                    txtLat.Text = "37.323273";
                    txtLon.Text = "-121.895397";
                    break;
                case "Beat 10 Valid":
                    txtLat.Text = "37.406162";
                    txtLon.Text = "-122.06636";
                    break;
                case "Beat 10 Invalid":
                    txtLat.Text = "37.406147";
                    txtLon.Text = "-122.0603";
                    break;
                default:
                    txtLat.Text = "35.100391";
                    txtLon.Text = "-106.571552";
                    break;
            }
        }

        void udp_gotMessage()
        {
            gotText(udp.lastMessage);
            if (udp.lastMessage.Contains("<Name>IPList</Name>"))
            {
                sendState("66.228.48.38|67.192.39.83,84|74.125.194.105;227.193,201;30.103,106,147,99");
            }
        }

        delegate void SetTextCallback(string text);

        private void gotText(string text)
        {
            if (txtReceived.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(gotText);
                Invoke(d, new object[] { text });
            }
            else
            {
                txtReceived.Text += DateTime.Now.ToString("HH:mm:ss") + Environment.NewLine + text + Environment.NewLine + Environment.NewLine;
            }
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            myUDP.stopThread();
            myUDP = null;
        }

        public void setText(string Msg)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(setText), new object[] { Msg });
                return;
            }
            txtMessageStatus.Text += Environment.NewLine + Msg + Environment.NewLine;

        }

        private void btnSendPacket_Click_1(object sender, EventArgs e)
        {
            txtTime.Text = DateTime.Now.ToString();
            GPS thisGPS = new GPS();
            string StartDate = "01/01/1970 00:00:00";
            TimeSpan nowSpan = DateTime.Now - Convert.ToDateTime(StartDate);
            thisGPS.Id = Math.Abs(nowSpan.Milliseconds);
            thisGPS.Speed = Convert.ToDouble(txtSpeed.Text);
            thisGPS.Lat = Convert.ToDouble(txtLat.Text);
            thisGPS.Lon = Convert.ToDouble(txtLon.Text);
            thisGPS.MaxSpd = Convert.ToDouble(txtMaxSpeed.Text);
            thisGPS.MLat = Convert.ToDouble(txtMLat.Text);
            thisGPS.MLon = Convert.ToDouble(txtMLon.Text);
            thisGPS.Time = Convert.ToDateTime(txtTime.Text);
            thisGPS.Cell = txtCell.Text;
            thisGPS.Status = "Valid";
            thisGPS.DOP = 7.0;
            thisGPS.Alt = 5280;
            thisGPS.Head = Convert.ToInt32(txtHead.Text);
            XmlSerializer ser = new XmlSerializer(typeof(GPS));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            StringWriter str = new StringWriter();
            ser.Serialize(str, thisGPS, namespaces);
            string msg = str.ToString();
            SendUDP myUDP = new SendUDP();
            string ipSend = string.Empty;
            Invoke(new MethodInvoker(delegate() { ipSend = ddlServiceAddress.Text; }));
            txtMessageStatus.Text += myUDP.SendUDPPacket(msg,ipSend);
        }

        private void btnSendAbePacket_Click(object sender, EventArgs e)
        {
            string msg = "<GPS><Id>67546</Id><MLon>-118.109129734817</MLon><Speed>0</Speed><Head>0</Head><Lon>-118.109129734817</Lon><Alt>1307</Alt><Time>09/06/2012 18:45:46</Time><MLat>33.7978439719052</MLat><Status>Valid</Status><MaxSpd>0</MaxSpd><Lat>33.7978439719052</Lat><DOP>6</DOP></GPS>";
            SendUDP myUDP = new SendUDP();
            txtMessageStatus.Text += myUDP.SendUDPPacket(msg, ddlServiceAddress.Text);
        }

        /* DEPRECATED
        private void timer1_Tick(object sender, EventArgs e)
        {
            string msg = "<GPS><Id>67546</Id><MLon>-118.109129734817</MLon><Speed>0</Speed><Head>0</Head><Lon>-118.109129734817</Lon><Alt>1307</Alt><Time>09/06/2012 18:45:46</Time><MLat>33.7978439719052</MLat><Status>Valid</Status><MaxSpd>0</MaxSpd><Lat>33.7978439719052</Lat><DOP>6</DOP></GPS>";
            SendUDP myUDP = new SendUDP();
            txtMessageStatus.Text += myUDP.SendUDPPacket(msg,ddlServiceAddress.Text);
        }

        private void btnStartTimer_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void btnStopTimer_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
        */
        private void btnSendState_Click(object sender, EventArgs e)
        {
            sendState(string.Empty);
        }

        public void sendState(string IPList)
        {
            State thisState = new State();
            string StartDate = "01/01/1970 00:00:00";
            TimeSpan nowSpan = DateTime.Now - Convert.ToDateTime(StartDate);
            thisState.Id = Math.Abs(nowSpan.Milliseconds);
            thisState.CarID = "NA";
            thisState.GpsRate = 30;
            thisState.Log = "F";
            thisState.ServerIP = "127.10.0.1";
            thisState.Version = "1.0.0";
            thisState.SFTPServerIP = "127.10.0.1";
            thisState.BillStartDay = "15";
            thisState.LastBillReset = "2015/05/15 13:04:12";
            thisState.DataUsed = "1.1234567890";
            thisState.IgnTimeoutSecs = "2400";
            if (!string.IsNullOrEmpty(IPList))
            {
                thisState.IPList = IPList;
            }
            XmlSerializer ser = new XmlSerializer(typeof(State));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            StringWriter str = new StringWriter();
            ser.Serialize(str, thisState, namespaces);
            //ser.Serialize(str, thisState);
            string msg = str.ToString();
            SendUDP myUDP = new SendUDP();
            string ipSend = string.Empty;
            Invoke(new MethodInvoker(delegate() { ipSend = ddlServiceAddress.Text; }));
            txtMessageStatus.Text += myUDP.SendUDPPacket(msg, ipSend);
        }

        private void btnSendAssistRequest_Click(object sender, EventArgs e)
        {
            AssistRequest myAssist = new AssistRequest();
            myAssist.Show();
        }

        #region " KML Playback "

        private void btnParseKML_Click(object sender, EventArgs e)
        {
            ParseKML myParser = new ParseKML();
            myParser.OpenKMLFile();
            if (runKMLThread.ThreadState != ThreadState.Running && runKMLThread.ThreadState != ThreadState.Suspended
                && runKMLThread.ThreadState != ThreadState.Stopped && runKMLThread.ThreadState != ThreadState.Aborted)
            {
                runKMLThread.Start();
            }
            else if (runKMLThread.ThreadState == ThreadState.Stopped || runKMLThread.ThreadState == ThreadState.Aborted)
            {
                runKMLThread = null;
                runKMLThread = new Thread(new ThreadStart(RunKMLThread));
                runKMLThread.IsBackground = true;
                runKMLThread.Start();
            }
            else
            {
                runKMLThread.Abort();
                runKMLThread = null;
                runKMLThread = new Thread(new ThreadStart(RunKMLThread));
                runKMLThread.Start();
            }

        }

        public void RunKMLThread()
        {
            for (int i = 0; i < RunKML.Coords.Count(); i++)
            {

                GPS thisGPS = new GPS();
                string StartDate = "01/01/1970 00:00:00";
                TimeSpan nowSpan = DateTime.Now - Convert.ToDateTime(StartDate);
                thisGPS.Id = Math.Abs(nowSpan.Milliseconds);
                thisGPS.Speed = Convert.ToDouble(txtSpeed.Text);
                thisGPS.Lat = RunKML.Coords[i].lat;
                thisGPS.Lon = RunKML.Coords[i].lon;
                thisGPS.MaxSpd = Convert.ToDouble(txtMaxSpeed.Text);
                thisGPS.MLat = Convert.ToDouble(txtMLat.Text);
                thisGPS.MLon = Convert.ToDouble(txtMLon.Text);
                thisGPS.Time = Convert.ToDateTime(txtTime.Text);
                thisGPS.Status = "Valid";
                thisGPS.DOP = 7.0;
                thisGPS.Alt = 5280;
                thisGPS.Cell = txtCell.Text;
                thisGPS.Head = Convert.ToInt32(txtHead.Text);
                XmlSerializer ser = new XmlSerializer(typeof(GPS));
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);
                StringWriter str = new StringWriter();
                ser.Serialize(str, thisGPS, namespaces);
                string msg = str.ToString();
                SendUDP myUDP = new SendUDP();
                SendUDP playbackUdp = new SendUDP();
                txtMessageStatus.Invoke(new MethodInvoker(delegate {
                    txtMessageStatus.Text += playbackUdp.SendUDPPacket(msg, ddlServiceAddress.Text);
                }));

                if (i == RunKML.Coords.Count() - 1)
                {
                    txtMessageStatus.Invoke(new MethodInvoker(delegate {
                        txtMessageStatus.Text = "Playback finished " + DateTime.Now.ToString();
                    }));
                    runKMLThread.Abort();
                    break;
                }

                double distBetween = 0.0;  //should be miles
                if ((i + 1) < RunKML.Coords.Count())
                {
                    distBetween = GeoCodeCalc.CalcDistance(RunKML.Coords[i].lat, RunKML.Coords[i].lon, RunKML.Coords[i + 1].lat, RunKML.Coords[i + 1].lon);
                }
                //int Pause = Convert.ToInt32(thisGPS.Speed * distBetween);
                int Pause = 5;
                System.Threading.Thread.Sleep(Pause * 1000);
            }

        }

        private void btnStopPlay_Click(object sender, EventArgs e)
        {
            runKMLThread.Abort();
            MessageBox.Show("Playback aborted");
        }

        #endregion

        private void btnClearStatus_Click(object sender, EventArgs e)
        {
            txtMessageStatus.Text = "";
            txtReceived.Text = "";
        }

        #region  " CSV Playback "

        private void btnPlayCSV_Click(object sender, EventArgs e)
        {
            ParseCSV myParser = new ParseCSV();
            myParser.ProcessData();
            if (runCSVThread.ThreadState != ThreadState.Running && runCSVThread.ThreadState != ThreadState.Suspended
                && runCSVThread.ThreadState != ThreadState.Stopped && runCSVThread.ThreadState != ThreadState.Aborted)
            {
                runCSVThread.Start();
            }
            else if (runCSVThread.ThreadState == ThreadState.Stopped || runCSVThread.ThreadState == ThreadState.Aborted)
            {
                runCSVThread = null;
                runCSVThread = new Thread(new ThreadStart(RunCSVThread));
                runCSVThread.IsBackground = true;
                runCSVThread.Start();
            }
            else
            {
                runCSVThread.Abort();
                runCSVThread = null;
                runCSVThread = new Thread(new ThreadStart(RunCSVThread));
                runCSVThread.Start();
            }
        }

        private void RunCSVThread()
        {
            for (int i = 0; i < RunCSV.playbackTrucks.Count; i++)
            {
                GPS thisGPS = new GPS();
                string StartDate = "01/01/1970 00:00:00";
                TimeSpan nowSpan = DateTime.Now - Convert.ToDateTime(StartDate);
                thisGPS.Id = Math.Abs(nowSpan.Milliseconds);
                thisGPS.Speed = RunCSV.playbackTrucks[i].Speed;
                thisGPS.Lat = RunCSV.playbackTrucks[i].Lat;
                thisGPS.Lon = RunCSV.playbackTrucks[i].Lon;
                thisGPS.MaxSpd = RunCSV.playbackTrucks[i].MaxSpeed;
                thisGPS.MLat = RunCSV.playbackTrucks[i].Lat;
                thisGPS.MLon = RunCSV.playbackTrucks[i].Lon;
                thisGPS.Time = RunCSV.playbackTrucks[i].timeStamp;
                thisGPS.Status = RunCSV.playbackTrucks[i].VehicleStatus;
                thisGPS.Cell = txtCell.Text;
                thisGPS.DOP = 7.0;
                thisGPS.Alt = 5280;
                thisGPS.Head = RunCSV.playbackTrucks[i].Direction;
                XmlSerializer ser = new XmlSerializer(typeof(GPS));
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);
                StringWriter str = new StringWriter();
                ser.Serialize(str, thisGPS, namespaces);
                string msg = str.ToString();
                SendUDP myUDP = new SendUDP();
                SendUDP playbackUdp = new SendUDP();

                txtMessageStatus.Invoke(new MethodInvoker(delegate
                {
                    txtMessageStatus.Text += playbackUdp.SendUDPPacket(msg, ddlServiceAddress.Text);
                }));

                if (i == RunCSV.playbackTrucks.Count() - 1)
                {
                    txtMessageStatus.Invoke(new MethodInvoker(delegate
                    {
                        txtMessageStatus.Text = "Playback finished " + DateTime.Now.ToString() + Environment.NewLine;
                    }));
                    runCSVThread.Abort();
                    break;
                }

                double distBetween = 0.0;  //should be miles
                if ((i + 1) < RunCSV.playbackTrucks.Count())
                {
                    distBetween = GeoCodeCalc.CalcDistance(RunCSV.playbackTrucks[i].Lat, RunCSV.playbackTrucks[i].Lon, RunCSV.playbackTrucks[i + 1].Lat, RunCSV.playbackTrucks[i + 1].Lon);
                }
                //int Pause = Convert.ToInt32(thisGPS.Speed * distBetween);
                int Pause = 5;
                if (thisGPS.Speed >= 0)
                {
                    System.Threading.Thread.Sleep(Pause * 1000);
                }
            }
            txtMessageStatus.Invoke(new MethodInvoker(delegate
                {
                    txtMessageStatus.Text = "Restarting.  Press Stop CSV to stop playback" + Environment.NewLine;
                    runCSVThread.Abort();
                    runCSVThread.Start();
                }));
        }

        private void btnStopCSV_Click(object sender, EventArgs e)
        {
            runCSVThread.Abort();
        }

        #endregion

        private void btnNewIncident_Click(object sender, EventArgs e)
        {
            ServiceRef.ServiceAddress = ddlServiceRef.Text;
            Incidents frmIncidents = new Incidents();
            frmIncidents.Show();
        }

        private void btnNewAssist_Click(object sender, EventArgs e)
        {
            ServiceRef.ServiceAddress = ddlServiceRef.Text;
            Assists frmAssists = new Assists();
            frmAssists.Show();
        }

        private void btnCurrentTrucks_Click(object sender, EventArgs e)
        {
            ServiceRef.ServiceAddress = ddlServiceRef.Text;
            TruckList myTruckList = new TruckList();
            myTruckList.Show();
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            ServiceRef.ServiceAddress = ddlServiceRef.Text;
            SendMessage myMessage = new SendMessage();
            myMessage.Show();
        }

        private void btnAlarmCheck_Click(object sender, EventArgs e)
        {

            if (alarmCheckStarted == false)
            {
                if (tmrAlarms == null)
                {
                    tmrAlarms = new System.Windows.Forms.Timer();
                }
                alarmCheckStarted = true;
                //start the timer
                tmrAlarms.Tick += new EventHandler(tmrAlarms_Tick);
                tmrAlarms.Interval = 30 * 1000; //30 seconds
                tmrAlarms.Start();
                MessageBox.Show("Timer Started, click again to stop");
                RunAlarmCheck();
            }
            else
            {
                alarmCheckStarted = false;
                tmrAlarms.Stop();
                if (tmrAlarms != null)
                {
                    tmrAlarms = null;
                }
                MessageBox.Show("Timer Stopped, click again to restart");
            }
        }

        private void RunAlarmCheck()
        {

            SendTimerGPS();
            SendTimerState();
        }

        private void SendTimerGPS()
        {
            txtTime.Text = DateTime.Now.ToString();
            GPS thisGPS = new GPS();
            string StartDate = "01/01/1970 00:00:00";
            TimeSpan nowSpan = DateTime.Now - Convert.ToDateTime(StartDate);
            thisGPS.Id = Math.Abs(nowSpan.Milliseconds);
            thisGPS.Speed = Convert.ToDouble(txtSpeed.Text);
            thisGPS.Lat = Convert.ToDouble(txtLat.Text);
            thisGPS.Lon = Convert.ToDouble(txtLon.Text);
            thisGPS.MaxSpd = Convert.ToDouble(txtMaxSpeed.Text);
            thisGPS.MLat = Convert.ToDouble(txtMLat.Text);
            thisGPS.MLon = Convert.ToDouble(txtMLon.Text);
            thisGPS.Time = Convert.ToDateTime(txtTime.Text);
            thisGPS.Status = "Valid";
            thisGPS.DOP = 7.0;
            thisGPS.Alt = 5280;
            thisGPS.Head = Convert.ToInt32(txtHead.Text);
            thisGPS.Cell = txtCell.Text;
            XmlSerializer ser = new XmlSerializer(typeof(GPS));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            StringWriter str = new StringWriter();
            ser.Serialize(str, thisGPS, namespaces);
            string msg = str.ToString();
            SendUDP myUDP = new SendUDP();
            txtMessageStatus.Text += myUDP.SendUDPPacket(msg, ddlServiceAddress.Text);
        }

        private void SendTimerState()
        {
            State thisState = new State();
            string StartDate = "01/01/1970 00:00:00";
            TimeSpan nowSpan = DateTime.Now - Convert.ToDateTime(StartDate);
            thisState.Id = Math.Abs(nowSpan.Milliseconds);
            thisState.CarID = "NA";
            thisState.GpsRate = 30;
            thisState.Log = "F";
            thisState.ServerIP = "127.10.0.1";
            thisState.Version = "1.0.0";
            thisState.SFTPServerIP = "127.10.0.1";
            XmlSerializer ser = new XmlSerializer(typeof(State));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            StringWriter str = new StringWriter();
            ser.Serialize(str, thisState, namespaces);
            //ser.Serialize(str, thisState);
            string msg = str.ToString();
            SendUDP myUDP = new SendUDP();
            txtMessageStatus.Text += myUDP.SendUDPPacket(msg, ddlServiceAddress.Text);
        }

        void tmrAlarms_Tick(object sender, EventArgs e)
        {
            RunAlarmCheck();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtTime.Text = DateTime.Now.ToString();
            GPS thisGPS = new GPS();
            string StartDate = "01/01/1970 00:00:00";
            TimeSpan nowSpan = DateTime.Now - Convert.ToDateTime(StartDate);
            thisGPS.Id = Math.Abs(nowSpan.Milliseconds);
            thisGPS.Speed = Convert.ToDouble(txtSpeed.Text);
            thisGPS.Lat = Convert.ToDouble(txtLat.Text);
            thisGPS.Lon = Convert.ToDouble(txtLon.Text);
            thisGPS.MaxSpd = Convert.ToDouble(txtMaxSpeed.Text);
            thisGPS.MLat = Convert.ToDouble(txtMLat.Text);
            thisGPS.MLon = Convert.ToDouble(txtMLon.Text);
            thisGPS.Time = Convert.ToDateTime(txtTime.Text);
            thisGPS.Status = "Valid";
            thisGPS.DOP = 7.0;
            thisGPS.Alt = 5280;
            thisGPS.Head = Convert.ToInt32(txtHead.Text);
            XmlSerializer ser = new XmlSerializer(typeof(GPS));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            StringWriter str = new StringWriter();
            ser.Serialize(str, thisGPS, namespaces);
            string msg = str.ToString();
            SendUDP myUDP = new SendUDP();
            txtMessageStatus.Text += myUDP.SendUDPPacket("<FWD>" + txtIPAddress.Text + "</FWD>" + msg, ddlServiceAddress.Text);
        }

        private void btnGetAlarms_Click(object sender, EventArgs e)
        {
            ServiceRef.ServiceAddress = ddlServiceRef.Text;
            Alarms al = new Alarms();
            al.Show();
        }

        private void btnAssistRequest_Click(object sender, EventArgs e)
        {
            ServiceRef.ServiceAddress = ddlServiceRef.Text;
            MTCAssistRequest mtc = new MTCAssistRequest();
            mtc.Show();
        }

        private void btnSendIPHistory_Click(object sender, EventArgs e)
        {
            string StartDate = "01/01/1970 00:00:00";
            TimeSpan nowSpan = DateTime.Now - Convert.ToDateTime(StartDate);
            string msg = "<IPHistory><Id>" + Math.Abs(nowSpan.Milliseconds).ToString() + "</Id><IP>" + txtIPHistory.Text + "</IP></IPHistory>";
            SendUDP myUDP = new SendUDP();
            myUDP.SendUDPPacket(msg, ddlServiceAddress.Text);
            
        }

        private void btnInjectWAZE_Click(object sender, EventArgs e)
        {
            WAZEMessage wm = new WAZEMessage();
            wm.Show();
        }
    }

    
}
