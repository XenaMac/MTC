<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="controltestpage.aspx.cs" Inherits="OnlineSurveys2.Admin.controltestpage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../themes/SurveyStyle.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        <!--
        <div><label>I think about things</label></div>
        <div class="scrollList">
            <label class="questionTextLabel">Select Answers</label>
        <asp:CheckBoxList ID="CheckBoxList1" runat="server" Height="101px">
            <asp:ListItem>Yes</asp:ListItem>
            <asp:ListItem>No</asp:ListItem>
            <asp:ListItem>Maybe</asp:ListItem>
            <asp:ListItem>Kinda</asp:ListItem>
            <asp:ListItem>Sorta</asp:ListItem>
            <asp:ListItem>Dunno</asp:ListItem>
            <asp:ListItem>Dude</asp:ListItem>
        </asp:CheckBoxList>
        </div>
        <div><label>Can we go thrift shopping?</label></div>
        <div class="scrollList">
            <label class="questionTextLabel">Select Answers</label>
        <asp:CheckBoxList ID="CheckBoxList2" runat="server" Height="101px">
            <asp:ListItem>Yes</asp:ListItem>
            <asp:ListItem>No</asp:ListItem>
            <asp:ListItem>Maybe</asp:ListItem>
            <asp:ListItem>Kinda</asp:ListItem>
            <asp:ListItem>Sorta</asp:ListItem>
            <asp:ListItem>Dunno</asp:ListItem>
            <asp:ListItem>Dude</asp:ListItem>
        </asp:CheckBoxList>
        </div>
                <div><label>I think about things</label></div>
        <div class="scrollList">
            <label class="questionTextLabel">Select Answers</label>
        <asp:RadioButtonList ID="rbl1" runat="server" Height="101px">
            <asp:ListItem>Yes</asp:ListItem>
            <asp:ListItem>No</asp:ListItem>
            <asp:ListItem>Maybe</asp:ListItem>
            <asp:ListItem>Kinda</asp:ListItem>
            <asp:ListItem>Sorta</asp:ListItem>
            <asp:ListItem>Dunno</asp:ListItem>
            <asp:ListItem>Dude</asp:ListItem>
        </asp:RadioButtonList>
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        </div>
        -->
        <asp:DropDownList ID="DropDownList1" runat="server">
        </asp:DropDownList>
&nbsp;<asp:Button ID="btnGetSelected" runat="server" OnClick="btnGetSelected_Click" Text="get selected" />
        <br />
        <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple"></asp:ListBox>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Get Selected" />
    </form>
</body>
</html>
