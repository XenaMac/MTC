using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineSurveys2
{
    public partial class Survey : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnTakeSurvey_Click(object sender, EventArgs e)
        {
            Response.Redirect("ClientSurvey.aspx?SurveyID=0449746c-e557-4afc-ba19-a265321fed2c");
        }
    }
}