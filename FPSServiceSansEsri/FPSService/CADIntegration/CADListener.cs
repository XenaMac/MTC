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

namespace FPSService.CADIntegration
{
    public class CADListener
    {
        TcpListener tcpListener;
        Thread listenerThread;
        Socket socket;
        int port = 20046;
        private readonly ManualResetEvent exitThreadEvent = new ManualResetEvent(true);
        public EventHandler msgReceived;
        public EventHandler hbReceived;
        public string msgData;
        bool ConnectStatus = false;
        public string lastHB = "01/01/2001 00:00:00";
        private System.Timers.Timer tmrConnectCheck;
        public EventHandler cadRecConnected;
        public EventHandler cadRecDisconnected;
        private bool bRunServer = true;
        NetworkStream stream;
        private const Byte SOH = 0x01;
        private const Byte ETX = 0x03;
        private const Byte ENQ = 0x05;
        private const Byte ACK = 0x06;
        Guid cadMsgID = new Guid();

        #region  " Event Hookups "

        protected virtual void gotMessage(EventArgs e)
        {
            EventHandler handler = msgReceived;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void gotHB(EventArgs e)
        {
            EventHandler handler = hbReceived;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void recDisconnected(EventArgs e)
        {
            EventHandler handler = cadRecDisconnected;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void recConnected(EventArgs e)
        {
            EventHandler handler = cadRecDisconnected;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        public CADListener()
        {
            listenerThread = new Thread(RunListener);
            listenerThread.Start();
            tmrConnectCheck = new System.Timers.Timer(30000);
            tmrConnectCheck.Enabled = true;
            tmrConnectCheck.Elapsed += tmrConnectCheck_Elapsed;
        }

        void tmrConnectCheck_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            bool connected = isConnected();
            if (connected == true)
            {
                //all good
            }
            else
            {
                //recDisconnected(EventArgs.Empty);
                try
                {
                    ResetListener();
                }
                catch (Exception ex)
                {
                    //don't fret
                }
            }
        }

        #region  " Connect, Disconnect, Check Status "

        public void ResetListener() //reset if something breaks; this needs to be automated soon
        {
            if (listenerThread != null)
            {
                if (tcpListener != null)
                {
                    tcpListener.Stop();
                    tcpListener = null;
                }
                if (socket != null)
                {
                    socket.Close();
                    socket = null;
                }
                listenerThread = null;
                listenerThread = new Thread(RunListener);
                listenerThread.Start();
            }
        }

        public void DisconnectListener() //disconnect on application shutdown
        {
            if (listenerThread != null)
            {
                if (tcpListener != null)
                {
                    tcpListener.Stop();
                    tcpListener = null;
                }
                if (socket != null)
                {
                    socket.Close();
                    socket = null;
                }
            }
        }

        public bool isConnected() //return current connection status
        {

            if (stream != null && stream.CanRead)
            {
                ConnectStatus = true;
            }
            else
            {
                return false;
            }

            if (socket != null && socket.IsBound && socket.Connected)
            {
                ConnectStatus = true;
            }
            else
            {
                ConnectStatus = false;
            }
            return ConnectStatus;
        }

        public void RunListener() //this is the actual listener process
        {
            const Byte ETX = 0x03;
            const Byte ENQ = 0x05;
            const Byte ACK = 0x06;

            try
            {
                // buffer for message from FSP to CAD
                Byte[] fsp2cadBytes = new Byte[1024];

                // buffer for message from CAD to FSP
                Byte[] cad2fspBytes = new Byte[256];

                // buffer to read 1 byte from FSP
                Byte[] charBuf = new Byte[1];
                tcpListener = new TcpListener(IPAddress.Any, port);
                tcpListener.Start();
                //Socket socket = tcpListener.AcceptSocket();
                socket = tcpListener.AcceptSocket();

                //while (exitThreadEvent.WaitOne(0))
                while (bRunServer)
                {
                    //using (socket = tcpListener.AcceptSocket())
                    //{
                    if (socket.Connected)
                    {
                        ConnectStatus = true;
                        using (stream = new NetworkStream(socket))
                        {
                            int nRead, msgLength = 0;
                            for (int i = 0; i < fsp2cadBytes.Length; i++)
                            {
                                nRead = stream.Read(charBuf, 0, 1);
                                if (nRead == 0)
                                {
                                    //null from stream, shut down
                                    return;
                                }
                                if (charBuf[0] == ETX)
                                {
                                    //end transmission
                                    break;
                                }
                                if (charBuf[0] == ENQ)
                                {
                                    //Heartbeat message
                                    gotHeartbeat();
                                    break;
                                }
                                fsp2cadBytes[i] = charBuf[0];
                                msgLength++;
                            }
                            if (msgLength != 0)
                            {
                                string cadText = System.Text.Encoding.ASCII.GetString(fsp2cadBytes, 1, msgLength - 1);
                                //cad2fspBytes[0] = ACK;
                                byte[] cadAck = new byte[1];
                                cadAck[0] = ACK;
                                stream.Write(cadAck, 0, 1);
                                //msgData = cadText;
                                //msgData = MessageParser.parseIncoming(cadText);
                                DataClasses.GlobalData.cadReceived.Add(DateTime.Now.ToString() + " " + cadText);
                                SQL.SQLCode mySQL = new SQL.SQLCode();
                                cadMsgID = (Guid)mySQL.logCADMessage("REC", cadText);
                                mySQL = null;
                                //send accept message
                                if(!cadText.Contains("\\AM.") && !cadText.Contains("\\RM."))
                                {
                                    if (cadText.Contains("II"))
                                    {
                                        //see if the truck is available.
                                        bool avail = CADHelpers.truckAvailable(cadText);
                                        if (avail == false)
                                        {
                                            string msg = CADHelpers.makeRM(cadText);
                                            //Global.cSender.sendMessage(msg);
                                        }
                                        else
                                        {
                                            MessageParser.processMessage(cadText, cadMsgID);
                                            string msg = CADHelpers.makeAM(cadText);
                                            Global.cSender.sendMessage(msg);
                                        }
                                    }
                                    else
                                    {
                                        MessageParser.processMessage(cadText, cadMsgID);
                                        string msg = CADHelpers.makeAM(cadText);
                                        Global.cSender.sendMessage(msg);
                                    }
                                }
                                
                                gotMessage(EventArgs.Empty);
                            }
                            stream.Flush();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
        }

        #endregion

        #region " Heartbeat processor "

        private void gotHeartbeat()
        {
            //msgData = "HB: " + DateTime.Now.ToString();
            //gotMessage(EventArgs.Empty);
            lastHB = DateTime.Now.ToString();
            gotHB(EventArgs.Empty);
        }

        #endregion
    }
}