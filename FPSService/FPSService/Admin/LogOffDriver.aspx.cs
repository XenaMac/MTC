using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPSService.Admin
{
    public partial class LogOffDriver : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Logon"] == null)
            {
                Response.Redirect("Logon.aspx");
            }
            string logon = Session["Logon"].ToString();
            if (logon != "true")
            {
                Response.Redirect("Logon.aspx");
            }
            LoadDrivers();
        }

        private void LoadDrivers()
        {
            List<driverLogoff> dlList = new List<driverLogoff>();
            foreach (TowTruck.TowTruck t in DataClasses.GlobalData.currentTrucks)
            {
                if (t.Driver.LastName != "No Driver")
                {
                    driverLogoff dl = new driverLogoff();
                    dl.TruckNumber = t.TruckNumber;
                    dl.DriverName = t.Driver.FirstName + " " + t.Driver.LastName;
                    dl.ContractorCompany = t.Driver.TowTruckCompany;
                    dlList.Add(dl);
                }
            }
            gvDrivers.DataSource = dlList;
            gvDrivers.DataBind();
        }

        protected void lbtnLogOff_Click(object sender, EventArgs e)
        {
            Session["Logon"] = null;
            Response.Redirect("Logon.aspx");
        }

        protected void gvDrivers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow gvr = gvDrivers.Rows[index];
            string truckNumber = gvr.Cells[1].Text;
            driverLogoff(truckNumber);
            LoadDrivers();
        }

        private void driverLogoff(string truckNumber)
        {
            TowTruck.TowTruck thisTruck = DataClasses.GlobalData.FindTowTruckByVehicleNumber(truckNumber);
            if (thisTruck != null)
            {
                SQL.SQLCode mySQL = new SQL.SQLCode();
                thisTruck.tts.startStatus("LogOff", thisTruck.TruckNumber, thisTruck.Driver.FirstName + " " + thisTruck.Driver.LastName,
                        thisTruck.Driver.TowTruckCompany, thisTruck.beatNumber, thisTruck.GPSPosition.Lat, thisTruck.GPSPosition.Lon, thisTruck.runID, thisTruck.location,
                        thisTruck.GPSPosition.Speed, thisTruck.GPSPosition.Head, thisTruck.Driver.schedule.scheduleID);
                mySQL.LogEvent(thisTruck.Driver.DriverID, "Driver Log Off");
                DataClasses.GlobalData.RemoveCover(thisTruck.TruckNumber);
                string cadMSG = "DU.";
                cadMSG += thisTruck.shiftType + "." + thisTruck.Driver.callSign;
                Global.cSender.sendMessage(cadMSG);
                mySQL.closeOutTruck(thisTruck.runID);
                thisTruck.Status.VehicleStatus = "Waiting for Driver Login";
                thisTruck.Driver.DriverID = new Guid("00000000-0000-0000-0000-000000000000");
                thisTruck.Driver.FSPID = "";
                thisTruck.Driver.FirstName = "No Driver";
                thisTruck.Driver.LastName = "No Driver";
                thisTruck.Driver.TowTruckCompany = "No Driver";
                thisTruck.Driver.schedule = null;
                //thisTruck.Driver.BreakStarted = Convert.ToDateTime("01/01/2001 00:00:00");
                /* OLD OCTA CODE - NOT USED IN MTC
                thisTruck.assignedBeat.BeatID = new Guid("00000000-0000-0000-0000-000000000000");
                thisTruck.assignedBeat.BeatExtent = null;
                thisTruck.assignedBeat.BeatNumber = "Not Assigned";
                thisTruck.assignedBeat.Loaded = false;
                 * */
                thisTruck.beatNumber = "NOBEAT";
                thisTruck.Status.SpeedingTime = Convert.ToDateTime("01/01/2001 00:00:00");
                thisTruck.Status.OutOfBoundsTime = Convert.ToDateTime("01/01/2001 00:00:00");
                thisTruck.Status.SpeedingValue = "0.0";
                thisTruck.Status.OutOfBoundsMessage = "Not logged on";
                thisTruck.Status.OutOfBoundsAlarm = false;
                thisTruck.Status.SpeedingAlarm = false;
                thisTruck.Status.StatusStarted = DateTime.Now;
                //thisTruck.thisSchedule = null;
                thisTruck.runID = new Guid("00000000-0000-0000-0000-000000000000");
                //reset status and alarms for truck
                thisTruck.tts = null;
                thisTruck.tta = null;
                thisTruck.tts = new TowTruck.TruckStatus();
                thisTruck.tta = new TowTruck.TowTruckAlarms();
                thisTruck.wentOnPatrol = false;
                thisTruck.rolledIn = false;
                thisTruck.Driver.schedule = null;
                thisTruck.cadCallSign = "NA";
                string msg = "<SetVar><Id>" + MakeMsgID() + "</Id><LoggedOn>F</LoggedOn></SetVar>";
                thisTruck.SendMessage(msg);
            }
        }

        private string MakeMsgID()
        {
            DateTime dtSeventy = Convert.ToDateTime("01/01/1970 00:00:00");
            TimeSpan tsSpan = DateTime.Now - dtSeventy;
            double ID = tsSpan.TotalMilliseconds;
            Int64 id = Convert.ToInt64(ID);
            return id.ToString();
        }
    }

    public class driverLogoff
    {
        public string TruckNumber { get; set; }
        public string DriverName { get; set; }
        public string ContractorCompany { get; set; }
    }
}