<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ThankYou.aspx.cs" Inherits="OnlineSurveys2.ThankYou" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Online Surveys - Provided by LATA</title>
    <link href="themes/SurveyStyle.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <header>Thank you for your participation</header>
        <div>You will be redirected back to the Metropolitan Transportation Commission&#39;s Freeway Service Patrol page in 30 seconds</div>
    </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Timer ID="Timer1" runat="server" Interval="30000" OnTick="Timer1_Tick">
        </asp:Timer>
    </form>
</body>
</html>
