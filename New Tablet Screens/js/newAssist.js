$(document).ready(function(){
	localStorage["beatNumber"] = "99";
	getDropSites(localStorage["beatNumber"]);
	checkCurrentData();
	
	$('#btnSubmitFinal').click(function(){
		//incidentID gets nulled after closeIncident. If that's happened, a new
		//new incident will need to be created to handle the assist
		if(localStorage["incidentID"] == null || localStorage["incidentID"] == "" || localStorage["incidentID"] == "null"){
			alert("No current incident, aborting");
			return;
		}
		clearVals();
		//each time an assist is posted the current assistid gets nulled,
		//check to see if that has happened and, if necessary, make a new one
		if(localStorage["assistID"] == null || localStorage["assistID"] == "" || localStorage["assistID"] == "null"){
			startAssist();
		}
		setVals();
		closeFinalAssist();
		
	});
	
	$('#btnSubmitAnother').click(function(){
		//incidentID gets nulled after closeIncident. If that's happened, a new
		//new incident will need to be created to handle the assist
		if(localStorage["incidentID"] == null || localStorage["incidentID"] == "" || localStorage["incidentID"] == "null"){
			alert("No current incident, aborting");
			return;
		}
		clearVals();
		//each time an assist is posted the current assistid gets nulled,
		//check to see if that has happened and, if necessary, make a new one
		if(localStorage["assistID"] == null || localStorage["assistID"] == "" || localStorage["assistID"] == "null"){
			startAssist();
		}
		setVals();
		closeAssist();
	});
	
	function checkCurrentData(){
		if(localStorage["incidentID"] == null || localStorage["incidentID"] == ""){
			alert("no incident");
		}
		if(localStorage["incidentID"] == 'ACTIVEINCIDENT'){
			//activeincident means there's an active incident for this
			//vehicle at the service so we have to close that one before
			//we start a new one, this will grab the current incident id
			//so we can use it here to update assists and close the incident
			getIncidentID();
		}
		$('#txtLicensePlate').val(localStorage["licensePlate"]);
		$('#txtStartODO').val(localStorage["startODO"]);
	}
	
	function clearVals(){
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
	
	function setVals(){
		//Problems
		var _problemType = '';
		var _collision = $('#selTrafficCollision option:selected').text();
		var _breakdown = $('#selBreakdown option:selected').text();
		var _debris = $('#selDebris option:selected').text();
		var _other = $('#selOther option:selected').text();
		if(_collision != 'Select'){
			_problemType += _collision + ',';
		}
		if(_breakdown != 'Select'){
			_problemType += _breakdown + ',';
		}
		if(_debris != 'Select'){
			_problemType += _debris + ',';
		}
		if(_other != 'Select'){
			_problemType += _other + ',';
		}
		_problemType = _problemType.substring(0, _problemType.length - 1);
		localStorage["problemType"] = _problemType;
		localStorage["problemNotes"] = $('#txtProblemNotes').val();
		//Actions taken
		var chkAction = $('input[name=action]:checked').map(function(){
			return this.id;
		}).get().join(',');
		if(chkAction != ""){
			var actionString = '';
			switch (chkAction){
				case 'chkPerformedAssist':
					actionString = 'ASST';
					break;
				case 'chkAssistedBeatPartner':
					actionString = 'PART';
					break;
				case 'chkAssistedPeaceOfficer':
					actionString = 'OFCR';
					break;
				case 'chkRefusedService':
					actionString = 'RFSD';
					break;
				case 'chkHelpEnroute':
					actionString = 'HLPW';
					break;
				case 'chkHelpEnrouteNoWait':
					actionString = 'HLPN';
					break;
				case 'chkMotoristDroveOff':
					actionString = 'DOFF';
					break;
				default:
					actionString = 'OTHR';
			}
			localStorage["actionTaken"] = actionString;
		}
		localStorage["actionNotes"] = $('#txtActionNotes').val();
		//Transportation
		var chkTransport = $('input[name=transport]:checked').map(function(){
			return this.id;
		}).get().join(',');
		if(chkTransport != ""){
			var transportString = '';
			transportString = chkTransport.replace("chkTransportedFemaleJuvenile","FJUV").replace("chkTransportedFemale","FPAS").
				replace("chkTransportedMaleJuvenile","MJUV").replace("chkTransportedMale","MPAS").
				replace("chkTransportedPush","P").replace("chkTransportedTow","T");
			localStorage["transportType"] = transportString;
		}
		var stOdo = $('#txtStartODO').val();
		var endOdo = $('#txtEndODO').val();
		
		if(stOdo == null || stOdo == "" || stOdo == 'Please enter'){
			localStorage["startODO"] = 0.0;
		}
		else{
			localStorage["startODO"] = stOdo;
		}
		
		if(endOdo == null || endOdo == "" || endOdo == 'Please enter'){
			localStorage["endODO"] = 0.0;
		}
		else{
			localStorage["endODO"] = endOdo;
		}
		localStorage["dropSite"] = $('#selDrop option:selected').text();
		if(localStorage["dropSite"] == 'Select'){
			//localStorage["dropZoneBeat"] = 'NA';
			localStorage["dropSite"] = 'NA';
			localStorage["dropSiteOther"] = 'NA';
		}
		if(localStorage["dropSite"] == 'Other'){
			localStorage["dropSiteOther"] = $('#txtOtherDrop').val();
			if(localStorage["dropSiteOther"] == ''){
				localStorage["dropSiteOther"] = 'Not Entered';
			}
		}
		//Vehicle Information
		localStorage["state"] = $('#selState option:selected').val();
		localStorage["licensePlate"] = $('#txtLicensePlate').val();
		localStorage["vehicleType"] = $('#selVehicleType option:selected').text();
		localStorage["OTAuthorizationNumber"] = $('#txtOvertime').val();
		localStorage["detailNotes"] = $('#txtDetailNotes').val();
	}
	
	function startAssist(){
		localStorage['licensePlate'] = $('#txtLicensePlate').val()
		localStorage['startODO'] = $('#txtStartODO').val();
		if(localStorage['incidentID'] == null || localStorage['incidentID'] == ''){
			alert('No current incident, aborting');
			return;
		}
		var _assistData;
		_assistData = {
			assistID: null,
			incidentID: localStorage['incidentID'],
			assistDatePosted: null,
			lastAssistInIncidentReport: 0,
			problemType: null,
			problemDetail: null,
			problemNote: null,
			otherNote: null,
			transportType: null,
			StartODO: localStorage['startODO'],
			EndODO: null,
			dropSite: null,
			state: null,
			licensePlate: localStorage['licensePlate'],
			vehicleType: null,
			OTAuthorizationNumber: null,
			detailNote: null,
			assistLat: null,
			assistLon: null,
			dropSiteOther: null,
			callSign: null,
			timeOnAssist: null,
			timeOffAssist: null,
			actionTaken: null
			};
		
		var assistString = JSON.stringify(_assistData);
        var _sendData = "a=" + assistString;

        var _url = ServiceLocation + 'AJAXFSPService.svc/createAssist';

        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _sendData,
            contentType: "application/json; charset=utf-8",
            success: createAssistSuccess,
            error: createAssistError
        });
	}
	
	function createAssistSuccess(result){
		alert(result.d);
		localStorage['assistID'] = result.d;
	}
	
	function createAssistError(error){
		alert(error);
	};
	
	function closeAssist(){
		if(localStorage['incidentID'] == null || localStorage['incidentID'] == ''){
			alert('No current incident, aborting');
			return;
		}
		if(localStorage['assistID'] == null || localStorage['assistID'] == ''){
			alert('No current assist, aborting');
			return;
		}
		var _assistData;
		_assistData = {
			assistID: localStorage['assistID'],
			incidentID: localStorage['incidentID'],
			assistDatePosted: null,
			lastAssistInIncidentReport: 0,
			problemType: localStorage["problemType"],
			problemDetail: localStorage["problemDetail"],
			problemNote: localStorage["problemNotes"],
			otherNote: localStorage["actionNotes"],
			transportType: localStorage["transportType"],
			StartODO: localStorage["startODO"],
			EndODO: localStorage["endODO"],
			dropSite: localStorage["dropSite"],
			state: localStorage["state"],
			licensePlate: localStorage["licensePlate"],
			vehicleType: localStorage["vehicleType"],
			OTAuthorizationNumber: localStorage["OTAuthorizationNumber"],
			detailNote: localStorage["detailNotes"],
			assistLat: null,
			assistLon: null,
			dropSiteOther: localStorage["dropSiteOther"],
			callSign: null,
			timeOnAssist: null,
			timeOffAssist: null,
			actionTaken: localStorage["actionTaken"]
			};
		
		var assistString = JSON.stringify(_assistData);
        var _sendData = "a=" + assistString;

        var _url = ServiceLocation + 'AJAXFSPService.svc/closeAssist';

        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _sendData,
            contentType: "application/json; charset=utf-8",
            success: closeAssistSuccess,
            error: closeAssistError
        });
	}
	
	function closeAssistSuccess(result){
		var res = result.d;
		if(res == 'OK'){
			alert('Assist closed');
		}
		else{
			alert(result.d);
		}
		localStorage['assistID'] = null;
		startAssist();
	}
	
	function closeAssistError(error){
		alert(error);
	};
	
	function closeFinalAssist(){
		if(localStorage['incidentID'] == null || localStorage['incidentID'] == ''){
			alert('No current incident, aborting');
			return;
		}
		if(localStorage['assistID'] == null || localStorage['assistID'] == ''){
			alert('No current assist, aborting');
			return;
		}
		var _assistData;
		_assistData = {
			assistID: localStorage['assistID'],
			incidentID: localStorage['incidentID'],
			assistDatePosted: null,
			lastAssistInIncidentReport: 1,
			problemType: localStorage["problemType"],
			problemDetail: localStorage["problemDetail"],
			problemNote: localStorage["problemNotes"],
			otherNote: localStorage["actionNotes"],
			transportType: localStorage["transportType"],
			StartODO: localStorage["startODO"],
			EndODO: localStorage["endODO"],
			dropSite: localStorage["dropSite"],
			state: localStorage["state"],
			licensePlate: localStorage["licensePlate"],
			vehicleType: localStorage["vehicleType"],
			OTAuthorizationNumber: localStorage["OTAuthorizationNumber"],
			detailNote: localStorage["detailNotes"],
			assistLat: null,
			assistLon: null,
			dropSiteOther: localStorage["dropSiteOther"],
			callSign: null,
			timeOnAssist: null,
			timeOffAssist: null,
			actionTaken: localStorage["actionTaken"]
			};
		
		var assistString = JSON.stringify(_assistData);
        var _sendData = "a=" + assistString;

        var _url = ServiceLocation + 'AJAXFSPService.svc/closeAssist';

        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _sendData,
            contentType: "application/json; charset=utf-8",
            success: closeFinalAssistSuccess,
            error: closeFinalAssistError
        });
	}
	
	function closeFinalAssistSuccess(result){
		var res = result.d;
		if(res == 'OK'){
			alert('Assist closed');
		}
		else{
			alert(result.d);
		}
		localStorage['assistID'] = null;
		closeIncident();
	}
	
	function closeFinalAssistError(error){
		alert(error);
	};
	
	function updateIncident(){
		var _incidentData;
		
		_incidentData = {
			incidentID: localStorage['incidentID'],
			incidentDatePosted: null,
			userPosted: null,
			callSign: null,
			fromTruck: true,
			lat: 0.0,
			lon: 0.0,
			canceled: false,
			reasonCanceled: 'na',
			beat: null,
			truckNumber: null,
			logID: null,
			wazeID: null,
			truckStatusID: null,
			FSPLocation: null,
			dispatchLocation: null,
			direction: localStorage['direction'],
			positionIncident: localStorage['position'],
			laneNumber: localStorage['laneNumber'],
			chpIncidentType: localStorage['incidentType'],
			briefUpdateLat: 0.0,
			briefUpdateLon: 0.0,
			freeway: null,
			briefUpdatePosted: true,
			timeOfBriefUpdate: null,
			CHPLogNumber: localStorage['chpLogNumber'],
			incidentSurveyNumber: localStorage['incidentSurveyNumber'],
			driverLastName: null,
			driverFirstName: null,
			driverID: null,
			runID: null
			};
			
		var incidentString = JSON.stringify(_incidentData);
        var _sendData = "i=" + incidentString;

        var _url = ServiceLocation + 'AJAXFSPService.svc/updateIncident';

        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _sendData,
            contentType: "application/json; charset=utf-8",
            success: updateIncidentSuccess,
            error: updateIncidentError
        });
	}
	
	function updateIncidentSuccess(result) {
        alert(result.d);
    }

    function updateIncidentError(error) {
        alert(error);
    }
	
	function closeIncident(){
		var _incidentData;
		
		_incidentData = {
			incidentID: localStorage['incidentID'],
			incidentDatePosted: null,
			userPosted: null,
			callSign: null,
			fromTruck: true,
			lat: 0.0,
			lon: 0.0,
			canceled: false,
			reasonCanceled: 'na',
			beat: null,
			truckNumber: null,
			logID: null,
			wazeID: null,
			truckStatusID: null,
			FSPLocation: null,
			dispatchLocation: null,
			direction: localStorage['direction'],
			positionIncident: localStorage['position'],
			laneNumber: localStorage['laneNumber'],
			chpIncidentType: localStorage['incidentType'],
			briefUpdateLat: 0.0,
			briefUpdateLon: 0.0,
			freeway: null,
			briefUpdatePosted: true,
			timeOfBriefUpdate: null,
			CHPLogNumber: localStorage['chpLogNumber'],
			incidentSurveyNumber: localStorage['incidentSurveyNumber'],
			driverLastName: null,
			driverFirstName: null,
			driverID: null,
			runID: null
			};
			
		var incidentString = JSON.stringify(_incidentData);
        var _sendData = "i=" + incidentString;

        var _url = ServiceLocation + 'AJAXFSPService.svc/closeIncident';

        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _sendData,
            contentType: "application/json; charset=utf-8",
            success: closeIncidentSuccess,
            error: closeIncidentError
        });
	}
	
	function closeIncidentSuccess(result) {
        alert(result.d);
		localStorage['incidentID'] = null;
		clearLocalIncidentStorage();
		clearLocalAssistStorage();
    }

    function closeIncidentError(error) {
        alert(error);
    }
	
	function getDropSites(BeatNumber){
		var _url = ServiceLocation + 'AJAXFSPService.svc/getDropSites';
		var _data = "BeatNumber=" + BeatNumber;
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
			data: _data,
            contentType: "application/json; charset=utf-8",
            success: GetDropsSuccess,
            error: GetDropsError
        });
	}

	function GetDropsSuccess(result){
		var _data = result.d;
        $('#selDrop').empty();
        _data = $.parseJSON(_data);
        var _selCode = '';
        for (var i = 0; i < _data.length; i++) {
            _selCode += '<option id="' + _data[i] + '">' + _data[i] + '</option>';
        }
        $('#selDrop').append(_selCode);
	}
	
	function GetDropsError(error){
		alert(error);
	}
	
	function getIncidentID(){
		var _url = ServiceLocation + 'AJAXFSPService.svc/getCurrentIncidentID';

        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            contentType: "application/json; charset=utf-8",
            success: getIncidentIDSuccess,
            error: getIncidentIDError
        });
	}
	
	function getIncidentIDSuccess(result){
		if(result.d != ""){
			localStorage["incidentID"] = result.d;
		}
	}
	
	function getIncidentIDError(error){
		alert(error);
	}
});