using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPSService.Admin
{
    public partial class SetLeeways : System.Web.UI.Page
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
                GetVars();
            }
        }

        private void GetVars()
        {
            txtExtended.Text = DataClasses.GlobalData.ExtendedLeeway.ToString();
            txtLogOff.Text = DataClasses.GlobalData.LogOffLeeway.ToString();
            txtLogOn.Text = DataClasses.GlobalData.LogOnLeeway.ToString();
            txtGPSIssue.Text = DataClasses.GlobalData.GPSIssueLeeway.ToString();
            txtOffBeat.Text = DataClasses.GlobalData.OffBeatLeeway.ToString();
            txtOnPatrol.Text = DataClasses.GlobalData.OnPatrollLeeway.ToString();
            txtRollIn.Text = DataClasses.GlobalData.RollInLeeway.ToString();
            txtRollOut.Text = DataClasses.GlobalData.RollOutLeeway.ToString();
            txtSpeeding.Text = DataClasses.GlobalData.SpeedingLeeway.ToString();
            txtStationary.Text = DataClasses.GlobalData.StationaryLeeway.ToString();
            txtForceLogoff.Text = DataClasses.GlobalData.ForceOff.ToString();
        }

        protected void btnUpdateVars_Click(object sender, EventArgs e)
        {
            SQL.SQLCode mySQL = new SQL.SQLCode();
            mySQL.SetVarValue("ExtendedLeeway", txtExtended.Text);
            mySQL.SetVarValue("LogOffLeeway", txtLogOff.Text);
            mySQL.SetVarValue("LogOnLeeway", txtLogOn.Text);
            mySQL.SetVarValue("GPSIssueLeeway", txtGPSIssue.Text);
            mySQL.SetVarValue("OffBeatLeeway", txtOffBeat.Text);
            mySQL.SetVarValue("OnPatrolLeeway", txtOnPatrol.Text);
            mySQL.SetVarValue("RollInLeeway", txtRollIn.Text);
            mySQL.SetVarValue("RollOutLeeway", txtRollOut.Text);
            mySQL.SetVarValue("SpeedingLeeway", txtSpeeding.Text);
            mySQL.SetVarValue("StationaryLeeway", txtStationary.Text);
            mySQL.SetVarValue("ForcedOffLeeway", txtForceLogoff.Text);
            mySQL.LoadLeeways();
            mySQL = null;
            GetVars();
        }

        protected void lbtnLogOff_Click(object sender, EventArgs e)
        {
            Session["Logon"] = null;
            Response.Redirect("Logon.aspx");
        }


    }
}