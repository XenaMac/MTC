<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuestionEditor.aspx.cs" Inherits="OnlineSurveys2.Admin.QuestionEditor" validateRequest="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LATA Question Editor</title>
    <link href="../themes/SurveyStyle.css" rel="stylesheet" />
    <style type="text/css">
        #Select1 {
            width: 414px;
        }
        #selAnswers {
            width: 416px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div><a href="Index.aspx">Home</a></div>
    <header>LATA Question Editor</header>
    <div id="selQuestion">
        <div><label class="surveySubHead">Questions</label></div>
        <asp:Label ID="lblSelectQuestion" runat="server" Text="Select Question" CssClass="formLabel"></asp:Label>
&nbsp;
        <asp:DropDownList ID="ddlQuestions" runat="server" Width="400px">
        </asp:DropDownList>
&nbsp;
        <asp:Button ID="btnLoad" runat="server" Text="Load" OnClick="btnLoad_Click" CssClass="styled-button-3" />
    
    </div>
    <hr />
        <div id="editQuestion">
            <div>
                <asp:Label ID="lblQuestionID" runat="server" Text="Question ID" CssClass="formLabel"></asp:Label>
            &nbsp;
                <asp:TextBox ID="txtQuestionID" runat="server" ReadOnly="True" Width="400px"></asp:TextBox>
            </div>
            <div>
                <asp:Label ID="lblQuestionText" runat="server" Text="Question Text" CssClass="formLabel"></asp:Label>
            &nbsp;
                <asp:TextBox ID="txtQuestionText" runat="server" Width="400px"></asp:TextBox>
            </div>
            <div>
                <asp:Label ID="lblQuestionType" runat="server" Text="Question Type" CssClass="formLabel"></asp:Label>
            &nbsp;
                <asp:DropDownList ID="ddlQuestionType" runat="server" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="ddlQuestionType_SelectedIndexChanged">
                </asp:DropDownList>
&nbsp;
                <br />
                <asp:Button ID="btnAddQuestion" runat="server" OnClick="btnAddQuestion_Click" Text="Add As New Question" CssClass="styled-button-3" />
                <asp:Button ID="btnUpdateQuestion" runat="server" OnClick="btnUpdateQuestion_Click" Text="Update Existing Question" CssClass="styled-button-3" />
                <asp:Button ID="btnDeleteQuestion" runat="server" Text="Delete This Question" OnClick="btnDeleteQuestion_Click" CssClass="styled-button-3" />
            </div>
        </div>
        <hr />
        <div id="editAnswer">
            <div><label class="surveySubHead">Answers for Question</label></div>
            <br />
            <div id="AnswerID">
                <asp:Label ID="lblAnswerID" runat="server" Text="Answer ID (no edit)" CssClass="formLabel"></asp:Label><asp:TextBox ID="txtAnswerID" runat="server" Width="261px" ReadOnly="True"></asp:TextBox>
            </div>
            <div id="AnswerText">
                <asp:Label ID="lblAnswerText" runat="server" Text="Enter answer text" CssClass="formLabel"></asp:Label> <asp:TextBox ID="txtAnswerText" runat="server" Width="261px"></asp:TextBox>
            </div>
            <div id="SortOrder">
                <asp:Label ID="lblSortOrder" runat="server" Text="Enter answer order" CssClass="formLabel"></asp:Label><asp:TextBox ID="txtSortOrder" runat="server"></asp:TextBox>

            &nbsp;<div>
                <asp:Button ID="btnAddAnswer" runat="server" Text="Add Answer" OnClick="btnAddAnswer_Click" CssClass="styled-button-3" />
                <asp:Button ID="btnUpdateAnswer" runat="server" Text="Update Answer" OnClick="btnUpdateAnswer_Click" CssClass="styled-button-3" />
                <asp:Button ID="btnRemoveAnswer" runat="server" Text="Remove Answer" OnClick="btnRemoveAnswer_Click" CssClass="styled-button-3" />
            </div>
            </div>
            <div><label>Answers for this question</label></div>
            <asp:ListBox ID="lbAnswers" runat="server" OnSelectedIndexChanged="lbAnswers_SelectedIndexChanged" Width="421px" AutoPostBack="True"></asp:ListBox>
            <br />
            <br />
            
        </div>
        <asp:HiddenField ID="questionTypesList" runat="server" />
        <asp:HiddenField ID="questionList" runat="server" />
        <asp:HiddenField ID="answerList" runat="server" />
    </form>
</body>
</html>
