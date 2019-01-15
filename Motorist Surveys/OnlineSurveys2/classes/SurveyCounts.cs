using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineSurveys2.classes
{
    public class SurveyCounts
    {
        public string SurveyName { get; set; }
        public Guid SurveyID { get; set; }
        public int SurveyCount { get; set; }
    }
}