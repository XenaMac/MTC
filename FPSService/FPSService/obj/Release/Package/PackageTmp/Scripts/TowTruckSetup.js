$(document).ready(function () {

    LoadCurrentTrucks();

    function LoadCurrentTrucks() {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: "../AJAXFSPService.svc/GetAllTrucks",
            contentType: "application/json; charset=utf-8",
            success: GetTrucksSuccess,
            error: GetTrucksError
        });
    }

    function GetTrucksSuccess(result) {
        var _data = result.d;
        var _truckList = $.parseJSON(_data);
        var _tblCode = '<center>';
        _tblCode += '<table class="hovertable">';
        _tblCode += '<tr><th>TruckID</th><th>IP Address</th><th>Lat</th><th>Lon</th></tr>';

        $('#connects').empty();
        for (var i = 0; i < _truckList.length; i++) {
            _tblCode += '<tr>';
            _tblCode += '<td>' + _truckList[i].TruckID + '</td>';
            _tblCode += '<td>' + _truckList[i].IPAddress + '</td>';
            _tblCode += '<td>' + _truckList[i].Lat + '</td>';
            _tblCode += '<td>' + _truckList[i].Lon + '</td>';
            _tblCode += '</tr>';
        }
        _tblCode += '</table>';
        _tblCode += '</center>';
        $('#connects').append(_tblCode);
    }

    function GetTrucksError(error) {
        alert(error.responseText);
    }
});