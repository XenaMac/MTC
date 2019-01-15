using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace OnlineSurveys2.Admin
{
    public partial class LinearAnalytics : System.Web.UI.Page
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
            }
        }

        private void LoadSurveys()
        {
            ddlSurveys.Items.Clear();
            classes.SQLCode mySQL = new classes.SQLCode();
            List<classes.Survey> allSurveys = mySQL.GetAllSurveys();
            allSurveys = allSurveys.OrderBy(s => s.SurveyName).ToList<classes.Survey>();
            foreach (classes.Survey s in allSurveys)
            {
                ListItem li = new ListItem();
                li.Text = s.SurveyName;
                li.Value = s.SurveyID.ToString();
                ddlSurveys.Items.Add(li);
            }
        }

        protected void btnLoadurveyData_Click(object sender, EventArgs e)
        {
            ListItem li = ddlSurveys.SelectedItem;
            Guid surveyID = new Guid(li.Value.ToString());
            classes.SQLCode mySQL = new classes.SQLCode();
            DataTable dt = mySQL.GetLegend(surveyID);
            gvLegend.DataSource = dt;
            gvLegend.DataBind();
            DataTable dtData = mySQL.GetAnalytics(surveyID);
            gvData.DataSource = dtData;
            gvData.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void btnExportLegend_Click(object sender, EventArgs e)
        {
            ListItem li = ddlSurveys.SelectedItem;
            string surveyName = li.Text;
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + surveyName + "Legend.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gvLegend.AllowPaging = false;
            //gvLegend.HeaderRow.Style.Add("background-color", "#FFFFFF");
            gvLegend.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }

        protected void btnExportData_Click(object sender, EventArgs e)
        {
            ListItem li = ddlSurveys.SelectedItem;
            string surveyName = li.Text;
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + surveyName + "Data.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gvData.AllowPaging = false;
            //gvLegend.HeaderRow.Style.Add("background-color", "#FFFFFF");
            gvData.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
    }
}