using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPSService.Admin
{
    public partial class TruckAlarmStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Logon"] == null)
            {
                Response.Redirect("Logon.aspx");
            }
            string logon = Session["Logon"].ToString();
            if (logon != "true")
            {
                Response.Redirect("Logon.aspx");
            }
            if (Request.QueryString == null)
            {
                Response.Redirect("Dashboard.aspx");
            }
            string ip = Request.QueryString["ip"];
            if (string.IsNullOrEmpty(ip))
            {
                Response.Redirect("Dashboard.aspx");
            }

            if (!Page.IsPostBack)
            {
                LoadTruckData();
            }
        }

        private void LoadTruckData()
        {
            string ip = Request.QueryString["ip"];
            TowTruck.TowTruck thisTruck = DataClasses.GlobalData.FindTowTruck(ip);
            if (thisTruck != null)
            {
                lblTruck.Text = thisTruck.Extended.TruckNumber;
                lblVehicleNumber.Text = thisTruck.Extended.TruckNumber;
                lblDriverName.Text = thisTruck.Driver.LastName + ", " + thisTruck.Driver.FirstName;
                lblContractCompanyName.Text = thisTruck.Extended.ContractorName;
                lblVehicleStatus.Text = thisTruck.Status.VehicleStatus;
                lblStatusStarted.Text = thisTruck.Status.StatusStarted.ToString();
                if (thisTruck.Driver.schedule != null)
                {
                    lblScheduleName.Text = thisTruck.Driver.schedule.ScheduleName;
                }
                else
                {
                    lblScheduleName.Text = "no schedule";
                }
                //Check for alarm status, set green or red accordingly.
                #region " Speeding Alarms **FIXED TO NEW WAY**"

                TowTruck.AlarmTimer aSpeed = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "Speeding"; });
                if (aSpeed != null)
                {
                    if (aSpeed.hasAlarm == true)
                    {
                        lblSpeedingAlarm.ForeColor = System.Drawing.Color.Red;
                        lblSpeedingValue.ForeColor = System.Drawing.Color.Red;
                        lblSpeedingTime.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lblSpeedingAlarm.ForeColor = System.Drawing.Color.Green;
                        lblSpeedingValue.ForeColor = System.Drawing.Color.Green;
                        lblSpeedingTime.ForeColor = System.Drawing.Color.Green;
                    }
                    lblSpeedingAlarm.Text = aSpeed.hasAlarm.ToString();
                    lblSpeedingValue.Text = aSpeed.alarmValue;
                    //lblSpeedingValue.Text = thisTruck.GPSPosition.Speed.ToString() + " / MAX: " + thisTruck.GPSPosition.MaxSpd.ToString();
                    lblSpeedingTime.Text = aSpeed.alarmStart.ToString("hh:mm:ss");
                }
                #endregion

                #region " Out of Bounds Alarms **FIXED TO NEW WAY** "

                TowTruck.AlarmTimer aOOB = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "OffBeat"; });
                if (aOOB != null)
                {
                    if (aOOB.hasAlarm == true)
                    {
                        lblOutOfBoundsAlarm.ForeColor = System.Drawing.Color.Red;
                        lblOutOfBoundsMessage.ForeColor = System.Drawing.Color.Red;
                        lblOutOfBoundsTime.ForeColor = System.Drawing.Color.Red;
                        lblOutOfBoundsStartTime.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lblOutOfBoundsAlarm.ForeColor = System.Drawing.Color.Green;
                        lblOutOfBoundsMessage.ForeColor = System.Drawing.Color.Green;
                        lblOutOfBoundsTime.ForeColor = System.Drawing.Color.Green;
                        lblOutOfBoundsStartTime.ForeColor = System.Drawing.Color.Green;
                    }
                }

                lblOutOfBoundsAlarm.Text = aOOB.hasAlarm.ToString();
                lblOutOfBoundsMessage.Text = "";
                lblOutOfBoundsTime.Text = aOOB.getStatusMinutes().ToString();
                lblOutOfBoundsStartTime.Text = aOOB.alarmStart.ToString("hh:mm:ss");

                #endregion

                
                #region " Log On Alarms DEPRECATED"
                /* OLD
                if (thisTruck.Status.LogOnAlarm == true)
                {
                    lblLogOnAlarm.ForeColor = System.Drawing.Color.Red;
                    lblLogOnAlarmTime.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    lblLogOnAlarm.ForeColor = System.Drawing.Color.Green;
                    lblLogOnAlarmTime.ForeColor = System.Drawing.Color.Green;
                }

                lblLogOnAlarm.Text = thisTruck.Status.LogOnAlarm.ToString();
                lblLogOnAlarmTime.Text = thisTruck.Status.LogOnAlarmTime.ToString();
                lblLogOnAlarmCleared.Text = thisTruck.Status.LogOnAlarmCleared.ToString();
                lblLogOnAlarmExcused.Text = thisTruck.Status.LogOnAlarmExcused.ToString();
                lblLogOnAlarmComments.Text = thisTruck.Status.LogOnAlarmComments;
                */
                #endregion

                #region " Old "
                /*
                #region " Roll Out Alarms "

                if (thisTruck.Status.RollOutAlarm == true)
                {
                    lblRollOutAlarm.ForeColor = System.Drawing.Color.Red;
                    lblRollOutAlarmTime.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    lblRollOutAlarm.ForeColor = System.Drawing.Color.Green;
                    lblRollOutAlarmTime.ForeColor = System.Drawing.Color.Green;
                }

                lblRollOutAlarm.Text = thisTruck.Status.RollOutAlarm.ToString();
                lblRollOutAlarmTime.Text = thisTruck.Status.RollOutAlarmTime.ToString();
                lblRollOutAlarmCleared.Text = thisTruck.Status.RollOutAlarmCleared.ToString();
                lblRollOutAlarmExcused.Text = thisTruck.Status.RollOutAlarmExcused.ToString();
                lblRollOutAlarmComments.Text = thisTruck.Status.RollOutAlarmComments;
                                */
                #endregion

                #region " On Patrol Alarms "

                TowTruck.AlarmTimer aOP = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "LateOnPatrol"; });
                if (aOP != null)
                {
                    if (aOP.hasAlarm == true)
                    {
                        lblOnPatrolAlarm.ForeColor = System.Drawing.Color.Red;
                        lblOnPatrolAlarmMessage.ForeColor = System.Drawing.Color.Red;
                        lblOnPatrolAlarmTime.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lblOnPatrolAlarm.ForeColor = System.Drawing.Color.Green;
                        lblOnPatrolAlarmMessage.ForeColor = System.Drawing.Color.Green;
                        lblOnPatrolAlarmTime.ForeColor = System.Drawing.Color.Green;
                    }
                    lblOnPatrolAlarm.Text = aOP.hasAlarm.ToString();
                    lblOnPatrolAlarmTime.Text = aOP.alarmStart.ToString("hh:mm:ss");
                    lblOnPatrolAlarmMessage.Text = aOP.alarmValue;
                    lblOnPatrolAlarmCleared.Text = aOP.alarmCleared.ToString();
                    lblOnPatrolAlarmExcused.Text = aOP.alarmExcused.ToString();
                }

                #endregion


                #region " Roll In Alarms "

                TowTruck.AlarmTimer aRI = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "EarlyOutOfService"; });
                if (aRI != null)
                {
                    if (aRI.hasAlarm == true)
                    {
                        lblRollInAlarm.ForeColor = System.Drawing.Color.Red;
                        lblRollInAlarmTime.ForeColor = System.Drawing.Color.Red;
                        lblRollInAlarmMessage.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lblRollInAlarm.ForeColor = System.Drawing.Color.Green;
                        lblRollInAlarmTime.ForeColor = System.Drawing.Color.Green;
                        lblRollInAlarmMessage.ForeColor = System.Drawing.Color.Green;
                    }
                    lblRollInAlarm.Text = aRI.hasAlarm.ToString();
                    lblRollInAlarmTime.Text = aRI.alarmStart.ToString("hh:mm:ss");
                    lblRollInAlarmMessage.Text = aRI.alarmValue;
                    lblRollInAlarmCleared.Text = aRI.alarmCleared.ToString();
                    lblRollInAlarmExcused.Text = aRI.alarmExcused.ToString();
                }

                #endregion

                #region " Log Off Alarms "
                /*
                if (thisTruck.Status.LogOffAlarm == true)
                {
                    lblLogOffAlarm.ForeColor = System.Drawing.Color.Red;
                    lblLogOffAlarmTime.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    lblLogOffAlarm.ForeColor = System.Drawing.Color.Green;
                    lblLogOffAlarmTime.ForeColor = System.Drawing.Color.Green;
                }

                lblLogOffAlarm.Text = thisTruck.Status.LogOffAlarm.ToString();
                lblLogOffAlarmTime.Text = thisTruck.Status.LogOffAlarmTime.ToString();
                lblLogOffAlarmCleared.Text = thisTruck.Status.LogOffAlarmCleared.ToString();
                lblLogOffAlarmExcused.Text = thisTruck.Status.LogOffAlarmExcused.ToString();
                lblLogOffAlarmComments.Text = thisTruck.Status.LogOffAlarmComments;
                */
                #endregion

                #region  " Long Incident Alarms **Fixed** "

                TowTruck.AlarmTimer aLI = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "LongIncident"; });
                if (aLI != null)
                {
                    if (aLI.hasAlarm == true)
                    {
                        lblIncidentAlarm.ForeColor = System.Drawing.Color.Red;
                        lblIncidentAlarmTime.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lblIncidentAlarm.ForeColor = System.Drawing.Color.Green;
                        lblIncidentAlarmTime.ForeColor = System.Drawing.Color.Green;
                    }
                }

                lblIncidentAlarm.Text = aLI.hasAlarm.ToString();
                lblIncidentAlarmTime.Text = aLI.alarmStart.ToString("hh:mm:ss");
                lblIncidentAlarmCleared.Text = aLI.alarmCleared.ToString();
                lblIncidentAlarmExcused.Text = aLI.alarmExcused.ToString();
                lblIncidentAlarmComments.Text = aLI.comment;

                #endregion

                #region " GPS Issue Alarms **FIXED TO NEW WAY** "

                TowTruck.AlarmTimer aGPS = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "GPSIssue"; });
                if (aGPS != null)
                {
                    if (aGPS.hasAlarm == true)
                    {
                        lblGPSIssueAlarm.ForeColor = System.Drawing.Color.Red;
                        lblGPSIssueStart.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lblGPSIssueAlarm.ForeColor = System.Drawing.Color.Green;
                        lblGPSIssueStart.ForeColor = System.Drawing.Color.Green;
                    }
                }

                lblGPSIssueAlarm.Text = aGPS.hasAlarm.ToString();
                lblGPSIssueStart.Text = aGPS.alarmStart.ToString("hh:mm:ss");
                lblGPSIssueCleared.Text = aGPS.alarmCleared.ToString();
                lblGPSIssueExcused.Text = aGPS.alarmExcused.ToString();
                lblGPSIssueComments.Text = aGPS.comment;

                #endregion

                #region  " Stationary Alarms **FIXED TO NEW WAY** "
                TowTruck.AlarmTimer aStationary = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "Stationary"; });
                if (aStationary != null)
                {
                    if (aStationary.hasAlarm == true)
                    {
                        lblStationaryAlarm.ForeColor = System.Drawing.Color.Red;
                        lblStationaryStart.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lblStationaryAlarm.ForeColor = System.Drawing.Color.Green;
                        lblStationaryStart.ForeColor = System.Drawing.Color.Green;
                    }
                }

                lblStationaryAlarm.Text = aStationary.hasAlarm.ToString();
                lblStationaryStart.Text = aStationary.alarmStart.ToString("hh:mm:ss");
                lblStationaryCleared.Text = aStationary.alarmCleared.ToString();
                lblStationaryExcused.Text = aStationary.alarmExcused.ToString();
                lblStationaryComments.Text = aStationary.comment;

                #endregion

                #region " Long Lunch Alarms "

                TowTruck.AlarmTimer aLongLunch = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "LongLunch"; });
                if (aLongLunch != null)
                {
                    if (aLongLunch.hasAlarm == true)
                    {
                        lblLongLunchAlarm.ForeColor = System.Drawing.Color.Red;
                        lblLongLunchComments.ForeColor = System.Drawing.Color.Red;
                        lblLongLunchStart.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lblLongLunchAlarm.ForeColor = System.Drawing.Color.Green;
                        lblLongLunchComments.ForeColor = System.Drawing.Color.Green;
                        lblLongLunchStart.ForeColor = System.Drawing.Color.Green;
                    }
                    lblLongLunchAlarm.Text = aLongLunch.hasAlarm.ToString();
                    lblLongLunchStart.Text = aLongLunch.GetStartTime();
                    lblLongLunchComments.Text = aLongLunch.alarmValue;
                    lblLongLunchCleared.Text = aLongLunch.alarmCleared.ToString();
                    lblLongLunchExcused.Text = aLongLunch.alarmExcused.ToString();
                }

                #endregion

                #region  " Long Break Alarms "

                TowTruck.AlarmTimer aLongBreak = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "LongBreak"; });
                if (aLongBreak != null)
                {
                    if (aLongBreak.hasAlarm == true)
                    {
                        lblLongBreakAlarm.ForeColor = System.Drawing.Color.Red;
                        lblLongBreakComments.ForeColor = System.Drawing.Color.Red;
                        lblLongBreakStart.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lblLongBreakAlarm.ForeColor = System.Drawing.Color.Green;
                        lblLongBreakComments.ForeColor = System.Drawing.Color.Green;
                        lblLongBreakStart.ForeColor = System.Drawing.Color.Green;
                    }
                    lblLongBreakAlarm.Text = aLongBreak.hasAlarm.ToString();
                    lblLongBreakStart.Text = aLongBreak.GetStartTime();
                    lblLongBreakComments.Text = aLongBreak.alarmValue;
                    lblLongBreakCleared.Text = aLongBreak.alarmCleared.ToString();
                    lblLongBreakExcused.Text = aLongBreak.alarmExcused.ToString();
                }

                #endregion

                #region " Overtime "

                TowTruck.AlarmTimer aOvertime = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "OvertimeActivity"; });
                if (aOvertime != null)
                {
                    if (aOvertime.hasAlarm == true)
                    {
                        lblOvertimeAlarm.ForeColor = System.Drawing.Color.Red;
                        lblOvertimeComments.ForeColor = System.Drawing.Color.Red;
                        lblOvertimeStart.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lblOvertimeAlarm.ForeColor = System.Drawing.Color.Green;
                        lblOvertimeComments.ForeColor = System.Drawing.Color.Green;
                        lblOvertimeStart.ForeColor = System.Drawing.Color.Green;
                    }
                    lblOvertimeAlarm.Text = aOvertime.hasAlarm.ToString();
                    lblOvertimeStart.Text = aOvertime.GetStartTime();
                    lblOvertimeComments.Text = aOvertime.alarmValue;
                    lblOvertimeCleared.Text = aOvertime.alarmCleared.ToString();
                    lblOvertimeExcused.Text = aOvertime.alarmExcused.ToString();
                }

                #endregion
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            LoadTruckData();
            
        }

        protected void lbtnLogOff_Click(object sender, EventArgs e)
        {
            Session["Logon"] = null;
            Response.Redirect("Logon.aspx");
        }
    }
}