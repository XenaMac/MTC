using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPSService.Admin
{
    public partial class CurrentBeatSchedules : System.Web.UI.Page
    {
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
            LoadCurrentSchedules();
        }

        private void LoadCurrentSchedules()
        {
            List<MiscData.BeatSchedule> theseBeatSchedules = DataClasses.GlobalData.theseSchedules;
            theseBeatSchedules = theseBeatSchedules.OrderBy(w=>w.Weekday).ThenBy(b => b.start).ThenBy(s=>s.BeatNumber).ToList<MiscData.BeatSchedule>();
            gvSchedules.DataSource = theseBeatSchedules;
            gvSchedules.DataBind();
            gvSchedules.Columns[0].Visible = false;
            gvSchedules.Columns[1].Visible = false;
        }

        protected void btnReloadSchedules_Click(object sender, EventArgs e)
        {
            SQL.SQLCode mySQL = new SQL.SQLCode();
            mySQL.LoadBeatSchedules();
            LoadCurrentSchedules();
        }

        protected void lbtnLogOff_Click(object sender, EventArgs e)
        {
            Session["Logon"] = null;
            Response.Redirect("Logon.aspx");
        }
    }
}