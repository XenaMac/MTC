using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace OnlineSurveys2.Admin
{
    public partial class SurveyAnalytics : System.Web.UI.Page
    {
        Guid SurveyID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserOK"] == null || Session["UserOK"].ToString() != "OK")
            {
                Response.Redirect("Logon.aspx");
            }

            if (Request.QueryString["SurveyID"] != null)
            {
                SurveyID = new Guid(Request.QueryString["SurveyID"].ToString());
                //lblSurveyID.Text = SurveyID.ToString();
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
            classes.SQLCode mySQL = new classes.SQLCode();
            List<string> surveyData = mySQL.GetSurveyName(SurveyID);
            lblSurveyName.Text = surveyData[0].ToString();
            LoadDataGrid();
        }

        private void LoadDataGrid()
        {
            classes.SQLCode mySQL = new classes.SQLCode();
            List<classes.PostedSurvey> thisSurveyData = mySQL.GetPostedSurveyData(SurveyID);
            //thisSurveyData = thisSurveyData.OrderBy(a => a.QuestionNumber).ToList();
            gvData.DataSource = thisSurveyData;
            gvData.DataBind();
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportExcel();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //used to render excel from gridview
        }

        public void ExportExcel()
        {
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + lblSurveyName.Text + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gvData.AllowPaging = false;
            //gvData.HeaderRow.Style.Add("background-color", "#FFFFFF");
            gvData.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
    }
}