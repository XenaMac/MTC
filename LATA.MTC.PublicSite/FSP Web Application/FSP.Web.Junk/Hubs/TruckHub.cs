using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FSP.Web.Junk.Services;
using SignalR.Hubs;

namespace FSP.Web.Junk.Hubs
{
    [HubName("truckHub")]
    public class TruckHub : Hub
    {       
        public static TruckService service = new TruckService();

        public void GetServiceState()
        {
            service.GetServiceState();
        }        
    }
}