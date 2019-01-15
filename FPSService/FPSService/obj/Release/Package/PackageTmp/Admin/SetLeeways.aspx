<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetLeeways.aspx.cs" Inherits="FPSService.Admin.SetLeeways" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reset Leeway Times</title>
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
                <li><a href="CurrentBeats.aspx"><span>Beats</span></a></li>
                <li><a href="Yards.aspx"><span>Yards</span></a></li>
                <li><a href="CurrentBeatSchedules.aspx"><span>Beat Schedules</span></a></li>
                <li><a href="SetLeeways.aspx" class="active"><span>Leeways</span></a></li>
                <li><a href="TruckMessages.aspx"><span>Messages</span></a></li>
                <li><a href="SetGPS.aspx"><span>Set GPS</span></a></li>
                <li><a href="SurveyControl.aspx"><span>Cell Survey</span></a></li>
                <li><a href="ControlMessages.aspx"><span>Unit Control</span></a></li>
            </ul>
            
       </div>
    &nbsp;<div id="leewayDiv" class="leeways">
        <div><span class="lbl">Extended</span><span class="txt"><asp:TextBox ID="txtExtended" runat=server></asp:TextBox></span></div>
        <div><span class="lbl">Log Off</span><span class="txt"><asp:TextBox ID="txtLogOff" runat=server></asp:TextBox></span></div>
        <div><span class="lbl">Log On</span><span class="txt"><asp:TextBox ID="txtLogOn" runat=server></asp:TextBox></span></div>
        <div><span class="lbl">GPS Issue</span><span class="txt"><asp:TextBox ID="txtGPSIssue" runat=server></asp:TextBox></span></div>
        <div><span class="lbl">Off Beat</span><span class="txt"><asp:TextBox ID="txtOffBeat" runat=server></asp:TextBox></span></div>
        <div><span class="lbl">On Patrol</span><span class="txt"><asp:TextBox ID="txtOnPatrol" runat=server></asp:TextBox></span></div>
        <div><span class="lbl">Roll In</span><span class="txt"><asp:TextBox ID="txtRollIn" runat=server></asp:TextBox></span></div>
        <div><span class="lbl">Roll Out</span><span class="txt"><asp:TextBox ID="txtRollOut" runat=server></asp:TextBox></span></div>
        <div><span class="lbl">Speeding</span><span class="txt"><asp:TextBox ID="txtSpeeding" runat=server></asp:TextBox></span></div>
        <div><span class="lbl">Stationary</span><span class="txt"><asp:TextBox ID="txtStationary" runat=server></asp:TextBox></span></div>
        <div><span class="lbl">Force Log Off</span><span class="txt"><asp:TextBox ID="txtForceLogoff" runat="server"></asp:TextBox></span></div>
        <div><asp:Button ID="btnUpdateVars" runat="server" Text="Update Vars" 
                onclick="btnUpdateVars_Click" /></div>
    </div>
    </form>
    </body>
</html>
