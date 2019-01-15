<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurveyControl.aspx.cs" Inherits="FPSService.Admin.SurveyControl" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../css/SiteStyle.css" rel="stylesheet" type="text/css" />
    <link href="../css/HoverTable.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.simple-dtpicker.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.simple-dtpicker.js"></script>
    <title>LATATrax MTC Survey Control</title>
        <style type="text/css">
        .auto-style4 {
            width: 27px;
            height: 24px;
        }
        .auto-style5 {
            width: 2px;
            height: 20px;
        }
        </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $(function () {
                $('#txtStartDT').appendDtpicker({ "dateFormat": "MM/DD/YYYY hh:mm:00" });
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="Title" class="titleBar"> <!--Start Whole Div-->
           <div class="logoSection"> <!--Start Logo Section -->
                
                    <img alt="BALT Logo" class="bannerelement" longdesc="BALT Logo" src="../Images/mtc%20ftt%20dashboard_v2_bay%20area%20lata%20trax%20logo.png" />
                    <img alt="bar" class="bannerelement" longdesc="bar" src="../Images/VertBar.png" />
                    <img alt="logo" class="bannerelement" longdesc="logo" src="../Images/mtc%20ftt%20dashboard_v2_mtc%20logo.png" />
                    <img alt="bar" class="bannerelement" longdesc="bar" src="../Images/VertBar.png" />
                    <h1 class="bannerelement"> Survey Control
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
                    Conducting Cellular Surveys can significantly impact cellular data plan usage due to Upload and Download time measurements</div>

        <ul class="menu">
                <li><a href="CurrentIncidents.aspx" class="active"><span>Incidents/Assists</span></a></li>
                <li><a href="LogOffDriver.aspx"><span>Log Off Driver</span></a></li>
                <li><a href="CurrentBeats.aspx"><span>Beats</span></a></li>
                <li><a href="Yards.aspx"><span>Yards</span></a></li>
                <li><a href="CurrentBeatSchedules.aspx"><span>Beat Schedules</span></a></li>
                <li><a href="SetLeeways.aspx"><span>Leeways</span></a></li>
                <li><a href="TruckMessages.aspx"><span>Messages</span></a></li>
                <li><a href="SetGPS.aspx"><span>Set GPS</span></a></li>
                <li><a href="SurveyControl.aspx"><span>Cell Survey</span></a></li>
                <li><a href="ControlMessages.aspx"><span>Unit Control</span></a></li>
            </ul>

        <!--End Title Section-->



       <div>
    </div>
        </div>
        <p>
            Select Truck
            <asp:DropDownList ID="ddlTrucks" runat="server" Width="150px">
            </asp:DropDownList>
&nbsp;<asp:Button ID="btnStartSurvey" runat="server" CssClass="myButtonGreen" Text="Start Survey" OnClick="btnStartSurvey_Click" />
        &nbsp; <asp:Button ID="btnForceStop" runat="server" CssClass="myButtonRed" Text="Stop Survey" OnClick="btnForceStop_Click" />
        </p>
        <p>
            Select Start Time <asp:TextBox ID="txtStartDT" runat="server"></asp:TextBox>
            &nbsp; Duration
            <asp:TextBox ID="txtRunTime" runat="server" Width="44px"></asp:TextBox>
&nbsp;<asp:CheckBox ID="chkDownload" runat="server" Text="Track Download" />
&nbsp;
            <asp:CheckBox ID="chkUpload" runat="server" Text="Track Upload" />
&nbsp;
            <asp:CheckBox ID="chkPing" runat="server" Text="Tack Ping" />
        </p>
        <asp:Panel ID="pnlRunningSurveys" runat="server" BorderColor="#666699" BorderStyle="Solid">
            <h2>Currently Running Surveys</h2>
            <asp:GridView ID="gvRunningSurveys" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal">
                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                <HeaderStyle BackColor="#000000" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                <SortedDescendingHeaderStyle BackColor="#242121" />
            </asp:GridView>
        </asp:Panel>
    </form>
</body>
</html>
