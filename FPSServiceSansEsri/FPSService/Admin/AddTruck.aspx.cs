using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPSService.Admin
{
    public partial class AddTruck : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SQL.SQLCode mySQL = new SQL.SQLCode();
                List<string> contractors = mySQL.getContractors();
                cboContractors.Items.Clear();
                cboContractors.Items.Add("SELECT");
                foreach (string s in contractors)
                {
                    cboContractors.Items.Add(s);
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(cboContractors.Text) || cboContractors.Text == "SELECT")
            {
                Response.Write("Please select a contactor");
                return;
            }
            if (string.IsNullOrEmpty(txtTruckNumber.Text))
            {
                Response.Write("Truck Number CANNOT be NULL");
                return;
            }
            if (string.IsNullOrEmpty(txtIPaddress.Text))
            {
                Response.Write("IP Address CANNOT be NULL");
                return;
            }
            SQL.SQLCode mySQL = new SQL.SQLCode();
            mySQL.addTruck(txtTruckNumber.Text, txtIPaddress.Text, cboContractors.Text);
            Response.Write("Added Truck: " + txtTruckNumber.Text + " with IP Address: " + txtIPaddress.Text + " for contractor: " + cboContractors.Text);
            txtTruckNumber.Text = "";
            txtIPaddress.Text = "";
        }
    }
}