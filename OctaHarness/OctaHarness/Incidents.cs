using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.ServiceModel;

namespace OctaHarness
{
    public partial class Incidents : Form
    {
        /* WHOLE SYSTEM CHANGED, NONE OF THIS WORKS ANYMORE
        private string ServiceAddress;
        public Incidents()
        {
            InitializeComponent();
            txtIncidentID.Text = MakeGuid();
            txtTimeStamp.Text = DateTime.Now.ToString();
            LoadFreeways();
            LoadUsers();
            LoadLocations();
            GetCurrentIncidents();
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

        private void LoadFreeways()
        {
            List<Freeway> theseFreeways = new List<Freeway>();
            using(SqlConnection conn = new SqlConnection(GetConn()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT FreewayID, FreewayName FROM Freeways", conn);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Freeway thisFreeway = new Freeway();
                    thisFreeway.FreewayID = Convert.ToInt32(rdr["FreewayID"]);
                    thisFreeway.FreewayName = rdr["FreewayName"].ToString();
                    theseFreeways.Add(thisFreeway);
                }
                rdr.Close();
                rdr = null;
                conn.Close();
            }
            cboFreeways.DataSource = theseFreeways;
            cboFreeways.DisplayMember = "FreewayName";
            cboFreeways.ValueMember = "FreewayID";
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
            cboCreatedBy.DataSource = theseUsers;
            cboCreatedBy.DisplayMember = "UserName";
            cboCreatedBy.ValueMember = "UserID";
        }

        private void LoadLocations()
        {
            List<Location> theseLocations = new List<Location>();
            using (SqlConnection conn = new SqlConnection(GetConn()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select LocationID, Location from locations", conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Location thisLocation = new Location();
                    thisLocation.LocationID = new Guid(rdr[0].ToString());
                    thisLocation.LocationName = rdr[1].ToString();
                    theseLocations.Add(thisLocation);
                }
            }
            cboLocations.DataSource = theseLocations;
            cboLocations.DisplayMember = "LocationName";
            cboLocations.ValueMember = "LocationID";
        }

        private void LoadSegments(string FreewayID)
        {
            cboBeatSegmentID.DataSource = null;
            cboBeatSegmentID.Items.Clear();
            List<BeatSegment> theseSegments = new List<BeatSegment>();
            using (SqlConnection conn = new SqlConnection(GetConn()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select BeatSegmentID, BeatSegmentNumber from beatsegments where beatsegmentnumber like '%" + FreewayID + "-%'", conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    BeatSegment thisSegment = new BeatSegment();
                    thisSegment.BeatSegmentID = new Guid(rdr[0].ToString());
                    thisSegment.BeatSegmentNumber = rdr[1].ToString();
                    theseSegments.Add(thisSegment);
                }
            }
            cboBeatSegmentID.DataSource = theseSegments;
            cboBeatSegmentID.DisplayMember = "BeatSegmentNumber";
            cboBeatSegmentID.ValueMember = "BeatSegmentID";
        }

        private void cboFreeways_SelectedIndexChanged(object sender, EventArgs e)
        {
            Freeway chosenFreeway = (Freeway)cboFreeways.SelectedItem;
            LoadSegments(chosenFreeway.FreewayID.ToString());
        }

        private void btnPostIncident_Click(object sender, EventArgs e)
        {
            
            
            ServiceReference1.TowTruckServiceClient myService = new ServiceReference1.TowTruckServiceClient();
            myService.Endpoint.Address = new EndpointAddress(new Uri("http://" + ServiceRef.ServiceAddress));
            ServiceReference1.IncidentIn thisIncident = new ServiceReference1.IncidentIn();
            Guid IncidentID = new Guid(txtIncidentID.Text);
            string Location = txtLocation.Text;
            Freeway chosenFreeway = (Freeway)cboFreeways.SelectedItem;
            int FreewayID = chosenFreeway.FreewayID;
            Location chosenLocation = (Location)cboLocations.SelectedItem;
            Guid LocationID = chosenLocation.LocationID;
            BeatSegment choseBeatSegment = (BeatSegment)cboBeatSegmentID.SelectedItem;
            Guid BeatSegmentID = choseBeatSegment.BeatSegmentID;
            DateTime TimeStamp = DateTime.Parse(txtTimeStamp.Text);
            CreatedBy chosenCreator = (CreatedBy)cboCreatedBy.SelectedItem;
            string CreatedBy = chosenCreator.UserName;
            string Description = txtDescription.Text;
            string IncidentNumber = txtIncidentNumber.Text;
            thisIncident.IncidentID = IncidentID;
            //thisIncident.Location = Location;
            thisIncident.FreewayID = FreewayID;
            thisIncident.LocationID = LocationID;
            thisIncident.BeatSegmentID = new Guid("00000000-0000-0000-0000-000000000000");
            thisIncident.TimeStamp = TimeStamp;
            thisIncident.CreatedBy = CreatedBy;
            thisIncident.Description = Description;
            thisIncident.IncidentNumber = IncidentNumber;
            thisIncident.CrossStreet1 = "My Cross Street 1";
            thisIncident.CrossStreet2 = "My Cross Street 2";
            myService.AddIncident(thisIncident);
            MessageBox.Show("Incident Added");
        }

        private void GetCurrentIncidents()
        {
            cboIncidents.DataSource = null;
            cboIncidents.Items.Clear();
            
            ServiceReference1.TowTruckServiceClient myService = new ServiceReference1.TowTruckServiceClient();
            myService.Endpoint.Address = new EndpointAddress(new Uri("http://" + ServiceRef.ServiceAddress));
            List<ServiceReference1.IncidentScreenData> theseIncidents = new List<ServiceReference1.IncidentScreenData>();
            //List<ServiceReference1.IncidentIn> theseIncidents = new List<ServiceReference1.IncidentIn>();
            theseIncidents = myService.GetAllIncidents().ToList<ServiceReference1.IncidentScreenData>();
            List<Incident> localIncidents = new List<Incident>();
            foreach (ServiceReference1.IncidentScreenData thisIncident in theseIncidents)
            {
                Incident thisLocalIncident = new Incident();
                thisLocalIncident.IncidentID = thisIncident.IncidentID;
                thisLocalIncident.Description = thisIncident.Description;
                localIncidents.Add(thisLocalIncident);
            }
            cboIncidents.DataSource = localIncidents;
            cboIncidents.DisplayMember = "Description";
            cboIncidents.ValueMember = "IncidentID";
        }*/
    }

    public class Incident
    {
        public Guid IncidentID { get; set; }
        public string Description { get; set; }
    }

    public class BeatSegment
    {
        public Guid BeatSegmentID { get; set; }
        public string BeatSegmentNumber { get; set; }
    }

    public class CreatedBy
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
    }

    public class Location
    {
        public Guid LocationID { get; set; }
        public string LocationName { get; set; }
    }

    public class Freeway
    {
        public int FreewayID{get;set;}
        public string FreewayName{get;set;}
    }
}
