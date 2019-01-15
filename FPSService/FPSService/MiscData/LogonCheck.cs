using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;

namespace FPSService.MiscData
{
    public class LogonCheck
    {
        Timer logonTimer;
        Logging.MessageLogger logger = new Logging.MessageLogger("LogonCheck.txt");

        public LogonCheck()
        {
            logonTimer = new Timer(60000);
            logonTimer.Elapsed += new ElapsedEventHandler(logonTimer_Elapsed);
            logonTimer.Start();
        }

        void logonTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                for (int i = DataClasses.GlobalData.currentTrucks.Count() - 1; i >= 0; i--)
                {
                    string TruckStatus = DataClasses.GlobalData.currentTrucks[i].Status.VehicleStatus;
                    string BeatNumber = DataClasses.GlobalData.currentTrucks[i].beatNumber;
                    if (TruckStatus != "Waiting for Driver Login" && string.IsNullOrEmpty(BeatNumber)) //assumes we have a logged on driver, but no beat number
                    {
                        DataClasses.GlobalData.currentTrucks[i].Status.VehicleStatus = "Waiting for Driver Login";
                        DataClasses.GlobalData.currentTrucks[i].Driver.LastName = "Not Logged On";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.writeToLogFile(DateTime.Now.ToString() + " Error checking logons:" + Environment.NewLine + ex.ToString());
            }
        }
    }
}