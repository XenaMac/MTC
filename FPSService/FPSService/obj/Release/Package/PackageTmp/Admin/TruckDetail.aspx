<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TruckDetail.aspx.cs" Inherits="FPSService.Admin.TruckDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/HoverTable.css" rel="stylesheet" type="text/css" />
    <link href="../css/SiteStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/TruckDetail.js" type="text/javascript"></script>
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
        <div class="multiColumn">
        <div class="leftCol">
        <table id="dataTable" class="hovertable">
            <tr><td><label class="lblDescriptor">Truck Number</label></td><td><label id="lblTruckNumber" class="lblVal">Truck Number</label></td></tr>
            <tr><td><label class="lblDescriptor">IP Address</label></td><td><label id="lblIPAddress" class="lblVal">IP Address</label></td></tr>
            <tr><td><label class="lblDescriptor">Direction</label></td><td><label id="lblDirection" class="lblVal">Direction</label></td></tr>
            <tr><td><label class="lblDescriptor">Speed</label></td><td><label id="lblSpeed" class="lblVal">Speed</label></td></tr>
            <tr><td><label class="lblDescriptor">Lat</label></td><td><label id="lblLat" class="lblVal">Lat</label></td></tr>
            <tr><td><label class="lblDescriptor">Lon</label></td><td><label id="lblLon" class="lblVal">Lon</label></td></tr>
            <tr><td><label class="lblDescriptor">Vehicle State</label></td><td><label id="lblVehicleState" class="lblVal">Vehicle State</label></td></tr>
            <tr><td><label class="lblDescriptor">Alarms</label></td><td><label id="lblAlarms" class="lblVal">Alarms</label></td></tr>
            <tr><td><label class="lblDescriptor">Speeding Alarm</label></td><td><label id="lblSpeedingAlarm" class="lblVal">Speeding Alarm</label></td></tr>
            <tr><td><label class="lblDescriptor">Speeding Value</label></td><td><label id="lblSpeedingValue" class="lblVal">Speeding Value</label></td></tr>
            <tr><td><label class="lblDescriptor">Speeding Time</label></td><td><label id="lblSpeedingTime" class="lblVal">Speeding Time</label></td></tr>
            <tr><td><label class="lblDescriptor">Out of Bounds Alarm</label></td><td><label id="lblOutOfBoundsAlarm" class="lblVal">Out of Bounds Alarm</label></td></tr>
            <tr><td><label class="lblDescriptor">Out of Bounds Message</label></td><td><label id="lblOutOfBoundsMessage" class="lblVal">Out of Bounds Message</label></td></tr>
            <tr><td><label class="lblDescriptor">Out of Bounds Time</label></td><td><label id="lblOutOfBoundsTime" class="lblVal">Out of Bounds Time</label></td></tr>
            <tr><td><label class="lblDescriptor">Heading</label></td><td><label id="lblHeading" class="lblVal">Heading</label></td></tr>
            <tr><td><label class="lblDescriptor">Last Message</label></td><td><label id="lblLastMessage" class="lblVal">Last Message</label></td></tr>
            <tr><td><label class="lblDescriptor">Contractor Name</label></td><td><label id="lblContractorName" class="lblVal">lblContractorName</label></td></tr>
            <tr><td><label class="lblDescriptor">Beat Number</label></td><td><label id="lblBeatNumber" class="lblVal">Beat Number</label></td></tr>
            <tr><td><label class="lblDescriptor">GPS Rate</label></td><td><label id="lblGPSRate" class="lblVal">GPS Rate</label></td></tr>
            <tr><td><label class="lblDescriptor">GPS Status</label></td><td><label id="lblGPSStatus" class="lblVal">GPS Status</label></td></tr>
            <tr><td><label class="lblDescriptor">GPS DOP</label></td><td><label id="lblGPSDOP" class="lblVal">GPS DOP</label></td></tr>
            <tr><td><label class="lblDescriptor">Log</label></td><td><label id="lblLog" class="lblVal">Log</label></td></tr>
            <tr><td><label class="lblDescriptor">Version</label></td><td><label id="lblVersion" class="lblVal">Version</label></td></tr>
            <tr><td><label class="lblDescriptor">Server IP</label></td><td><label id="lblServerIP" class="lblVal">Server IP</label></td></tr>
            <tr><td><label class="lblDescriptor">SFTP Server IP</label></td><td><label id="lblSFTPServerIP" class="lblVal">SFTP Server IP</label></td></tr>
        </table>
        </div>
        <div class="rightCol">
            <table id="extendedTable" class="hovertable">
                <tr><td>IP List</td><td><asp:Label ID="lblIPList" runat="server" Text="IP List"></asp:Label></td></tr>
                <tr><td>Bill Start Day</td><td><asp:Label ID="lblBillStartDay" runat="server" Text="Bill Start Day"></asp:Label></td></tr>
                <tr><td>Last Bill Reset</td><td><asp:Label ID="lblLastBillReset" runat="server" Text="Last Bill Reset"></asp:Label></td></tr>
                <tr><td>Data Used (GB)</td><td><asp:Label ID="lblDataUsed" runat="server" Text="Data Used (GB)"></asp:Label></td></tr>
                <tr><td>Ignition Timeout (secs)</td><td><asp:Label ID="lblIgnTimeoutSecs" runat="server" Text="Ingnition Timeout (secs)"></asp:Label></td></tr>
                <tr><td>Current Status</td><td><asp:Label ID="lblCurrentNewStatus" runat="server" Text="Current Status"></asp:Label></td></tr>
                <tr><td>Driver Name</td><td><asp:Label ID="lblDriverName" runat="server" Text="Driver Name"></asp:Label></td></tr>
                <tr><td>Driver FSPID</td><td><asp:Label ID="lblDriverFSPID" runat="server" Text="Driver FSP ID"></asp:Label></td></tr>
                <tr><td>Assigned Beat</td><td><asp:Label ID="lblAssignedBeat" runat="server" Text="Assigned Beat"></asp:Label></td></tr>
                <tr><td>Call Sign</td><td><asp:Label ID="lblCallSign" runat="server" Text="Call Sign"></asp:Label></td></tr>
                <tr><td>Assigned Shift</td><td><asp:Label ID="lblAssignedShift" runat="server" Text="AssignedShift"></asp:Label></td></tr>
                <tr><td>Vehicle Type</td><td><asp:Label ID="lblVehicleType" runat="server" Text="Vehicle Type"></asp:Label></td></tr>
                <tr><td>Vehicle Year</td><td><asp:Label ID="lblVehicleYear" runat="server" Text="Vehicle Year"></asp:Label></td></tr>
                <tr><td>Vehicle Make</td><td><asp:Label ID="lblVehicleMake" runat="server" Text="Vehicle Make"></asp:Label></td></tr>
                <tr><td>Vehicle Model</td><td><asp:Label ID="lblVehicleModel" runat="server" Text="Vehicle Model"></asp:Label></td></tr>
                <tr><td>License Plate</td><td><asp:Label ID="lblLicensePlate" runat="server" Text="License Plate"></asp:Label></td></tr>
                <tr><td>Registration Expiration</td><td><asp:Label ID="lblRegistrationExpiration" runat="server" Text="Registration Expiration"></asp:Label></td></tr>
                <tr><td>Insurance Expiration</td><td><asp:Label ID="lblInsuranceExpiration" runat="server" Text="Insurance Expiration"></asp:Label></td></tr>
                <tr><td>Last CHP Inspection</td><td><asp:Label ID="lblLastCHPInspection" runat="server" Text="Last CHP Inspection"></asp:Label></td></tr>
                <tr><td>Program End Date</td><td><asp:Label ID="lblProgramEndDate" runat="server" Text="Program End Date"></asp:Label></td></tr>
            </table>
        </div>
        <div class="clear"></div>
     </div>

    </form>
</body>
</html>
