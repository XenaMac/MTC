<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurveyEditor.aspx.cs" Inherits="OnlineSurveys2.Admin.SurveyEditor" validateRequest="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LATA Survey Editor</title>
    <link href="../themes/SurveyStyle.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div><a href="Index.aspx">Home</a></div>
        <header>LATA Survey Editor</header>
        <div id="selSurvey">
            
            <asp:Label ID="lblSelectSurvey" runat="server" Text="Select Your Survey" CssClass="formLabel"></asp:Label>
&nbsp;
            <asp:DropDownList ID="ddlSurveys" runat="server" Width="400px">
            </asp:DropDownList>
&nbsp;<asp:Button ID="btnLoad" runat="server" Text="Load" OnClick="btnLoad_Click" CssClass="styled-button-3" />
            
        </div>
        <hr />
        <div id="editSurvey">
            <div>
                <asp:Label ID="lblSurveyID" runat="server" Text="Survey ID" CssClass="formLabel"></asp:Label>
            &nbsp;
                <asp:TextBox ID="txtSurveyID" runat="server" Width="399px" ReadOnly="True"></asp:TextBox>
            </div>
            <div>
                <asp:Label ID="lblSurveyName" runat="server" Text="Survey Name" CssClass="formLabel"></asp:Label>
            &nbsp;
                <asp:TextBox ID="txtSurveyName" runat="server" Width="399px"></asp:TextBox>
            </div>
            <div>
                <asp:Label ID="lblCreatedBy" runat="server" Text="Created By" CssClass="formLabel"></asp:Label>
            &nbsp;
                <asp:TextBox ID="txtCreatedBy" runat="server" Width="399px"></asp:TextBox>
            </div>
            <div>
                <asp:Label ID="lblDateCreated" runat="server" Text="Date Created" CssClass="formLabel"></asp:Label>
            &nbsp;
                <asp:TextBox ID="txtDateCreated" runat="server" Width="399px"></asp:TextBox>
            </div>
            <div>
                <asp:Label ID="lblModifiedBy" runat="server" Text="Modified By" CssClass="formLabel"></asp:Label>
            &nbsp;
                <asp:TextBox ID="txtModifiedBy" runat="server" Width="399px"></asp:TextBox>
            </div>
            <div>
                <asp:Label ID="lblDateModified" runat="server" Text="Date Modified" CssClass="formLabel"></asp:Label>
            &nbsp;
                <asp:TextBox ID="txtDateModified" runat="server" Width="399px"></asp:TextBox>
            </div>
            <div>
                <asp:Label ID="lblSurveyBoilerplate" runat="server" Text="Survey Boilerplate" CssClass="formLabel"></asp:Label>
            &nbsp;
                <asp:TextBox ID="txtSurveyBoilerplate" runat="server" Height="149px" TextMode="MultiLine" Width="399px"></asp:TextBox>
              </div>
            <div>
                <asp:Label ID="lblSurveyNotes" runat="server" Text="Survey Notes" CssClass="formLabel"></asp:Label>
            &nbsp;
                <asp:TextBox ID="txtSurveyNotes" runat="server" Height="149px" TextMode="MultiLine" Width="399px"></asp:TextBox>
&nbsp;
                <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" Text="Add / Update" CssClass="styled-button-3" />
            </div>
        </div>
        <asp:HiddenField ID="surveyList" runat="server" />
    </form>
</body>
</html>
