using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using System.IO;

namespace OnlineSurveys2.Admin
{
    public partial class SurveyEditor : System.Web.UI.Page
    {
        List<classes.Survey> theseSurveys = new List<classes.Survey>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserOK"] == null || Session["UserOK"].ToString() != "OK")
            {
                Response.Redirect("Logon.aspx");
            }

            if (!Page.IsPostBack)
            {
                LoadSurveyList();
            }
        }

        private void LoadSurveyList()
        {
            ddlSurveys.Items.Clear();
            
            classes.SQLCode thisSQL = new classes.SQLCode();
            theseSurveys = thisSQL.GetAllSurveys();
            //serialize theseSurveys to string so we can stuff them in a hidden control
            XmlSerializer ser = new XmlSerializer(theseSurveys.GetType());
            using (StringWriter writer = new StringWriter())
            {
                ser.Serialize(writer, theseSurveys);
                surveyList.Value = writer.ToString();
            }
            ListItem li = new ListItem();
            li.Text = "New Survey";
            li.Value = new Guid("00000000-0000-0000-0000-000000000000").ToString();
            ddlSurveys.Items.Add(li);
            foreach (classes.Survey thisSurveyListItem in theseSurveys)
            {
                li = new ListItem();
                li.Text = thisSurveyListItem.SurveyName;
                li.Value = thisSurveyListItem.SurveyID.ToString();
                ddlSurveys.Items.Add(li);
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            //classes.SQLCode thisSQL = new classes.SQLCode();
            XmlSerializer ser = new XmlSerializer(theseSurveys.GetType());
            using (StringReader reader = new StringReader(surveyList.Value))
            {
                theseSurveys = (List<classes.Survey>)ser.Deserialize(reader);
            }
            if (ddlSurveys.Text != "New Survey")
            {
                classes.Survey thisSurvey = theseSurveys.Find(delegate(classes.Survey mySurvey) { return mySurvey.SurveyID == new Guid(ddlSurveys.SelectedValue); });
                /*
                classes.SQLCode thisSQL = new classes.SQLCode();
                classes.Survey thisSurvey = thisSQL.GetSurvey(new Guid(ddlSurveys.SelectedValue));
                 *                  * */
                if (thisSurvey != null)
                {
                    txtSurveyID.Text = thisSurvey.SurveyID.ToString();
                    txtSurveyName.Text = thisSurvey.SurveyName;
                    txtCreatedBy.Text = thisSurvey.CreatedBy;
                    txtDateCreated.Text = thisSurvey.DateCreated.ToString();
                    txtDateModified.Text = thisSurvey.DateModified.ToString();
                    txtModifiedBy.Text = thisSurvey.ModifiedBy;
                    txtSurveyNotes.Text = thisSurvey.SurveyNotes;
                    txtSurveyBoilerplate.Text = thisSurvey.SurveyBoilerplate;
                }

            }
            else
            {
                txtSurveyID.Text = "";
                txtSurveyName.Text = "";
                txtCreatedBy.Text = "";
                txtDateCreated.Text = "";
                txtDateModified.Text = "";
                txtModifiedBy.Text = "";
                txtSurveyNotes.Text = "";
                txtSurveyBoilerplate.Text = "";
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            classes.Survey thisSurvey = new classes.Survey();
            if (string.IsNullOrEmpty(txtSurveyID.Text) || ddlSurveys.Text == "New Survey" || txtSurveyID.Text == "00000000-0000-0000-0000-000000000000")
            {
                thisSurvey.SurveyID = Guid.NewGuid();
            }
            else
            {
                thisSurvey.SurveyID = new Guid(txtSurveyID.Text);
            }
            thisSurvey.SurveyName = txtSurveyName.Text;
            thisSurvey.CreatedBy = txtCreatedBy.Text;
            thisSurvey.DateCreated = Convert.ToDateTime(txtDateCreated.Text);
            string ModifiedBy;
            DateTime dtModified;
            if (!string.IsNullOrEmpty(txtModifiedBy.Text))
            {
                ModifiedBy = txtModifiedBy.Text;
            }
            else
            {
                ModifiedBy = txtCreatedBy.Text;
            }
            if (!string.IsNullOrEmpty(txtDateModified.Text))
            {
                dtModified = Convert.ToDateTime(txtDateModified.Text);
            }
            else
            {
                dtModified = Convert.ToDateTime(txtDateCreated.Text);
            }
            thisSurvey.ModifiedBy = ModifiedBy;
            thisSurvey.DateModified = dtModified;
            thisSurvey.SurveyNotes = txtSurveyNotes.Text;
            thisSurvey.SurveyBoilerplate = txtSurveyBoilerplate.Text;
            classes.SQLCode thisSQL = new classes.SQLCode();
            thisSQL.PostSurvey(thisSurvey);
            LoadSurveyList();
            Response.Write("Survey data persisted");
        }
    }
}