using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPSService.Admin
{
    public partial class CadMessages : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void loadData()
        {
            gvSent.DataSource = DataClasses.GlobalData.cadSent;
            gvReceived.DataSource = DataClasses.GlobalData.cadReceived;
            lblLastHBValue.Text = Global.cListener.lastHB;
            lblLastSentHBValue.Text = Global.cSender.lastHBSent;
            gvSent.DataBind();
            gvReceived.DataBind();
        }

        protected void btnResetCad_Click(object sender, EventArgs e)
        {
            Global.cListener.ResetListener();
            Global.cSender.Disconnect();
            Global.cSender.Connect();
            Response.Write("CAD Link reset for Listener and Sender");
        }

        protected void btnClearMessages_Click(object sender, EventArgs e)
        {
            DataClasses.GlobalData.cadSent.Clear();
            DataClasses.GlobalData.cadReceived.Clear();
            loadData();
        }
    }
}