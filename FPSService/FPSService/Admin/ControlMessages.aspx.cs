using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPSService.Admin
{
    public partial class ControlMessages : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadTrucks();
            }
        }

        private void LoadTrucks()
        {
            ddlFTTs.Items.Clear();

            List<TowTruck.TowTruck> theseTrucks = DataClasses.GlobalData.currentTrucks;
            theseTrucks = theseTrucks.OrderBy(t => t.TruckNumber).ToList();

            foreach (TowTruck.TowTruck thisTruck in theseTrucks)
            {
                ListItem thisItem = new ListItem();
                string TruckID;
                if (string.IsNullOrEmpty(thisTruck.TruckNumber))
                {
                    TruckID = "NOID";
                }
                else
                {
                    TruckID = thisTruck.TruckNumber;
                }
                thisItem.Text = TruckID;
                thisItem.Value = thisTruck.Identifier;
                ddlFTTs.Items.Add(thisItem);
            }
        }

        private void ConstructMessage()
        {
            string msg = "<" + ddlMessages.Text + ">";
            msg += "<Id>" + MakeMsgID() + "</Id>";
            string VarName = string.Empty;
            string VarValue = string.Empty;
            if (ddlMessages.Text == "GetVar")
            {
                VarName = ddlVarNames.Text;
                msg += "<Name>" + VarName + "</Name>";
            }
            if (ddlMessages.Text == "SetVar")
            {
                if (string.IsNullOrEmpty(ddlVarNames.Text) || string.IsNullOrEmpty(txtVarValue.Text))
                {
                    Response.Write("No data for either var name or var value, aborting");
                    return;
                }
                VarName = ddlVarNames.Text;
                VarValue = txtVarValue.Text;
                msg += "<" + VarName + ">" + VarValue + "</" + VarName + ">";
            }

            msg += "</" + ddlMessages.Text + ">";
            TowTruck.TowTruck thisTruck = DataClasses.GlobalData.FindTowTruck(ddlFTTs.SelectedValue.ToString());
            if (thisTruck != null)
            {
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            ConstructMessage();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            LoadTrucks();
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            string SelectedTruck = ddlFTTs.SelectedValue.ToString();
            if (string.IsNullOrEmpty(SelectedTruck))
            { return; }
            DataClasses.GlobalData.RemoveTowTruck(SelectedTruck);
            LoadTrucks();
        }

        protected void lbtnLogOff_Click(object sender, EventArgs e)
        {
            Session["Logon"] = null;
            Response.Redirect("Logon.aspx");
        }
    }
}