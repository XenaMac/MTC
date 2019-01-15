<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="FPSService.Admin.Dashboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../css/SiteStyle.css" rel="stylesheet" type="text/css" />
    <link href="../css/HoverTable.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/CurrentConnectsJSONtoTable.js" type="text/javascript"></script>
    <!--<script src="../Scripts/CurrentStateJSONtoTable.js" type="text/javascript"></script>-->
    <title>LATATrax MTC Dashboard</title>
    <style type="text/css">
        .auto-style4 {
            width: 27px;
            height: 24px;
        }
        .auto-style5 {
            width: 2px;
            height: 20px;
        }
        .auto-style6 {
            width: 44px;
            height: 27px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
       <!--Begin Title Section-->
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

        <div id="conHead" class="subTitle">
            <h2>
                <img alt="Connected Trucks" class="auto-style6" longdesc="Connected Trucks" src="../Images/mtc%20ftt%20dashboard_v2_connected%20vehicle%20icon.png" /> Currently Connected Vehicles
                <img alt="bar" class="auto-style5" longdesc="bar" src="../Images/SmallVertBar.png" />&nbsp;<asp:Label ID="conCount" runat="server" Text="Connection Count:"></asp:Label>
                
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <asp:UpdatePanel ID="updateCad" runat="server">
                    <ContentTemplate>
                        <asp:Timer ID="tmrCheckCADConnect" runat="server" Interval="10000" OnTick="tmrCheckCADConnect_Tick">
                        </asp:Timer>
                        <asp:Label ID="lblSendStatus" Text="WAIT" runat="server" />&nbsp;
                        <asp:Label ID="lblReceieveStatus" Text="WAIT" runat="server" />&nbsp;
                        <asp:Label ID="lblHeartBeat" Text="WAIT" runat="server" />
                </ContentTemplate>
                </asp:UpdatePanel>
            </h2>
        </div>
    <div id="CurrentConnects">
        <br />
    </div>
    <!--
    <div id="CurrentState">
    
    </div>-->

    </form>
</body>
</html>
