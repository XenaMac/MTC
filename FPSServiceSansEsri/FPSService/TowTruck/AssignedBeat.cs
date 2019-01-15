using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SqlServer.Types;

namespace FPSService.TowTruck
{
    public class AssignedBeat
    {
        public Guid BeatID { get; set; }
        public SqlGeography BeatExtent { get; set; }
        public bool Loaded { get; set; }
        public string BeatNumber { get; set; }
    }
}