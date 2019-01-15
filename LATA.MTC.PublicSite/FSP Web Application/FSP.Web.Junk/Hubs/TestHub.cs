using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Hubs;

namespace FSP.Web.Junk.Hubs
{
    [HubName("testHub")]
    public class TestHub : Hub
    {
        public static List<string> messages = new List<string>();

        public void GetServiceState()
        {
            Clients.updateMessages(messages);
        }

        public void UpdateServiceState()
        {
            messages.Add(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

            Clients.updateMessages(messages);
        }
    }
}