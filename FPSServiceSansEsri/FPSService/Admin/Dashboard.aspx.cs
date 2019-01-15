using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace FPSService.Admin
{
    public partial class Dashboard : System.Web.UI.Page
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
            lblLoggedOnTo.Text = "Currently logged on to: " + System.Environment.MachineName;
            checkConnectStatus();
        }

        private void checkConnectStatus()
        {
            bool listenStatus = Global.cListener.isConnected();
            bool sendStatus = Global.cSender.ConnectStatus();
            string lastHB = Global.cListener.lastHB;
            if (listenStatus == true)
            {
                lblReceieveStatus.Text = "LISTENING";
                lblReceieveStatus.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblReceieveStatus.Text = "NOT LISTENING";
                lblReceieveStatus.ForeColor = System.Drawing.Color.Red;
            }
            if (sendStatus == true)
            {
                lblSendStatus.Text = "SENDING";
                lblSendStatus.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblSendStatus.Text = "NOT SENDING";
                lblSendStatus.ForeColor = System.Drawing.Color.Red;
            }
            lblHeartBeat.Text = lastHB;
        }

        protected void btnSetVar_Click(object sender, EventArgs e)
        {
            /*
            SQL.SQLCode mySQL = new SQL.SQLCode();
            mySQL.SetVarValue(txtVarName.Text, txtVarValue.Text);
            if (txtVarName.Text == "Speeding")
            {
                DataClasses.GlobalData.SpeedingValue = Convert.ToInt32(txtVarValue.Text);
            }
            if (txtVarName.Text.Contains("Leeway"))
            {
                mySQL.LoadLeeways();
            }
             * */
        }

        protected void btnReloadBeats_Click(object sender, EventArgs e)
        {
            try
            {
                //BeatData.Beats.LoadBeats();
                BeatData.Beats.LoadBeatInfo();
                BeatData.BeatSegments.LoadSegments();
                ClientScript.RegisterClientScriptBlock(GetType(), "Javascript", "<script>alert('All beat data reloaded')</script>");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "Javascript", "<script>alert('" + ex.Message + "')</script>");
            }
        }

        protected void lbtnLogOff_Click(object sender, EventArgs e)
        {
            Session["Logon"] = null;
            Response.Redirect("Logon.aspx");
        }

        protected void tmrCheckCADConnect_Tick(object sender, EventArgs e)
        {
            checkConnectStatus();
        }

    }
}