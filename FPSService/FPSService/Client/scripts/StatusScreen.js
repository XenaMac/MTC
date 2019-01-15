$('#StatusScreen').live('pageshow', function(){
	$('#StatusScreenHeader').empty();
	$('#StatusScreenHeader').append('FSP Status Screen for: ' + DriverName + ' Truck: ' + TruckID);
	$('#map_canvas').gmap('refresh');
});

$('#StatusScreen').live('pageinit', function(event){

	FindAssistRequests();
	GetTruckStatus();
	
	try{
	/*
	$('#map_canvas').gmap().bind('init', function(ev, map) {
		$('#map_canvas').gmap('addMarker', {'position': '57.7973333,12.0502107', 'bounds': true}).click(function() {
			$('#map_canvas').gmap('openInfoWindow', {'content': 'Hello World!'}, this);
		});
	});
	*/
	var currentZoom = 14;
	$('#map_canvas').gmap({'center': new google.maps.LatLng(12.7866, -42.909878),'zoom':currentZoom});


	}

	catch(error){
		//alert(error);
	}
	var map = $('#map_canvas').gmap('get','map');
	$(map).addEventListener('zoom_changed', function(){ getZoom(); });
	
	function getZoom(){
		var map = $('#map_canvas').gmap('get','map');
		currentZoom = $(map).gmap('option','zoom');
		//alert(currentZoom);
	}
	
	GetPosition();
	assistTrigger();
	positionTrigger();
	statusTrigger();
	function assistTrigger(){
		return window.setInterval(function(){ FindAssistRequests(); },10000);
	}
	function positionTrigger(){
		return window.setInterval(function(){ GetPosition(); },20000);
	}
	function statusTrigger(){
		return window.setInterval(function(){GetTruckStatus()},20000);
	}
	
	var gpID = positionTrigger();
	var asID = assistTrigger();
	var stID = statusTrigger();

	$('#OnResponse').click(function(){
		SetTruckStatus("On Assist");
	});
	
	$('#OnPatrol').click(function(){
		SetTruckStatus("On Patrol");
	});
	
	function LogOff(){
		//add set logoff on service code, set truck status to logged off or something
		DriverName = "";
		TruckID = "";
		//Truck is no longer reporting, force Logoff and stop timer
		window.clearInterval(gpID);
		window.clearInterval(asID);
		ClientStatus = "LoggedOff";
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
	
	$('#LogOff').click(function(){
		LogOff();
	});
		
	function LogOffError(error){
		//alert(error.responseText);
	}
	
	function RedirectToLogon(){
		$.mobile.changePage("Logon.html", "slideup");
	}
	
	function GetPosition(){
		if(ClientStatus == "LoggedOn"){
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
			var _data = $.parseJSON(result.d);
			var zoomLevel = $('#map_canvas').gmap('option','zoom');
			var Content = 'TruckID: ' + _data.TruckID + ' Speed: ' + _data.Speed;
			try{
				var position = new google.maps.LatLng(_data.Lat, _data.Lon);
				$('#map_canvas').gmap('clear','markers');
				$('#map_canvas').gmap('addMarker', {'position': position});
				$('#map_canvas').gmap('get','map').setOptions({
					'center':position,
					'zoom':currentZoom
				});
			}
			catch(error){
				//alert(error); alert was for testing only
			}
		}
		else{
			LogOff();
		}
	}

	function GetPositionError(error){
		alert(error);
	}
	
	function SetTruckStatus(statusMsg){
		if(ClientStatus == "LoggedOn"){
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
			$('#StatusScreenHeader').append('FSP Status Screen for: ' + DriverName + ' Truck: ' + TruckID + ' ON ASSIST');
		}
		if (TruckStatus == "On Patrol"){
			$('#StatusScreenHeader').empty();
			$('#StatusScreenHeader').append('FSP Status Screen for: ' + DriverName + ' Truck: ' + TruckID + ' ON PATROL');
		}
		GetTruckStatus();
	}
	
	function SetStatusError(error){
		//alert(error);
	}
	
	function GetTruckStatus(){
		var _url = ServiceLocation + 'AJAXFSPService.svc/GetTruckStatus';
		$.ajax({
			type: "GET",
			dataType: "json",
			url: _url,
			contentType: "application/json; charset=utf-8",
			success: GetTruckStatusSuccess,
			error: GetTruckStatusError
		});
	}
	
	function GetTruckStatusSuccess(result){
		var _result = $.parseJSON(result.d);
		$('#statusDiv').empty();
		var msg = '';
		msg += '<div>TruckID: ' + _result.TruckID + '</div>';
		msg += '<div>Speed: ' + _result.Speed + '</div>';
		msg += '<div>Status: ' + _result.TruckStatus + '</div>';
		$('#statusDiv').append(msg);
	}
	
	function GetTruckStatusError(error){ 
		//alert(error);
	}
	
	function FindAssistRequests(){
		if(ClientStatus == "LoggedOn"){
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
			$('#assistRequests').empty(); //clear the ul
			var _data = result.d;
			var _requests = $.parseJSON(_data);
			var _requestCode = "";
			for(var i = 0;i<_requests.length;i++){
				var ButtonText = 'ASSIST';
				var dtText = FixDate(_requests[i].DispatchTime);
				ButtonText += ' ' + dtText;
				_requestCode += '<li><a href="#" id="' + _requests[i].AssistID + '" class="request">' + ButtonText + '</a></li>';
			}
			$('#assistRequests').append(_requestCode).listview('refresh');
			$('.request').bind("click", function(event){ RespondToRequestClick(event.target.id);});
				}
		catch(error){
			//alert(error); Testing only
		}
	}
	
	function GetAssistsError(error){
		//alert(error.responseText);
	}
	
	function RespondToRequestClick(e){
		//alert('clicked' + e.toString());
		//$('RequestData').empty();
		if(ClientStatus == "LoggedOn"){
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
		if(ClientStatus == "LoggedOn"){
			var id = e.Id;
			$.mobile.changePage("LogAssist.html", "slideup");
		}
	}
	
	function GetRequestDetailSuccess(result){
		$('#RequestData').empty();
		var _data = $.parseJSON(result.d);
		var _requestCode = '<div id="RequestInfo">';
		_requestCode += '<button id=" + _data.AssistID + " class="getRequestDetail">Respond to Assist</button>';
		_requestCode += '<span><b>Service Type:</b> ' + _data.ServiceType + '</span><span>&nbsp<b>Vehicle Position:</b> ' + _data.VehiclePosition + '</span>';
		_requestCode += '</div>';
		_requestCode += '<div id="VehicleInfo">';
		_requestCode += '<span><b>Make:</b> ' + _data.Make + '</span><span>&nbsp<b>Color:</b> ' + _data.Color + '</span><span>&nbsp<b>License Plate:</b> ' + _data.State + ' ' + _data.LicensePlate + '</span>';
		_requestCode += '</div>';
		$('#RequestData').append(_requestCode);
		$('.getRequestDetail').bind("click", function(event){ RespondToRequestButtonClick(event.target.id);});
		//$('#RequestInfo').page();
		//$('#VehicleInfo').page();
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