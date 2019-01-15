using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Web.Script.Serialization;
using System.Threading;

namespace OctaHarness
{
    public class ReceiveUDP
    {
        private byte[] data = new byte[4096];
        private Thread listenThread;
        public ReceiveUDP()
        {
            listenThread = new Thread(new ThreadStart(UDPListenThread));
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        public void stopThread()
        {
            listenThread.Abort();
            listenThread = null;
        }

        private void UDPListenThread()
        {
            Socket udpListener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ourEndPoint = new IPEndPoint(IPAddress.Any, 9009);
            EndPoint Identifier = (EndPoint)ourEndPoint;
            udpListener.Bind(ourEndPoint);
            while (true)
            {
                int length = udpListener.ReceiveFrom(data, ref Identifier);
                string msg = System.Text.Encoding.ASCII.GetString(data, 0, length);
                Program.myForm.setText(msg);

                //JavaScriptSerializer js = new JavaScriptSerializer();
                //Ack myAck = js.Deserialize<Ack>(msg);
            }
        }
    }
}
