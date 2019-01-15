using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR;

namespace FSP.Web._4._5.Hubs
{
    [HubName("junkHub")]
    public class JunkHub : Hub
    {
        public void Initialize()
        {
            Debug.WriteLine("Hub Initialized");
            Clients.writeMessage("initialized...");
        }
    }
}