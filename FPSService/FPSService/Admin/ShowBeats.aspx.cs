using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPSService.Admin
{
    public partial class ShowBeats : System.Web.UI.Page
    {
        public List<shBeat> AllBeats = new List<shBeat>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Logon"] == null)
            {
                Response.Redirect("Logon.aspx");
            }
            string logon = Session["Logon"].ToString();
            if (logon != "true")
            {
                Response.Redirect("Logon.aspx");
            }
            LoadBeats();
            DisplayBeats();
        }

        private void LoadBeats()
        {
            foreach (BeatData.Beat thisBeat in BeatData.Beats.AllBeats)
            {
                shBeat thisShBeat = new shBeat();
                thisShBeat.FreewayID = thisBeat.FreewayID;
                thisShBeat.BeatDescription = thisBeat.BeatDescription;
                thisShBeat.Polygon = thisBeat.BeatExtent.ToString();
                AllBeats.Add(thisShBeat);
            }
        }

        private void DisplayBeats()
        {
            gvBeats.DataSource = AllBeats;
            gvBeats.DataBind();
        }
    }

    public class shBeat
    {
        public int FreewayID { get; set; }
        public string BeatDescription { get; set; }
        public string Polygon { get; set; }
    }

}