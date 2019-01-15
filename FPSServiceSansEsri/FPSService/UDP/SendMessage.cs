using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FPSService.UDP
{
    public class SendMessage
    {
        Logging.EventLogger logger = new Logging.EventLogger();

        public void SendMyMessage(string Message, string IPAddress = "127.0.0.1")
        {
            try
            {
                UdpClient udpClient = new UdpClient(IPAddress, 9018);
                Byte[] sendBytes = Encoding.ASCII.GetBytes(Message);
                udpClient.Send(sendBytes, sendBytes.Length);
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + ex.ToString(), true);
            }
        }

        public void ForwardMessage(string Message, string IPAddress)
        {
            if (string.IsNullOrEmpty(IPAddress) || string.IsNullOrEmpty(Message))
            {
                return;
            }
            try
            {
                UdpClient udpClient = new UdpClient(IPAddress, 9018);
                Byte[] sendBytes = Encoding.ASCII.GetBytes(Message);
                udpClient.Send(sendBytes, sendBytes.Length);
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + ex.ToString(), true);
            }
        }
    }
}