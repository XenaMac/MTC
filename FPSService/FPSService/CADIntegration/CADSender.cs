using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Timers;
using System.Configuration;

namespace FPSService.CADIntegration
{
    public class CADSender
    {
        private int port = 20047;
        private string cadIp = string.Empty;
        private const Byte SOH = 0x01;
        private const Byte ETX = 0x03;
        private const Byte ENQ = 0x05;
        private const Byte ACK = 0x06;
        //private int fspSeq = 1;
        private TcpClient client;
        private NetworkStream stream;
        private System.Timers.Timer tmrHB;
        private System.Timers.Timer tmrConnectCheck;
        private int CadWriterAckTimeout = 10000;
        public EventHandler cadSendConnected;
        public EventHandler cadSendDisconnected;
        public string lastHBSent = "01/01/2001 00:00:00";
        

        public CADSender()
        {
            cadIp = ConfigurationManager.AppSettings["CADIP"].ToString();
            tmrConnectCheck = new System.Timers.Timer(10000);
            tmrConnectCheck.Enabled = true;
            tmrConnectCheck.Elapsed += tmrConnectCheck_Elapsed;
            Connect();
        }

        void tmrConnectCheck_Elapsed(object sender, ElapsedEventArgs e)
        {
            bool connected = ConnectStatus();
            if (connected == true)
            {
                //all good
            }
            else
            {
                //sendDisconnected(EventArgs.Empty);
                Disconnect();
                Connect();
            }
        }

        #region " Event Handlers "

        protected virtual void sendDisconnected(EventArgs e)
        {
            EventHandler handler = cadSendDisconnected;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void sendConnected(EventArgs e)
        {
            EventHandler handler = cadSendDisconnected;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region " Connect, Disconnect, and Check Status "

        public void Connect()
        {
            try
            {
                client = new TcpClient(cadIp, port);
                stream = client.GetStream();
                stream.ReadTimeout = CadWriterAckTimeout;

                if (tmrHB == null)
                {
                    tmrHB = new System.Timers.Timer(5000);
                }

                sendConnected(EventArgs.Empty);

                tmrHB.Elapsed += tmrHB_Elapsed;
                tmrHB.Start();

            }
            catch (Exception ex)
            {
                //throw new Exception("CAD NOT CONNECTED: " + ex.Message);
                string err = ex.ToString();
            }
        }

        public void Disconnect()
        {
            try
            {
                stream.Close();
                client.Close();
                stream = null;
                client = null;
                sendDisconnected(EventArgs.Empty);
                tmrHB.Stop();
                tmrHB = null;
            }
            catch (Exception ex)
            {
                //throw new Exception("CAD FAILED TO DISCONNECT: " + ex.Message);
            }
        }

        public bool ConnectStatus()
        {
            bool connectStatus = false;
            if (client != null && client.Connected == true)
            {
                connectStatus = true;
            }
            return connectStatus;
        }

        #endregion

        #region  " Timer Elapsed "

        void tmrHB_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (ConnectStatus())
            {
                sendHeartBeat();
            }
        }

        #endregion

        #region " Message Senders "



        public void sendMessage(string command)
        {
            if(ConnectStatus())
            { 
                if (command.ToUpper() == "ACK")
                {
                    sendAck();
                    //return "ACKED";
                }
                else
                {
                    SendCadCommand(stream, CADHelpers.GetSeq(), command);
                    //return ret;
                }
            }
        }

        private void sendHeartBeat()
        {
            try
            {
                //Byte[] bytes = new Byte[1];
                //bytes[0] = ENQ;
                //stream.Write(bytes, 0, bytes.Length);
                SendCadCommand(stream, CADHelpers.GetSeq(), "ENQ");
                lastHBSent = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        private void sendAck()
        {
            try
            {
                //Byte[] bytes = new Byte[1];
                //bytes[0] = ACK;
                //stream.Write(bytes, 0, bytes.Length);
                SendCadCommand(stream, CADHelpers.GetSeq(), "ACK");
            }
            catch (Exception ex)
            {
                //throw new Exception("ERROR Acking CAD: " + ex.Message);
                string err = ex.ToString();
            }
        }

        private void SendCadCommand(NetworkStream stream, int seqNum, string cadCommand)
        {
            try
            {
                if (cadCommand != "ENQ" && cadCommand != "ACK")
                {
                    string message = String.Format(@"{0:D5}.{1:MMddyy.HHmmss}.000-FSP\{2}", seqNum, DateTime.Now, cadCommand);
                    Byte[] bytes = new Byte[message.Length + 2];
                    bytes[0] = SOH;
                    Array.Copy((new ASCIIEncoding()).GetBytes(message), 0, bytes, 1, message.Length);
                    bytes[message.Length + 1] = ETX;
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                    DataClasses.GlobalData.cadSent.Add(DateTime.Now.ToString() + " " + message);
                    //Log data
                    SQL.SQLCode mySQL = new SQL.SQLCode();
                    mySQL.logCADMessage("SENT", message);
                    mySQL = null;
                }
                else
                {
                    Byte[] bytes = new Byte[1];
                    if (cadCommand == "ACK")
                    {
                        bytes[0] = ACK;
                    }
                    if (cadCommand == "ENQ")
                    {
                        bytes[0] = ENQ;
                    }
                    stream.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("ERROR: " + ex.Message);
                string err = ex.ToString();
            }
        }

        #endregion
    }
}