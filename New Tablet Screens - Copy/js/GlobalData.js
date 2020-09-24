	var DriverName = "";
	var TruckID = "";
	var TruckStatus = "";
    var ServiceLocation = "http://38.124.164.212:9017/";
	//var ServiceLocation = "http://localhost:9017/";
	var ClientStatus = "LoggedOff";

	function clearLocalIncidentStorage(){
		localStorage['direction'] = null;
		localStorage['position'] = null;
		localStorage['laneNumber'] = null;
		localStorage['incidentType'] = null;
		localStorage['incidentID'] = null;
		localStorage['position'] = null;
		localStorage['laneNumber'] = null;
		localStorage['chpIncidentType'] = null;
		localStorage['chpLogNumber'] = null;
		localStorage['incidentSurveyNumber'] = null;
	}

	function clearLocalAssistStorage(){
		localStorage['assistID'] = null;
		localStorage['licensePlate'] = null;
		localStorage['startODO'] = null;
		localStorage["problemType"] = null;
		localStorage["problemNotes"] = null;
		localStorage["actionTaken"] = null;
		localStorage["actionNotes"] = null;
		localStorage["transportType"] = null;
		localStorage["startODO"] = null;
		localStorage["endODO"] = null;
		localStorage["dropSite"] = null;
		localStorage["dropSiteOther"] = null;
		localStorage["state"] = null;
		localStorage["licensePlate"] = null;
		localStorage["vehicleType"] = null;
		localStorage["OTAuthorizationNumber"] = null;
		localStorage["detailNotes"] = null;
	}

	function isGuid(value) {
		var regex = /[a-f0-9]{8}(?:-[a-f0-9]{4}){3}-[a-f0-9]{12}/i;
		var match = regex.exec(value);
		return match != null;
	}