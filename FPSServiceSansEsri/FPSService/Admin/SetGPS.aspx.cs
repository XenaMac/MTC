using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPSService.Admin
{
    public partial class SetGPS : System.Web.UI.Page
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
            if (!Page.IsPostBack)
            {
                LoadAllTrucks();
            }
        }

        private void LoadAllTrucks()
        {
            var tList = from t in DataClasses.GlobalData.currentTrucks
                        select t;
            List<TowTruck.TowTruck> truckList = tList.ToList<TowTruck.TowTruck>();
            truckList = truckList.OrderBy(t => t.TruckNumber).ToList<TowTruck.TowTruck>();
            foreach (TowTruck.TowTruck t in truckList)
            {
                ddlIndividualTrucks.Items.Add(t.TruckNumber);
            }
        }

        private void SendMessages(List<TowTruck.TowTruck> truckList)
        {
            if (string.IsNullOrEmpty(txtGPSTime.Text))
            {
                SetPanel("You must enter a value for GPS Send Rate: Aborting");
                return;
            }
            string msg = "<SetVar><Id>" + MakeMsgID() + "</Id><SetGPS>" + txtGPSTime.Text + "</SetGPS></SetVar>";
            foreach (TowTruck.TowTruck t in truckList)
            {
                t.SendMessage(msg);
            }
            SetPanel("Successfully sent GPS Interval change requests");
        }

        private string MakeMsgID()
        {
            DateTime dtSeventy = Convert.ToDateTime("01/01/1970 00:00:00");
            TimeSpan tsSpan = DateTime.Now - dtSeventy;
            double ID = tsSpan.TotalMilliseconds;
            Int64 id = Convert.ToInt64(ID);
            return id.ToString();
        }

        private List<TowTruck.TowTruck> getSelectedTrucks(List<string> TruckNumbers)
        {
            List<TowTruck.TowTruck> trucks = new List<TowTruck.TowTruck>();

            foreach (string s in TruckNumbers)
            {
                TowTruck.TowTruck found = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck t) { return t.TruckNumber == s; });
                if (found != null)
                {
                    TowTruck.TowTruck listTruck = trucks.Find(delegate(TowTruck.TowTruck t) { return t.TruckNumber == found.TruckNumber; });
                    if (listTruck == null)
                    {
                        trucks.Add(found);
                    }
                }
            }

            return trucks;
        }

        private void SetPanel(string msg)
        {
            pnlMessages.Controls.Clear();
            pnlMessages.Controls.Add(new LiteralControl("<h2>" + msg + "</h2>"));
        }

        protected void btnIndividualTrucks_Click(object sender, EventArgs e)
        {
            List<string> truckStrings = new List<string>();
            truckStrings.Add(ddlIndividualTrucks.Text);
            List<TowTruck.TowTruck> truck = getSelectedTrucks(truckStrings);
            SendMessages(truck);
        }

        protected void lbtnLogOff_Click(object sender, EventArgs e)
        {
            Session["Logon"] = null;
            Response.Redirect("Logon.aspx");
        }
    }
}