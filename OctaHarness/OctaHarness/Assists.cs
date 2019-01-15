using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;

namespace OctaHarness
{
    public partial class Assists : Form
    {
        /* WHOLE SYSTEM CHANGED NONE OF THIS WORKS ANYMORE
        public Assists()
        {
            InitializeComponent();
            LoadIncidents();
            LoadTruck();
            txtAssistID.Text = MakeGuid();
        }

        private string MakeGuid()
        {
            Guid g;
            g = Guid.NewGuid();
            return g.ToString();
        }

        private void LoadIncidents()
        {
            ServiceReference1.TowTruckServiceClient myService = new ServiceReference1.TowTruckServiceClient();
            myService.Endpoint.Address = new EndpointAddress(new Uri("http://" + ServiceRef.ServiceAddress));
            List<ServiceReference1.IncidentScreenData> theseIncidents = new List<ServiceReference1.IncidentScreenData>();
            List<Incident> localIncidents = new List<Incident>();
            theseIncidents = myService.GetAllIncidents().ToList<ServiceReference1.IncidentScreenData>();
            foreach (ServiceReference1.IncidentScreenData thisIncidentIn in theseIncidents)
            {
                Incident thisIncident = new Incident();
                thisIncident.Description = thisIncidentIn.Description;
                thisIncident.IncidentID = thisIncidentIn.IncidentID;
             
                localIncidents.Add(thisIncident);
            }
            cboIncidents.DataSource = localIncidents;
            cboIncidents.DisplayMember = "Description";
            cboIncidents.ValueMember = "IncidentID";
        }

        private void LoadTruck()
        {
            ServiceReference1.TowTruckServiceClient myService = new ServiceReference1.TowTruckServiceClient();
            myService.Endpoint.Address = new EndpointAddress(new Uri("http://" + ServiceRef.ServiceAddress));
            List<ServiceReference1.AssistTruck> theseTrucks = myService.GetAssistTrucks().ToList<ServiceReference1.AssistTruck>();
            List<TruckData> localTrucks = new List<TruckData>();
            foreach (ServiceReference1.AssistTruck thisAssistTruck in theseTrucks)
            {
                TruckData thisTruck = new TruckData();
                thisTruck.TruckID = thisAssistTruck.TruckID;
                thisTruck.TruckNumber = thisAssistTruck.TruckNumber;
                thisTruck.ContractorID = thisAssistTruck.ContractorID;
                thisTruck.ContractorName = thisAssistTruck.ContractorName;
                localTrucks.Add(thisTruck);
            }
            cboTruck.DataSource = localTrucks;
            cboTruck.DisplayMember = "TruckNumber";
            cboTruck.ValueMember = "TruckID";
        }

        private void btnPostAssist_Click(object sender, EventArgs e)
        {
            Incident selIncident = (Incident)cboIncidents.SelectedItem;
            Guid IncidentID = selIncident.IncidentID;
            TruckData selTruck = (TruckData)cboTruck.SelectedItem;
            Guid TruckID = selTruck.TruckID;
            Guid ContractorID = selTruck.ContractorID;
            ServiceReference1.TowTruckServiceClient myService = new ServiceReference1.TowTruckServiceClient();
            myService.Endpoint.Address = new EndpointAddress(new Uri("http://" + ServiceRef.ServiceAddress));
            ServiceReference1.AssistReq thisAssist = new ServiceReference1.AssistReq();
            thisAssist.AssistID = new Guid(txtAssistID.Text);
            
            thisAssist.IncidentID = IncidentID;
            thisAssist.FleetVehicleID = TruckID;
            thisAssist.ContractorID = ContractorID;
            thisAssist.DispatchTime = DateTime.Now;
            thisAssist.x1097 = DateTime.Now;
            myService.AddAssist(thisAssist);
            MessageBox.Show("Assist posted");
        }*/
    }

    public class TruckData
    {
        public Guid TruckID { get; set; }
        public string TruckNumber { get; set; }
        public string ContractorName { get; set; }
        public Guid ContractorID { get; set; }
        public string IPAddress { get; set; }
    }

}
