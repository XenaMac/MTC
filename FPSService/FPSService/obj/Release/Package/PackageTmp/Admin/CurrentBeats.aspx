<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CurrentBeats.aspx.cs" Inherits="FPSService.Admin.CurrentBeats" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FSP Beat Data</title>
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
    <div>
    
         
        <br />
        <asp:Button ID="btnReloadBeats" runat="server" onclick="btnReloadBeats_Click" CssClass="myButton"
            Text="Reload Beat Data" />
    
    </div>
        <br />
    <div id="beatDiv">
        <div class="subTitle"><label>Beat Data</label></div>
        <asp:GridView ID="gvBeats" runat="server" CssClass="dashboardTable" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="BeatID" HeaderText="Beat ID" SortExpression="BeatID" />
                <asp:BoundField DataField="BeatDescription" HeaderText="Beat Description" SortExpression="BeatDescription" />
                <asp:BoundField DataField="beatData" HeaderText="BeatData" SortExpression="BeatData" />
            </Columns>
        </asp:GridView>
        <br />

        <hr />
    </div>
        <br />
    <div id="beatSegmentDiv">
        <div  class="subTitle"><label>Beat Segment Data</label></div>
        <hr />
        <asp:GridView ID="gvBeatSegments" runat="server" CssClass="dashboardTable" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="BeatID" HeaderText="Beat ID" SortExpression="BeatID" />
                <asp:BoundField DataField="SegmentID" HeaderText="Segment ID" SortExpression="SegmentID" />
                <asp:BoundField DataField="SegmentDescription" HeaderText="Segment Description" SortExpression="SegmentDescription" />
                <asp:BoundField DataField="segmentData" HeaderText="Segment Data" SortExpression="segmentData" />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
