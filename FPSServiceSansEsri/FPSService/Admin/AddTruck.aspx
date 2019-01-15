<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddTruck.aspx.cs" Inherits="FPSService.Admin.AddTruck" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Select Contractor
        <asp:DropDownList ID="cboContractors" runat="server"></asp:DropDownList>
        <br />
        IP Address
        <asp:TextBox ID="txtIPaddress" runat="server" Width="233px"></asp:TextBox>
        <br />
        Truck #
        <asp:TextBox ID="txtTruckNumber" runat="server" Width="252px"></asp:TextBox>
        <br />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Add Truck" />
    
    </div>
    </form>
</body>
</html>
