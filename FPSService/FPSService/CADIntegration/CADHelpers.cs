using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.CADIntegration
{
    public static class CADHelpers
    {
        public static int GetSeq()
        {
            int seq = DataClasses.GlobalData.CadSeqNum;
            DataClasses.GlobalData.CadSeqNum += 1;
            return seq;
        }

        public static string makeAM(string msg)
        {
            string message = "AM";
            string[] splitter = msg.Split('.');
            string seqNum = splitter[0].ToString();
            message += "." + seqNum;
            return message;
        }

        public static string makeRM(string msg)
        {
            string message = "RM";
            string[] splitter = msg.Split('.');
            string seqNum = splitter[0].ToString();
            message += "." + seqNum;
            return message;
        }

        public static bool truckAvailable(string data)
        {
            bool available = false;
            string[] splitter = data.Split('\\');
            string[] splitData = splitter[1].Split('.');
            string _callSign = splitData[1];
            string shift = string.Empty;
            if (_callSign.Substring(0, 1) == "A" || _callSign.Substring(0, 1) == "*" || _callSign.Substring(0, 1) == "B")
            {
                switch (_callSign.Substring(0, 1))
                {
                    case "A":
                        shift = "AM";
                        break;
                    case "B":
                        shift = "PM";
                        break;
                    case "*":
                        shift = "MID";
                        break;
                }
                _callSign = _callSign.Substring(1, _callSign.Length - 1);

            }
            /*
            if (_callSign.Substring(_callSign.Length - 1, 1) == "A" || _callSign.Substring(_callSign.Length - 1, 1) == "B")
            {
                _callSign = _callSign.Substring(0, _callSign.Length - 1);
            }
            */
            TowTruck.TowTruck truck = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck find)
            {
                return find.Driver.callSign == _callSign && find.Driver.AssignedShift.ToUpper().Contains(shift);
            });
            if (truck != null)
            {
                if (truck.Status.VehicleStatus.ToUpper() == "ENROUTE" || truck.Status.VehicleStatus.ToUpper() == "ON INCIDENT" || truck.Status.VehicleStatus.ToUpper() == "EN ROUTE")
                {
                    available = false;
                }
                else
                {
                    available = true;
                }
            }
            return available;
        }
    }
}