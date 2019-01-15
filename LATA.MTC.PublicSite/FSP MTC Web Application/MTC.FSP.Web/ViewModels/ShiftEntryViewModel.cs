using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTC.FSP.Web.ViewModels
{
    public class ShiftEntryViewModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public String Driver { get; set; }
        public String Beat { get; set; }
        public String CallSign { get; set; }
        public String Truck { get; set; }

        public DateTime OnPatrol { get; set; }
        public DateTime RollIn { get; set; }
        public int LogOnOd { get; set; }
        public int LogOffOd { get; set; }

        public String Notes { get; set; }
        public String CHPTOTLogNum { get; set; }
        public String DropSiteNum { get; set; }
        public String IncidentSurvNum { get; set; }

        public Guid? FleetVehicleId { get; set; }
        public String TruckNumber { get; set; }

        public String Assists { get; set; }

        public class Assist
        {
            public String IC { get; set; }
            public String TC { get; set; }
            public String BD { get; set; }
            public String DO { get; set; }
            public String O { get; set; }
            public String AP { get; set; }
            public String DIR { get; set; }
            public String Highway { get; set; }
            public String Area { get; set; }
            public String POS { get; set; }
            public String PTN { get; set; }
            public String TransCode { get; set; }
            public String VehType { get; set; }
            public String LicPlateState { get; set; }
            public String LicPlateNum { get; set; }
            public String CHPIncLogNum { get; set; }
            public String DetailNote { get; set; }
            public String TimeOnInc { get; set; }
            public String TimeOffInc { get; set; }
        }
    }
}