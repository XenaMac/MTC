var ipAdd;
var txtInstaller;
var txtVehicleID;
var txtIPAdress;
var txtSSN;
var txtIDate;
var txtQAssurance;
var txtSig;
var txtUSpeed;
var txtDspeed;
var txtTowTruckCompany;
var txtSecure;
var txtCell;
var txtPower;
var txtRouterSecure;
var txtMoisture;
var txtSpeedTest;
var txtGPS;

var check = new Array();
check[0];
check[1];
check[2];
check[3];
check[4];
check[5];
check[6];
check[7];
check[8];
check[9];
check[10];
check[11];
check[12];
var total = 0;
var clicked = true;

function checks() {
 
        txtInstaller = document.getElementById('installerName');
        if (txtInstaller.value == "") {
            document.getElementById('iName').innerHTML = "*";
            check[0] = 1;
        } else {
            document.getElementById('iName').innerHTML = "";
            check[0] = 0;
        }
        txtIDate = document.getElementById('installDate');
        if (txtIDate.value == "") {
            document.getElementById('ID').innerHTML = "*";
            check[4] = 1;
        } else {
            document.getElementById('ID').innerHTML = "";
            check[4] = 0;
        }
        txtTowTruckCompany = document.getElementById('towTruckCompany');
        if (txtTowTruckCompany.value == "") {
            document.getElementById('tTC').innerHTML = "*";
            check[5] = 1;
        } else {
            document.getElementById('tTC').innerHTML = "";
            check[5] = 0;
        }
        txtVehicleID = document.getElementById('vID');
        if (txtVehicleID.value == "") {
            document.getElementById('VehID').innerHTML = "*";
            check[1] = 1;
        } else {
            document.getElementById('VehID').innerHTML = "";
            check[1] = 0;
        }
        txtIPAdress = document.getElementById('IPAdd');
        if (txtIPAdress.value == "") {
            document.getElementById('IPA').innerHTML = "*";
            check[2] = 1;
        } else {
            document.getElementById('IPA').innerHTML = "";
            check[2] = 0;
        }
        txtSSN = document.getElementById('sysSerNumber');
        if (txtSSN.value == "") {
            document.getElementById('S').innerHTML = "*";
            check[3] = 1;
        } else {
            document.getElementById('S').innerHTML = "";
            check[3] = 0;
        }
        txtUSpeed = document.getElementById('UploadSpeed');
        if (txtUSpeed.value == "") {
            document.getElementById('US').innerHTML = "*";
            check[6] = 1;
        } else {
            document.getElementById('US').innerHTML = "";
            check[6] = 0;
        }
        txtDspeed = document.getElementById('DownloadSpeed');
        if (txtDspeed.value == "") {
            document.getElementById('DS').innerHTML = "*";
            check[7] = 1;
        } else {
            document.getElementById('DS').innerHTML = "";
            check[7] = 0;
        }


        //Check box checking
        txtSecure = document.getElementById('Secure');
        if (txtSecure.checked == false) {
            document.getElementById('Sec').innerHTML = "*";
            check[8] = 1;
        } else {
            document.getElementById('Sec').innerHTML = "";
            check[8] = 0;
        }
        txtCell = document.getElementById('Cell');
        if (txtCell.value == 0) {
            document.getElementById('Ce').innerHTML = "";
            check[9] = 1;
        } else {
            document.getElementById('Ce').innerHTML = "*";
            check[9] = 0;
        }
        txtPower = document.getElementById('Power');
        if (txtPower.value == 0) {
            document.getElementById('Pow').innerHTML = "";
            check[10] = 1;
        } else {
            document.getElementById('Pow').innerHTML = "*";
            check[10] = 0;
        }
        txtRouterSecure = document.getElementById('routerSecure');
        if (txtRouterSecure.value == 0) {
            document.getElementById('RoutSec').innerHTML = "";
            check[11] = 1;
        } else {
            document.getElementById('RoutSec').innerHTML = "*";
            check[11] = 0;
        }
        txtMoisture = document.getElementById('Moisture');
        if (txtMoisture.value == 0) {
            document.getElementById('Moist').innerHTML = "";
            check[12] = 1;
        } else {
            document.getElementById('Moist').innerHTML = "*";
            check[12] = 0;
        }
        txtSpeedTest = document.getElementById('SpeedTest');
        if (txtSpeedTest.value == 0) {
            document.getElementById('SpeedT').innerHTML = "";
            check[13] = 1;
        } else {
            document.getElementById('SpeedT').innerHTML = "*";
            check[13] = 0;
        }
        txtGPS = document.getElementById('GPS');
        if (txtGPS.value == 0) {
            document.getElementById('G').innerHTML = "";
            check[14] = 1;
        } else {
            document.getElementById('G').innerHTML = "*";
            check[14] = 0;
        }
        

        for (var i = 0; i < check.length; i++) {
            if (check[i] == 1) {
                document.getElementById('emsg').innerHTML = "* Indicates a required field.";
                break;
            }
        }
        if (check[0] == 0
                        && check[1] == 0
                        && check[2] == 0
                        && check[3] == 0
                        && check[4] == 0
                        && check[5] == 0
                        && check[6] == 0
                        && check[7] == 0
                        && check[8] == 0
                        && check[9] == 0
                        && check[10] == 0
                        && check[11] == 0
                        && check[12] == 0
                        && check[13] == 0
                        && check[14] == 0)) 
                        {
            document.getElementById('emsg').innerHTML = "";
        }

    }


function errorMsg() {
    clicked = true;
}

function showScreenRes() {
    document.write("Your screen resolution is " + screen.width + "x" + screen.height + ".");
}