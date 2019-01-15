<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TruckAlarmStatus.aspx.cs" Inherits="FPSService.Admin.TruckAlarmStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vehicle Alarm Status</title>
    <link href="../css/SiteStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">

        <div id="Title" class="titleBar"> <!--Start Whole Div-->
           <div class="logoSection"> <!--Start Logo Section -->
                
                    <img alt="BALT Logo" class="bannerelement" longdesc="BALT Logo" src="../Images/mtc%20ftt%20dashboard_v2_bay%20area%20lata%20trax%20logo.png" />
                    <img alt="bar" class="bannerelement" longdesc="bar" src="../Images/VertBar.png" />
                    <img alt="logo" class="bannerelement" longdesc="logo" src="../Images/mtc%20ftt%20dashboard_v2_mtc%20logo.png" />
                    <img alt="bar" class="bannerelement" longdesc="bar" src="../Images/VertBar.png" />
                    <h1 class="bannerelement"> Current Beats
                </h1>
           </div><!--End Logo Section-->
           <div class="logonSection"><!-- Start Logon Status Section -->
               <div><!-- logged on text and log off link -->
                    <asp:Label ID="lblLoggedOn" runat="server" Text="Administrator"></asp:Label> &nbsp;
                    <asp:Label ID="lblLogonRole" runat="server" Text="(Administrator)"></asp:Label> &nbsp;<img alt="bar" class="auto-style5" longdesc="bar" src="../Images/SmallVertBar.png" />
                    <asp:LinkButton ID="lbtnLogOff" runat="server" CssClass="logOff" OnClick="lbtnLogOff_Click">Log Off</asp:LinkButton>
               </div>
               <div>
                   <asp:Label ID="lblLoggedOnTo" runat="server" Text="Current Logged On To Server" CssClass="loggedOnTo" />
               </div>
           </div>
           <div class="clear"></div><!--End Logon Status Section-->
       </div><!-- End Whole Div-->
        <div class="warningBanner"><!-- Orange Warning Banner-->
            <img alt="Warning!" class="auto-style4" longdesc="Warning!" src="../Images/mtc%20ftt%20dashboard_v2_alert%20icon.png" />
            <label class="Attention">ATTENTION:</label>&nbsp;
            <label class="AttentionText">This is a technical interface.  Incorrect configuration may result in permanent damage</label>
        </div>
        <!--End Title Section-->



       <div>
           <ul class="menu">
                <li><a href="Dashboard.aspx"><span>Dashboard</span></a></li>
                <li><a href="CurrentIncidents.aspx"><span>Incidents/Assists</span></a></li>
                <li><a href="LogOffDriver.aspx"><span>Log Off Driver</span></a></li>
                <li><a href="CurrentBeats.aspx" class="active"><span>Beats</span></a></li>
                <li><a href="Yards.aspx"><span>Yards</span></a></li>
                <li><a href="CurrentBeatSchedules.aspx"><span>Beat Schedules</span></a></li>
                <li><a href="SetLeeways.aspx"><span>Leeways</span></a></li>
                <li><a href="TruckMessages.aspx"><span>Messages</span></a></li>
                <li><a href="SetGPS.aspx"><span>Set GPS</span></a></li>
                <li><a href="SurveyControl.aspx"><span>Cell Survey</span></a></li>
                <li><a href="ControlMessages.aspx"><span>Unit Control</span></a></li>
            </ul>
            
       </div>

    <div id="Title"><h1>Vehicle Alarm Status&nbsp;<asp:Label ID="lblTruck" Text="truck num" runat="server" /></h1></div>
    <asp:HyperLink ID="hlDashboard" Text="Dashboard" NavigateUrl="~/Admin/Dashboard.aspx" runat="server"/>
    <div id="alarms">
        <div><span class="lbl">Vehicle Number</span><span class="txt"><asp:Label ID="lblVehicleNumber" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Driver Name</span><span class="txt"><asp:Label ID="lblDriverName" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Contract Company Name</span><span class="txt"><asp:Label ID="lblContractCompanyName" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Schedule Name</span><span class="txt"><asp:Label ID="lblScheduleName" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Vehicle Status</span><span class="txt"><asp:Label ID="lblVehicleStatus" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Status Started</span><span class="txt"><asp:Label ID="lblStatusStarted" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Speeding Alarm</span><span class="txt"><asp:Label ID="lblSpeedingAlarm" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Speeding Value</span><span class="txt"><asp:Label ID="lblSpeedingValue" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Speeding Time</span><span class="txt"><asp:Label ID="lblSpeedingTime" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Out of Bounds Alarm</span><span class="txt"><asp:Label ID="lblOutOfBoundsAlarm" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Out of Bounds Message</span><span class="txt"><asp:Label ID="lblOutOfBoundsMessage" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Out of Bounds Time</span><span class="txt"><asp:Label ID="lblOutOfBoundsTime" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Out of Bounds Start Time</span><span class="txt"><asp:Label ID="lblOutOfBoundsStartTime" runat="server" Text="val"></asp:Label></span></div>
        <!--
        <div><span class="lbl">Log On Alarm</span><span class="txt"><asp:Label ID="lblLogOnAlarm" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Log On Alarm Time</span><span class="txt"><asp:Label ID="lblLogOnAlarmTime" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Log On Alarm Cleared</span><span class="txt"><asp:Label ID="lblLogOnAlarmCleared" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Log On Alarm Excused</span><span class="txt"><asp:Label ID="lblLogOnAlarmExcused" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Log On Alarm Comments</span><span class="txt"><asp:Label ID="lblLogOnAlarmComments" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Roll Out Alarm</span><span class="txt"><asp:Label ID="lblRollOutAlarm" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Roll Out Alarm Time</span><span class="txt"><asp:Label ID="lblRollOutAlarmTime" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Roll Out Alarm Cleared</span><span class="txt"><asp:Label ID="lblRollOutAlarmCleared" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Roll Out Alarm Excused</span><span class="txt"><asp:Label ID="lblRollOutAlarmExcused" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Roll Out Alarm Comments</span><span class="txt"><asp:Label ID="lblRollOutAlarmComments" runat="server" Text="val"></asp:Label></span></div>
        -->
        <div><span class="lbl">On Patrol Alarm</span><span class="txt"><asp:Label ID="lblOnPatrolAlarm" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">On Patrol Alarm Time</span><span class="txt"><asp:Label ID="lblOnPatrolAlarmTime" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">On Patrol Alarm Cleared</span><span class="txt"><asp:Label ID="lblOnPatrolAlarmCleared" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">On Patrol Alarm Excused</span><span class="txt"><asp:Label ID="lblOnPatrolAlarmExcused" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">On Patrol Alarm Message</span><span class="txt"><asp:Label ID="lblOnPatrolAlarmMessage" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Roll In Alarm</span><span class="txt"><asp:Label ID="lblRollInAlarm" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Roll In Alarm Time</span><span class="txt"><asp:Label ID="lblRollInAlarmTime" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Roll In Alarm Cleared</span><span class="txt"><asp:Label ID="lblRollInAlarmCleared" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Roll In Alarm Excused</span><span class="txt"><asp:Label ID="lblRollInAlarmExcused" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Roll In Alarm Message</span><span class="txt"><asp:Label ID="lblRollInAlarmMessage" runat="server" Text="val"></asp:Label></span></div>
        <!--
        <div><span class="lbl">Log Off Alarm</span><span class="txt"><asp:Label ID="lblLogOffAlarm" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Log Off Alarm Time</span><span class="txt"><asp:Label ID="lblLogOffAlarmTime" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Log Off Alarm Cleared</span><span class="txt"><asp:Label ID="lblLogOffAlarmCleared" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Log Off Alarm Excused</span><span class="txt"><asp:Label ID="lblLogOffAlarmExcused" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Log Off Alarm Comments</span><span class="txt"><asp:Label ID="lblLogOffAlarmComments" runat="server" Text="val"></asp:Label></span></div>
            -->
        <div><span class="lbl">Incident Alarm</span><span class="txt"><asp:Label ID="lblIncidentAlarm" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Incident Alarm Time</span><span class="txt"><asp:Label ID="lblIncidentAlarmTime" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Incident Alarm Cleared</span><span class="txt"><asp:Label ID="lblIncidentAlarmCleared" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Incident Alarm Excused</span><span class="txt"><asp:Label ID="lblIncidentAlarmExcused" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Incident Alarm Comments</span><span class="txt"><asp:Label ID="lblIncidentAlarmComments" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">GPS Issue Alarm</span><span class="txt"><asp:Label ID="lblGPSIssueAlarm" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">GPS Issue Start</span><span class="txt"><asp:Label ID="lblGPSIssueStart" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">GPS Issue Cleared</span><span class="txt"><asp:Label ID="lblGPSIssueCleared" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">GPS Issue Excused</span><span class="txt"><asp:Label ID="lblGPSIssueExcused" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">GPS Issue Comments</span><span class="txt"><asp:Label ID="lblGPSIssueComments" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Stationary Alarm</span><span class="txt"><asp:Label ID="lblStationaryAlarm" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Stationary Start</span><span class="txt"><asp:Label ID="lblStationaryStart" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Stationary Cleared</span><span class="txt"><asp:Label ID="lblStationaryCleared" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Stationary Excused</span><span class="txt"><asp:Label ID="lblStationaryExcused" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Stationary Comments</span><span class="txt"><asp:Label ID="lblStationaryComments" runat="server" Text="val"></asp:Label></span></div>

        <div><span class="lbl">Long Break Alarm</span><span class="txt"><asp:Label ID="lblLongBreakAlarm" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Long Break Start</span><span class="txt"><asp:Label ID="lblLongBreakStart" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Long Break Cleared</span><span class="txt"><asp:Label ID="lblLongBreakCleared" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Long Break Excused</span><span class="txt"><asp:Label ID="lblLongBreakExcused" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Long Break Comments</span><span class="txt"><asp:Label ID="lblLongBreakComments" runat="server" Text="val"></asp:Label></span></div>

        <div><span class="lbl">Long Lunch Alarm</span><span class="txt"><asp:Label ID="lblLongLunchAlarm" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Long Lunch Start</span><span class="txt"><asp:Label ID="lblLongLunchStart" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Long Lunch Cleared</span><span class="txt"><asp:Label ID="lblLongLunchCleared" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Long Lunch Excused</span><span class="txt"><asp:Label ID="lblLongLunchExcused" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Long Lunch Comments</span><span class="txt"><asp:Label ID="lblLongLunchComments" runat="server" Text="val"></asp:Label></span></div>

        <div><span class="lbl">Overtime Alarm</span><span class="txt"><asp:Label ID="lblOvertimeAlarm" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Overtime Start</span><span class="txt"><asp:Label ID="lblOvertimeStart" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Overtime Cleared</span><span class="txt"><asp:Label ID="lblOvertimeCleared" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Overtime Excused</span><span class="txt"><asp:Label ID="lblOvertimeExcused" runat="server" Text="val"></asp:Label></span></div>
        <div><span class="lbl">Overtime Comments</span><span class="txt"><asp:Label ID="lblOvertimeComments" runat="server" Text="val"></asp:Label></span></div>
    </div>
    <asp:Timer ID="Timer1" runat="server" Interval="10000" ontick="Timer1_Tick">
    </asp:Timer>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    </form>
</body>
</html>
