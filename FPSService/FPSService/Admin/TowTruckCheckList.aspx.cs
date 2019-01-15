using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace FPSService.Admin
{
    public partial class TowTruckCheckList : System.Web.UI.Page
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
            }*/
            if (!Page.IsPostBack)
            {
                LoadContractors();
            }

            string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            if (ip == "::1")
            { ip = "127.0.0.1"; }
            IPAdd.Text = ip;

            TowTruck.TowTruck thisTruck = DataClasses.GlobalData.FindTowTruck(ip);
            double lat = 0.0;
            double lon = 0.0;
            if (thisTruck != null)
            {
                lat = thisTruck.GPSPosition.Lat;
                lon = thisTruck.GPSPosition.Lon;
                //use these values in labels or boxes or something
            }
            if (lat != 0.0 && lon != 0.0)
            {
                GPS.Checked = true;
            }

        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void YES_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void YES_CheckedChanged1(object sender, EventArgs e)
        {

        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {


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

        protected void Button1_Click(object sender, EventArgs e)
        {
            /*
             * Open connection to DB
             */
                String ConnStr = ConfigurationManager.AppSettings["FSPdb"].ToString();
                string txtInstaller, txtVehicleID, txtIPAddress, txtSSN, txtIDate, txtUSpeed, txtDspeed, txtTTC, txtSSC;
                bool txtSecure, txtCell, txtPower, txtRouterSecure, txtMoisture, txtSpeedTest, txtGPS;

                int[] check;
                check = new int[16];
                bool clicked = true;

                if (clicked == true)
                {
                    txtInstaller = installerName.Text;
                    if (txtInstaller.Equals(""))
                    {
                        iName.InnerText = "*";
                        check[0] = 1;
                    }
                    else
                    {
                        iName.InnerText = "";
                        check[0] = 0;
                    }
                    txtVehicleID = vID.Text;
                    if (txtVehicleID.Equals(""))
                    {
                        VehID.InnerText = "*";
                        check[1] = 1;
                    }
                    else
                    {
                        VehID.InnerText = "";
                        check[1] = 0;
                    }
                    txtIPAddress = IPAdd.Text;
                    if (txtIPAddress.Equals(""))
                    {
                        IPA.InnerText = "*";
                        check[2] = 1;
                    }
                    else
                    {
                        IPA.InnerText = "";
                        check[2] = 0;
                    }
                    txtSSN = sysSerNumber.Text;
                    if (txtSSN.Equals(""))
                    {
                        S.InnerText = "*";
                        check[3] = 1;
                    }
                    else
                    {
                        S.InnerText = "";
                        check[3] = 0;
                    }
                    txtIDate = installDate.Text;
                    if (txtIDate.Equals(""))
                    {
                        ID.InnerText = "*";
                        check[4] = 1;
                    }
                    else
                    {
                        ID.InnerText = "";
                        check[4] = 0;
                    }
                    txtUSpeed = UploadSpeed.Text;
                    if (txtUSpeed.Equals(""))
                    {
                        US.InnerText = "*";
                        check[5] = 1;
                    }
                    else
                    {
                        US.InnerText = "";
                        check[5] = 0;
                    }
                    txtDspeed = DownloadSpeed.Text;
                    if (txtDspeed.Equals(""))
                    {
                        DS.InnerText = "*";
                        check[6] = 1;
                    }
                    else
                    {
                        DS.InnerText = "";
                        check[6] = 0;
                    }
                    txtTTC = towTruckCompany.Text;
                    if (txtTTC.Equals(""))
                    {
                        tTC.InnerText = "*";
                        check[7] = 1;
                    }
                    else
                    {
                        tTC.InnerText = "";
                        check[7] = 0;
                    }
                    //Check box checking
                    txtSecure = Secure.Checked;
                    if(txtSecure == false)
                    {
                        Sec.InnerHtml = "*";
                        check[8] = 1;
                    }
                    else{
                        Sec.InnerHtml = "";
                        check[8] = 0;
                    }
                    txtCell = Cell.Checked;
                    if(txtCell == false)
                    {
                        Ce.InnerHtml = "*";
                        check[9] = 1;
                    }
                    else{
                        Ce.InnerHtml = "";
                        check[9] = 0;
                    }
                    txtPower = Power.Checked;
                    if(txtPower == false)
                    {
                        Pow.InnerHtml = "*";
                        check[10] = 1;
                    }
                    else{
                        Pow.InnerHtml = "";
                        check[10] = 0;
                    }
                    txtRouterSecure = routerSecure.Checked;
                    if(txtRouterSecure == false)
                    {
                        RoutSec.InnerHtml = "*";
                        check[11] = 1;
                    }
                    else{
                        RoutSec.InnerHtml = "";
                        check[11] = 0;
                    }
                    txtMoisture = Moisture.Checked;
                    if(txtMoisture == false)
                    {
                        Moist.InnerHtml = "*";
                        check[12] = 1;
                    }
                    else{
                        Moist.InnerHtml = "";
                        check[12] = 0;
                    }
                    txtSpeedTest = SpeedTest.Checked;
                    if(txtSpeedTest == false)
                    {
                        SpeedT.InnerHtml = "*";
                        check[13] = 1;
                    }
                    else{
                        SpeedT.InnerHtml = "";
                        check[13] = 0;
                    }
                    txtGPS = GPS.Checked;
                    if (txtGPS == false)
                    {
                        G.InnerHtml = "*";
                        check[14] = 1;
                    }
                    else
                    {
                        G.InnerHtml = "";
                        check[14] = 0;
                    }
                    txtSSC = ddlContractors.Text;
                    if (txtSSC.Equals(""))
                    {
                        Cont.InnerHtml = "*";
                        check[15] = 1;
                    }
                    else
                    {
                        Cont.InnerHtml = "";
                        check[15] = 0;
                    }

                    for (int i = 0; i < check.Length; i++)
                    {
                        if (check[i] == 1)
                        {
                            emsg.InnerText = "* Indicates a required field.";
                        }
                    }
                    if (check[0] == 0
                            && check[1] == 0
                            && check[2] == 0
                            && check[3] == 0
                            && check[4] == 0
                            && check[5] == 0
                            && check[6] == 0
                            && check[7] == 0
                            && check[8] == 0
                            && check[9] == 0
                            && check[10] == 0
                            && check[11] == 0
                            && check[12] == 0
                            && check[13] == 0
                            && check[14] == 0
                            && check[15]==0)
                           
                    {
                        emsg.InnerText = "";
                    }
                    else
                    {
                        return;
                    }
                    
                }
                try{
                    SqlConnection conn = new SqlConnection(ConnStr);
                       {
                           conn.Open();
                           String SQL = "LogTowTruckSetup";
                           String SQL2 = "FixTruckNumber";
                           SqlCommand cmd = new SqlCommand(SQL, conn);
                           SqlCommand cmd2 = new SqlCommand(SQL2, conn);
                           cmd.CommandType = CommandType.StoredProcedure;
                           cmd2.CommandType = CommandType.StoredProcedure;

                           //cmd.Parameters.AddWithValue("@ParameterName", "ParameterValue");
                           cmd.Parameters.AddWithValue("@MountedSecurely", Secure.Checked);
                           cmd.Parameters.AddWithValue("@ConnectedToCell", Cell.Checked);
                           cmd.Parameters.AddWithValue("@DCPowerConnected", Power.Checked);
                           cmd.Parameters.AddWithValue("@RouterUnitMountedSecurely", routerSecure.Checked);
                           cmd.Parameters.AddWithValue("@MoistureFree", Moisture.Checked);
                           cmd.Parameters.AddWithValue("@Speedtest", SpeedTest.Text);
                           cmd.Parameters.AddWithValue("@GPSSentProperly", GPS.Checked);
                           //Text Box input
                           cmd.Parameters.AddWithValue("@InstallerName", installerName.Text);
                           cmd.Parameters.AddWithValue("@TowTruckCompany", towTruckCompany.Text);
                           cmd.Parameters.AddWithValue("@VehicleID", vID.Text);
                           cmd.Parameters.AddWithValue("@SystemSerialNumber", sysSerNumber.Text); 
                           cmd.Parameters.AddWithValue("@IPAddress", IPAdd.Text);
                           cmd.Parameters.AddWithValue("@UploadSpeed", UploadSpeed.Text);
                           cmd.Parameters.AddWithValue("@DownloadSpeed", DownloadSpeed.Text);
                           cmd.ExecuteNonQuery();
                           cmd = null;
  
                           
                           cmd2.Parameters.AddWithValue("@IPAddress", IPAdd.Text);
                           cmd2.Parameters.AddWithValue("@ContractorID", ddlContractors.Text);
                           cmd2.Parameters.AddWithValue("@VehicleNumber", vID.Text);
                           cmd2.ExecuteNonQuery();
                           cmd2 = null;
                           conn.Close();

                       }

                }catch(Exception ex)
                    {
                        Logging.EventLogger logger = new Logging.EventLogger();
                        logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error uploading TowTruck CheckList" + Environment.NewLine + ex.ToString(), true);
                        Response.Write("The following error has occured" + ex);
                    }


                
                installerName.Text = "";
                installDate.Text = "";
                towTruckCompany.Text = "";
                vID.Text = "";
                sysSerNumber.Text = "";
                IPAdd.Text = "";
                UploadSpeed.Text = "";
                DownloadSpeed.Text = "";
                Secure.Checked = false;
                Cell.Checked = false;
                Power.Checked = false;
                routerSecure.Checked = false;
                Moisture.Checked = false;
                SpeedTest.Checked = false;
                GPS.Checked = false;

                

                /*
                 * Checks for filled out text boxes

                if (string.IsNullOrEmpty(installerName.Text))
                {
                    Response.Write("Please fill out the Installer Name");
                    return;
                }
                if (string.IsNullOrEmpty(vehicleIDDescription.Text))
                {
                    Response.Write("Please enter Vehicle ID/Description");
                }
                if (string.IsNullOrEmpty(IPAddress.Text))
                {
                    Response.Write("Please enter an IP Address");
                }
                if (string.IsNullOrEmpty(sysSerialNumber.Text))
                {
                    Response.Write("Please enter a System Serial Number");
                }
                if (string.IsNullOrEmpty(installationDate.Text))
                {
                    Response.Write("Please fill out the Installation Date");
                }
                if (string.IsNullOrEmpty(qualityAssurInspectDate.Text))
                {
                    Response.Write("Please fill out the Quality Assurance Inspection Date");
                }
                 * 
                 *              */
            /*
             * Checks for checked boxes

        if(Secure.Checked.Equals(false))
        {
            Response.Write("Is the Roof Antenna Mounted Securely?");
        }
        if (Sealed.Checked.Equals(false))
        {
            Response.Write("Is the Roof Antenna Sealed with Silicone Sealant?");
        }
        if (Cell.Checked.Equals(false))
        {
            Response.Write("Is the Roof Antenna Connected to \"Cell\" on the Router?");
        }
        if (GPS.Checked.Equals(false))
        {
            Response.Write("Is the Roof Antenna Connected to \"GPS\" on the Router?");
        }
        if (Wifi.Checked.Equals(false))
        {
            Response.Write("Is the roof Antenna Connected to \"WIFI\" on the Router?");
        }
        if (Power.Checked.Equals(false))
        {
            Response.Write("Is the DC Power Connected Properly to the Vehicle?");
        }
        if (Moisture.Checked.Equals(false))
        {
            Response.Write("Is the Router Unit Mounted Securely and Free from Moisture?");
        }
        if (SSID.Checked.Equals(false))
        {
            Response.Write("Is the Wireless SSID \"OCTAWIFI\" visible from test laptop?");
        }
        if (Browse.Checked.Equals(false))
        {
            Response.Write("Are You Able to Browse to ______?");
        }
        if (Setup.Checked.Equals(false))
        {
            Response.Write("Input Information on Tow Truck Setup?");
        }
        if (Google.Checked.Equals(false))
        {
            Response.Write("Are You Able to Browse to www.google.com?");
        }
        if (Browse2.Checked.Equals(false))
        {
            Response.Write("Are You Able to Browse to other Websites?");
        }
        if (Speed.Checked.Equals(false))
        {
            Response.Write("Successful Speed Test? (Please Record Kbits/s)");
        }
             *                  */
            /*
             * Checks for signature on setup sheet

            if (string.IsNullOrEmpty(signature.Text))
            {
                Response.Write("Please Sign at the Bottom of the Setup Sheet");
            }
             *              */
        }
    }
}