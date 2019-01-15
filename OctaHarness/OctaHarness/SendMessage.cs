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
    public partial class SendMessage : Form
    {
        
        public SendMessage()
        {
            InitializeComponent();
            LoadTrucks();
            LoadUsers();
        }

        private string GetConn()
        {
            string Conn = ConfigurationManager.ConnectionStrings["OctaHarness.Properties.Settings.db"].ToString();
            return Conn;
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
                SqlCommand cmd = new SqlCommand("select UserID as 'userid', LastName + ',' + FirstName as 'name' From users union all select DriverID as 'userid', LastName + ',' + FirstName as 'name' FROM Drivers", conn);
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

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            ServiceReference1.TowTruckServiceClient myService = new ServiceReference1.TowTruckServiceClient();
            myService.Endpoint.Address = new EndpointAddress(new Uri("http://" + ServiceRef.ServiceAddress));
            ServiceReference1.TruckMessage thisMessage = new ServiceReference1.TruckMessage();
            thisMessage.MessageID = Guid.NewGuid();
            thisMessage.MessageText = txtMessage.Text;
            TruckData selTruck = (TruckData)cboTrucks.SelectedItem;
            string TruckIP = selTruck.IPAddress;
            CreatedBy selUser = (CreatedBy)cboUsers.SelectedItem;
            string UserID = selUser.UserName;
            thisMessage.UserEmail = UserID;
            thisMessage.TruckIP = TruckIP;
            thisMessage.SentTime = DateTime.Now;
            if (chkExpectResponse.Checked == true)
            {
                thisMessage.MessageType = 1;
            }
            else
            {
                thisMessage.MessageType = 0;
            }
            thisMessage.MessageResponse = "0";
            myService.SendMessage(thisMessage);
            MessageBox.Show("Message sent");
        }

        private void btnRefreshTrucks_Click(object sender, EventArgs e)
        {
            LoadTrucks();
        }

        private void btnRefreshUsers_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }
    }
}
