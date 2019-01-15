using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPSService.Admin
{
    public partial class TruckMessages : System.Web.UI.Page
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
            if (!Page.IsPostBack)
            {
                LoadGrid();
            }
        }

        private void LoadGrid()
        {
            gvMessages.DataSource = null;
            List<TruckMessage> theseMessages = DataClasses.GlobalData.theseMessages;
            gvMessages.DataSource = theseMessages;
            gvMessages.DataBind();
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void lbtnLogOff_Click(object sender, EventArgs e)
        {
            Session["Logon"] = null;
            Response.Redirect("Logon.aspx");
        }
    }
}