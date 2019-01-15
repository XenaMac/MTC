<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetGPS.aspx.cs" Inherits="FPSService.Admin.SetGPS" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LATATrax FSP Service | Set GPS</title>
    <link href="../css/SiteStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.8.1.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="Title" class="titleBar"> <!--Start Whole Div-->
           <div class="logoSection"> <!--Start Logo Section -->
                
                    <img alt="BALT Logo" class="bannerelement" longdesc="BALT Logo" src="../Images/mtc%20ftt%20dashboard_v2_bay%20area%20lata%20trax%20logo.png" />
                    <img alt="bar" class="bannerelement" longdesc="bar" src="../Images/VertBar.png" />
                    <img alt="logo" class="bannerelement" longdesc="logo" src="../Images/mtc%20ftt%20dashboard_v2_mtc%20logo.png" />
                    <img alt="bar" class="bannerelement" longdesc="bar" src="../Images/VertBar.png" />
                    <h1 class="bannerelement"> GPS Unit Control
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
                <li><a href="SetLeeways.aspx"><span>Leeways</span></a></li>
                <li><a href="TruckMessages.aspx"><span>Messages</span></a></li>
                <li><a href="SetGPS.aspx" class="active"><span>Set GPS</span></a></li>
                <li><a href="SurveyControl.aspx"><span>Cell Survey</span></a></li>
                <li><a href="ControlMessages.aspx"><span>Unit Control</span></a></li>
            </ul>
            
       </div>
        <asp:Panel ID="pnlMessages" runat="server"></asp:Panel>
        <div id="truckDiv">
            <div class="subTitle"><label>Set GPS Send Rate</label></div>
            <br />
            <div>
                <asp:Label ID="lblSetGPSInterval" runat="server">Set GPS Send Rate</asp:Label>&nbsp;
                <asp:TextBox ID="txtGPSTime" runat="server"></asp:TextBox>
            </div>
            <div>
                <asp:Label ID="lblSelectIndividualTrucks" runat="server">Select Individual Trucks</asp:Label>
                <asp:DropDownList ID="ddlIndividualTrucks" runat="server"></asp:DropDownList>
                <asp:Button ID="btnIndividualTrucks" runat="server" CssClass="myButton" Text="Send to Selected Trucks" OnClick="btnIndividualTrucks_Click" />
            </div>
            <div>
                <asp:Label ID="lblSelectBeats" runat="server">Select Beats</asp:Label>
                <asp:DropDownList ID="ddlBeats" runat="server"></asp:DropDownList>
                <asp:Button ID="btnBeats" runat="server" CssClass="myButton" Text="Send to Selected Beats" />
            </div>
            <div>
                <asp:Label ID="lblSelectContractors" runat="server">Select Contractors</asp:Label>
                <asp:DropDownList ID="ddlContractors" runat="server"></asp:DropDownList>
                <asp:Button ID="Button1" runat="server" CssClass="myButton" Text="Send to Selected Contractors" />
            </div>
        </div>
    </form>
</body>
</html>
