using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace FPSService
{
    public partial class MTCWaze : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string wazeFeed = JsonConvert.SerializeObject(DataClasses.GlobalData.wazeFeed);
            Response.Write(wazeFeed);
        }
    }
}