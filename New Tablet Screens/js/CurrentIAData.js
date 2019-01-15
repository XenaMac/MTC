$(document).ready(function(){
	
	$('#btnGetIncident').click(function(){
		getIncident();
	});
	
	$('#btnGetAssistList').click(function(){
		getAssistList();
	});
	
	function getIncident(){
		var _url = ServiceLocation + 'AJAXFSPService.svc/getCurrentIncident';

        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            contentType: "application/json; charset=utf-8",
            success: getIncidentSuccess,
            error: getIncidentError
        });
	}
	
	function getIncidentSuccess(result){
		$('#returnData').empty();
		var _data = result.d;
		if(_data == 'NOTRUCK' || _data == 'NOINCIDENT'){
			$('#returnData').append(_data);
			return;
		}
		var i = $.parseJSON(_data);
		var _tblCode = '<table>';
		_tblCode += '<thead><th>key</th><th>val</th></thead>';
		_tblCode += '<tbody>';
		_tblCode += '<tr><td>incidentID</td><td>' + i.incidentID + '</td></tr>';
		_tblCode += '<tr><td>incidentDatePosted</td><td>' + FixDate(i.incidentDatePosted) + '</td></tr>';
		_tblCode += '<tr><td>userPosted</td><td>' + i.userPosted + '</td></tr>';
		_tblCode += '<tr><td>callSign</td><td>' + i.callSign + '</td></tr>';
		_tblCode += '<tr><td>fromTruck</td><td>' + i.fromTruck + '</td></tr>';
		_tblCode += '<tr><td>lat</td><td>' + i.lat + '</td></tr>';
		_tblCode += '<tr><td>lon</td><td>' + i.lon + '</td></tr>';
		_tblCode += '<tr><td>canceled</td><td>' + i.canceled + '</td></tr>';
		_tblCode += '<tr><td>reasonCanceled</td><td>' + i.reasonCanceled + '</td></tr>';
		_tblCode += '<tr><td>beat</td><td>' + i.beat + '</td></tr>';
		_tblCode += '<tr><td>truckNumber</td><td>' + i.truckNumber + '</td></tr>';
		_tblCode += '<tr><td>logID</td><td>' + i.logID + '</td></tr>';
		_tblCode += '<tr><td>wazeID</td><td>' + i.wazeID + '</td></tr>';
		_tblCode += '<tr><td>truckStatusID</td><td>' + i.truckStatusID + '</td></tr>';
		_tblCode += '<tr><td>FSPLocation</td><td>' + i.FSPLocation + '</td></tr>';
		_tblCode += '<tr><td>dispatchLocation</td><td>' + i.dispatchLocation + '</td></tr>';
		_tblCode += '<tr><td>direction</td><td>' + i.direction + '</td></tr>';
		_tblCode += '<tr><td>positionIncident</td><td>' + i.positionIncident + '</td></tr>';
		_tblCode += '<tr><td>laneNumber</td><td>' + i.laneNumber + '</td></tr>';
		_tblCode += '<tr><td>chpIncidentType</td><td>' + i.chpIncidentType + '</td></tr>';
		_tblCode += '<tr><td>briefUpdateLat</td><td>' + i.briefUpdateLat + '</td></tr>';
		_tblCode += '<tr><td>briefUpdateLon</td><td>' + i.briefUpdateLon + '</td></tr>';
		_tblCode += '<tr><td>freeway</td><td>' + i.freeway + '</td></tr>';
		_tblCode += '<tr><td>briefUpdatePosted</td><td>' + i.briefUpdatePosted + '</td></tr>';
		_tblCode += '<tr><td>timeOfBriefUpdate</td><td>' + FixDate(i.timeOfBriefUpdate) + '</td></tr>';
		_tblCode += '<tr><td>CHPLogNumber</td><td>' + i.CHPLogNumber + '</td></tr>';
		_tblCode += '<tr><td>incidentSurveyNumber</td><td>' + i.incidentSurveyNumber + '</td></tr>';
		_tblCode += '<tr><td>driverLastName</td><td>' + i.driverLastName + '</td></tr>';
		_tblCode += '<tr><td>driverFirstName</td><td>' + i.driverFirstName + '</td></tr>';
		_tblCode += '<tr><td>driverID</td><td>' + i.driverID + '</td></tr>';
		_tblCode += '<tr><td>runID</td><td>' + i.runID + '</td></tr>';
		_tblCode += '<tr><td>timeOnIncident</td><td>' + FixDate(i.timeOnIncident) + '</td></tr>';
		_tblCode += '<tr><td>timeOffIncident</td><td>' + FixDate(i.timeOffIncident) + '</td></tr>';
		_tblCode += '<tr><td>sentToTruck</td><td>' + i.sentToTruck + '</td></tr>';
		_tblCode += '<tr><td>acked</td><td>' + i.acked + '</td></tr>';
		_tblCode += '<tr><td>comment</td><td>' + i.comment + '</td></tr>';
		_tblCode += '<tr><td>crossStreet</td><td>' + i.crossStreet + '</td></tr>';
		_tblCode += '<tr><td>requestSent</td><td>' + i.requestSent + '</td></tr>';
		_tblCode += '</tbody>';
		_tblCode += '</table>';
		$('#returnData').append(_tblCode);
	}
	
	function getIncidentError(error){
		var e = error;
	}
	
	function getAssistList(){
		var _url = ServiceLocation + 'AJAXFSPService.svc/getCurrentAssist';

        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            contentType: "application/json; charset=utf-8",
            success: getAssistListSuccess,
            error: getAssistListError
        });
	}
	
	function getAssistListSuccess(result){
		$('#returnData').empty();
		var _data = result.d;
		if(_data == 'NOTRUCK' || _data == 'NOASSIST'){
			$('#returnData').append(_data);
			return;
		}
		var a = $.parseJSON(_data);
		var _tblCode = '<table>';
		_tblCode += '<thead><th>key</th><th>val</th></thead>';
		_tblCode += '<tbody>';
		for(var i = 0;i < a.length;i++){
			_tblCode += '<tr><td>assistID</td><td>' + a[i].assistID + '</td></tr>';
			_tblCode += '<tr><td>incidentID</td><td>' + a[i].incidentID + '</td></tr>';
			_tblCode += '<tr><td>assistDatePosted</td><td>' + FixDate(a[i].assistDatePosted) + '</td></tr>';
			_tblCode += '<tr><td>lastAssistInIncidentReport</td><td>' + a[i].lastAssistInIncidentReport + '</td></tr>';
			_tblCode += '<tr><td>problemType</td><td>' + a[i].problemType + '</td></tr>';
			_tblCode += '<tr><td>problemDetail</td><td>' + a[i].problemDetail + '</td></tr>';
			_tblCode += '<tr><td>problemNote</td><td>' + a[i].problemNote + '</td></tr>';
			_tblCode += '<tr><td>otherNote</td><td>' + a[i].otherNote + '</td></tr>';
			_tblCode += '<tr><td>transportType</td><td>' + a[i].transportType + '</td></tr>';
			_tblCode += '<tr><td>StartODO</td><td>' + a[i].StartODO + '</td></tr>';
			_tblCode += '<tr><td>EndODO</td><td>' + a[i].EndODO + '</td></tr>';
			_tblCode += '<tr><td>dropSite</td><td>' + a[i].dropSite + '</td></tr>';
			_tblCode += '<tr><td>state</td><td>' + a[i].state + '</td></tr>';
			_tblCode += '<tr><td>licensePlate</td><td>' + a[i].licensePlate + '</td></tr>';
			_tblCode += '<tr><td>vehicleType</td><td>' + a[i].vehicleType + '</td></tr>';
			_tblCode += '<tr><td>OTAuthorizationNumber</td><td>' + a[i].OTAuthorizationNumber + '</td></tr>';
			_tblCode += '<tr><td>detailNote</td><td>' + a[i].detailNote + '</td></tr>';
			_tblCode += '<tr><td>assistLat</td><td>' + a[i].assistLat + '</td></tr>';
			_tblCode += '<tr><td>assistLon</td><td>' + a[i].assistLon + '</td></tr>';
			_tblCode += '<tr><td>dropSiteOther</td><td>' + a[i].dropSiteOther + '</td></tr>';
			_tblCode += '<tr><td>callSign</td><td>' + a[i].callSign + '</td></tr>';
			_tblCode += '<tr><td>timeOnAssist</td><td>' + FixDate(a[i].timeOnAssist) + '</td></tr>';
			_tblCode += '<tr><td>timeOffAssist</td><td>' + FixDate(a[i].timeOffAssist) + '</td></tr>';
			_tblCode += '<tr><td>actionTaken</td><td>' + a[i].actionTaken + '</td></tr>';
		}
		
		_tblCode += '</tbody>';
		_tblCode += '</table>';
		$('#returnData').append(_tblCode);
	}
	
	function getAssistListError(error){
		var e = error;
	}
	
	function FixDate(dtVal) {
		if(dtVal == "null" || dtVal == null){
			return "NOT SET";
		}
        try {
            var valFix = dtVal;
            valFix = dtVal.replace("/Date(", "").replace(")/", "");
            var iMil = parseInt(valFix, 10);
            var d = new Date(iMil);
            var Month = (d.getMonth() + 1).toString();
            if (Month.length < 2)
            { Month = "0" + Month; }
            var _date = (d.getDate()).toString();
            if (_date.length < 2)
            { _date = "0" + _date; }
            var Year = (d.getFullYear()).toString();
            var Hour = (d.getHours()).toString();
            if (Hour.length < 2)
            { Hour = "0" + Hour; }
            var Minute = (d.getMinutes()).toString();
            if (Minute.length < 2)
            { Minute = "0" + Minute; }
            var Second = (d.getSeconds()).toString();
            if (Second.length < 2)
            { Second = "0" + Second; }
            return Month + "/" + _date + "/" + Year + " " + Hour + ":" + Minute + ":" + Second;
        }
        catch (err) {
            return "Bad Input";
        }
    }
});