<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LinearAnalytics.aspx.cs" Inherits="OnlineSurveys2.Admin.LinearAnalytics" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LATA Online Survey Linear Analytics</title>
    <link href="../themes/SurveyStyle.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div><h2>LATA Online Survey Linear Analytics</h2></div>
        <div>This tool will generate analytics with one line per posted survey</div>
    <div>
        <label for="ddlSurveys">Select a survey</label>
        <asp:DropDownList ID="ddlSurveys" runat="server"></asp:DropDownList>
        <asp:Button ID="btnLoadurveyData" runat="server" Text="Load Data" OnClick="btnLoadurveyData_Click" />
    </div>
        <hr />
    <div>
            <div>Survey Legend</div>
            <div><asp:DataGrid runat="server" ID="gvLegend" CellPadding="4" ForeColor="#333333" GridLines="None">
                <AlternatingItemStyle BackColor="White" ForeColor="#284775" />
                <EditItemStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <ItemStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <SelectedItemStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                </asp:DataGrid></div>
            <div><asp:Button ID="btnExportLegend" runat="server" Text="Export Legend" OnClick="btnExportLegend_Click" /></div>
        </div>
        <div>
            <div>Survey Data</div>
            <div><asp:DataGrid runat="server" ID="gvData" CellPadding="4" ForeColor="#333333" GridLines="None">
                <AlternatingItemStyle BackColor="White" ForeColor="#284775" />
                <EditItemStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <ItemStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <SelectedItemStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                </asp:DataGrid></div>
            <div><asp:Button ID="btnExportData" runat="server" Text="Export Data" OnClick="btnExportData_Click" /></div>
        </div>
    </form>
</body>
</html>
