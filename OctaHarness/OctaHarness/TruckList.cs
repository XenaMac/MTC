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
    public partial class TruckList : Form
    {
        public TruckList()
        {
            InitializeComponent();
            LoadTruckList();
            timer1.Start();
        }

        private void LoadTruckList()
        {
            ServiceReference1.TowTruckServiceClient myService = new ServiceReference1.TowTruckServiceClient();
            myService.Endpoint.Address = new EndpointAddress(new Uri("http://" + ServiceRef.ServiceAddress));
            List<ServiceReference1.TowTruckData> theseTrucks = new List<ServiceReference1.TowTruckData>();
            theseTrucks = myService.CurrentTrucks().ToList<ServiceReference1.TowTruckData>();
            theseTrucks = theseTrucks.OrderBy(c => c.TruckNumber).ToList<ServiceReference1.TowTruckData>();
            gvTruckList.DataSource = theseTrucks;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ServiceReference1.TowTruckServiceClient myService = new ServiceReference1.TowTruckServiceClient();
            myService.Endpoint.Address = new EndpointAddress(new Uri("http://" + ServiceRef.ServiceAddress));
            List<ServiceReference1.TowTruckData> theseTrucks = new List<ServiceReference1.TowTruckData>();
            theseTrucks = myService.CurrentTrucks().ToList<ServiceReference1.TowTruckData>();
            theseTrucks = theseTrucks.OrderBy(c => c.TruckNumber).ToList<ServiceReference1.TowTruckData>();
            gvTruckList.DataSource = theseTrucks;
        }
    }
}
