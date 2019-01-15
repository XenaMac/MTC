$(document).ready(function(){

	/*******************
	*
	* INIT
	*
	*******************/
	
	dojo.require("esri.map");
	dojo.require("esri.layers.agsdynamic");
	var map; //main map
	getPosition();
	var showDrops = 'F'; //toggle drop layer on and off
	
	//set up for repeat checking
	var positionTimer;
	positionTimer = setInterval(function(){getPosition()}, 20000);
	
	/*********************
	*
	* Button Click
	*
	*********************/
	
	$('#btnDropSites').click(function(){
		if(showDrops == 'F'){
			showDrops = 'T';
			showFallR('Turning on drop sites view, wait for refresh');
		}
		else{
			showDrops = 'F';
			showFallR('Turning off drop sites view, wait for refresh');
		}
	});
	
	/*******************
	*
	* Helpers
	*
	*******************/
	
	function showFallR(message){ //messaging
		$.fallr('show', {
            content: '<p>' + message + '</p>',
            position: 'center',
            icon: 'lamp'
        });
	}
	
	/*********************************
	*
	* Force Log Off
	*
	*********************************/
	
	function setLogOff(){
		localStorage["TruckStatus"] = status;
		$('#statusName').text(status);
		//tell the service truck status has changed
		var logonStatus = localStorage["logonStatus"];
		if(logonStatus != "1"){
			localStorage.clear();
			window.location.href='Logon.html';
		}
		else{
			var _data = "_ok=force";
			var _url = ServiceLocation + 'AJAXFSPService.svc/DriverLogoff';
			$.ajax({
                type: "GET",
                dataType: "json",
                url: _url,
                data: _data,
                contentType: "application/json; charset=utf-8",
                success: SetLogOffSuccess,
                error: SetLogOffError
            });
		}
	}
	
	function SetLogOffSuccess(result) {
        localStorage.clear();
		document.location.href = "Logon.html";
    }
	
	function SetLogOffError(error) {
        localStorage.clear();
		document.location.href = "Logon.html";
    }
	
	/*********************************
	*
	* Get Vehicle Lat/Lon from service
	*
	**********************************/

	function getPosition(){
		var logonStatus = localStorage["logonStatus"];
		if(logonStatus != "1"){
			localStorage.clear();
			window.location.href='Logon.html';
		}
		else{
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

	function GetPositionSuccess(result) {
		if (result.d != "") {
			var _data = $.parseJSON(result.d);
			if (_data.TruckStatus == "Waiting for Driver Login" || _data.TruckStatus == "NA" || _data.TruckStatus == "LOG OFF") {
				setLogOff();
			}
		else{
			localStorage["lat"] = _data.Lat;
			localStorage["lon"] = _data.Lon;
			var head = _data.Direction;
			var status = _data.TruckStatus;
			var truckNumber = _data.TruckNumber
			localStorage["trucknumber"] = truckNumber;
			$('#truckNumber').text(truckNumber);
			if(status == 'Driver Logged On'){
				status = 'LOGGED ON';
			}
			$('#statusName').text(status);
			try{
				makeMap(localStorage["lat"], localStorage["lon"], head, status);
			}
			catch(error){
				showFallR('Can\'t find position');
			}
			localStorage["TruckStatus"] = status;
		}
    }
		else {
			setLogOff();
		}
	}

    function GetPositionError(error) {
        showFallR('Can\'t find position');
    }
	
	function initMap(lat, lon, head, status){
		require([
			"esri/map",
			"dojo/domReady!"
		], function(Map) { 
			map = new Map("map", {
				center: [lon, lat],
				zoom: 15,
				basemap: "streets"
			});
		});
	}
	
	/************************
	*
	* Make Map
	*
	*************************/
	function makeMap(lat, lon, head, status){
	//if(map != null){map.destroy();}

	require([
		"esri/map",
		"esri/geometry/Point",
		"esri/symbols/SimpleMarkerSymbol",
		"esri/graphic", 
		"esri/layers/GraphicsLayer",
		"esri/layers/ArcGISDynamicMapServiceLayer",
		"esri/layers/ImageParameters",
		"dojo/domReady!"
	], function(Map, Point, SimpleMarkerSymbol, Graphic, GraphicsLayer, ArcGISDynamicMapServiceLayer) { 
		if(map == null){
			map = new Map("mapDiv", {
			center: [lon, lat],
			zoom: 15,
			basemap: "streets"
			});
		}
	
	if(showDrops == 'T'){
		//construct ArcGISDynamicMapServiceLayer with imageParameters from above
		var dropSitesLayer = new ArcGISDynamicMapServiceLayer("http://38.124.164.214:6080/arcgis/rest/services/DropZones/MapServer");
		map.addLayer(dropSitesLayer);
	}
		
	var urlBase = 'C:\\MTC\\MTC Tablet Screens\\images\\';
	var urlIcon = 'mtc_icons_v2_blue.png';
	switch(status){
		case 'ON PATROL':
			urlIcon = 'mtc_icons_v2_green.png';
			break;
		case 'ON BREAK':
			urlIcon = 'mtc_icons_v2_lt_blue.png';
			break;
		case 'ON LUNCH':
			urlIcon = 'mtc_icons_v2_lt_blue.png';
			break;
		case 'ON INCIDENT':
			urlIcon = 'mtc_icons_v2_blue.png';
			break;
		case 'LOGGED ON':
			urlIcon = 'mtc_icons_v2_gray.png';
			break;
		case 'ROLL OUT':
			urlIcon = 'mtc_icons_v2_lt_blue.png';
			break;
		case 'ROLL IN':
			urlIcon = 'mtc_icons_v2_lt_blue.png';
			break;
		case 'ON TOW':
			urlIcon = 'mtc_icons_v2_yellow.png';
			break;
	}
	var url = urlBase + urlIcon;

	
	
	map.on("load", function(){


		
	});
		var gl = new GraphicsLayer("truck");
		var p = new Point(lon, lat);
		var s = new esri.symbol.PictureMarkerSymbol({
			"angle":head,
			"xoffset":0,
			"yoffset":0,
			"type":"esriPMS",
			"url":url,
			"contentType":"image/png",
			"width":24,
			"height":24
		});
		if(map!=null){
			if(map.graphics!=null){
				map.graphics.clear();
			}
			if(map.layers!=null){
				map.removeLayer("truck");
				map.removeAllLayers();
			}
			var g = new Graphic(p,s);
			gl.add(g);
			map.addLayer(gl);
			map.centerAt(p);
		}
});
}
});