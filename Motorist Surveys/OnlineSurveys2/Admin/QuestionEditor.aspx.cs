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
    public partial class QuestionEditor : System.Web.UI.Page
    {
        List<classes.Question> theseQuestions = new List<classes.Question>();
        List<classes.QuestionType> theseQuestionTypes = new List<classes.QuestionType>();
        List<classes.Answer> theseAnswers = new List<classes.Answer>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserOK"] == null || Session["UserOK"].ToString() != "OK")
            {
                Response.Redirect("Logon.aspx");
            }
            if (!Page.IsPostBack)
            {
                LoadQuestions();
                LoadQuestionTypes();
            }
        }

        private void LoadQuestions()
        {
            classes.SQLCode mySQL = new classes.SQLCode();
            theseQuestions = mySQL.GetAllQuestions();
            SerializeQuestions(theseQuestions);
            ddlQuestions.Items.Clear();
            ListItem li = new ListItem();
            li.Text = "New Question";
            li.Value = new Guid("00000000-0000-0000-0000-000000000000").ToString();
            ddlQuestions.Items.Add(li);
            foreach (classes.Question thisQuestion in theseQuestions)
            {
                li = new ListItem();
                li.Text = thisQuestion.QuestionText;
                li.Value = thisQuestion.QuestionID.ToString();
                ddlQuestions.Items.Add(li);
            }
            txtAnswerID.Text = "";
            txtAnswerText.Text = "";
            txtSortOrder.Text = "";
            txtQuestionID.Text = "";
            txtQuestionText.Text = "";
            lbAnswers.Items.Clear();
        }

        private void LoadQuestionTypes()
        {
            classes.SQLCode mySQL = new classes.SQLCode();
            theseQuestionTypes = mySQL.GetQuestionTypes();
            XmlSerializer ser = new XmlSerializer(theseQuestionTypes.GetType());
            using (StringWriter writer = new StringWriter())
            {
                ser.Serialize(writer, theseQuestionTypes);
                questionTypesList.Value = writer.ToString();
            }
            ddlQuestionType.Items.Clear();
            ListItem li = new ListItem();
            li.Text = "New Question";
            li.Value = new Guid("00000000-0000-0000-0000-000000000000").ToString();
            foreach (classes.QuestionType thisQuestionType in theseQuestionTypes)
            {
                li = new ListItem();
                li.Text = thisQuestionType.QuestionTypeName;
                li.Value = thisQuestionType.QuestionTypeID.ToString();
                ddlQuestionType.Items.Add(li);
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            ListItem li = ddlQuestions.SelectedItem;
            if (li.Text == "New Question")
            {
                //Initialize new question, clear theseAnswers and all extraneous text boxes;
                Guid gQuestionID = Guid.NewGuid();
                txtQuestionID.Text = gQuestionID.ToString();
                txtQuestionText.Text = "";
                theseAnswers.Clear();
                answerList.Value = "";
                txtAnswerID.Text = "";
                txtAnswerText.Text = "";
                txtSortOrder.Text = "";
                lbAnswers.DataSource = theseAnswers;
                lbAnswers.DataBind();
            }
            else
            {
                theseQuestions = DeserialzeQuestions();
                classes.Question thisQuestion = theseQuestions.Find(delegate(classes.Question myQuestion) { return myQuestion.QuestionID == new Guid(ddlQuestions.SelectedValue); });
                if (thisQuestion != null)
                {
                    txtQuestionID.Text = thisQuestion.QuestionID.ToString();
                    txtQuestionText.Text = thisQuestion.QuestionText;
                    ddlQuestionType.SelectedValue = thisQuestion.QuestionTypeID.ToString();
                }
                classes.SQLCode mySQL = new classes.SQLCode();
                theseAnswers.Clear();
                //if we have a question, look for answers for that question
                if (!string.IsNullOrEmpty(txtQuestionID.Text))
                {
                    theseAnswers = mySQL.GetAnswers(thisQuestion.QuestionID);
                    SerializeAnswers(theseAnswers);
                    lbAnswers.DataSource = theseAnswers;
                    lbAnswers.DataTextField = "AnswerText";
                    lbAnswers.DataValueField = "AnswerID";
                    lbAnswers.DataBind();
                    /*
                    ListItem li = new ListItem();
                    li.Text = "New Answer";
                    li.Value = new Guid("00000000-0000-0000-0000-000000000000").ToString();
                    foreach (classes.Answer thisAnswer in theseAnswers)
                    {
                        li = new ListItem();
                        li.Text = thisAnswer.AnswerText;
                        li.Value = thisAnswer.AnswerID.ToString();
                        ddlAnswers.Items.Add(li);
                    }
                     * */
                }
            }
            txtAnswerText.Text = "";
            txtSortOrder.Text = "";
        }

        #region " Add / Update Answer "

        protected void btnAddUpdateAnswer_Click(object sender, EventArgs e)
        {
            theseAnswers = DeserializeAnswers();
            Guid g = Guid.NewGuid();
            classes.Answer foundAnswer = theseAnswers.Find(delegate(classes.Answer myAnswer) { return myAnswer.AnswerID == new Guid(txtAnswerID.Text); });
            if (foundAnswer != null)
            {
                foundAnswer.AnswerText = txtAnswerText.Text;
                foundAnswer.SortOrder = Convert.ToInt32(txtSortOrder.Text);
                foundAnswer.AnswerID = new Guid(txtAnswerID.Text);
            }
            else
            {
                classes.Answer thisAnswer = new classes.Answer();
                thisAnswer.AnswerText = txtAnswerText.Text;
                if (string.IsNullOrEmpty(txtQuestionID.Text))
                {

                    txtQuestionID.Text = g.ToString();
                }
                g = Guid.NewGuid();
                thisAnswer.AnswerID = new Guid("00000000-0000-0000-0000-000000000000");
                //txtAnswerID.Text = thisAnswer.AnswerID.ToString();
                thisAnswer.QuestionID = new Guid(txtQuestionID.Text);
                thisAnswer.SortOrder = Convert.ToInt32(txtSortOrder.Text);
                theseAnswers.Add(thisAnswer);
            }
            theseAnswers = theseAnswers.OrderBy(a => a.SortOrder).ToList();
            SerializeAnswers(theseAnswers);
            lbAnswers.DataSource = theseAnswers;
            lbAnswers.DataTextField = "AnswerText";
            lbAnswers.DataValueField = "AnswerID";
            lbAnswers.DataBind();
            
            /*
            ddlAnswers.Items.Clear();
            ListItem li = new ListItem();
            li.Text = "New Answer";
            li.Value = new Guid("00000000-0000-0000-0000-000000000000").ToString();
            foreach (classes.Answer thisAnswer in theseAnswers)
            {
                li = new ListItem();
                li.Text = thisAnswer.AnswerText;
                li.Value = thisAnswer.AnswerID.ToString();
                ddlAnswers.Items.Add(li);
            }
             * */
        }

        protected void btnAddAnswer_Click(object sender, EventArgs e)
        {
            theseAnswers = DeserializeAnswers();
            Guid g = Guid.NewGuid();
            //check for valid number in sort order
            int numVal;
            bool result = Int32.TryParse(txtSortOrder.Text, out numVal);
            if (!result)
            {
                txtSortOrder.BackColor = System.Drawing.Color.Red;
                txtSortOrder.Text = "Enter a valid number";
                return;
            }
            else 
            {
                txtSortOrder.BackColor = System.Drawing.Color.White;
            }
            classes.Answer thisAnswer = new classes.Answer();
            thisAnswer.AnswerText = txtAnswerText.Text;
            if (string.IsNullOrEmpty(txtQuestionID.Text))
            {
                txtQuestionID.Text = g.ToString();
            }
            g = Guid.NewGuid();
            thisAnswer.AnswerID = g;
            thisAnswer.QuestionID = new Guid(txtQuestionID.Text);
            thisAnswer.SortOrder = numVal;
            thisAnswer.Status = "KEEP";
            theseAnswers.Add(thisAnswer);
            
            theseAnswers = theseAnswers.OrderBy(a => a.SortOrder).ToList();
            SerializeAnswers(theseAnswers);
            lbAnswers.DataSource = theseAnswers;
            lbAnswers.DataTextField = "AnswerText";
            lbAnswers.DataValueField = "AnswerID";
            lbAnswers.DataBind();
        }

        protected void btnUpdateAnswer_Click(object sender, EventArgs e)
        {
            theseAnswers = DeserializeAnswers();
            Guid g = Guid.NewGuid();
            classes.Answer foundAnswer = theseAnswers.Find(delegate(classes.Answer myAnswer) { return myAnswer.AnswerID == new Guid(txtAnswerID.Text); });
            if (foundAnswer == null)
            {
                Response.Write("Couldn't update answer, no answer selected");
                return;
            }
            foundAnswer.AnswerText = txtAnswerText.Text;
            foundAnswer.SortOrder = Convert.ToInt32(txtSortOrder.Text);
            foundAnswer.Status = "KEEP";
            theseAnswers = theseAnswers.OrderBy(a => a.SortOrder).ToList();
            SerializeAnswers(theseAnswers);
            lbAnswers.DataSource = theseAnswers;
            lbAnswers.DataTextField = "AnswerText";
            lbAnswers.DataValueField = "AnswerID";
            lbAnswers.DataBind();
        }

        protected void btnRemoveAnswer_Click(object sender, EventArgs e)
        {
            theseAnswers = DeserializeAnswers();
            Guid g = Guid.NewGuid();
            classes.Answer foundAnswer = theseAnswers.Find(delegate(classes.Answer myAnswer) { return myAnswer.AnswerID == new Guid(txtAnswerID.Text); });
            if (foundAnswer != null)
            {
                foundAnswer.Status = "REMOVE";
                foundAnswer.AnswerText += " REMOVED";
            }
            else
            {
                Response.Write("Couldn't find answer to remove");
                return;
            }
            theseAnswers = theseAnswers.OrderBy(a => a.SortOrder).ToList();
            SerializeAnswers(theseAnswers);
            lbAnswers.DataSource = theseAnswers;
            lbAnswers.DataTextField = "AnswerText";
            lbAnswers.DataValueField = "AnswerID";
            lbAnswers.DataBind();
        }

        #endregion

        protected void lbAnswers_SelectedIndexChanged(object sender, EventArgs e)
        {
            theseAnswers = DeserializeAnswers();
            classes.Answer foundAnswer = theseAnswers.Find(delegate(classes.Answer myAnswer) { return myAnswer.AnswerID == new Guid(lbAnswers.SelectedValue.ToString()); });
            if (foundAnswer != null)
            {
                txtAnswerText.Text = foundAnswer.AnswerText;
                txtSortOrder.Text = foundAnswer.SortOrder.ToString();
                txtAnswerID.Text = foundAnswer.AnswerID.ToString();
            }
        }

        #region " Serialize, Deserialize "

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

        #endregion

        protected void ddlQuestionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQuestionType.Text == "Free Text")
            {
                txtAnswerText.ReadOnly = true;
                txtSortOrder.ReadOnly = true;
            }
            else
            {
                txtAnswerText.ReadOnly = false;
                txtSortOrder.ReadOnly = false;
            }
        }

        #region " Add / Update / Delete Question "

        protected void btnGo_Click(object sender, EventArgs e)
        {
            classes.Question thisQuestion = new classes.Question();
            if (string.IsNullOrEmpty(txtQuestionID.Text))
            {
                Guid g = Guid.NewGuid();

                txtQuestionID.Text = g.ToString();
            }
            thisQuestion.QuestionID = new Guid(txtQuestionID.Text);
            thisQuestion.QuestionText = txtQuestionText.Text;
            thisQuestion.QuestionTypeID = new Guid(ddlQuestionType.SelectedValue.ToString());
            classes.SQLCode mySQL = new classes.SQLCode();
            ListItem li = ddlQuestionType.SelectedItem;
            string QuestionType = li.Text;
            theseAnswers = DeserializeAnswers();
            //when you add a free text question there is only one answer, which = the text of the question.  Since the answer editor controls are
            //disabled for free text questions this won't allow a user to create an answer, therefore: on create answer list is null and uneditable.
            if (QuestionType == "Free Text" && theseAnswers.Count < 1)
            {
                classes.Answer thisAnswer = new classes.Answer();
                Guid g = Guid.NewGuid();
                thisAnswer.AnswerID = g;
                thisAnswer.QuestionID = thisQuestion.QuestionID;
                thisAnswer.AnswerText = thisQuestion.QuestionText;
                thisAnswer.SortOrder = 1;
                theseAnswers.Add(thisAnswer);
            }
            else if (QuestionType == "Free Text" && theseAnswers.Count > 0)
            {
                theseAnswers[0].AnswerText = txtQuestionText.Text;
                theseAnswers[0].SortOrder = 1;
            }

            mySQL.PostQuestion(thisQuestion, theseAnswers);
            LoadQuestions();
            Response.Write("Question added");
        }

        protected void btnAddQuestion_Click(object sender, EventArgs e)
        {
            classes.Question thisQuestion = new classes.Question();
            if (string.IsNullOrEmpty(txtQuestionID.Text))
            {
                Guid g = Guid.NewGuid();

                txtQuestionID.Text = g.ToString();
            }
            thisQuestion.QuestionID = new Guid(txtQuestionID.Text);
            thisQuestion.QuestionText = txtQuestionText.Text;
            thisQuestion.QuestionTypeID = new Guid(ddlQuestionType.SelectedValue.ToString());
            classes.SQLCode mySQL = new classes.SQLCode();
            ListItem li = ddlQuestionType.SelectedItem;
            string QuestionType = li.Text;
            theseAnswers = DeserializeAnswers();
            //when you add a free text question there is only one answer, which = the text of the question.  Since the answer editor controls are
            //disabled for free text questions this won't allow a user to create an answer, therefore: on create answer list is null and uneditable.
            if (QuestionType == "Free Text" && theseAnswers.Count < 1)
            {
                classes.Answer thisAnswer = new classes.Answer();
                Guid g = Guid.NewGuid();
                thisAnswer.AnswerID = g;
                thisAnswer.QuestionID = thisQuestion.QuestionID;
                thisAnswer.AnswerText = thisQuestion.QuestionText;
                thisAnswer.SortOrder = 1;
                thisAnswer.Status = "KEEP";
                theseAnswers.Add(thisAnswer);
            }
            else if (QuestionType == "Free Text" && theseAnswers.Count > 0)
            {
                theseAnswers[0].AnswerText = txtQuestionText.Text;
                theseAnswers[0].SortOrder = 1;
                theseAnswers[0].Status = "KEEP";
            }

            mySQL.PostQuestion(thisQuestion, theseAnswers);
            LoadQuestions();

            Response.Write("Question added");
        }

        protected void btnUpdateQuestion_Click(object sender, EventArgs e)
        {
            classes.Question thisQuestion = new classes.Question();
            thisQuestion.QuestionID = new Guid(txtQuestionID.Text);
            thisQuestion.QuestionText = txtQuestionText.Text;
            thisQuestion.QuestionTypeID = new Guid(ddlQuestionType.SelectedValue.ToString());
            classes.SQLCode mySQL = new classes.SQLCode();
            ListItem li = ddlQuestionType.SelectedItem;
            string QuestionType = li.Text;
            theseAnswers = DeserializeAnswers();
            //when you add a free text question there is only one answer, which = the text of the question.  Since the answer editor controls are
            //disabled for free text questions this won't allow a user to create an answer, therefore: on create answer list is null and uneditable.
            if (QuestionType == "Free Text" && theseAnswers.Count < 1)
            {
                classes.Answer thisAnswer = new classes.Answer();
                Guid g = Guid.NewGuid();
                thisAnswer.AnswerID = g;
                thisAnswer.QuestionID = thisQuestion.QuestionID;
                thisAnswer.AnswerText = thisQuestion.QuestionText;
                thisAnswer.SortOrder = 1;
                theseAnswers.Add(thisAnswer);
            }
            else if (QuestionType == "Free Text" && theseAnswers.Count > 0)
            {
                theseAnswers[0].AnswerText = txtQuestionText.Text;
                theseAnswers[0].SortOrder = 1;
            }

            mySQL.PostQuestion(thisQuestion, theseAnswers);
            LoadQuestions();
            Response.Write("Question added");
        }

        protected void btnDeleteQuestion_Click(object sender, EventArgs e)
        {
            classes.Question thisQuestion = new classes.Question();
            thisQuestion.QuestionID = new Guid(txtQuestionID.Text);
            thisQuestion.QuestionText = txtQuestionText.Text;
            thisQuestion.QuestionTypeID = new Guid(ddlQuestionType.SelectedValue.ToString());
            classes.SQLCode mySQL = new classes.SQLCode();
            mySQL.DeleteQuestion(thisQuestion);
            LoadQuestions();
            Response.Write("Question deleted");
        }

        #endregion








    }
}