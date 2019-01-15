<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TowTruckSetup.aspx.cs" Inherits="FPSService.Admin.TowTruckSetup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LATA Freeway Service Patrol Tow Truck Setup</title>
    <link href="../css/jquery-ui-1.8.23.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/HoverTable.css" rel="stylesheet" type="text/css" />
    <link href="../css/SiteStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script src="../Scripts/TowTruckSetup.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="header">
        <center>
            <h1>LATA Freeway Service Patrol Tow Truck Setup</h1>
            <img src="../Images/LATAShort.jpg" alt="LATA Logo"/>
            <br />
            <br />
            Your IP Address<br />
            <asp:Label ID="lblIPAddress" runat="server"></asp:Label>
        </center>
    </div>
    <hr />
    <center><b>Current Connects</b></center>
    <div id="connects"></div>
    <hr />
    <div id="newTruck" class="newTruck">
        <b>Add New Truck</b><br />
        Select Contractor Company<br />
        <asp:DropDownList ID="ddlContractors" runat="server">
        </asp:DropDownList>
        <br />
        <asp:Label ID="lblTruckNumber" runat="server" Text="Enter Truck Number"></asp:Label>
        <br />
        <asp:TextBox ID="txtTruckNumber" runat="server" Width="142px"></asp:TextBox>
        <br />
        Enter Truck IP Address<br />
        <asp:TextBox ID="txtIPAddress" runat="server" Width="142px"></asp:TextBox>
        <br />
        <asp:Button ID="btnAddNewTruck" runat="server" Text="Add New Truck" 
            onclick="btnAddNewTruck_Click" />
    </div>
    </form>
</body>
</html>
