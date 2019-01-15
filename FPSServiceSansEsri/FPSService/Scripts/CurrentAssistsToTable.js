$(document).ready(function () {

    LoadCurrentAssists();
    //Load data from AJAXFSPService.GetAllTrucks into an HTML Table and append the table to a div with id=CurrentConnects
    window.setInterval(function () { LoadCurrentAssists(); }, 10000);

    function LoadCurrentAssists() {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: "../AJAXFSPService.svc/GetAllAssists",
            contentType: "application/json; charset=utf-8",
            success: GetAssistsSuccess,
            error: GetAssistsError
        });
    }

    function GetAssistsSuccess(result) {
        var _data = result.d;
        if (_data != "err");
        {
            var _assistList = $.parseJSON(_data);
            var _tblCode = '<div class="subTitle"><h2>Current Assists</h2></div>';
            _tblCode += '<center>';
            _tblCode += '<table class="dashboardTable">';
            _tblCode += '<tr><th>Remove</th><th>Assist ID</th><th>Incident ID</th><th>Assist Number</th><th>Driver</th><th>Truck Number</th><th>Dispatch Time</th><th>Customer Wait Time</th><th>Vehicle Position</th><th>Incident Type</th>' +
            '<th>Traffic Speed</th><th>Service Type</th><th>Drop Zone</th><th>Make</th><th>Vehicle Type</th><th>Color</th><th>License Plate</th><th>State</th><th>Start OD</th><th>End OD</th>' +
            '<th>TowLocation</th><th>Tip</th><th>Tip Detail</th><th>Customer Last Name</th><th>Comments</th><th>MDC</th><th>1097 Time</th><th>1098 Time</th><th>Contractor</th><th>Log Number</th>' +
            '<th>Lat</th><th>Lon</th><th>Acked</th></tr>';
            $('#CurrentAssists').empty();
            for (var i = 0; i < _assistList.length; i++) {
                _tblCode += '<tr>';
                //_tblCode += '<td><a href="" class="clearAssist" id="' + _assistList[i].AssistID + '">Remove</a></td>';
                _tblCode += '<td><button class="clearAssist" id="' + _assistList[i].AssistID + '">Remove</button></td>';
                _tblCode += '<td>' + _assistList[i].AssistID + '</td>';
                _tblCode += '<td>' + _assistList[i].IncidentID + '</td>';
                _tblCode += '<td>' + _assistList[i].AssistNumber + '</td>';
                _tblCode += '<td>' + _assistList[i].Driver + '</td>';
                _tblCode += '<td>' + _assistList[i].FleetVehicle + '</td>';
                _tblCode += '<td>' + _assistList[i].DispatchTime + '</td>';
                _tblCode += '<td>' + _assistList[i].CustomerWaitTime + '</td>';
                _tblCode += '<td>' + _assistList[i].VehiclePosition + '</td>';
                _tblCode += '<td>' + _assistList[i].IncidentType + '</td>';
                _tblCode += '<td>' + _assistList[i].TrafficSpeed + '</td>';
                _tblCode += '<td>' + _assistList[i].ServiceType + '</td>';
                _tblCode += '<td>' + _assistList[i].DropZone + '</td>';
                _tblCode += '<td>' + _assistList[i].Make + '</td>';
                _tblCode += '<td>' + _assistList[i].VehicleType + '</td>';
                _tblCode += '<td>' + _assistList[i].Color + '</td>';
                _tblCode += '<td>' + _assistList[i].LicensePlate + '</td>';
                _tblCode += '<td>' + _assistList[i].State + '</td>';
                _tblCode += '<td>' + _assistList[i].StartOD + '</td>';
                _tblCode += '<td>' + _assistList[i].EndOD + '</td>';
                _tblCode += '<td>' + _assistList[i].TowLocation + '</td>';
                _tblCode += '<td>' + _assistList[i].Tip + '</td>';
                _tblCode += '<td>' + _assistList[i].TipDetail + '</td>';
                _tblCode += '<td>' + _assistList[i].CustomerLastName + '</td>';
                _tblCode += '<td>' + _assistList[i].Comments + '</td>';
                _tblCode += '<td>' + _assistList[i].IsMDC + '</td>';
                _tblCode += '<td>' + _assistList[i].x1097 + '</td>';
                _tblCode += '<td>' + _assistList[i].x1098 + '</td>';
                _tblCode += '<td>' + _assistList[i].Contractor + '</td>';
                _tblCode += '<td>' + _assistList[i].LogNumber + '</td>';
                _tblCode += '<td>' + _assistList[i].Lat + '</td>';
                _tblCode += '<td>' + _assistList[i].Lon + '</td>';
                _tblCode += '<td>' + _assistList[i].Acked + '</td>';
                _tblCode += '</tr>'
            }
            _tblCode += '</table></center>';
            $('#CurrentAssists').append(_tblCode);
            $(".clearAssist").on("click", RemoveAssist);
        }
    }

    function GetAssistsError(error) {
        //alert(error);
    }

    function RemoveAssist() {
        var ID = $(this).attr("id");
        var _data = "_AssistID=" + ID.toString();
        $.ajax({
            type: "GET",
            dataType: "json",
            url: "../AJAXFSPService.svc/ClearAssist",
            contentType: "application/json; charset=utf-8",
            data: _data,
            success: ClearAssistSuccess,
            error: ClearAssistError
        });
        return false;
    }

    function ClearAssistSuccess() {
        LoadCurrentAssists();
    }

    function ClearAssistError(request, type, errorThrown) {
        //alert(request.responseText);
    }

});