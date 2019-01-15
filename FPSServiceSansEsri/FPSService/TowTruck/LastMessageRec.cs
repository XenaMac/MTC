using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.TowTruck
{
    public class LastMessageRec
    {
        public LastMessageRec()
        {
            LastMessageReceived = DateTime.Now;
        }
        public DateTime LastMessageReceived { get; set; }

    }
}