$(document).ready(function () {

    LoadCurrentDrivers();
    window.setInterval(function () { LoadCurrentDrivers(); }, 10000);

    function LoadCurrentDrivers() {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: "../AJAXFSPService.svc/GetDrivers",
            contentType: "application/json; charset=utf-8",
            success: GetDriversSuccess,
            error: GetDriversError
        });
    }

    function GetDriversSuccess(result) {
        var _data = result.d;
        var _driverList = _data;
        $('#CurrentDrivers').empty();
        var _tblCode = '<div class="subTitle"><h2>Current Drivers</h2></div>';
        _tblCode += '<center>';
        _tblCode += '<table class="dashboardTable">';
        _tblCode += '<thead><tr><th>Log Off</th><th>Truck Number</th><th>Driver Name</th><th>Contract Company</th></tr></thead>';
        _tblCode += '<tbody>';
        for (var i = 0; i < _driverList.length; i++) {
            _tblCode += '<tr>';
            _tblCode += '<td><input type="button" id="' + _driverList[i].DriverID + '" value="Log Off" class="myButtonSmall"/></td>';
            _tblCode += '<td>' + _driverList[i].TruckNumber + '</td>';
            _tblCode += '<td>' + _driverList[i].DriverName + '</td>';
            _tblCode += '<td>' + _driverList[i].ContractorName + '</td>';
            _tblCode += '</tr>';
        }
        _tblCode += '</tbody>';
        _tblCode += '</table>';
        $('#CurrentDrivers').append(_tblCode);
        $('.logoffClass').on("click", LogOffDriver);

    }

    function GetDriversError(error) { }

    function LogOffDriver() {
        var id = this.id;
        var _data = "_DriverID=" + id;
        $.ajax({
            type: "GET",
            dataType: "json",
            url: "../AJAXFSPService.svc/LogOffDriver",
            data: _data,
            success: alert('Driver logged off, wait for refresh')
        });
    }
});