using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPSService.Admin
{
    public partial class ShowDrops : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                List<dropdata> drops = new List<dropdata>();
                foreach (BeatData.esriDropSite ds in BeatData.EsriDropSites.dropPolygons)
                {
                    dropdata dd = new dropdata();
                    dd.dsName = ds.Name;
                    dd.beatID = ds.BeatID;
                    drops.Add(dd);
                }
                GridView1.DataSource = drops;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }
    }

    public class dropdata
    {
        public string dsName { get; set; }
        public string beatID { get; set; }
    }
}