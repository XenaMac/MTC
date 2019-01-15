<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurveyAnalytics.aspx.cs" Inherits="OnlineSurveys2.Admin.SurveyAnalytics" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LATA Online Survey Analytics</title>
    <link href="../themes/SurveyStyle.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div><a href="Index.aspx">Home</a></div>
    <div>
    
        <header><asp:Label ID="lblSurveyName" runat="server" Text="Label"></asp:Label></header>
    
        <br />
        <asp:GridView ID="gvData" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:GridView>
    
        <br />
        <asp:Button ID="btnExportExcel" runat="server" CssClass="styled-button-3" OnClick="btnExportExcel_Click" Text="Export to Excel" />
    
    </div>
    </form>
</body>
</html>
