using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using System.IO;

namespace OnlineSurveys2
{
    public partial class ClientSurvey : System.Web.UI.Page
    {
        List<classes.Question> theseQuestions = new List<classes.Question>();
        List<classes.QuestionType> theseQuestionTypes = new List<classes.QuestionType>();
        List<classes.Answer> theseAnswers = new List<classes.Answer>();
        Guid SurveyID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["SurveyID"] != null)
            {
                SurveyID = new Guid(Request.QueryString["SurveyID"].ToString());
            }
            else
            {
                Response.Redirect("Survey.aspx");
            }
            //if (!Page.IsPostBack)
            //{
            LoadQuestionTypes();
            LoadQuestions(SurveyID);
            classes.SQLCode mySQL = new classes.SQLCode();
            classes.Survey mySurvey = mySQL.GetSurvey(SurveyID);
            lblSurveyName.Text = mySurvey.SurveyName;
            lblSurveyName.CssClass = "surveyName";
            pnlBoilerplate.Controls.Add(new LiteralControl("<label>" + mySurvey.SurveyBoilerplate + "</label>"));
            LayoutPage();
            //}
        }

        private void LayoutPage()
        {
            theseQuestions = DeserialzeQuestions();
            theseQuestionTypes = DeserialzeQuestionTypes();
            theseAnswers = DeserializeAnswers();

            foreach (classes.Question thisQuestion in theseQuestions)
            {
                classes.QuestionType foundType = theseQuestionTypes.Find(delegate(classes.QuestionType myType) { return myType.QuestionTypeID == thisQuestion.QuestionTypeID; });
                if (foundType == null)
                {
                    Response.Write("No type, ending");
                    return;
                }
                pnlQuestions.Controls.Add(new LiteralControl("<div id=\"q" + thisQuestion.QuestionNumber + "\"><div class=\"qText\"><label class=\"qNum\">" + thisQuestion.QuestionNumber.ToString() + "</label>&nbsp" +
                    "<label class=\"questionTextLabel\">" + thisQuestion.QuestionText +
                    "</label></div>"));
                string qType = foundType.QuestionTypeName;

                if (qType == "Calendar")
                {
                    classes.Answer foundAnswer = theseAnswers.Find(delegate(classes.Answer myAnswer) { return myAnswer.QuestionID == thisQuestion.QuestionID; });
                    if (foundAnswer == null)
                    {
                        Response.Write("Couldn't find answer, ending");
                        return;
                    }
                    Calendar c = new Calendar();
                    c.SelectedDate = DateTime.Now;
                    c.ID = foundAnswer.AnswerID.ToString();
                    pnlQuestions.Controls.Add(new LiteralControl("<div class=\"outer\">"));
                    pnlQuestions.Controls.Add(new LiteralControl("<div class=\"inner\">"));
                    pnlQuestions.Controls.Add(c);
                    pnlQuestions.Controls.Add(new LiteralControl("</div>"));
                    pnlQuestions.Controls.Add(new LiteralControl("</div>"));
                }

                if (qType == "Free Text")
                {
                    classes.Answer foundAnswer = theseAnswers.Find(delegate(classes.Answer myAnswer) { return myAnswer.QuestionID == thisQuestion.QuestionID; });
                    if (foundAnswer == null)
                    {
                        Response.Write("Couldn't find answer, ending");
                        return;
                    }
                    TextBox qBox = new TextBox();
                    qBox.TextMode = TextBoxMode.MultiLine;
                    qBox.Rows = 4;
                    qBox.CssClass = "surveyTextBox";
                    qBox.ID = foundAnswer.AnswerID.ToString();
                    pnlQuestions.Controls.Add(new LiteralControl("<div class=\"outer\">"));
                    pnlQuestions.Controls.Add(new LiteralControl("<div class=\"inner\">"));
                    pnlQuestions.Controls.Add(qBox);
                    pnlQuestions.Controls.Add(new LiteralControl("</div>"));
                    pnlQuestions.Controls.Add(new LiteralControl("</div>"));
                }
                if (qType == "Multi Select")
                {
                    List<classes.Answer> qAnswers = new List<classes.Answer>();
                    foreach (classes.Answer thisAnswer in theseAnswers)
                    {
                        if (thisAnswer.QuestionID == thisQuestion.QuestionID)
                        {
                            qAnswers.Add(thisAnswer);
                        }
                    }
                    qAnswers = qAnswers.OrderBy(a => a.SortOrder).ToList();
                    pnlQuestions.Controls.Add(new LiteralControl("<div class=\"outer\">"));
                    pnlQuestions.Controls.Add(new LiteralControl("<div class=\"inner\">"));
                    //CheckBoxList cbl = new CheckBoxList();
                    ListBox cbl = new ListBox();
                    cbl.SelectionMode = ListSelectionMode.Multiple;
                    cbl.ID = thisQuestion.QuestionID.ToString();
                    pnlQuestions.Controls.Add(new LiteralControl("<div class=\"scrollList\">"));
                    pnlQuestions.Controls.Add(new LiteralControl("<div><label class=\"italicLabel\">Select All That Apply (hold Ctrl to multi-select)</label></div>"));
                    foreach (classes.Answer thisAnswer in qAnswers)
                    {
                        ListItem li = new ListItem();
                        li.Text = thisAnswer.AnswerText;
                        li.Value = thisAnswer.AnswerID.ToString();
                        cbl.Items.Add(li);
                        /*
                        CheckBox qCB = new CheckBox();
                        qCB.ID = thisAnswer.AnswerID.ToString();
                        qCB.Text = thisAnswer.AnswerText;
                        pnlQuestions.Controls.Add(qCB);
                         * */
                    }
                    pnlQuestions.Controls.Add(cbl);
                    pnlQuestions.Controls.Add(new LiteralControl("</div>")); //End scrollList
                    pnlQuestions.Controls.Add(new LiteralControl("</div>")); //End inner
                    pnlQuestions.Controls.Add(new LiteralControl("</div>")); //End Outer
                }
                if (qType == "Single Select")
                {
                    List<classes.Answer> qAnswers = new List<classes.Answer>();
                    foreach (classes.Answer thisAnswer in theseAnswers)
                    {
                        if (thisAnswer.QuestionID == thisQuestion.QuestionID)
                        {
                            qAnswers.Add(thisAnswer);
                        }
                    }
                    qAnswers = qAnswers.OrderBy(a => a.SortOrder).ToList();
                    pnlQuestions.Controls.Add(new LiteralControl("<div class=\"outer\">"));
                    pnlQuestions.Controls.Add(new LiteralControl("<div class=\"inner\">"));
                    //RadioButtonList rbl = new RadioButtonList();
                    DropDownList rbl = new DropDownList();
                    rbl.ID = thisQuestion.QuestionID.ToString();
                    pnlQuestions.Controls.Add(new LiteralControl("<div class=\"scrollList\">"));
                    pnlQuestions.Controls.Add(new LiteralControl("<div><label class=\"italicLabel\">Select One</label></div>"));
                    foreach (classes.Answer thisAnswer in qAnswers)
                    {
                        ListItem li = new ListItem();
                        li.Value = thisAnswer.AnswerID.ToString();
                        li.Text = thisAnswer.AnswerText;
                        rbl.Items.Add(li);
                        /*
                        RadioButton rb = new RadioButton();
                        rb.ID = thisAnswer.AnswerID.ToString();
                        rb.Text = thisAnswer.AnswerText;
                        rb.GroupName = thisQuestion.QuestionID.ToString();
                        pnlQuestions.Controls.Add(rb);
                         * */
                    }
                    pnlQuestions.Controls.Add(rbl);
                    pnlQuestions.Controls.Add(new LiteralControl("</div>")); //End scrollList
                    pnlQuestions.Controls.Add(new LiteralControl("</div>")); //End inner
                    pnlQuestions.Controls.Add(new LiteralControl("</div>")); //End outer
                }
                pnlQuestions.Controls.Add(new LiteralControl("</div>"));
            }
        }

        private void LoadQuestionTypes()
        {
            classes.SQLCode mySQL = new classes.SQLCode();
            theseQuestionTypes = mySQL.GetQuestionTypes();
            SerializeQuestionTypes(theseQuestionTypes);
        }

        private void LoadQuestions(Guid SurveyID)
        {
            classes.SQLCode mySQL = new classes.SQLCode();
            theseQuestions = mySQL.GetQuestionsBySurvey(SurveyID);
            foreach (classes.Question thisQuestion in theseQuestions)
            {
                List<classes.Answer> qAnswers = mySQL.GetAnswers(thisQuestion.QuestionID);
                foreach (classes.Answer thisAnswer in qAnswers)
                {
                    theseAnswers.Add(thisAnswer);
                }
            }
            SerializeQuestions(theseQuestions);
            SerializeAnswers(theseAnswers);
        }

        #region " Serializers / Deserializers "

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

        private void SerializeQuestions(List<classes.Question> questions)
        {
            XmlSerializer ser = new XmlSerializer(theseQuestions.GetType());
            using (StringWriter writer = new StringWriter())
            {
                ser.Serialize(writer, questions);
                questionList.Value = writer.ToString();
            }
        }

        private List<classes.Answer> DeserializeAnswers()
        {
            XmlSerializer ser = new XmlSerializer(theseAnswers.GetType());
            if (!string.IsNullOrEmpty(answerList.Value))
            {
                using (StringReader reader = new StringReader(answerList.Value))
                {
                    theseAnswers = (List<classes.Answer>)ser.Deserialize(reader);
                }
            }
            return theseAnswers;
        }

        private void SerializeAnswers(List<classes.Answer> answers)
        {
            XmlSerializer ser = new XmlSerializer(theseAnswers.GetType());
            using (StringWriter writer = new StringWriter())
            {
                ser.Serialize(writer, answers);
                answerList.Value = writer.ToString();
            }
        }

        private void SerializeQuestionTypes(List<classes.QuestionType> questionsTypes)
        {
            XmlSerializer ser = new XmlSerializer(theseQuestionTypes.GetType());
            using (StringWriter writer = new StringWriter())
            {
                ser.Serialize(writer, questionsTypes);
                questionTypeList.Value = writer.ToString();
            }
        }
        private List<classes.QuestionType> DeserialzeQuestionTypes()
        {
            XmlSerializer ser = new XmlSerializer(theseQuestionTypes.GetType());
            if (!string.IsNullOrEmpty(questionTypeList.Value))
            {
                using (StringReader reader = new StringReader(questionTypeList.Value))
                {
                    theseQuestionTypes = (List<classes.QuestionType>)ser.Deserialize(reader);
                }
            }
            return theseQuestionTypes;
        }


        #endregion 

        protected void btnPost_Click(object sender, EventArgs e)
        {
            classes.SQLCode mySQL = new classes.SQLCode();
            //string answers = "";
            //Generate a PostedSurveyID, we'll use this in the database to identify all the answers associated with a discrete posted survey
            Guid PostedSurveyID = Guid.NewGuid();
            mySQL.PostSurvey(PostedSurveyID, SurveyID);
            foreach (Control c in pnlQuestions.Controls)
            {
                if (c is TextBox)
                {
                    //answers += "TextBox: " + c.ID + " : " + ((TextBox)c).Text + Environment.NewLine;
                    mySQL.PostAnswer(PostedSurveyID, new Guid(c.ID.ToString()), 3, ((TextBox)c).Text);
                }
                if (c is Calendar)
                {
                    mySQL.PostAnswer(PostedSurveyID, new Guid(c.ID.ToString()), 3, ((Calendar)c).SelectedDate.ToString());
                }
                if (c is RadioButtonList)
                {
                    RadioButtonList rbl = (RadioButtonList)c;
                    foreach (ListItem li in rbl.Items)
                    {
                        int checkedVal = 0;
                        if (li.Selected == true)
                        {
                            checkedVal = 1;
                        }
                        mySQL.PostAnswer(PostedSurveyID, new Guid(li.Value.ToString()), checkedVal, "NT");
                    }
                    /*
                    //answers += "RadioButton: " + c.ID + " : CHECKED: " + ((RadioButton)c).Checked + Environment.NewLine;
                    int checkedVal = 0;
                    if(((RadioButton)c).Checked == true)
                    {
                        checkedVal = 1;
                    }
                    mySQL.PostAnswer(PostedSurveyID, new Guid(c.ID.ToString()), checkedVal, "NT");
                     * */
                }
                if (c is DropDownList)
                {
                    DropDownList ddl = (DropDownList)c;
                    int i = ddl.SelectedIndex;
                    ListItem li = ddl.Items[i];
                    foreach (ListItem liLoop in ddl.Items)
                    {
                        if (liLoop == li)
                        {
                            mySQL.PostAnswer(PostedSurveyID, new Guid(liLoop.Value.ToString()), 1, "NT");
                        }
                        else
                        {
                            mySQL.PostAnswer(PostedSurveyID, new Guid(liLoop.Value.ToString()), 0, "NT");
                        }
                    }
                }
                if (c is ListBox)
                {
                    ListBox lb = (ListBox)c;
                    foreach (ListItem li in lb.Items)
                    {
                        int checkedVal = 0;
                        if (li.Selected == true)
                        {
                            checkedVal = 1;
                        }
                        mySQL.PostAnswer(PostedSurveyID, new Guid(li.Value.ToString()), checkedVal, "NT");
                    }
                }
                if (c is CheckBoxList)
                {
                    CheckBoxList cbl = (CheckBoxList)c;
                    foreach (ListItem li in cbl.Items)
                    {
                        int checkedVal = 0;
                        if (li.Selected == true)
                        {
                            checkedVal = 1;
                        }
                        mySQL.PostAnswer(PostedSurveyID, new Guid(li.Value.ToString()), checkedVal, "NT");
                    }
                    /*
                    //answers += "CheckBox: " + c.ID + " : CHECKED: " + ((CheckBox)c).Checked + Environment.NewLine;
                    int checkedVal = 0;
                    if (((CheckBox)c).Checked == true)
                    {
                        checkedVal = 1;
                    }
                    mySQL.PostAnswer(PostedSurveyID, new Guid(c.ID.ToString()), checkedVal, "NT");
                     */
                }
            }
            Response.Redirect("ThankYou.aspx");
        }
    }
}