using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPSService.Admin
{
    public partial class SurveyControl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadTruckDropDown();
                LoadTruckGrid();
            }
        }

        private void LoadTruckDropDown()
        {
            ddlTrucks.DataSource = null;
            List<string> truckNums = new List<string>();
            foreach (TowTruck.TowTruck t in DataClasses.GlobalData.currentTrucks)
            {
                if (!string.IsNullOrEmpty(t.TruckNumber))
                {
                    truckNums.Add(t.TruckNumber);
                }
            }
            truckNums = truckNums.OrderBy(s => s).ToList<string>();
            ddlTrucks.DataSource = truckNums;
            ddlTrucks.DataBind();
        }

        private void LoadTruckGrid()
        {
            gvRunningSurveys.DataSource = null;
            List<TowTruck.SurveyTruck> surveyTrucks = new List<TowTruck.SurveyTruck>();
            foreach (TowTruck.TowTruck t in DataClasses.GlobalData.currentTrucks)
            {
                
                TowTruck.SurveyTruck st = new TowTruck.SurveyTruck();
                st.TruckNumber = t.TruckNumber;
                st.PDOP = (double)t.GPSPosition.DOP;
                if (t.GPSPosition.Cell.Contains("|"))
                {
                    string[] splitter = t.GPSPosition.Cell.Split('|');
                    if(!string.IsNullOrEmpty(splitter[0]))
                    {
                        st.SignalStrength = Convert.ToDouble(splitter[0]);
                    }
                    if (!string.IsNullOrEmpty(splitter[1]))
                    {
                        st.Down = Convert.ToDouble(splitter[1]);
                    }
                    if(!string.IsNullOrEmpty(splitter[2]))
                    {
                        st.Up = Convert.ToDouble(splitter[2]);
                    }
                    if (!string.IsNullOrEmpty(splitter[3]))
                    {
                        st.Ping = Convert.ToDouble(splitter[3]);
                    }
                }
                else
                {
                    st.SignalStrength = 0.0;
                    st.Down = 0.0;
                    st.Up = 0.0;
                    st.Ping = 0.0;
                }

                surveyTrucks.Add(st); //this could add a truck that's not currently running a survey
            }
            surveyTrucks = surveyTrucks.OrderBy(s => s.TruckNumber).ToList<TowTruck.SurveyTruck>();
            //got the current truck list, merge with known running surveys
            foreach (TowTruck.SurveyTruck stfound in surveyTrucks)
            {
                TowTruck.SurveyTruck find = DataClasses.GlobalData.surveyTrucks.Find(delegate(TowTruck.SurveyTruck get) { return get.TruckNumber == stfound.TruckNumber; });
                if (find != null)
                {
                    find.PDOP = stfound.PDOP;
                    find.SignalStrength = stfound.SignalStrength;
                    find.Up = stfound.Up;
                    find.Down = stfound.Down;
                    find.Ping = stfound.Ping;
                }
                else
                {
                    DataClasses.GlobalData.surveyTrucks.Add(stfound);
                }
            }
            gvRunningSurveys.DataSource = DataClasses.GlobalData.surveyTrucks;
            gvRunningSurveys.DataBind();
        }

        protected void btnForceStop_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlTrucks.Text))
            {

                TowTruck.TowTruck tFound = DataClasses.GlobalData.FindTowTruckByVehicleNumber(ddlTrucks.Text);
                if (tFound == null)
                {
                    Response.Write("Couldn't find truck");
                    return;
                }
                string IPAddress = tFound.Identifier;
                string cmd = "<Survey>F</Survey>";
                UDP.SendMessage sm = new UDP.SendMessage();
                sm.SendMyMessage(cmd, IPAddress);
                TowTruck.SurveyTruck find = DataClasses.GlobalData.surveyTrucks.Find(delegate(TowTruck.SurveyTruck get) { return get.TruckNumber == tFound.TruckNumber; });
                if (find != null)
                {
                    DataClasses.GlobalData.surveyTrucks.Remove(find);
                }
                LoadTruckGrid();
            }
        }

        protected void btnStartSurvey_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtRunTime.Text) || string.IsNullOrEmpty(txtStartDT.Text) || (chkDownload.Checked == false && chkUpload.Checked == false && chkPing.Checked == false))
            {
                Response.Write("No run time or start time or upload, download, and ping are unchecked");
                return;
            }
            if (!string.IsNullOrEmpty(ddlTrucks.Text))
            {
                TowTruck.TowTruck tFound = DataClasses.GlobalData.FindTowTruckByVehicleNumber(ddlTrucks.Text);
                if (tFound == null)
                {
                    Response.Write("Couldn't find truck");
                    return;
                }
                string IPAddress = tFound.Identifier;
                string msg = string.Empty;

                string dt = txtStartDT.Text;
                string duration = txtRunTime.Text;
                string ud = string.Empty;
                if(chkUpload.Checked == true)
                {
                    ud += "U";
                }
                if (chkDownload.Checked == true)
                {
                    ud += "D";
                }
                if (chkPing.Checked == true)
                {
                    ud += "P";
                }
                msg = "<Survey>" + dt + "|" + duration + "|" + ud + "</Survey>";
                UDP.SendMessage sm = new UDP.SendMessage();
                sm.SendMyMessage(msg, IPAddress);
                TowTruck.SurveyTruck st = new TowTruck.SurveyTruck();
                string[] splitStart = txtStartDT.Text.Split(' ');
                string startDate = splitStart[0].ToString() + " 00:00:00";
                DateTime dtEndDate = Convert.ToDateTime(txtStartDT.Text).AddMinutes(Convert.ToInt32(txtRunTime.Text));
                string endDate = dtEndDate.ToString();
                st.TruckNumber = ddlTrucks.Text;
                st.StartTime = Convert.ToDateTime(txtStartDT.Text);
                st.EndTime = dtEndDate;

                TowTruck.SurveyTruck stAdd = DataClasses.GlobalData.surveyTrucks.Find(delegate(TowTruck.SurveyTruck find) { return find.TruckNumber == ddlTrucks.Text; });
                if (stAdd == null)
                {
                    DataClasses.GlobalData.surveyTrucks.Add(st);
                }
                else
                {
                    DataClasses.GlobalData.surveyTrucks.Remove(stAdd);
                    DataClasses.GlobalData.surveyTrucks.Add(st);
                }
                LoadTruckGrid();
            }
        }
    }
}