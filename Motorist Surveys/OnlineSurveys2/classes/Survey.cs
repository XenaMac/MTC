using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineSurveys2.classes
{
    public class Survey
    {
        public Guid SurveyID { get; set; }
        public string SurveyName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
        public string SurveyNotes { get; set; }
        public string SurveyBoilerplate { get; set; }
    }
}