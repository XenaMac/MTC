using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineSurveys2.classes
{
    public class Answer
    {
        public Guid AnswerID { get; set; }
        public Guid QuestionID { get; set; }
        public string AnswerText { get; set; }
        public int SortOrder { get; set; }
        public string Status { get; set; }
    }
}