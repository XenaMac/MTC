using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class TowTruckSetup
    {
        public string MountedSecurely { get; set; }
        public string ConnectedToCell { get; set; }
        public string DCPowerConnected { get; set; }
        public string RouterUnitMountedSecurely { get; set; }
        public string MoistureFree { get; set; }
        public string Speedtest { get; set; }
        public string GPSSentProperly { get; set; }
        public string InstallerName { get; set; }
        public System.DateTime InstallationDate { get; set; }
        public string TowTruckCompany { get; set; }
        public string VehicleID { get; set; }
        public string SystemSerialNumber { get; set; }
        public string IPAddress { get; set; }
        public string UploadSpeed { get; set; }
        public string DownloadSpeed { get; set; }
    }
}
