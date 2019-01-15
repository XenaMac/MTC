using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPSService.Admin
{
    public partial class TestUtils : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadTrucks();
        }

        private void LoadTrucks()
        {
            ddlTrucks.Items.Clear();
            foreach (TowTruck.TowTruck t in DataClasses.GlobalData.currentTrucks)
            {
                ddlTrucks.Items.Add(t.TruckNumber);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            foreach (TowTruck.TowTruck t in DataClasses.GlobalData.currentTrucks)
            {
                if (t.TruckNumber == ddlTrucks.Text)
                {
                    t.setStatus("FORCED BREAK", true);
                }
            }
        }
    }
}