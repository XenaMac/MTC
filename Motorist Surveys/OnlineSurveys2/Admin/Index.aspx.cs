using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineSurveys2.Admin
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserOK"] == null || Session["UserOK"].ToString() != "OK")
            {
                Response.Redirect("Logon.aspx");
            }
            if (!Page.IsPostBack)
            {
                LoadSurveys();
                LoadSurveyCounts();
            }
            gvSurveyList.RowCommand += gvSurveyList_RowCommand;
        }

        void gvSurveyList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow gvr = gvSurveyList.Rows[index];
            string SurveyID = gvr.Cells[2].Text;
            Response.Redirect("SurveyExporter.aspx?SurveyID=" + SurveyID);
        }

        private void LoadSurveys()
        {
            classes.SQLCode mySQL = new classes.SQLCode();
            List<classes.Survey> theseSurveys = mySQL.GetAllSurveys();
            foreach (classes.Survey thisSurvey in theseSurveys)
            {
                ListItem li = new ListItem();
                li.Text = thisSurvey.SurveyName;
                li.Value = thisSurvey.SurveyID.ToString();
                ddlSurveys.Items.Add(li);
            }
        }

        private void LoadSurveyCounts()
        {
            classes.SQLCode mySQL = new classes.SQLCode();
            List<classes.SurveyCounts> theseCounts = mySQL.GetSurveyCounts();
            gvSurveyList.DataSource = theseCounts;
            gvSurveyList.AutoGenerateColumns = true;
            gvSurveyList.DataBind();
        }

        protected void btnTestSurvey_Click(object sender, EventArgs e)
        {
            ListItem li = ddlSurveys.SelectedItem;
            Response.Redirect("TestSurvey.aspx?SurveyID=" + li.Value.ToString());
        }

        protected void AnalyzeSurvey(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow gvr = gvSurveyList.Rows[index];
            string SurveyID = gvr.Cells[2].Text;
            Response.Redirect("SurveyExporter.aspx?SurveyID=" + SurveyID);
        }
    }
}