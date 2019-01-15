using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace OctaHarness
{
    class SendUDP
    {
        //67.0.129.211
        //174.28.185.218
        public string SendUDPPacket(string Message, string URL)
        {
            if (string.IsNullOrEmpty(URL))
            {
                URL = "127.0.0.1";
            }
            IPAddress[] addresslist = Dns.GetHostAddresses(URL);
            IPAddress address;
            foreach (IPAddress thisAddress in addresslist)
            {
                address = thisAddress;
            }
            UdpClient udpClient = new UdpClient(URL, 9017);
            Byte[] sendBytes = Encoding.ASCII.GetBytes(Message);
            udpClient.Send(sendBytes, sendBytes.Length);
            return DateTime.Now.ToShortTimeString() + " Sent" + Environment.NewLine + 
                Message + Environment.NewLine;
        }
    }
}
