using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPSService.Admin
{
    public partial class TimeChecker : System.Web.UI.Page
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
            LoadData();
        }

        private void LoadData()
        {
            List<MiscData.GPSTimeCheck> checks = new List<MiscData.GPSTimeCheck>();
            foreach (TowTruck.TowTruck thisTruck in DataClasses.GlobalData.currentTrucks)
            {
                MiscData.GPSTimeCheck thisCheck = new MiscData.GPSTimeCheck();
                thisCheck.TruckNumber = thisTruck.TruckNumber;
                thisCheck.IPAddress = thisTruck.Identifier;
                thisCheck.LastUpdate = thisTruck.LastMessage.LastMessageReceived;
                thisCheck.GPSTime = Convert.ToDateTime(thisTruck.GPSPosition.Time);
                thisCheck.SpeedingTime = thisTruck.Status.SpeedingTime;
                checks.Add(thisCheck);
            }
            gvData.DataSource = checks;
            gvData.DataBind();
        }
    }
}