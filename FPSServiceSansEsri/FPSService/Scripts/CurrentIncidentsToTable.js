$(document).ready(function () {

    LoadCurrentIncidents();
    //Load data from AJAXFSPService.GetAllTrucks into an HTML Table and append the table to a div with id=CurrentConnects
    window.setInterval(function () { LoadCurrentIncidents(); }, 10000);

    function LoadCurrentIncidents() {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: "../AJAXFSPService.svc/GetAllIncidents",
            contentType: "application/json; charset=utf-8",
            success: GetIncidentsSuccess,
            error: GetIncidentsError
        });
    }

    function GetIncidentsSuccess(result) {
        var _data = result.d;
        if (_data != "err") {
            var _incidentList = $.parseJSON(_data);
            var _tblCode = '<div class="subTitle"><h2>Current Incidents</h2></div>';
            _tblCode += '<center>';
            _tblCode += '<table class="dashboardTable">';
            _tblCode += '<tr><th>Remove</th><th>Incident ID</th><th>Direction</th><th>Cross Street 1</th><th>Cross Street 2</th><th>Freeway</th><th>Location</th><th>Beat Number</th><th>Time Stamp</th><th>Created By</th>' +
            '<th>Description</th><th>Incident Number</th></tr>';
            $('#CurrentIncidents').empty();
            for (var i = 0; i < _incidentList.length; i++) {
                _tblCode += '<tr>';
                _tblCode += '<td><button class="clearIncident" id="' + _incidentList[i].IncidentID + '">Remove</button></td>';
                _tblCode += '<td>' + _incidentList[i].IncidentID + '</td>';
                _tblCode += '<td>' + _incidentList[i].Direction + '</td>';
                _tblCode += '<td>' + _incidentList[i].CrossStreet1 + '</td>';
                _tblCode += '<td>' + _incidentList[i].CrossStreet2 + '</td>';
                _tblCode += '<td>' + _incidentList[i].Freeway + '</td>';
                _tblCode += '<td>' + _incidentList[i].Location + '</td>';
                _tblCode += '<td>' + _incidentList[i].BeatNumber + '</td>';
                _tblCode += '<td>' + _incidentList[i].TimeStamp + '</td>';
                _tblCode += '<td>' + _incidentList[i].CreatedBy + '</td>';
                _tblCode += '<td>' + _incidentList[i].Description + '</td>';
                _tblCode += '<td>' + _incidentList[i].IncidentNumber + '</td>';
                _tblCode += '</tr>'
            }
            _tblCode += '</table></center>';
            $('#CurrentIncidents').append(_tblCode);
            $(".clearIncident").on("click", RemoveIncident);
        }

    }

    function GetIncidentsError(error) {
        alert("Bad fooey");
    }

    function RemoveIncident() {
        var ID = $(this).attr("id");
        var _data = "_IncidentID=" + ID.toString();
        $.ajax({
            type: "GET",
            dataType: "json",
            url: "../AJAXFSPService.svc/ClearIncident",
            contentType: "application/json; charset=utf-8",
            data: _data,
            success: ClearIncidentSuccess,
            error: ClearIncidentError
        });
        return false;
    }

    function ClearIncidentSuccess() {
        LoadCurrentIncidents();
    }

    function ClearIncidentError(request, type, errorThrown) {
        alert(request.responseText);
    }

});