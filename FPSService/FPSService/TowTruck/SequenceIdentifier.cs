using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.TowTruck
{
    public sealed class SequenceIdentifier
    {
        private static readonly SequenceIdentifier _instance = new SequenceIdentifier();

        private SequenceIdentifier() { }

        public static SequenceIdentifier Instance
        {
            get
            {
                return _instance;
            }
        }

        public Int32 SequenceNumber
        {
            get
            {
                Int32 sn;
                lock (lockObject)
                {
                    sn = sequenceNumber++;
                }
                return sn;
            }
        }

        object lockObject = new object();
        private Int32 sequenceNumber = 0;
    }
}