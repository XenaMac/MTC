using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPSService.WebSite
{
    public partial class CurrentConnects : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetCurrentConnects();
        }

        private void GetCurrentConnects()
        {
            List<TowTruck.GPS> myGPS = new List<TowTruck.GPS>();
            List<TowTruck.TowTruckExtended> myExtended = new List<TowTruck.TowTruckExtended>();
            foreach (TowTruck.TowTruck thisTruck in DataClasses.GlobalData.currentTrucks)
            {
                myGPS.Add(thisTruck.GPSPosition);
                myExtended.Add(thisTruck.Extended);
            }
            if (myGPS != null)
            {
                GridView1.DataSource = myGPS;
                GridView1.DataBind();
            }
            if (myExtended != null)
            {
                gvExtended.DataSource = myExtended;
                gvExtended.DataBind();
            }
        }
    }
}