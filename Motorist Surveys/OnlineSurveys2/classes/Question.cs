using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineSurveys2.classes
{
    public class Question
    {
        public Guid QuestionID { get; set; }
        public string QuestionText { get; set; }
        public Guid QuestionTypeID { get; set; }
        public int QuestionNumber { get; set; }
        public string QuestionStatus { get; set; }
    }
}