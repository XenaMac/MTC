<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TowTruckCheckList.aspx.cs" Inherits="FPSService.Admin.TowTruckCheckList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="shortcut icon" href="../Images/LATA Trax_icon_checkbox.png"/>
    <title>Tow Truck Check List</title>
<script type="application/javascript" src="http://jsonip.appspot.com/?callback=getip"></script>
<script src="../Scripts/TowTruckCheckList.js" type="text/javascript"></script> 
<script src="http://s3.amazonaws.com/speedchecker/speedchecker.js" type="text/javascript"></script>  
<link href="../css/TowTruckCheckList.css" rel="Stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server" name="form1">
    <div id="page_container">  
        <div id="header">
            <div id="hd1">               
                    <img src="../Images/LATATRAX LOGO.png"  alt="../Images/12-012 LOGO - LATA Trax_PNG-01.png"/>   
            </div>
            <div id="hd2">
                <div id="hd3">
                    <img src="../Images/LATATRAX ScreenName.png"  alt="../Images/12-012 LOGO - LATA Trax_PNG-01.png"/>
                </div>
            </div>
        </div>
   
        <div id="inputDiv">
            <div id="inputAll">
                <div class="indiInput">
                    <label for="installerName">Installer Name:</label><br />
                    <span id="iName" runat="server" type="text" ></span>
                    <asp:TextBox id="installerName" name="installerName" type="text" class="text" value="" runat="server"  />
                 </div>
                 <div class="indiInput">
                    <label for="installDate">Installation Date:</label><br />
                    <span id="ID" runat="server" type="text" ></span>
                    <asp:TextBox id="installDate" name="installDate" type="text" class="text" value="" runat="server"  />
                 </div>
                 <div class="indiInput">
                    <label for="towTruckCompany">Tow Truck Company:</label><br />
                    <span id="tTC" type="text" runat="server"></span>
                    <asp:TextBox id="towTruckCompany" name="towTruckCompany" type="text" class="text" value="" runat="server" />
                 </div>
                 <div class="indiInput">
                    <label for="vID">VehicleID:</label><br />
                    <span id="VehID" type="text" runat="server"></span>
                    <asp:TextBox id="vID" name="vID" type="text" class="text"  value="" runat="server" />
                 </div>
                  <div class="indiInput">
                    <label for="sysSerNumber">System Serial Number:</label><br />
                    <span id="S" type="text" runat="server"></span>
                    <asp:TextBox id="sysSerNumber" name="sysSerNumber" type="text" class="text" value="" runat="server" />
                 </div> 
                 <div class="indiInput">
                    <label for="IPAdd">IP Address:</label><br />
                    <span id="IPA" type="text" runat="server"></span>
                    <asp:TextBox ID="IPAdd" name="IPAdd" type="text" class="text" value="" runat="server"  />
                 </div>
                  <div class="indiInput">
                 <label for="SCC">Select Contractor Company:</label><br />
                 <span id="Cont" type="text" runat="server"></span>
                 <asp:DropDownList ID="ddlContractors" runat="server">
                 </asp:DropDownList>
                 </div>

                 <div id="emsg" type="text" runat="server"></div>
                 <p>
                 <!--<button id="registerNew" type="button" name="" value="" class="css3button" onserverclick="Button1_Click" onclick="errorMsg()" runat="server" >Add New Truck</button>-->
                 <asp:Button ID="Button1" Text="Add New Truck" runat="server" class="css3button" OnClick="Button1_Click" />
                 </p>
            </div>
        </div>
        <div id="cboxDiv">
            <div id="cboxAll">
                <div class="indiCbox">
                <p>
                    <span id="Sec" type="text" runat="server"></span>
                    <asp:CheckBox class="cbox" ID="Secure" runat="server" value="0" oncheckedchanged="YES_CheckedChanged1" />
                    <label class="lbox" for="Secure">Roof antenna mounted securely?</label>   
                 </p>
                 </div>
                 <div class="indiCbox">
                 <p>
                 <span id="Ce" type="text" runat="server"></span>
                    <asp:CheckBox class="cbox" ID="Cell" runat="server" oncheckedchanged="YES_CheckedChanged1" />
                    <label class="lbox" for="Cell">RF cables connected to "GPS", "CELL", and "WIFI"?</label>
                 </p>
                 </div>
                 <div class="indiCbox">
                 <p> 
                 <span id="Pow" type="text" runat="server"></span>
                    <asp:CheckBox class="cbox" ID="Power" runat="server" oncheckedchanged="YES_CheckedChanged1" />
                    <label class="lbox" for="Power">DC power connected with no exposed wires?</label>
                 </p>
                 </div>
                 <div class="indiCbox">
                 <p>
                 <span id="RoutSec" type="text" runat="server"></span> 
                    <asp:CheckBox class="cbox" ID="routerSecure" runat="server" oncheckedchanged="YES_CheckedChanged1" />
                    <label class="lbox" for="routerSecured">Router unit mounted securely?</label>
                 </p> 
                 </div>
                 <div class="indiCbox">
                  <p> 
                  <span id="Moist" type="text" runat="server"></span>
                    <asp:CheckBox class="cbox" ID="Moisture" runat="server" oncheckedchanged="YES_CheckedChanged1" />
                    <label class="lbox" for="Moisture">Router unit free from moisture and heat source?</label>
                 </p> 
                 </div>  
                 <div class="indiCbox">
                  <p> 
                  <span id="SpeedT" type="text" runat="server"></span>
                    <asp:CheckBox class="cbox" ID="SpeedTest" runat="server" oncheckedchanged="YES_CheckedChanged1" />
                    <label class="lbox" for="SpeedTest">Speed test was performed successfully?</label>
                 </p> 
                 </div> 
                 <div class="indiCbox">
                 
                 <div id="blah">
                 <p><font size="2.5px" color="#9FBD19">If GPS is being sent properly the box below will be automatically checked.<br /><br /></font>
                    <span id="G" type="text" runat="server"></span>
                    <asp:CheckBox class="cbox" ID="GPS" runat="server" oncheckedchanged="YES_CheckedChanged1" />
                    <label class="lbox" for="GPS">GPS data is being sent properly?</label>
                    </p>
                    </div>
                 
                 </div>  
            </div>
        </div>
        <div id="speedHolder">
        <div id="speedchecker_link"><font size="1" color="#9FBD19">&nbsp&nbsp&nbsp&nbsp&nbsp Powered by </font><a href="http://www.broadbandspeedchecker.co.uk"><font size="2">Broadband Speed Test</font></a></div>
        <div id="speedcheckerdiv">
        <iframe src="http://www.broadbandspeedchecker.co.uk/html5_speedchecker.html?sc_w=375&amp;sc_h=235&amp;holdingPageUrl=http%3A//www.broadbandspeedchecker.co.uk/&amp;baseDownloadUrl=http%3A//cdn1.broadbandspeedchecker.co.uk/&amp;licenseID=c2fb400b7f9c484cfef9ea69a0a48ca3&amp;speedchecker_linkID=&amp;serverType=aspx&amp;hostedUpload=1" scrolling="no" width="375px" height="235px" marginheight="0" marginwidth="0" frameborder="0"></iframe>
        </div>
        <script language="javascript" type="text/javascript">sc_hc = "0xCC0000"; sc_bc = "0xFFFFFF"; sc_bgc = "0x123456"; sc_cc = "0x99FF00"; sc_w = 300; sc_location = "US"; sc_skin = ""; sc_userid = 22246106; {}
        </script><script src="http://s3.amazonaws.com/speedchecker/speedchecker.js" type="text/javascript"></script>
        <div class="indiInput">
                    <label for="DownloadSpeed"><br />DOWNLOAD SPEED:</label><br />
                    <span id="DS" type="text" runat="server"></span>
                    <asp:TextBox id="DownloadSpeed" name="DownloadSpeed" type="text" class="text" value="" runat="server" onChange="checks()" />
                    <span><label>(Mb/s)</label></span>
            </div>
            <div class="indiInput">
                    <label for="UploadSpeed">UPLOAD SPEED:</label><br />
                    <span id="US" type="text" runat="server"></span>
                    <asp:TextBox id="UploadSpeed" name="UploadSpeed" type="text" class="text" value="" runat="server" onChange="checks()" />
                    <span><label>(Mb/s)</label></span>
            </div>
            <div id="speedchecker_link2"></div>
            </div>         
        <div id="footer">
            <p>Los Alamos Technical Associates, Inc.</p>
        </div>
	</div>     
    </form>
</body>
</html>



