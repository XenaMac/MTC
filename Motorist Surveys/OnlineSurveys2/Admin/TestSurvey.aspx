<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestSurvey.aspx.cs" Inherits="OnlineSurveys2.Admin.TestSurvey" validateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LATA Survey System</title>
    <link href="../themes/SurveyStyle.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {
            width: 784px;
            height: 103px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div><a href="Index.aspx">Home</a></div>
        &nbsp;<img alt="MTCLogo" class="auto-style1" src="../images/mtc_survey_v2_mtc%20fsp%20text.png" />
        <header><asp:Label ID="lblSurveyName" runat="server" Text="Survey Name"></asp:Label></header>
        <div class="blueBar"></div>
        <div class="yellowBar"></div>
        <br />
        <div>
            <asp:Panel ID="pnlBoilerplate" runat="server"></asp:Panel>
        </div>
        <br />
    <div>
        <asp:Panel ID="pnlQuestions" runat="server"></asp:Panel>
    </div>
        <br />

        <div>
            <asp:Button ID="btnPost" Text="Post Survey" runat="server" OnClick="btnPost_Click" CssClass="styled-button-3" />
        </div>
        <asp:HiddenField ID="questionList" runat="server" />
        <asp:HiddenField ID="answerList" runat="server" />
        <asp:HiddenField ID="questionTypeList" runat="server" />
    </form>
</body>
</html>
