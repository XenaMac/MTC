using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SqlServer.Types;

namespace FPSService.MiscData
{
    public class DropZone
    {
        public Guid DropZoneID { get; set; }
        public string Location { get; set; }
        public string Comments { get; set; }
        public string Restrictions { get; set; }
        public string DropZoneNumber { get; set; }
        public string DropZoneDescription { get; set; }
        public string City { get; set; }
        public string PDPhoneNumber { get; set; }
        public int Capacity { get; set; }
        public SqlGeography Position { get; set; }
    }
}