using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.Configuration;

namespace OctaHarness
{
    public partial class WAZEMessage : Form
    {
        public WAZEMessage()
        {
            InitializeComponent();
            txtUUID.Text = Guid.NewGuid().ToString();
            //35.096695, -106.566354
            //string uuid, string title, double lat, double lon, int nThumbsUp, int confidence, int reliability, string street
        }

        private void btnSendWAZE_Click(object sender, EventArgs e)
        {
            ServiceRef.ServiceAddress = ddlServiceRef.Text;
            ServiceReference1.TowTruckServiceClient myService = new ServiceReference1.TowTruckServiceClient();
            myService.Endpoint.Address = new EndpointAddress(new Uri("http://" + ServiceRef.ServiceAddress));
            myService.addWAZE(txtUUID.Text, txtAlert.Text, Convert.ToDouble(txtLat.Text), Convert.ToDouble(txtLon.Text), Convert.ToInt32(txtNThumbsUp.Text),
                Convert.ToInt32(txtConfidence.Text), Convert.ToInt32(txtReliability.Text), txtStreet.Text);
        }

    }
}
