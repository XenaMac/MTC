using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace OnlineSurveys2.classes
{
    public class SQLCode
    {
        DataTable dt;
        #region " Conn String "

        private string GetConn()
        {
            return ConfigurationManager.AppSettings["db"].ToString();
        }

        #endregion

        #region  " New Survey Exporter 02/11/2016 EL "

        public DataTable getSurveyData(Guid SurveyID, DateTime dtStart, DateTime dtEnd)
        {
            DataTable dt = new DataTable();

            List<string> columns = getColumnData(SurveyID);
            foreach (string s in columns)
            {
                dt.Columns.Add(s, Type.GetType("System.String"));
            }
            List<Admin.surveyData> responses = getResponses(SurveyID, dtStart, dtEnd);
            foreach (Admin.surveyData sd in responses)
            {
                DataRow row = dt.NewRow();
                //start looping the responses
                foreach (Admin.surveyResponse sr in sd.responses)
                {
                    int colID = 0;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if(dc.ColumnName.Contains(sr.questionID.ToString().ToUpper()))
                        {
                            colID = dt.Columns.IndexOf(dc.ColumnName);
                            if (sr.answerVal != "NT")
                            {
                                row[colID] = sr.answerVal;
                            }
                            if (sr.answerChecked == true)
                            {
                                row[colID] = sr.answerText;
                            }
                        }
                    }
                }
                dt.Rows.Add(row);
            }
            foreach (DataColumn dc in dt.Columns)
            {
                string[] splitter = dc.ColumnName.Split('|');
                dc.ColumnName = splitter[1].ToString();
            }
            return dt;
        }

        private List<string> getColumnData(Guid SurveyID)
        {
            List<string> columns = new List<string>();
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConn()))
                {
                    conn.Open();

                    string SQL = "SELECT CONVERT(varchar(100), q.questionid) + '|' + q.questiontext FROM questions q" +
                        " INNER JOIN SurveysQuestions sq on q.questionid = sq.questionid" +
                        " inner join surveys s on sq.surveyid = s.surveyid" +
                        " where s.surveyid = '" + SurveyID.ToString() + "'" +
                        " order by sq.questionnumber";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        columns.Add(rdr[0].ToString());
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null; 

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return columns;
        }

        private List<Admin.surveyData> getResponses(Guid SurveyID, DateTime dtStart, DateTime dtEnd)
        {
            List<Admin.surveyData> responses = new List<Admin.surveyData>();

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConn()))
                {
                    conn.Open();

                    string SQL = "SELECT DISTINCT postedSurveyID FROM PostedSurveys WHERE PostDate BETWEEN '" + dtStart.ToString() + "' AND '" + dtEnd.ToString() + "'" +
                        " AND SurveyID = '" + SurveyID.ToString() + "'";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Admin.surveyData sd = new Admin.surveyData();
                        sd.postedSurveyID = new Guid(rdr["postedSurveyID"].ToString());
                        sd.responses = new List<Admin.surveyResponse>();
                        responses.Add(sd);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;

                    foreach (Admin.surveyData sd in responses)
                    {
                        SQL = "SELECT ps.postedsurveyid, q.questionid, q.Questiontext,a.answerid, a.answertext, pa.answerval, pa.checked from questions q" +
                                               " inner join answers a on q.questionid = a.questionid" +
                                               " inner join surveysquestions sq on q.questionid = sq.questionid" +
                                               " inner join surveys s on sq.surveyid = s.surveyid" +
                                               " inner join postedanswers pa on a.answerid = pa.answerid" +
                                               " inner join postedsurveys ps on pa.postedsurveyid = ps.postedsurveyid" +
                                               " where ps.postedsurveyid = '" + sd.postedSurveyID + "'" +
                                               " order by ps.postedsurveyid, sq.questionnumber";
                        cmd = new SqlCommand(SQL, conn);
                        rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            Admin.surveyResponse sr = new Admin.surveyResponse();
                            sr.questionID = new Guid(rdr["questionid"].ToString());
                            sr.answerID = new Guid(rdr["answerid"].ToString());
                            sr.answerText = rdr["answertext"].ToString();
                            sr.answerVal = rdr["answerval"].ToString();
                            string checkedVal = rdr["checked"].ToString();
                            if (checkedVal == "1")
                            {
                                sr.answerChecked = true;
                            }
                            else
                            {
                                sr.answerChecked = false;
                            }
                            sd.responses.Add(sr);
                        }
                        rdr.Close();
                        rdr = null;
                        cmd = null;
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return responses;
        }

        #endregion

        #region "Get / Create / Edit Surveys "

        public List<string> GetSurveyName(Guid SurveyID)
        {
            List<string> SurveyData = new List<string>();

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConn()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetSurveyName", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SurveyID", SurveyID);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        SurveyData.Add(rdr["SurveyName"].ToString());
                        SurveyData.Add(rdr["SurveyBoilerplate"].ToString());
                    }
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Redirect("Error.aspx?error=" + ex.Message, true);
            }

            return SurveyData;
        }

        public List<PostedSurvey> GetPostedSurveyData(Guid SurveyID)
        {
            List<PostedSurvey> thisPostedData = new List<PostedSurvey>();

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConn()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetPostedSurveyData", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SurveyID", SurveyID);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        thisPostedData.Add(new PostedSurvey { 
                            ClientSurveyID = new Guid(rdr["ClientSurveyID"].ToString()),
                            PostDate = rdr["PostDate"].ToString(),
                            QuestionNumber = Convert.ToInt32(rdr["QuestionNumber"]),
                            QuestionText = rdr["QuestionText"].ToString(),
                            AnswerText = rdr["AnswerText"].ToString(),
                            Checked = Convert.ToInt32(rdr["Checked"]),
                            AnswerVal = rdr["AnswerVal"].ToString()
                        });
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Redirect("Error.aspx?error=" + ex.Message, true);
            }

            return thisPostedData;
        }

        public List<SurveyCounts> GetSurveyCounts()
        {
            List<SurveyCounts> theseCounts = new List<SurveyCounts>();

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConn()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetSurveyCounts", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        theseCounts.Add(new SurveyCounts { 
                            SurveyName = rdr["SurveyName"].ToString(),
                            SurveyID = new Guid(rdr["SurveyID"].ToString()),
                            SurveyCount = Convert.ToInt32(rdr["SurveyCount"])
                        });
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Redirect("Error.aspx?error=" + ex.Message, true);
            }

            return theseCounts;
        }

        public DataTable GetAnalytics(Guid SurveyID)
        {
            dt = new DataTable();
            string SQL = "select DISTINCT(CONVERT(varchar(10), sq.QuestionNumber) + ':' + CONVERT(varchar(10), a.SortOrder)), sq.QuestionNumber,a.SortOrder from surveys s" +
                " inner join surveysquestions sq on s.surveyid = sq.surveyid" +
                " inner join questions q on sq.questionid = q.questionid" +
                " inner join QuestionTypes qt on q.QuestionTypeID = qt.QuestionTypeID" +
                " inner join Answers a on q.questionid = a.questionid " +
                " where s.SurveyID = '" + SurveyID.ToString() + "'" +
                " order by sq.QuestionNumber, a.SortOrder";
            //Load dt columns
            using (SqlConnection conn = new SqlConnection(GetConn()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL, conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                dt.Columns.Add("PostedSurveyID", Type.GetType("System.String"));
                dt.Columns.Add("PostedDate", Type.GetType("System.String"));
                DataColumn[] keys = new DataColumn[1];
                while (rdr.Read())
                {
                    dt.Columns.Add(rdr[0].ToString(), Type.GetType("System.String"));
                }
                keys[0] = dt.Columns[0];
                dt.PrimaryKey = keys;
                rdr.Close();
                rdr = null;
                cmd = null;
                SQL = "select ps.PostedSurveyID, ps.PostDate, CONVERT(varchar(10), sq.QuestionNumber) + ':' + CONVERT(varchar(10), a.SortOrder) as 'col', qt.QuestionType, pa.Checked, pa.AnswerVal  from surveys s" +
                    " inner join surveysquestions sq on s.surveyid = sq.surveyid" +
                    " inner join questions q on sq.questionid = q.questionid" +
                    " inner join QuestionTypes qt on q.QuestionTypeID = qt.QuestionTypeID" +
                    " inner join Answers a on q.questionid = a.questionid" +
                    " INNER JOIN PostedSurveys ps ON s.SurveyID = ps.SurveyID" +
                    " INNER JOIN PostedAnswers pa on a.AnswerID = pa.AnswerID AND pa.PostedSurveyID = ps.PostedSurveyID" +
                    " where s.SurveyID = '" + SurveyID.ToString() + "'" +
                    " order by ps.PostDate, ps.PostedSurveyID, sq.QuestionNumber, a.SortOrder";
                cmd = new SqlCommand(SQL, conn);
                rdr = cmd.ExecuteReader();
                Guid startID = new Guid();
                while (rdr.Read())
                {
                    Guid currentID = new Guid(rdr[0].ToString());
                    string colName = rdr["col"].ToString();
                    string QuestionType = rdr["QuestionType"].ToString();
                    string Checked = rdr["Checked"].ToString();
                    string AnswerVal = rdr["AnswerVal"].ToString();
                    if (currentID != startID)
                    {
                        DataRow row = dt.NewRow();
                        row["PostedSurveyID"] = rdr["PostedSurveyID"].ToString();
                        row["PostedDate"] = Convert.ToDateTime(rdr["PostDate"].ToString());
                        if (QuestionType == "Single Select" || QuestionType == "Multi Select")
                        {
                            row[colName] = rdr["Checked"].ToString();
                        }
                        if (QuestionType == "Free Text")
                        {
                            row[colName] = rdr["AnswerVal"].ToString();
                        }
                        dt.Rows.Add(row);
                        startID = currentID;
                    }
                    else
                    {
                        DataRow[] rows = dt.Select("PostedSurveyID='" + currentID.ToString() + "'");
                        DataRow row = rows[0];
                        if (QuestionType == "Single Select" || QuestionType == "Multi Select")
                        {
                            row[colName] = rdr["Checked"].ToString();
                        }
                        if (QuestionType == "Free Text")
                        {
                            row[colName] = rdr["AnswerVal"].ToString();
                        }
                    }
                }
                conn.Close();
            }
            return dt;
        }

        public DataTable GetLegend(Guid SurveyID)
        {
            dt = new DataTable();
            string SQL = "select s.SurveyName, sq.QuestionNumber, a.SortOrder, qt.QuestionType, q.QuestionText, a.AnswerText  from surveys s" +
                " inner join surveysquestions sq on s.surveyid = sq.surveyid" +
                " inner join questions q on sq.questionid = q.questionid" +
                " inner join QuestionTypes qt on q.QuestionTypeID = qt.QuestionTypeID" +
                " inner join Answers a on q.questionid = a.questionid" +
                " where s.SurveyID = '" + SurveyID.ToString() + "'" +
                " order by s.SurveyName, sq.QuestionNumber, a.SortOrder";
            using (SqlConnection conn = new SqlConnection(GetConn()))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(SQL, conn);
                da.Fill(dt);
                da = null;
                conn.Close();
            }
            return dt;
        }

        public List<Survey> GetAllSurveys()
        {
            List<Survey> theseSurveys = new List<Survey>();
            using (SqlConnection conn = new SqlConnection(GetConn()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetSurveyList", conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        theseSurveys.Add(new Survey { 
                            SurveyName = rdr["SurveyName"].ToString(),
                            SurveyID = new Guid(rdr["SurveyID"].ToString()),
                            CreatedBy = rdr["CreatedBy"].ToString(),
                            DateCreated = Convert.ToDateTime(rdr["DateCreated"].ToString()),
                            ModifiedBy = rdr["ModifiedBy"].ToString(),
                            DateModified = Convert.ToDateTime(rdr["DateModified"].ToString()),
                            SurveyNotes = rdr["SurveyNotes"].ToString(),
                            SurveyBoilerplate = rdr["SurveyBoilerplate"].ToString()
                        });
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    return theseSurveys;
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Response.Redirect("Error.aspx?error=" + ex.Message, true);
                    return null;
                }
            }
        }

        public Survey GetSurvey(Guid SurveyID)
        {
            Survey thisSurvey = new Survey();
            using (SqlConnection conn = new SqlConnection(GetConn()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetSurveyByID", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SurveyID", SurveyID.ToString());
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        thisSurvey.SurveyID = new Guid(rdr["SurveyID"].ToString());
                        thisSurvey.SurveyName = rdr["SurveyName"].ToString();
                        thisSurvey.CreatedBy = rdr["CreatedBy"].ToString();
                        thisSurvey.DateCreated = Convert.ToDateTime(rdr["DateCreated"]);
                        thisSurvey.ModifiedBy = rdr["ModifiedBy"].ToString();
                        thisSurvey.DateModified = Convert.ToDateTime(rdr["DateModified"]);
                        thisSurvey.SurveyNotes = rdr["SurveyNotes"].ToString();
                        thisSurvey.SurveyBoilerplate = rdr["SurveyBoilerplate"].ToString();
                    }
                    rdr.Close();
                    rdr = null;
                    conn.Close();
                    cmd = null;
                    return thisSurvey;
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Response.Redirect("Error.aspx?error=" + ex.Message, true);
                    return null;
                }
                
            }
        }

        public void PostSurvey(Survey thisSurvey)
        {
            using (SqlConnection conn = new SqlConnection(GetConn()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("AddSurvey", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SurveyID", thisSurvey.SurveyID);
                    cmd.Parameters.AddWithValue("@SurveyName", thisSurvey.SurveyName);
                    cmd.Parameters.AddWithValue("@CreatedBy", thisSurvey.CreatedBy);
                    cmd.Parameters.AddWithValue("@ModifiedBy", thisSurvey.ModifiedBy);
                    cmd.Parameters.AddWithValue("@SurveyNotes", thisSurvey.SurveyNotes);
                    cmd.Parameters.AddWithValue("@SurveyBoilerplate", thisSurvey.SurveyBoilerplate);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Response.Redirect("Error.aspx?error=" + ex.Message, true);
                }
            }
        }

        #endregion

        #region " Get / Create / Edit Questions and Answers "

        public List<Question> GetAllQuestions()
        {
            List<Question> theseQuestions = new List<Question>();
            using (SqlConnection conn = new SqlConnection(GetConn()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetAllQuestions", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        theseQuestions.Add(new Question { 
                            QuestionID = new Guid(rdr["QuestionID"].ToString()),
                            QuestionText = rdr["QuestionText"].ToString(),
                            QuestionTypeID = new Guid(rdr["QuestionTypeID"].ToString())
                        });
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                    return theseQuestions;
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Response.Redirect("Error.aspx?error=" + ex.Message, true);
                    return null;
                }
            }
        }

        public List<QuestionType> GetQuestionTypes()
        {
            List<QuestionType> theseQuestionTypes = new List<QuestionType>();
            using (SqlConnection conn = new SqlConnection(GetConn()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetQuestionTypes", conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        theseQuestionTypes.Add(new QuestionType { 
                            QuestionTypeID = new Guid(rdr["QuestionTypeID"].ToString()),
                            QuestionTypeName = rdr["QuestionType"].ToString()
                        });
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                    return theseQuestionTypes;
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Response.Redirect("Error.aspx?error=" + ex.Message, true);
                    return null;
                }
            }
        }

        public List<Question> GetQuestionsBySurvey(Guid SurveyID)
        {
            List<Question> surveyQuestions = new List<Question>();
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConn()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetQuestionsBySurvey", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SurveyID", SurveyID.ToString());
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        surveyQuestions.Add(new Question { 
                            QuestionNumber = Convert.ToInt32(rdr["QuestionNumber"]),
                            QuestionID = new Guid(rdr["QuestionID"].ToString()),
                            QuestionText = rdr["QuestionText"].ToString(),
                            QuestionTypeID = new Guid(rdr["QuestionTypeID"].ToString()),
                            QuestionStatus = "KEEP"
                        });
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Redirect("Error.aspx?error=" + ex.Message, true);
            }
            return surveyQuestions;
        }

        public void DeleteQuestion(Question thisQuestion)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConn()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RemoveQuestion", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@QuestionID", thisQuestion.QuestionID.ToString());
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Redirect("Error.aspx?error=" + ex.Message, true);
            }
        }

        public void PostQuestion(Question thisQuestion, List<Answer> theseAnswers)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConn()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("AddQuestion", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@QuestionID", thisQuestion.QuestionID);
                    cmd.Parameters.AddWithValue("@QuestionText", thisQuestion.QuestionText);
                    cmd.Parameters.AddWithValue("@QuestionTypeID", thisQuestion.QuestionTypeID);
                    cmd.ExecuteNonQuery();
                    foreach (Answer thisAnswer in theseAnswers)
                    {
                        SqlCommand cmdAnswer = new SqlCommand("AddAnswer", conn);
                        cmdAnswer.CommandType = CommandType.StoredProcedure;
                        cmdAnswer.Parameters.AddWithValue("@AnswerID", thisAnswer.AnswerID);
                        cmdAnswer.Parameters.AddWithValue("@QuestionID", thisQuestion.QuestionID);
                        cmdAnswer.Parameters.AddWithValue("@AnswerText", thisAnswer.AnswerText);
                        cmdAnswer.Parameters.AddWithValue("@SortOrder", thisAnswer.SortOrder);
                        cmdAnswer.Parameters.AddWithValue("@Status", thisAnswer.Status);
                        cmdAnswer.ExecuteNonQuery();
                        cmdAnswer = null;
                    }
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Redirect("Error.aspx?error=" + ex.Message, true);
            }
        }

        public void PostSurveyDesign(Guid SurveyID, List<Question> theseQuestions)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConn()))
                {
                    conn.Open();
                    foreach (Question thisQuestion in theseQuestions)
                    {
                        SqlCommand cmd = new SqlCommand("LinkSurveyQuestion", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SurveyID", SurveyID.ToString());
                        cmd.Parameters.AddWithValue("@QuestionID", thisQuestion.QuestionID.ToString());
                        cmd.Parameters.AddWithValue("@QuestionNumber", thisQuestion.QuestionNumber.ToString());
                        cmd.Parameters.AddWithValue("@QuestionStatus", thisQuestion.QuestionStatus);
                        cmd.ExecuteNonQuery();
                        cmd = null;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Redirect("Error.aspx?error=" + ex.Message, true);
            }
        }

        public List<Answer> GetAnswers(Guid QuestionID)
        {
            try
            {
                List<Answer> theseAnswers = new List<Answer>();
                using (SqlConnection conn = new SqlConnection(GetConn()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetAnswerList", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@QuestionID", QuestionID);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        theseAnswers.Add(new Answer { 
                            AnswerID = new Guid(rdr["AnswerID"].ToString()),
                            QuestionID = new Guid(rdr["QuestionID"].ToString()),
                            AnswerText = rdr["AnswerText"].ToString(),
                            SortOrder = Convert.ToInt32(rdr["SortOrder"]),
                            Status = "KEEP"
                        });
                    }
                    return theseAnswers;
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Redirect("Error.aspx?error=" + ex.Message, true);
                return null;
            }
        }

        #endregion

        #region " Post Survey "

        public void PostSurvey(Guid PostedSurveyID, Guid SurveyID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConn()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("PostSurvey", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PostedSurveyID", PostedSurveyID.ToString());
                    cmd.Parameters.AddWithValue("@SurveyID", SurveyID.ToString());
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Redirect("Error.aspx?error=" + ex.Message, true);
            }
        }

        public void PostAnswer(Guid PostedSurveyID, Guid AnswerID, int Checked, string AnswerVal)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConn()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("PostAnswer", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PostedSurveyID", PostedSurveyID.ToString());
                    cmd.Parameters.AddWithValue("@AnswerID", AnswerID.ToString());
                    cmd.Parameters.AddWithValue("@Checked", Checked.ToString());
                    cmd.Parameters.AddWithValue("@AnswerVal", AnswerVal);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Redirect("Error.aspx?error=" + ex.Message, true);
            }
        }

        #endregion

    }
}