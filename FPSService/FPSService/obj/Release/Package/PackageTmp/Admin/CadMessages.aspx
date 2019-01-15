<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CadMessages.aspx.cs" Inherits="FPSService.Admin.CadMessages" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CAD Message History</title>
    <link href="../css/SiteStyle.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="Title" class="titleBar"> <!--Start Whole Div-->
           <div class="logoSection"> <!--Start Logo Section -->
                
                    <img alt="BALT Logo" class="bannerelement" longdesc="BALT Logo" src="../Images/mtc%20ftt%20dashboard_v2_bay%20area%20lata%20trax%20logo.png" />
                    <img alt="bar" class="bannerelement" longdesc="bar" src="../Images/VertBar.png" />
                    <img alt="logo" class="bannerelement" longdesc="logo" src="../Images/mtc%20ftt%20dashboard_v2_mtc%20logo.png" />
                    <img alt="bar" class="bannerelement" longdesc="bar" src="../Images/VertBar.png" />
                    <h1 class="bannerelement"> Vehicle IT Dashboard
                </h1>
           </div><!--End Logo Section-->
           <div class="logonSection"><!-- Start Logon Status Section -->
               <div><!-- logged on text and log off link -->
                    <asp:Label ID="lblLoggedOn" runat="server" Text="Administrator"></asp:Label> &nbsp;
                    <asp:Label ID="lblLogonRole" runat="server" Text="(Administrator)"></asp:Label> &nbsp;<img alt="bar" class="auto-style5" longdesc="bar" src="../Images/SmallVertBar.png" />
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


        <!-- Menu -->
       <div>
           <ul class="menu">
               <li><a href="Dashboard.aspx" class="active"><span>Dashboard</span></a></li>
                <li><a href="CurrentIncidents.aspx"><span>Incidents/Assists</span></a></li>
                <li><a href="LogOffDriver.aspx"><span>Log Off Driver</span></a></li>
                <li><a href="CurrentBeats.aspx"><span>Beats</span></a></li>
                <li><a href="Yards.aspx"><span>Yards</span></a></li>
                <li><a href="CurrentBeatSchedules.aspx"><span>Beat Schedules</span></a></li>
                <li><a href="SetLeeways.aspx"><span>Leeways</span></a></li>
                <li><a href="TruckMessages.aspx"><span>Messages</span></a></li>
                <li><a href="SetGPS.aspx"><span>Set GPS</span></a></li>
                <li><a href="SurveyControl.aspx"><span>Cell Survey</span></a></li>
                <li><a href="ControlMessages.aspx"><span>Unit Control</span></a></li>
                <li><a href="CadMessages.aspx"><span>CAD History</span></a></li>
            </ul>
       </div>
        <!-- End Menu -->
    <div>
        <h2>CAD Messages</h2>
    </div>
    <div>
        <asp:Button ID="btnResetCad" Text="resetCad" CssClass="detailClass" runat="server" OnClick="btnResetCad_Click" />
    &nbsp;
        <asp:Button ID="btnClearMessages" Text="Clear Msgs" CssClass="detailClass" runat="server" OnClick="btnClearMessages_Click" />
    </div>
        <br />
    <div>
        <div>
            <asp:Label ID="lblLastHB" runat="server" Text="Last Heartbeat Received"></asp:Label> &nbsp; 
            <asp:Label ID="lblLastHBValue" runat="server" Text="WAIT"></asp:Label> &nbsp;
            <asp:Label ID="lblLastHBSent" runat="server" Text="Last Heartbeat Sent"></asp:Label> &nbsp;
            <asp:Label ID="lblLastSentHBValue" runat="server" Text="WAIT"></asp:Label>
        </div>
        <hr />
        <div>
            <h3>Received Messages</h3>
        </div>
        <asp:DataGrid ID="gvReceived" runat="server"></asp:DataGrid>
        <hr />
        <div>
            <h3>Sent Messages</h3>
        </div>
        <asp:DataGrid ID="gvSent" runat="server"></asp:DataGrid>
    </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Timer ID="Timer1" runat="server" Interval="30000">
        </asp:Timer>
    </form>
</body>
</html>
