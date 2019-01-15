$(document).ready(function(){
	//localStorage['incidentID'] = null;
	localStorage['licensePlate'] = null;
	localStorage['direction'] = null;
	localStorage['position'] = null;
	localStorage['laneNumber'] = null;
	localStorage['chpIncidentType'] = null;
	localStorage['chpLogNumber'] = null;
	localStorage['incidentSurveyNumber'] = null;
	
	checkIncID();
	getIncidentTypes();
	getSurveyNumber();
	setVals();

	function checkIncID(){
		
		if(localStorage['incidentID'] == null || localStorage['incidentID'] == "" || localStorage['incidentID'] == "null"){
			//no assigned incident, start one up.
			startIncident();
		}
		
		if(localStorage['incidentID'] != null){
		//check if it's a GUID
			var vGuid = isGuid(localStorage['incidentID']);
			if(!vGuid){
				getCurrentIncidentID();
			}
		}
		/*
		if(localStorage['incidentID'] == 'ACTIVEINCIDENT'){
			localStorage['incidentID'] = localStorage['IncidentID'];
		}
		*/
	}

	$('#btnUpdateIncident').click(function(){
		updateIncident();
	});
	
	$('#btnCancelIncident').click(function(){
		cancelIncident();
	});
	
	function setVals(){
		$('#txtCHPLogNum').val(localStorage['CHPLogNumber']);
	}
	
	//Button clicks

	
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
		//alert(result.d);
		localStorage['assistID'] = result.d;
		window.location.href = "newAssist.html"
	}
	
	function createAssistError(error){
		alert(error);
	};
	
	//Update Incident
	function updateIncident(){
		var _incidentData;
		var selectedDirection = $("input[type='radio'][name='Direction']:checked");
		if(selectedDirection.length > 0){
            localStorage['direction'] = selectedDirection.val();
        }
		var selectedPosition = $("input[type='radio'][name='position']:checked");
		if(selectedPosition.length > 0){
			localStorage['position'] = selectedPosition.val();
		}

		localStorage['laneNumber'] = $('#txtLane').val();
		localStorage['incidentType'] = $('#selIncidentType option:selected').text();
		if(localStorage['position'] == 'InLane'){
			localStorage['laneNumber'] = $('#txtLane').val();
		}
		localStorage['chpLogNumber'] = $('#txtCHPLogNum').val();
		localStorage['incidentSurveyNumber'] = $('#txtIncidentSurveyNum').val();
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
		startAssist();
    }

    function updateIncidentError(error) {
        alert(error);
    }
	
	//Start Assist
	
	//Start Up Helper functions
	function startIncident(){
		localStorage['incidentID'] = null;
		var _incidentData;
		// most of this data will get overwritten at the service end.
		// technically the only reason this has to happen now is to 
		//get the incident id and store it and get the incident ball rolling
		_incidentData = {
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
			direction: null,
			positionIncident: null,
			laneNumber: null,
			chpIncidentType: null,
			briefUpdateLat: 0.0,
			briefUpdateLon: 0.0,
			freeway: '9',
			briefUpdatePosted: false,
			timeOfBriefUpdate: null,
			CHPLogNumber: null,
			incidentSurveyNumber: null,
			driverLastName: null,
			driverFirstName: null,
			driverID: null,
			runID: null
			};
			
		var incidentString = JSON.stringify(_incidentData);
        var _sendData = "i=" + incidentString;

        var _url = ServiceLocation + 'AJAXFSPService.svc/createIncident';

        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _sendData,
            contentType: "application/json; charset=utf-8",
            success: postIncidentSuccess,
            error: postIncidentError
        });
	}
	
	function postIncidentSuccess(result) {
        //alert(result.d);
		localStorage["incidentID"] = result.d;
    }

    function postIncidentError(error) {
        alert(error);
    }
	
	function cancelIncident(){
		var _incidentData;
		// most of this data will get overwritten at the service end.
		// technically the only reason this has to happen now is to 
		//get the incident id and store it and get the incident ball rolling
		_incidentData = {
			incidentDatePosted: null,
			userPosted: null,
			callSign: null,
			fromTruck: true,
			lat: 0.0,
			lon: 0.0,
			canceled: true,
			reasonCanceled: 'UTL',
			beat: null,
			truckNumber: null,
			logID: null,
			wazeID: null,
			truckStatusID: null,
			FSPLocation: null,
			dispatchLocation: null,
			direction: null,
			positionIncident: null,
			laneNumber: null,
			chpIncidentType: null,
			briefUpdateLat: 0.0,
			briefUpdateLon: 0.0,
			freeway: '9',
			briefUpdatePosted: false,
			timeOfBriefUpdate: null,
			CHPLogNumber: null,
			incidentSurveyNumber: null,
			driverLastName: null,
			driverFirstName: null,
			driverID: null,
			runID: null
			};
			
		var incidentString = JSON.stringify(_incidentData);
        var _sendData = "i=" + incidentString;

        var _url = ServiceLocation + 'AJAXFSPService.svc/cancelIncident';

        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            data: _sendData,
            contentType: "application/json; charset=utf-8",
            success: cancelIncidentSuccess,
            error: cancelIncidentError
        });
	}
	
	function cancelIncidentSuccess(result) {
        alert(result.d);
		localStorage["incidentID"] = null;
		localStorage["assistID"] = null;
    }

    function cancelIncidentError(error) {
        alert(error);
    }
	
    function getSurveyNumber() {
        var _url = ServiceLocation + 'AJAXFSPService.svc/MakeSurveyNum';
        $.ajax({
            type: "GET",
            dataType: "json",
            url: _url,
            contentType: "application/json; charset=utf-8",
            success: getSurveyNumberSuccess,
            error: getSurveyNumberError
        });
    }

    function getSurveyNumberSuccess(result) {
        $('#txtIncidentSurveyNum').empty();
        $('#txtIncidentSurveyNum').val(result.d);
		localStorage["SurveyNumber"] = result.d;
		localStorage["preAssistSurveyNum"] = result.d;
    }

    function getSurveyNumberError(error) {
        alert(error);
    }
	
	function getIncidentTypes(){
		var _url = ServiceLocation + 'AJAXFSPService.svc/getDispatchCodes';
		$.ajax({
			type: "GET",
			dataType: "json",
			url: _url,
			contentType: "application/json; charset=utf-8",
			success: getIncidentTypesSuccess,
			error: getIncidentTypesError
		});
	}
	
	function getIncidentTypesSuccess(result){
		var _data = $.parseJSON(result.d);
		var _selCode = '';
		for(var i = 0;i<_data.length;i++){
			_selCode += '<option id="' + _data[i] + '">' + _data[i] + '</option>';
		}
		_selCode += '<option id="Other">Other</option>';
		$('#selIncidentType').empty();
		$('#selIncidentType').append(_selCode);
	}
	
	function getIncidentTypesError(error){
		alert(error);
	}
	
	function getCurrentIncidentID(){
		var _url = ServiceLocation + 'AJAXFSPService.svc/getCurrentIncidentID';
		$.ajax({
			type: "GET",
			dataType: "json",
			url: _url,
			contentType: "application/json; charset=utf-8",
			success: getCurrentIncidentSuccess,
			error: getCurrentIncidentError
		});
	}
	
	function getCurrentIncidentSuccess(result){
		if(result != null){
			var Guidity = isGuid(result.d);
			if(Guidity){
				localStorage['incidentID'] = result.d;
			}
		}
	}
	
	function getCurrentIncidentError(error){
		alert(error);
	}
});