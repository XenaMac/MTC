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
    public partial class Alarms : Form
    {
        List<srTTSrv.AlarmStatus> alarms = new List<srTTSrv.AlarmStatus>();
        srTTSrv.TowTruckServiceClient srv;
        public Alarms()
        {
            InitializeComponent();
            srv = new srTTSrv.TowTruckServiceClient();
            srv.Endpoint.Address = new EndpointAddress(new Uri("http://" + ServiceRef.ServiceAddress));
            LoadAllAlarms();
        }

        private void LoadAllAlarms()
        {
            alarms = srv.GetAllAlarms().ToList<srTTSrv.AlarmStatus>();
            dataGridView1.DataSource = alarms;
        }
    }
}
