$(document).ready(function () {

    LoadCurrentState();

    window.setInterval(function () { LoadCurrentState(); }, 10000);

    function LoadCurrentState() {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: "../AJAXFSPService.svc/GetAllState",
            contentType: "application/json; charset=utf-8",
            success: GetStateSuccess,
            error: GetStateError
        });
    }

    function GetStateSuccess(result) {
        $('#CurrentState').empty();
        var _data = result.d;
        var _truckList = $.parseJSON(_data);
        var _tblCode = '<div class="subTitle"><h2>Currently Connected Trucks State</h2></div>';
        _tblCode += '<center>';
        _tblCode += '<table class="hovertable">';
        _tblCode += '<tr><th>TruckNumber</th><th>IP Address</th><th>GPS Rate</th><th>Log</th><th>Version</th><th>Server IP</th><th>SFTP Server IP</th></tr>';
        for (var i = 0; i < _truckList.length; i++) {
            _tblCode += '<tr>';
            _tblCode += '<td>' + _truckList[i].TruckNumber + '</td>';
            _tblCode += '<td>' + _truckList[i].IPAddress + '</td>';
            _tblCode += '<td>' + _truckList[i].GPSRate + '</td>';
            _tblCode += '<td>' + _truckList[i].Log + '</td>';
            _tblCode += '<td>' + _truckList[i].Version + '</td>';
            _tblCode += '<td>' + _truckList[i].ServerIP + '</td>';
            _tblCode += '<td>' + _truckList[i].SFTPServerIP + '</td>';
            _tblCode += '</tr>';
        }
        _tblCode += '</table>';
        _tblCode += '</center>';
        $('#CurrentState').append(_tblCode);
    }

    function GetStateError(error) {
        alert(error.responseText);
    }

});