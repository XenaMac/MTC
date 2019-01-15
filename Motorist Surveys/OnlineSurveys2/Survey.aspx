<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Survey.aspx.cs" Inherits="OnlineSurveys2.Survey" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Online Surveys - Provided by LATA</title>
    <link href="themes/SurveyStyle.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnTakeSurvey" runat="server" Text="Take Survey" CssClass="styled-button-3" OnClick="btnTakeSurvey_Click" />
    </div>
    </form>
</body>
</html>
