<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ControlMessages.aspx.cs" Inherits="FPSService.Admin.ControlMessages" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../css/SiteStyle.css" rel="stylesheet" type="text/css" />
    <link href="../css/HoverTable.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.8.1.min.js" type="text/javascript"></script>
    <title>LATATrax MTC Control Messages</title>
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
                <li><a href="SetGPS.aspx"><span>Set GPS</span></a></li>
                <li><a href="SurveyControl.aspx"><span>Cell Survey</span></a></li>
                <li><a href="ControlMessages.aspx" class="active"><span>Unit Control</span></a></li>
            </ul>
            
       </div>
        <br />

        <div id="msgHead" class="subTitle">
            <h2><img alt="vehicleMessageIcon" src="../Images/mtc%20ftt%20dashboard_v2_message%20vehicle%20icon.png" />
            Send Control Message to Vehicle</h2></div>
        <!--
        <div class="colmask threecol">
	        <div class="colmid">
		        <div class="colleft">
			    <div class="col1">
                    
                </div>
                <div class="col2">
                    
                </div>
                <div class="col3">
                    
                </div><!--Right Column-->
            <!--</div> <!--Colleft-->
        <!--</div> <!--ColMid-->
        <!--</div> <!--ColMask-->
        <!--
        <div id="selMessage">
            
        </div>
        <div id="extendMessage">

        </div>-->

        <div id="columnHolder">
            <div id="leftColumn" class="leftColumn">
                <h3>Select Vehicle</h3>
                    <asp:DropDownList ID="ddlFTTs" runat="server" Height="22px" Width="250px">
                    </asp:DropDownList>
            </div>
            <div id="middleColumn" class="middleColumn">
                <h3>Select Message </h3>
                    <asp:DropDownList ID="ddlMessages" runat="server" Width="250px">
                        <asp:ListItem>GetGPS</asp:ListItem>
                        <asp:ListItem>GetState</asp:ListItem>
                        <asp:ListItem>Reboot</asp:ListItem>
                        <asp:ListItem>GetVar</asp:ListItem>
                        <asp:ListItem>SetVar</asp:ListItem>
                    </asp:DropDownList>
            </div>
            <div id="rightColumn" class="rightColumn">
                <h3>Option: Set Value (Type =&gt; Value)</h3>
                    <asp:DropDownList ID="ddlVarNames" runat="server">
                        <asp:ListItem>TruckID</asp:ListItem>
                        <asp:ListItem>GpsRate</asp:ListItem>
                        <asp:ListItem>Log</asp:ListItem>
                        <asp:ListItem>Version</asp:ListItem>
                        <asp:ListItem>ServerIP</asp:ListItem>
                        <asp:ListItem>SFTPServerIP</asp:ListItem>
                        <asp:ListItem>IgnTimeoutSecs</asp:ListItem>
                        <asp:ListItem>BillStartDay</asp:ListItem>
                        <asp:ListItem>LoggedOn</asp:ListItem>
                        <asp:ListItem>AutoLogOffTime</asp:ListItem>
                </asp:DropDownList>
                    <asp:TextBox ID="txtVarValue" runat="server" Width="156px" 
                        ToolTip=" For GetVar use Name|VarName, for SetVar use VarName|Value" />
            </div>
                    <div class="clear"></div>
        </div>

        <div id="btn">
            <asp:Button ID="btnSendMessage" CssClass="myButton" runat="server" Text="Send Message" 
                onclick="Button1_Click" ToolTip="Send selected message to selected vehicle" />&nbsp;
            <asp:Button ID="btnRefreshList" CssClass="myButton" runat="server" Text="Refresh Vehicle List" 
                ToolTip="Refresh the Select FTT List" onclick="Button2_Click" />&nbsp;
            <asp:Button ID="btnResetVehicle" CssClass="myButton" runat="server" onclick="Button3_Click" 
                Text="Reset Selected Vehicle" />
        </div>
    </form>
</body>
</html>
