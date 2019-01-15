using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using System.Data.SqlClient;
using System.Configuration;

namespace OctaHarness
{
    public partial class MTCAssistRequest : Form
    {
        public MTCAssistRequest()
        {
            InitializeComponent();
            loadDispatchCodes();
            LoadTrucks();
            LoadUsers();
        }

        private void loadDispatchCodes()
        {
            cboDispatchCodes.Items.Clear();
            using (SqlConnection conn = new SqlConnection("Initial Catalog=MTCDB;Data Source=OCTA-DEV\\OCTA,5815;User Id=sa;Password=J@bb@Th3Hu22"))
            {
                conn.Open();

                string SQL = "SELECT Code FROM DispatchCodes ORDER BY Code";
                SqlCommand cmd = new SqlCommand(SQL, conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    cboDispatchCodes.Items.Add(rdr[0].ToString());
                }

                conn.Close();
            }
        }

        private string GetConn()
        {
            string Conn = ConfigurationManager.ConnectionStrings["OctaHarness.Properties.Settings.db"].ToString();
            return Conn;
        }

        private string MakeGuid()
        {
            Guid g;
            g = Guid.NewGuid();
            return g.ToString();
        }

        private void btnPostAssist_Click(object sender, EventArgs e)
        {
            ServiceReference1.TowTruckServiceClient myService = new ServiceReference1.TowTruckServiceClient();
            myService.Endpoint.Address = new EndpointAddress(new Uri("http://" + ServiceRef.ServiceAddress));
            ServiceReference1.MTCPreAssistData pad = new ServiceReference1.MTCPreAssistData();
            CreatedBy chosenCreator = (CreatedBy)cboUsers.SelectedItem;
            string userName = chosenCreator.UserName;
            TruckData selTruck = (TruckData)cboTrucks.SelectedItem;
            string ipAddr = selTruck.IPAddress;
            string dir = "";
            if (rbEB.Checked == true)
            {
                dir = "EB";
            }
            if (rbNB.Checked == true)
            {
                dir = "NB";
            }
            if (rbSB.Checked == true)
            {
                dir = "SB";
            }
            if (rbWB.Checked == true)
            {
                dir = "WB";
            }
            pad.DispatchCode = cboDispatchCodes.Text;
            pad.Comment = txtComment.Text;
            pad.CrossStreet = txtCrossStreet.Text;
            pad.Direction = dir;
            pad.FSPLocation = txtFSPLocation.Text;
            pad.Freeway = txtFreeway.Text;
            pad.LaneNumber = txtLaneNumber.Text;
            myService.addAssist(userName, ipAddr, pad);
            MessageBox.Show("Added");
        }

        private void LoadTrucks()
        {
            ServiceReference1.TowTruckServiceClient myService = new ServiceReference1.TowTruckServiceClient();
            myService.Endpoint.Address = new EndpointAddress(new Uri("http://" + ServiceRef.ServiceAddress));
            List<ServiceReference1.TowTruckData> theseTrucks = myService.CurrentTrucks().ToList<ServiceReference1.TowTruckData>();
            List<TruckData> localTrucks = new List<TruckData>();
            foreach (ServiceReference1.TowTruckData thisAssistTruck in theseTrucks)
            {
                TruckData thisTruck = new TruckData();
                thisTruck.IPAddress = thisAssistTruck.IPAddress;
                thisTruck.TruckNumber = thisAssistTruck.TruckNumber;
                thisTruck.ContractorName = thisAssistTruck.ContractorName;
                localTrucks.Add(thisTruck);
            }
            cboTrucks.DataSource = localTrucks;
            cboTrucks.DisplayMember = "TruckNumber";
            cboTrucks.ValueMember = "IPAddress";
        }

        private void LoadUsers()
        {
            List<CreatedBy> theseUsers = new List<CreatedBy>();
            using (SqlConnection conn = new SqlConnection(GetConn()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select UserID as 'userid', LastName + ',' + FirstName as 'name' From users union all select DriverID as 'userid', LastName + ',' + FirstName as 'name' FROM Drivers ORDER BY name", conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    CreatedBy thisUser = new CreatedBy();
                    thisUser.UserID = new Guid(rdr[0].ToString());
                    thisUser.UserName = rdr[1].ToString();
                    theseUsers.Add(thisUser);
                }
            }
            cboUsers.DataSource = theseUsers;
            cboUsers.DisplayMember = "UserName";
            cboUsers.ValueMember = "UserID";
        }
    }      
}
