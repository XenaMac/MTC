using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineSurveys2.classes
{
    public class PostedSurvey
    {
        public string PostDate { get; set; }
        public Guid ClientSurveyID { get; set; }
        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }
        public int Checked { get; set; }
        public string AnswerVal { get; set; }
    }
}