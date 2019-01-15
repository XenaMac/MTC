using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FSP.Web.Services;
using SignalR.Hubs;

namespace FSP.Web.Hubs
{
    [HubName("towTruckHub")]
    public class TowTruckHub : Hub
    {
        public static TruckCollectionService truckCollectionService = new TruckCollectionService();

        public void Initialize()
        {
            truckCollectionService.GetCurrentTowTrucks();
        }   
    }
}