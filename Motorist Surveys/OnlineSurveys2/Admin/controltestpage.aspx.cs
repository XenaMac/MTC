using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineSurveys2.Admin
{
    public partial class controltestpage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadDropDownList1();
                LoadListBox1();
            }
        }

        private void LoadDropDownList1()
        {
            List<string> myList = new List<string>();
            myList.Add("Hello");
            myList.Add("Goodbye");
            myList.Add("Booga booga");
            myList.Add("Nyuk nyuk nyuk");
            myList.Add("HOLA!");

            int i = 0;
            foreach (string s in myList)
            {
                ListItem li = new ListItem();
                li.Value = i.ToString();
                li.Text = s;
                DropDownList1.Items.Add(li);
                i++;
            }
        }

        private void LoadListBox1()
        {
            List<string> myList = new List<string>();
            myList.Add("Hello");
            myList.Add("Goodbye");
            myList.Add("Booga booga");
            myList.Add("Nyuk nyuk nyuk");
            myList.Add("HOLA!");

            int i = 0;
            foreach (string s in myList)
            {
                ListItem li = new ListItem();
                li.Value = i.ToString();
                li.Text = s;
                ListBox1.Items.Add(li);
                i++;
            }
        }

        protected void btnGetSelected_Click(object sender, EventArgs e)
        {
            int i = DropDownList1.SelectedIndex;
            ListItem li = DropDownList1.Items[i];
            string ItemText = li.Text;
            string itemVal = li.Value;
            Response.Write(ItemText + " " + itemVal);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string response = "";
            foreach (ListItem li in ListBox1.Items)
            {
                if (li.Selected == true)
                {
                    response += li.Value + " " + li.Text + "<br>";
                }
            }
            Response.Write(response);
        }
    }
}