using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Web;
using FSP.Web.Junk.Hubs;
using SignalR;

namespace FSP.Web.Junk.Services
{
    public class TruckService
    {
        public static List<string> messages = null;
        public static Timer timer = null;

        public TruckService()
        {
            Debug.WriteLine("Truck Service Constructor :" + DateTime.Now);
            messages = new List<string>();

            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
            
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            messages.Add(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            GetClients().updateMessages(messages);
        }

        public void GetServiceState()
        {
            GetClients().updateMessages(messages);
        }
       
        private static dynamic GetClients()
        {
            return GlobalHost.ConnectionManager.GetHubContext<TruckHub>().Clients;
        }
    }
}