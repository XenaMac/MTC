using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;

namespace FPSService.Admin
{
    public partial class TowTruckSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            if (Session["Logon"] == null)
            {
                Response.Redirect("Logon.aspx");
            }
            string logon = Session["Logon"].ToString();
            if (logon != "true")
            {
                Response.Redirect("Logon.aspx");
            }
             * */
            if (!Page.IsPostBack)
            {
                LoadContractors();
            }
            string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            if (ip == "::1")
            { ip = "127.0.0.1"; }
            lblIPAddress.Text = ip;
            TowTruck.TowTruck thisTruck = DataClasses.GlobalData.FindTowTruck(ip);
            double lat = 0.0;
            double lon = 0.0;
            if (thisTruck != null)
            {
                lat = thisTruck.GPSPosition.Lat;
                lon = thisTruck.GPSPosition.Lon;
                //use these values in labels or boxes or something
            }
        }


        private void LoadContractors()
        {
            ddlContractors.Items.Clear();
            foreach (MiscData.Contractors contractor in DataClasses.GlobalData.Contractors)
            {
                ListItem thisItem = new ListItem();
                thisItem.Text = contractor.ContractCompanyName;
                thisItem.Value = contractor.ContractorID.ToString();
                ddlContractors.Items.Add(thisItem);
            }
        }

        protected void btnAddNewTruck_Click(object sender, EventArgs e)
        {
            Guid ContractorID = new Guid(ddlContractors.SelectedValue);
            TowTruck.TowTruck thisTruck = DataClasses.GlobalData.FindTowTruck(txtIPAddress.Text);
            if (thisTruck != null)
            { thisTruck.TruckNumber = txtTruckNumber.Text; }
            DataClasses.GlobalData.UpdateTowTruck(txtIPAddress.Text, thisTruck);
            SQL.SQLCode mySQL = new SQL.SQLCode();
            mySQL.FixTruckNumber(txtIPAddress.Text, txtTruckNumber.Text, ContractorID);
        }
    }
}