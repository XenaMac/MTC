using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.TowTruck
{
    public class State
    {
        //NOTE: This class pertains to internal state variables for trucks, not the status of a given vehicle which is in TowTruckStatus
        public int Id { get; set; }
        public string CarID { get; set; } // may need to reformat to truckid
        public int GpsRate { get; set; }
        public string Log {get;set;}
        public string Version { get; set; }
        public string ServerIP { get; set; }
        public string SFTPServerIP { get; set; }
        public string IPList { get; set; }
        public string BillStartDay { get; set; }
        public string LastBillReset { get; set; }
        public string DataUsed { get; set; }
        public string IgnTimeoutSecs { get; set; }
    }

    public class IPHistory
    {
        public int Id { get; set; }
        public string IP { get; set; }
    }
}