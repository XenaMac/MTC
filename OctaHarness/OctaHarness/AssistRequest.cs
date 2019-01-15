using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OctaHarness
{
    public partial class AssistRequest : Form
    {
        /* WHOLE SYSTEM CHANGED NONE OF THIS WORKS ANYMORE
        List<ServiceReference1.TowTruckData> myData;
        ServiceReference1.TowTruckServiceClient myService;
        ServiceReference1.AssistReq myReq;
        List<TruckItem> ConnectedTrucks = new List<TruckItem>();
        public AssistRequest()
        {
           
            InitializeComponent();
            myService = new ServiceReference1.TowTruckServiceClient();
            cboTrucks.Items.Clear();
            LoadTrucks();
            LoadDropDownData();
        }

        private void LoadDropDownData()
        {
            string[] ServiceTypes = myService.GetPreloadedData("ServiceTypes");
            cboServiceTypes.Items.Clear();
            for (int i = 0; i < ServiceTypes.Count(); i++)
            {
                cboServiceTypes.Items.Add(ServiceTypes[i].ToString());
            }
            string[] VehicleTypes = myService.GetPreloadedData("VehicleTypes");
            cboVehicleTypes.Items.Clear();
            for (int i = 0; i < VehicleTypes.Count(); i++)
            {
                cboVehicleTypes.Items.Add(VehicleTypes[i].ToString());
            }
            string[] VehiclePositions = myService.GetPreloadedData("VehiclePositions");
            cboVehiclePosition.Items.Clear();
            for (int i = 0; i < VehiclePositions.Count(); i++)
            {
                cboVehiclePosition.Items.Add(VehiclePositions[i].ToString());
            }
        }

        private void LoadTrucks()
        {
            myData = myService.CurrentTrucks().ToList<ServiceReference1.TowTruckData>();
            foreach (ServiceReference1.TowTruckData myTruck in myData)
            {
                ConnectedTrucks.Add(new TruckItem { 
                    TruckID = myTruck.TruckNumber,
                    TruckIPAddress = myTruck.IPAddress
                });
            }
            cboTrucks.DataSource = ConnectedTrucks;
            cboTrucks.DisplayMember = "TruckID";
            cboTrucks.ValueMember = "TruckIPAddress";
        }

        public class TruckItem
        {
            public string TruckID { get; set; }
            public string TruckIPAddress { get; set; }
        }

        private void btnSendRequest_Click(object sender, EventArgs e)
        {
            string _ip = cboTrucks.SelectedValue.ToString();
            string _data = txtComments.Text;
            /*
            myReq = new ServiceReference1.AssistReq();
            myReq.AssistID = Guid.NewGuid();
            myReq.AssistType = "Assist";
            myReq.IncidentID = Guid.NewGuid();
            myReq.DispatchTime = DateTime.Now;
            myReq.ServiceType = cboServiceTypes.Text;
            myReq.Make = txtVehicleMake.Text;
            myReq.VehicleType = cboVehicleTypes.Text;
            myReq.VehiclePosition = cboVehiclePosition.Text;
            myReq.Color = txtVehicleColor.Text;
            myReq.LicensePlate = txtLicensePlate.Text;
            myReq.State = txtState.Text;
            myReq.CustomerLastName = txtCustomerLastName.Text;
            myReq.x1097 = DateTime.Now;
            myReq.IsMDC = false;

            myService.AddTruckAssistRequest(_ip, myReq, new Guid());
            MessageBox.Show("Request sent at " + DateTime.Now.ToString());

        }*/
    }
}
