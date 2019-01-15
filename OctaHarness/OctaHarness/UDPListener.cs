using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace OctaHarness
{
    public class UDPListener
    {
        public delegate void MessageReceived();
        public event MessageReceived gotMessage;
        private byte[] data = new byte[4096];
        public string lastMessage;
        private Thread listenThread;

        public UDPListener()
        {
            listenThread = new Thread(new ThreadStart(UDPListenThread));
            listenThread.Start();
        }

        public void UDPListenThread()
        {
            Socket udpListener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ourEndPoint = new IPEndPoint(IPAddress.Any, 9018);
            IPEndPoint end = new IPEndPoint(IPAddress.Any, 9018);
            EndPoint Identifier = (EndPoint)end;

            udpListener.Bind(ourEndPoint);

            while (true)
            {
                string msg = null;
                try
                {
                    int length = udpListener.ReceiveFrom(data, ref Identifier);
                    string IP = ((IPEndPoint)Identifier).Address.ToString();
                    msg = System.Text.Encoding.UTF8.GetString(data, 0, length);
                    lastMessage = msg;
                    gotMessage();
                }
                catch (Exception ex)
                {
                    string err = ex.ToString();
                }
            }
        }
    }
}
