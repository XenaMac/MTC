<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="OnlineSurveys2.Admin.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LATA Online Survey Admin</title>
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
        <img alt="MTC Banner" class="auto-style1" src="../images/mtc_survey_v2_1280x161%20banner.png" />
    <header>LATA Online Survey Administration</header>
    <div>
        <div><label class="surveySubHead">Survey Editing</label></div>
        <div id="outer" class="outer">
            <div id="inner" class="inner">
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Admin/SurveyEditor.aspx">Create/Edit a Survey</asp:HyperLink>
                <br />
                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Admin/QuestionEditor.aspx">Create/Edit Questions</asp:HyperLink>
                <br />
                <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Admin/SurveyQuestions.aspx">Add/Remove Question from Survey</asp:HyperLink>
            </div>
        </div>
    </div>

    <hr />
    <div>
        <div><label class="surveySubHead">Survey Testing</label></div>
        <div class="outer">
            <div class="inner">
                <label>Test Survey:</label>
                <asp:DropDownList ID="ddlSurveys" runat="server" Height="25px" Width="377px">
                </asp:DropDownList>
                &nbsp;
                <asp:Button ID="btnTestSurvey" runat="server" OnClick="btnTestSurvey_Click" Text="Test Survey" CssClass="styled-button-3" />
            </div>
        </div>
    </div>
    <hr />
    <div>
        <div><label class="surveySubHead">Survey Analytics</label></div>
        <div class="outer">
            <div class="inner">
                <asp:GridView ID="gvSurveyList" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:ButtonField CommandName="AnalyzeSurvey" Text="Select" ButtonType="Button" >
                        <ControlStyle CssClass="styled-button-3" />
                        </asp:ButtonField>
                    </Columns>
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
            </div>
        </div>
    </div>
        <!--
        <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/Admin/LinearAnalytics.aspx">Linear Analytics</asp:HyperLink>
        <br />
        <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/Admin/SurveyExporter.aspx">Survey Exporter</asp:HyperLink>
        -->
    </form>
</body>
</html>
