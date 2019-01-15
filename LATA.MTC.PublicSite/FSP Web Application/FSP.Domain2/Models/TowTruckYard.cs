using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class TowTruckYard
    {
        public System.Guid TowTruckYardID { get; set; }
        public string Location { get; set; }
        public string Comments { get; set; }
        public string TowTruckYardNumber { get; set; }
        public string TowTruckYardDescription { get; set; }
        public string TowTruckCompanyName { get; set; }
        public string TowTruckCompanyPhoneNumber { get; set; }
        public System.Data.Entity.Spatial.DbGeography Position { get; set; }
    }
}
