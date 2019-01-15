<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientSurvey.aspx.cs" Inherits="OnlineSurveys2.ClientSurvey" validateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Online Surveys Provided by LATA</title>
    <link href="themes/SurveyStyle.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {
            width: 1280px;
            height: 161px;
        }
    </style>
</head>
<body>
<form id="form1" runat="server">
    <img alt="MTCBanner" class="auto-style1" src="images/mtc_survey_v2_1280x161%20banner.png" />
        <header><asp:Label ID="lblSurveyName" runat="server" Text="Survey Name"></asp:Label></header>
        <!--<div class="blueBar"></div>
        <div class="yellowBar"></div>
            <br />-->
        <div class="boilerplate">
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
