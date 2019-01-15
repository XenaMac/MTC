$(document).ready(function(){
	localStorage.setItem("zoom", 5);
	checkLogonStatus();
	FindAssistRequests();
	assistTrigger();
	//hide the assist respond button until we need it
	$('#btnRespond').hide();
	
	$('#btnRespond').click(function(){
		document.location.href = "Assist.html";
	});
	
	$('#btnIncident').click(function(){
		localStorage.setItem("assistid", "");
		document.location.href = "Assist.html";
	});
	
	function assistTrigger(){
		return window.setInterval(function(){ FindAssistRequests(); },10000);
	}
	
	function checkLogonStatus(){
		var logon = localStorage.getItem("logon");
		var driver = localStorage.getItem("name");
		var truckNumber = localStorage.getItem("trucknumber");
		$('#driverNameVal').empty();
		$('#driverNameVal').append(driver);
		$('#logonStatusVal').empty();
		$('#logonStatusVal').append(logon);
		$('#truckIdVal').empty();
		$('#truckIdVal').append(truckNumber);
	}
	
	var asID = assistTrigger();
	
	//GPS Tracking Section (GetPosition, GetPositionSuccess, GetPositionError)
	
	GetPosition();
	
	function positionTrigger(){
		return window.setInterval(function(){ GetPosition(); },20000);
	}
	
	var gpID = positionTrigger();
	
	var map = $('#map_canvas').gmap('get','map');
	$(map).addEventListener('zoom_changed', function(){ getZoom(); });
	
	function getZoom(){
		try{
			var map = $('#map_canvas').gmap('get','map');
			//var currentZoom = $(map).gmap('option','zoom');
			var currentZoom = map.getZoom();
			localStorage.setItem("zoom", currentZoom);
		}
		catch(error){
			alert(error);
		}
	}
	
	function GetPosition(){
		var logon = localStorage.getItem("logon");
		
		if(logon == "true"){
			var _url = ServiceLocation + 'AJAXFSPService.svc/GetTruckPosition';
			$.ajax({
				type: "GET",
				dataType: "json",
				url: _url,
				contentType: "application/json; charset=utf-8",
				success: GetPositionSuccess,
				error: GetPositionError
			});
		}
	}
	
	function GetPositionSuccess(result){
		if (result.d != ""){
			var currentZoom = localStorage.getItem("zoom");
			var _data = $.parseJSON(result.d);
			if(_data.TruckStatus == "Waiting for Driver Login"){
				LogOff();
			}
			var zoomLevel = $('#map_canvas').gmap('option','zoom');
			var Content = 'TruckNumber: ' + _data.TruckNumber + ' Speed: ' + _data.Speed;
			$('#speedVal').empty();
			$('#speedVal').append(_data.Speed);
			$('#truckStatusVal').empty();
			$('#truckStatusVal').append(_data.TruckStatus);
			
			try{
				var position = new google.maps.LatLng(_data.Lat, _data.Lon);
				$('#map_canvas').gmap('clear','markers');
				$('#map_canvas').gmap('addMarker', {'position': position});
				$('#map_canvas').gmap('get','map').setOptions({
					'center':position
				});
				$('map_canvas').gmap('option','zoom',currentZoom);
				$('map_canvas').gmap('refresh');
			}
			catch(error){
				alert(error); //alert was for testing only
			}
			localStorage.setItem("truckstate", _data.TruckStatus);
			checkStatus(_data.TruckStatus);
		}
		else{
			LogOff();
		}
	}

	function GetPositionError(error){
		alert(error);
	}
	
	function checkStatus(status){
		if(status=="On Patrol" || status=="On Assist" || status=="On Break" || status=="On Lunch"){
			$('#btnRoll').prop('value','Roll In');
			$('#btnRoll').css("background-color","green");
		}
		if(status=="On Break"){
			$('#btnBreak').prop('value','Off Break');
			$('#btnBreak').css("background-color","red");
			GetBreakTimeRemaining();
		}
		if(status=="On Lunch"){
			$('#btnLunch').prop('value','Off Lunch');
			$('#btnLunch').css("background-color","red");
		}
		
		if(status=="Roll In"){
			$('#btnRoll').prop('value','Roll Out');
			$('#btnRoll').css("background-color","");
		}
	}
	
	function GetBreakTimeRemaining(){
		var _url = ServiceLocation + 'AJAXFSPService.svc/GetUsedBreakTime';
		var driverid = localStorage.getItem("driverid");
		var _data = 'DriverID=' + driverid;
		$.ajax({
			type: "GET",
			dataType: "json",
			url: _url,
			data: _data,
			contentType: "application/json; charset=utf-8",
			success: SetBreakTimeRemaining,
			error: SetBreakTimeRemainingError
		});
	}
	
	function SetBreakTimeRemaining(result){
		
		var timeDiff = result.d;
		BreakDuration = BreakDuration - timeDiff;
		BreakTimer();
	}
	
	function SetBreakTimeRemainingError(error){
		alert(error);
	}
	
	//Log Off events
	function LogOff(){
		//add set logoff on service code, set truck status to logged off or something
		localStorage.setItem("name", "");
		localStorage.setItem("trucknumber", "");
		localStorage.setItem("truckid","");
		localStorage.setItem("logon", "false");
		//Truck is no longer reporting, force Logoff and stop timer
		window.clearInterval(gpID);
		//window.clearInterval(asID);
		var _url = ServiceLocation + 'AJAXFSPService.svc/DriverLogoff';
		$.ajax({
			type: "GET",
			dataType: "json",
			url: _url,
			contentType: "application/json; charset=utf-8",
			success: RedirectToLogon,
			error: LogOffError
		});
	}
	
	$('#btnLogOff').click(function(){
		LogOff();
	});
		
	function LogOffError(error){
		//alert(error.responseText);
	}
	
	function RedirectToLogon(){
		document.location.href = "Logon.html";
	}
	
	//Status changes
	
	$('#btnRoll').click(function(){
		var RollStatus = "Roll Out";
		if($('#btnRoll').val() == "Roll Out"){
			RollStatus = "Roll Out";
			$('#btnRoll').prop('value','Roll In');
			$('#btnRoll').css("background-color","green");
		}
		else{
			RollStatus = "Roll In";
			$('#btnRoll').prop('value','Roll Out');
			$('#btnRoll').css("background-color","");
		}
		SetTruckStatus(RollStatus);
	});
	
	$('#btnBreak').click(function(){
		var BreakStatus = "On Break";
		if($('#btnBreak').val() == "On Break"){
			BreakStatus = "On Break";
			$('#btnBreak').prop('value','Off Break');
			$('#btnBreak').css("background-color","red");
			localStorage.setItem("truckstate","On Break");
			FindBreakDuration();
			BreakTimer();
		}
		else{
			BreakStatus = "Off Break";
			$('#btnBreak').prop('value','On Break');
			$('#btnBreak').css("background-color","");
			clearInterval(counter);
			$('#breakTimeRemaining').empty();
		}
		SetTruckStatus(BreakStatus);
	});
	
	var BreakDuration = 15;
	var BreakDurationInSeconds = 15 * 60;
	
	function FindBreakDuration(DriverID){
		var DriverID = localStorage.getItem("driverid");
		var _data = "DriverID=" + DriverID;
		var _url = ServiceLocation + 'AJAXFSPService.svc/GetBreakDuration';
		$.ajax({
			type: "GET",
			dataType: "json",
			url: _url,
			data: _data,
			contentType: "application/json; charset=utf-8",
			success: FindBreakDurationSuccess,
			error: FindBreakDurationError
		});
	}
	
	function FindBreakDurationSuccess(result){
		BreakDuration = result.d;
	}
	
	function FindBreakDurationError(error){
		alert(error);
	}
	
	var counter;
	
	function BreakTimer(){
		BreakDurationInSeconds = BreakDuration * 60;
		//counter = setInterval(timer,1000);
		counter = setInterval(timer,1000);
	}
	
	function timer(){
		BreakDurationInSeconds = BreakDurationInSeconds - 1
		var TruckState = localStorage.getItem("truckstate");
		if (TruckState != "On Break"){
			clearInterval(counter);
			$('#breakTimeRemaining').empty();
		}
		if (BreakDurationInSeconds <= 0){
			clearInterval(counter);
			$('#breakTimeRemaining').empty();
			$('#breakTimeRemaining').append("Break Over");
		}
		else{
			var date = new Date(null);
			date.setSeconds(BreakDurationInSeconds);
			var time = date.toTimeString().substr(3,5);
			$('#breakTimeRemaining').empty();
			$('#breakTimeRemaining').append(time);
		}
	}
	
	$('#btnLunch').click(function(){
		var LunchStatus = "On Lunch";
		if($('#btnLunch').val() == "On Lunch"){
			LunchStatus = "On Lunch";
			$('#btnLunch').prop('value','Off Lunch');
			$('#btnLunch').css("background-color","red");
		}
		else{
			LunchStatus = "Off Lunch";
			$('#btnLunch').prop('value','On Lunch');
			$('#btnLunch').css("background-color","");
		}
		SetTruckStatus(LunchStatus);
	});
	
	$('#btnAssist').click(function(){
		var AssistStatus = "On Assist";
		if($('#btnAssist').val() == "On Assist"){
			AssistStatus = "On Assist";
			$('#btnAssist').prop('value', 'Off Assist');
			$('#btnAssist').css("background-color","red");
		}
		else{
			AssistStatus = "On Patrol";
			$('#btnAssist').prop('value', 'On Assist');
			$('#btnAssist').css("background-color","");
		}
		SetTruckStatus(AssistStatus);
	});
	
	function SetTruckStatus(statusMsg){
		var logon = localStorage.getItem("logon");
		if(logon == "true"){
			var _data = "Status=" + statusMsg;
			var _url = ServiceLocation + 'AJAXFSPService.svc/SetTruckStatus';
			TruckStatus = statusMsg;
			$.ajax({
				type: "GET",
				dataType: "json",
				url: _url,
				data: _data,
				contentType: "application/json; charset=utf-8",
				success: SetStatusSuccess,
				error: SetStatusError
			});
		}
	}
	
	function SetStatusSuccess(result){
		$('#result').empty();
		$('#result').append(result.d);
		if(TruckStatus == "On Assist"){
			$('#StatusScreenHeader').empty();
			$('#StatusScreenHeader').append('FSP Status Screen for: ' + DriverName + ' Truck: ' + TruckNumber + ' ON ASSIST');
		}
		if (TruckStatus == "On Patrol"){
			$('#StatusScreenHeader').empty();
			$('#StatusScreenHeader').append('FSP Status Screen for: ' + DriverName + ' Truck: ' + TruckNumber + ' ON PATROL');
		}
		//GetTruckStatus();
	}
	
	function SetStatusError(error){
		//alert(error);
	}
	
	function FindAssistRequests(){
		var logon = localStorage.getItem("logon");
		
		if(logon == "true"){
			var _url = ServiceLocation + 'AJAXFSPService.svc/GetAssistRequests';
			$.ajax({
				type: "GET",
				dataType: "json",
				url: _url,
				contentType: "application/json; charset=utf-8",
				success: GetAssistsSuccess,
				error: GetAssistsError
			});
		}
	}
		
	function GetAssistsSuccess(result){
		try{
		
			//display modal dialog
			var _data = result.d;
			if(_data.length > 2){
				var _requests = $.parseJSON(_data);
				localStorage.setItem('assistid', _requests[0].AssistID);
				$('#AssistInfo').empty();
				$('#AssistInfo').append(_requests[0].AssistInfo);
				$('#dialog').jqm({modal:true});
				$('#dialog').jqmShow();
			}
			/*
			$('#assistRequests').empty(); //clear the ul

			var _requestCode = "";
			for(var i = 0;i<_requests.length;i++){
				var ButtonText = 'ASSIST';
				var dtText = FixDate(_requests[i].DispatchTime);
				ButtonText += ' ' + dtText;
				_requestCode += '<input type="button" id="' + _requests[i].AssistID + '" class="assistButton" value="Assist Request" />'
				//_requestCode += '<li><a href="#" id="' + _requests[i].AssistID + '" class="request">' + ButtonText + '</a></li>';
			}
			$('#assistRequests').append(_requestCode);
			$('.assistButton').bind("click", function(event){ RespondToRequestClick(event.target.id);});
				}
				*/
		}
		catch(error){
			alert(error); //Testing only
		}
	}
	
	function GetAssistsError(error){
		//alert(error.responseText);
	}
	
	$('#btnAckAssist').click(function(){
		AckAssistRequest();
	});
	
	function AckAssistRequest()
	{
		var logon = localStorage.getItem("logon");
		if(logon == "true"){
			var _url = ServiceLocation + 'AJAXFSPService.svc/AckAssistRequest';
			var _data = "_AssistID=" + localStorage.getItem('assistid');
			try{
				$.ajax({
				type: "GET",
				dataType: "json",
				url: _url,
				data: _data,
				contentType: "application/json; charset=utf-8",
				success: AckSuccess,
				error: AckError
				});
			}
			catch(error){
				alert(error);
			}
		}
	}
	
	function AckSuccess()
	{
		alert('Acknowledged');
		SetTruckStatus('On Assist');
	}
	
	function AckError(error)
	{
		alert('uh,oh');
	}
	
	function RespondToRequestClick(e){
		//alert('clicked' + e.toString());
		//$('RequestData').empty();
		var logon = localStorage.getItem("logon");
		
		if(logon == "true"){
			var _url = ServiceLocation + 'AJAXFSPService.svc/GetAssistRequestDetail';
			var _data = "_AssistID=" + e.toString();
			try{
				$.ajax({
				type: "GET",
				dataType: "json",
				url: _url,
				data: _data,
				contentType: "application/json; charset=utf-8",
				success: GetRequestDetailSuccess,
				error: GetRequestDetailError
			});
			}
			catch(error){
				alert(error);
			}
		}
	}
	
	function RespondToRequestButtonClick(e){
		var logon = localStorage.getItem("logon");
		
		if(logon == "true"){
			var id = e.Id;
			$.mobile.changePage("LogAssist.html", "slideup");
		}
	}
	
	function GetRequestDetailSuccess(result){
		$('#RequestData').empty();
		var _data = $.parseJSON(result.d);
		$('#vehicleMakeVal').empty();
		$('#vehicleMakeVal').append(_data.Make);
		$('#vehicleTypeVal').empty();
		$('#vehicleTypeVal').append(_data.VehicleType);
		$('#vehicleColorVal').empty();
		$('#vehicleColorVal').append(_data.Color);
		$('#vehiclePositionVal').empty();
		$('#vehiclePositionVal').append(_data.VehiclePosition);
		$('#dispatchTimeVal').empty();
		$('#dispatchTimeVal').append(FixDate(_data.x1097.toString()));
		$('#btnRespond').show();
		localStorage.setItem("assistid", _data.AssistID);
		localStorage.setItem("assisttype", _data.AssistType);
		localStorage.setItem("incidentid", _data.IncidentID);
		localStorage.setItem("dispatchtime", _data.DispatchTime);
		localStorage.setItem("servicetype", _data.ServiceType);
		localStorage.setItem("dropzone", _data.DropZone);
		localStorage.setItem("make", _data.Make);
		localStorage.setItem("vehicletype", _data.VehicleType);
		localStorage.setItem("vehicleposition", _data.VehiclePosition);
		localStorage.setItem("color", _data.Color);
		localStorage.setItem("licenseplate", _data.LicensePlate);
		localStorage.setItem("state", _data.State);
		localStorage.setItem("startod", _data.StartOD);
		localStorage.setItem("endod", _data.EndOD);
		localStorage.setItem("towlocation", _data.TowLocation);
		localStorage.setItem("tip", _data.Tip);
		localStorage.setItem("tipdetail", _data.TipDetail);
		localStorage.setItem("customerlastname", _data.CustomerLastName);
		localStorage.setItem("comments", _data.Comments);
		localStorage.setItem("ismdc", _data.IsMDC);
		localStorage.setItem("x1097", _data.x1097);
		localStorage.setItem("x1098", _data.x1098);
		localStorage.setItem("contractor", _data.Contractor);
		localStorage.setItem("lognumber", _data.LogNumber);
		localStorage.setItem("beatsegmentid", _data.BeatSegmentID);
		//var _requestCode = '<div id="RequestInfo">';
		//_requestCode += '<button id=" + _data.AssistID + " class="getRequestDetail">Respond to Assist</button>';
		//_requestCode += '<span><b>Service Type:</b> ' + _data.ServiceType + '</span><span>&nbsp<b>Vehicle Position:</b> ' + _data.VehiclePosition + '</span>';
		//_requestCode += '</div>';
		//_requestCode += '<div id="VehicleInfo">';
		//_requestCode += '<span><b>Make:</b> ' + _data.Make + '</span><span>&nbsp<b>Color:</b> ' + _data.Color + '</span><span>&nbsp<b>License Plate:</b> ' + _data.State + ' ' + _data.LicensePlate + '</span>';
		//_requestCode += '</div>';
		//$('#RequestData').append(_requestCode);
		//$('.getRequestDetail').bind("click", function(event){ RespondToRequestButtonClick(event.target.id);});
	}
	
	function GetRequestDetailError(error){
		//alert(error);
	}
	
	function FixDate(dtVal) {
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
            alert("Bad Input");
        }
	}
});