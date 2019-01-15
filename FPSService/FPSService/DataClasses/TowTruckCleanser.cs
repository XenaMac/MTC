using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;

namespace FPSService.DataClasses
{
    public class TowTruckCleanser
    {
        Timer CleanserTimer;
        Logging.MessageLogger logger = new Logging.MessageLogger("TowTruckCleanser.txt");
        public TowTruckCleanser()
        {
            CleanserTimer = new Timer(300000);
            CleanserTimer.Elapsed += new ElapsedEventHandler(CleanserTimer_Elapsed);
            CleanserTimer.Start();
        }

        void CleanserTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                for (int i = GlobalData.currentTrucks.Count - 1; i >= 0; i--)
                {
                    if (GlobalData.currentTrucks[i].LastMessage.LastMessageReceived < DateTime.Now.AddMinutes(DataClasses.GlobalData.ForceOff * -1)) //remove any trucks that haven't talked in ForceOff minutes
                    {
                        if (GlobalData.currentTrucks[i].Driver.DriverID != new Guid("00000000-0000-0000-0000-000000000000") &&
                            GlobalData.currentTrucks[i].Status.VehicleStatus != "Waiting for Driver Login")
                        {
                            GlobalData.currentTrucks[i].setStatus("ROLL IN", false);
                            GlobalData.currentTrucks[i].logOff();
                        }
                        GlobalData.currentTrucks.RemoveAt(i);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.writeToLogFile(DateTime.Now.ToString() + " Error purging old trucks:" + Environment.NewLine + ex.ToString());
            }
        }
    }
}