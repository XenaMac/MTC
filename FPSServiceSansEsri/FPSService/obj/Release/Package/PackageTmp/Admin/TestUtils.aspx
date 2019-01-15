<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestUtils.aspx.cs" Inherits="FPSService.Admin.TestUtils" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dashboard Test Utilities</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="Label1" runat="server" Text="Select Truck"></asp:Label>
&nbsp;<asp:DropDownList ID="ddlTrucks" runat="server">
        </asp:DropDownList>
&nbsp;</div>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Set Forced Break" />
    </form>
</body>
</html>
