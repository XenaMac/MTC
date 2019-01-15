<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logon.aspx.cs" Inherits="OnlineSurveys2.Admin.Logon" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Online Survey Admin: Logon</title>
    <link href="../themes/SurveyStyle.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {
            width: 1280px;
            height: 161px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
<div>
        <asp:Panel ID="errPanel" runat="server" />
    &nbsp;<img alt="MTC Banner" class="auto-style1" src="../images/mtc_survey_v2_1280x161%20banner.png" /></div>
        <div><h1>MTC Survey Design Logon</h1></div>
        <div>
            <div>User Name</div>
            <div><asp:TextBox ID="txtUserName" runat="server" /></div>
            <div>Password</div>
            <div><asp:TextBox ID="txtPassword" runat="server" TextMode="Password" /></div>
            <div><asp:Button ID="btnGo" runat="server" Text="Logon" CssClass="myButton" OnClick="btnGo_Click" /></div>
        </div>
    </form>
</body>
</html>
