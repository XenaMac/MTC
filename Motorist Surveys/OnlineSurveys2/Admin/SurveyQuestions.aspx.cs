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
    public partial class SurveyQuestions : System.Web.UI.Page
    {
        List<classes.Question> theseQuestions = new List<classes.Question>();
        List<classes.Question> surveyQuestions = new List<classes.Question>();
        List<classes.Survey> theseSurveys = new List<classes.Survey>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserOK"] == null || Session["UserOK"].ToString() != "OK")
            {
                Response.Redirect("Logon.aspx");
            }

            if (!Page.IsPostBack)
            {
                LoadSurveys();
                LoadQuestions();
            }

        }

        private void LoadSurveys()
        {
            classes.SQLCode mySQL = new classes.SQLCode();
            theseSurveys = mySQL.GetAllSurveys();
            foreach (classes.Survey thisSurvey in theseSurveys)
            {
                ListItem li = new ListItem();
                li.Text = thisSurvey.SurveyName;
                li.Value = thisSurvey.SurveyID.ToString();
                ddlSurveys.Items.Add(li);
            }
        }

        private void LoadQuestions()
        {
            classes.SQLCode mySQL = new classes.SQLCode();
            theseQuestions = mySQL.GetAllQuestions();
            SerializeQuestions(theseQuestions);
            ddlQuestions.Items.Clear();
            ListItem li = new ListItem();
            ddlQuestions.Items.Add(li);
            foreach (classes.Question thisQuestion in theseQuestions)
            {
                li = new ListItem();
                li.Text = thisQuestion.QuestionText;
                li.Value = thisQuestion.QuestionID.ToString();
                ddlQuestions.Items.Add(li);
            }
        }

        private void LoadSurveyQuestions(Guid SurveyID)
        {
            classes.SQLCode mySQL = new classes.SQLCode();
            List<classes.Question> surveyQuestions = mySQL.GetQuestionsBySurvey(SurveyID);
            surveyQuestions = surveyQuestions.OrderBy(a => a.QuestionNumber).ToList();
            SerializeSurveyQuestions(surveyQuestions);
            
            lbCurrentQuestions.DataSource = surveyQuestions;
            lbCurrentQuestions.DataTextField = "QuestionText";
            lbCurrentQuestions.DataValueField = "QuestionID";
            lbCurrentQuestions.DataBind();
        }

        #region " Serialization Routines "

        private List<classes.Question> DeserialzeQuestions()
        {
            XmlSerializer ser = new XmlSerializer(theseQuestions.GetType());
            if (!string.IsNullOrEmpty(questionList.Value))
            {
                using (StringReader reader = new StringReader(questionList.Value))
                {
                    theseQuestions = (List<classes.Question>)ser.Deserialize(reader);
                }
            }
            return theseQuestions;
        }

        private List<classes.Question> DeserialzeSurveyQuestions()
        {
            XmlSerializer ser = new XmlSerializer(surveyQuestions.GetType());
            if (!string.IsNullOrEmpty(questionList.Value))
            {
                using (StringReader reader = new StringReader(surveyQuestionsList.Value))
                {
                    surveyQuestions = (List<classes.Question>)ser.Deserialize(reader);
                }
            }
            return surveyQuestions;
        }

        private void SerializeQuestions(List<classes.Question> questions)
        {
            XmlSerializer ser = new XmlSerializer(theseQuestions.GetType());
            using (StringWriter writer = new StringWriter())
            {
                ser.Serialize(writer, questions);
                questionList.Value = writer.ToString();
            }
        }

        private void SerializeSurveyQuestions(List<classes.Question> questions)
        {
            XmlSerializer ser = new XmlSerializer(surveyQuestions.GetType());
            using (StringWriter writer = new StringWriter())
            {
                ser.Serialize(writer, questions);
                surveyQuestionsList.Value = writer.ToString();
            }
        }

        #endregion

        protected void btnLoadSurvey_Click(object sender, EventArgs e)
        {
            ListItem li = ddlSurveys.SelectedItem;
            Guid SurveyID = new Guid(li.Value.ToString());
            LoadSurveyQuestions(SurveyID);
        }

        protected void btnPostSurveyDesign_Click(object sender, EventArgs e)
        {
            ListItem li = ddlSurveys.SelectedItem;
            Guid SurveyID = new Guid(li.Value.ToString());
            classes.SQLCode mySQL = new classes.SQLCode();
            surveyQuestions = DeserialzeSurveyQuestions();
            mySQL.PostSurveyDesign(SurveyID, surveyQuestions);
            surveyQuestions.Clear();
            lbCurrentQuestions.DataSource = surveyQuestions;
            lbCurrentQuestions.DataBind();
            Response.Write("Survey design posted");
        }

        protected void btnAddQuestion_Click(object sender, EventArgs e)
        {
            ListItem li = ddlQuestions.SelectedItem;
            Guid QuestionID = new Guid(li.Value.ToString());
            theseQuestions = DeserialzeQuestions();
            surveyQuestions = DeserialzeSurveyQuestions();
            classes.Question foundQuestion = surveyQuestions.Find(delegate(classes.Question thisQuestion) { return thisQuestion.QuestionID == QuestionID; });
            if (foundQuestion != null)
            {
                Response.Write("Question already associated with this survey");
                return;
            }
            else
            {
                classes.Question selQuestion = theseQuestions.Find(delegate(classes.Question thisQuestion) { return thisQuestion.QuestionID == QuestionID; });
                if (selQuestion != null)
                {
                    int numVal;
                    bool result = Int32.TryParse(txtQuestionOrder.Text, out numVal);
                    if (!result)
                    {
                        txtQuestionOrder.BackColor = System.Drawing.Color.Red;
                        txtQuestionOrder.Text = "Please enter a valid number";
                        return;
                    }
                    else
                    {
                        txtQuestionOrder.BackColor = System.Drawing.Color.White;
                    }
                    selQuestion.QuestionNumber = numVal;
                    selQuestion.QuestionStatus = "KEEP";
                    surveyQuestions.Add(selQuestion);
                }
                else
                {
                    Response.Write("Couldn't find question");
                }
            }
            lbCurrentQuestions.DataSource = surveyQuestions;
            lbCurrentQuestions.DataTextField = "QuestionText";
            lbCurrentQuestions.DataValueField = "QuestionID";
            lbCurrentQuestions.DataBind();
            SerializeSurveyQuestions(surveyQuestions);
        }

        protected void btnRemoveQuestion_Click(object sender, EventArgs e)
        {
            surveyQuestions = DeserialzeSurveyQuestions();
            classes.Question foundQuestion = surveyQuestions.Find(delegate(classes.Question thisQuestion) { return thisQuestion.QuestionID == new Guid(lbCurrentQuestions.SelectedValue.ToString()); });
            if (foundQuestion == null)
            {
                Response.Write("Couldn't find question");
                return;
            }
            else
            {
                foundQuestion.QuestionStatus = "REMOVE";
                foundQuestion.QuestionText += " REMOVED";
            }
            lbCurrentQuestions.DataSource = surveyQuestions;
            lbCurrentQuestions.DataTextField = "QuestionText";
            lbCurrentQuestions.DataValueField = "QuestionID";
            lbCurrentQuestions.DataBind();
            SerializeSurveyQuestions(surveyQuestions);
        }

        protected void lbCurrentQuestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            surveyQuestions = DeserialzeSurveyQuestions();
            classes.Question foundQuestion = surveyQuestions.Find(delegate(classes.Question thisQuestion) { return thisQuestion.QuestionID == new Guid(lbCurrentQuestions.SelectedValue.ToString()); });
            if (foundQuestion != null)
            {
                txtQuestionOrder.Text = foundQuestion.QuestionNumber.ToString();
            }
        }

        protected void btnUpdateSelected_Click(object sender, EventArgs e)
        {
            surveyQuestions = DeserialzeSurveyQuestions();
            classes.Question foundQuestion = surveyQuestions.Find(delegate(classes.Question thisQuestion) { return thisQuestion.QuestionID == new Guid(lbCurrentQuestions.SelectedValue.ToString()); });
            if (foundQuestion != null)
            {
                foundQuestion.QuestionNumber = Convert.ToInt32(txtQuestionOrder.Text);
            }
            lbCurrentQuestions.DataSource = surveyQuestions;
            lbCurrentQuestions.DataTextField = "QuestionText";
            lbCurrentQuestions.DataValueField = "QuestionID";
            lbCurrentQuestions.DataBind();
            SerializeSurveyQuestions(surveyQuestions);
        }
    }
}