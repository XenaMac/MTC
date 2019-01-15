using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class DropZone
    {
        public System.Guid DropZoneID { get; set; }
        public string Location { get; set; }
        public string Comments { get; set; }
        public string Restrictions { get; set; }
        public string DropZoneNumber { get; set; }
        public string DropZoneDescription { get; set; }
        public string City { get; set; }
        public string PDPhoneNumber { get; set; }
        public int Capacity { get; set; }
        public System.Data.Entity.Spatial.DbGeography Position { get; set; }
    }
}
