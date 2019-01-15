using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;

namespace FPSService.DataClasses
{
    public class SurveyControlChecker
    {
        Timer tmrCheck;
        public SurveyControlChecker()
        {
            tmrCheck = new Timer(5 * 60 * 100); //five minutes
            tmrCheck.Elapsed += tmrCheck_Elapsed;
            tmrCheck.Start();
        }

        void tmrCheck_Elapsed(object sender, ElapsedEventArgs e)
        {
            for (int i = DataClasses.GlobalData.surveyTrucks.Count - 1; i >= 0; i--)
            {
                if (DataClasses.GlobalData.surveyTrucks[i].EndTime < DateTime.Now)
                {
                    //send a notification to the truck to shut down the survey, just to be sure
                    TowTruck.TowTruck t = DataClasses.GlobalData.FindTowTruckByVehicleNumber(DataClasses.GlobalData.surveyTrucks[i].TruckNumber);
                    if (t != null)
                    {
                        string IPAddress = t.Identifier;
                        string cmd = "<Survey>F</Survey>";
                        UDP.SendMessage sm = new UDP.SendMessage();
                        sm.SendMyMessage(cmd, IPAddress);
                    }
                    DataClasses.GlobalData.surveyTrucks.RemoveAt(i);
                }
            }
        }
    }
}