using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPSService.Admin
{
    public partial class Logon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text.ToUpper() != "MTCADMIN" || txtPassword.Text != "MTC2014")
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "Javascript", "<script>alert('Incorrect username or password')</script>");
            }
            else
            {
                Session["Logon"] = "true";
                Response.Redirect("Dashboard.aspx");
            }
        }
    }
}