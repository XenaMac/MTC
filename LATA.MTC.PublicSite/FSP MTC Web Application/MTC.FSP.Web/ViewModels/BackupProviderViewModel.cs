using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.ViewModels
{
    public class BackupProviderViewModel
    {
        public int Id { get; set; }

        public int BackupBeatId { get; set; }

        public Guid BeatId { get; set; }

        public BeatViewModel Beat { get; set; }

        public Guid ContractorId { get; set; }

        public ContractorViewModel Contractor { get; set; }    

        public String Phone { get; set; }

        public Guid FleetVehicleId { get; set; }

        public VehicleViewModel FleetVehicle { get; set; }     

        public DateTime CreatedOn { get; set; }

        public String CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public String ModifiedBy { get; set; }
    }
}