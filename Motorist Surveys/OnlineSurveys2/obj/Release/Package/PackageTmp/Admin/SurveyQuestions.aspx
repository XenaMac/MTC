<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurveyQuestions.aspx.cs" Inherits="OnlineSurveys2.Admin.SurveyQuestions" validateRequest="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LATA Online Surveys - Survey Question Association</title>
    <link href="../themes/SurveyStyle.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div><a href="Index.aspx">Home</a></div>
    <div>
        <header>Survey Question Association</header>
        <div id="SelectSurvey">
            <asp:label runat="server" ID="lblSelectSurvey" CssClass="formLabel">Select your survey</asp:label>
            <asp:DropDownList ID="ddlSurveys" runat="server" Width="300px"></asp:DropDownList>
            <asp:Button ID="btnLoadSurvey" runat="server" Text="Select Survey" OnClick="btnLoadSurvey_Click" CssClass="styled-button-3" />
        </div>
        <div id="surveyQuestions">
            <div>
            <asp:Label runat="server" ID="lblSelectQuestion" CssClass="formLabel">Select your questions</asp:Label>
            <asp:DropDownList ID="ddlQuestions" runat="server" Width="500px"></asp:DropDownList>
            </div>
            <asp:Label runat="server" ID="lblQuestionOrder" CssClass="formLabel">Enter question order</asp:Label>
            <asp:TextBox ID="txtQuestionOrder" runat="server"></asp:TextBox>
            <div>
                <asp:Button ID="btnAddQuestion" runat="server" Text="Add Question" OnClick="btnAddQuestion_Click" CssClass="styled-button-3" />
                <asp:Button ID="btnRemoveQuestion" Text="Remove Selected" runat="server" OnClick="btnRemoveQuestion_Click" CssClass="styled-button-3" />
                <asp:Button ID="btnUpdateSelected" runat="server" OnClick="btnUpdateSelected_Click" Text="Update Selected" CssClass="styled-button-3" />
            </div>
            <div>
            <asp:Label runat="server" ID="lblCurrentQuestions" CssClass="formLabel">Current Questions</asp:Label><asp:ListBox ID="lbCurrentQuestions" runat="server" Width="519px" OnSelectedIndexChanged="lbCurrentQuestions_SelectedIndexChanged" AutoPostBack="True"></asp:ListBox>
            </div>
            <div>
                <asp:Button ID="btnPostSurveyDesign" Text="Post Survey Design" runat="server" OnClick="btnPostSurveyDesign_Click" CssClass="styled-button-3" />
            </div>
        </div>
     
    </div>
    <asp:HiddenField ID="questionList" runat="server" />
        <asp:HiddenField ID="surveyID" runat="server" />
        <asp:HiddenField ID="surveyQuestionsList" runat="server" />
    </form>
</body>
</html>
