using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.TowTruck
{
    public class TowTruckDriver
    {
        public Guid DriverID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string TowTruckCompany { get; set; }
        public string FSPID { get; set; }
        public Guid AssignedBeat { get; set; }
        public DateTime BreakStarted { get; set; }
        public DateTime LunchStarted { get; set; }
        public string callSign { get; set; }
        public double startODO { get; set; }
        public string AssignedShift { get; set; }
        public MiscData.BeatSchedule schedule { get; set; }
    }
}