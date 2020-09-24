var mapProp;
var map;
var _buddyData = "NA";
var showDrops = "0";
var gmarkers = [];
var NoGPS = 0;
var WMarker = null;

function myMap() {
	mapProp= {
		center:new google.maps.LatLng(35.0853,-106.6056),
		zoom:15,
	};
	map=new google.maps.Map(document.getElementById("mapDiv"),mapProp);
}

$(document).ready(function(){

	updatePosition();
	var positionTimer;
	positionTimer = setInterval(function(){updatePosition()},20000);
	var buddyTimer;
	buddyTimer = setInterval(function(){findBeatPartners()},20000);
	
	$('#btnFindMe').click(function(){
		if(map != null && localStorage["lat"] != null && localStorage["lon"] != null){
			var lat = localStorage["lat"];
			var lon = localStorage["lon"];
			
			var myCenter = new google.maps.LatLng(lat, lon);
			map.setCenter(myCenter);
		}
	});
	
	function updatePosition(){
		//var logonStatus = localStorage["logonStatus"];
		var logonStatus = "1";
		if(logonStatus != "1"){
			localStorage.clear();
            window.location.href = 'Logon.html';
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
            else {
                localStorage["lat"] = _data.Lat;
                localStorage["lon"] = _data.Lon;
                var head = _data.Direction;
                var status = _data.TruckStatus;
                var truckNumber = _data.TruckNumber
                localStorage["trucknumber"] = truckNumber;
                $('#truckNumber').text(truckNumber);
                if (status == 'Driver Logged On') {
                    status = 'LOGGED ON';
                }
                $('#statusName').text(status);
                try {
                    addPosition(localStorage["lon"], localStorage["lat"], status, head);
                }
                catch (error) {
                    //showFallR('Can\'t find position');
                    //alert(error);
                }
                localStorage["TruckStatus"] = status;
            }
        }
        else {
            setLogOff();
        }
    }

    function GetPositionError(error) {
        if(NoGPS > 5) {
			//alert('Can\'t find position');
		} else {
			NoGPS += 1;
		}
    }
	
	function addPosition(lon, lat, status, head){
		for (var i = 0; i < gmarkers.length; i++) {
          gmarkers[i].setMap(null);
        }
		gmarkers = [];
		
		if(map == null){
			alert('no map');
		}
		else{
			var url = findImage(status);
			var myCenter = new google.maps.LatLng(lat, lon);
			var mapCanvas = document.getElementById("mapDiv");
            var mapOptions = {center: myCenter, zoom:15};

			//map = new google.maps.Map(mapCanvas, mapOptions);
			map.setCenter(myCenter);
			var marker = new google.maps.Marker({position:myCenter, icon:url, rotation:head});
            marker.setMap(map);
            gmarkers.push(marker);
			marker.setMap(map);
            if(localStorage["WazeMarker"] == "true") {
                //alert('Waze Initiated. Set WazeMarker.');
                setWazeMarker();
            } else {
                //alert('No Waze');
                deleteWazeMarker();
            }

			if(_buddyData != "NA"){
				for(var i = 0;i<_buddyData.length;i++){
					var stat = _buddyData[i].status;
					var burl = findImage(_buddyData[i].status);
					var bCenter = new google.maps.LatLng(_buddyData[i].lat, _buddyData[i].lon)
					var bMarker = new google.maps.Marker({position:bCenter,icon:burl, rotation:_buddyData[i].heading});
					gmarkers.push(bMarker);
					bMarker.setMap(map);
				}
			}
			$('img[src="${url}]').css({
				'-webkit-transform' : 'rotate('+ head +'deg)',
				'-moz-transform' : 'rotate('+ head +'deg)',
				'-ms-transform' : 'rotate('+ head +'deg)',
				'transform' : 'rotate('+ head +'deg)'
			});
		}
	}
	/*************************
	*
	* Display Waze Marker
	*
    *************************/
    
    function setWazeMarker() {        
        if(WMarker != null) { WMarker.setMap(null); }; 

        var icon = {
            url: 'css/images/Waze_Thumb.png', // url
            scaledSize: new google.maps.Size(30, 30) // scaled size
        };
		
		var content = localStorage["assistMessage"];

		var infowindow = new google.maps.InfoWindow({
			content: content
		});
        
        var WCenter = new google.maps.LatLng(localStorage["WazeLat"], localStorage["WazeLon"]);
       	WMarker = new google.maps.Marker({
               position: WCenter, 
               icon: icon,
               size: new google.maps.Size(30, 30)
            });
		WMarker.addListener('click', function() {
			infowindow.open(map, WMarker);
		  });

        WMarker.setMap(map);
    }

    function deleteWazeMarker() {    
        var Wurl = null;
        var WCenter =null;   
        if(WMarker != null) { WMarker.setMap(null); };  
        WMarker = null;
    }
	
	/*************************
	*
	* Display beat partners
	*
	*************************/

    function findBeatPartners() {
        var _url = ServiceLocation + 'AJAXFSPService.svc/GetBeatPartners';
        var _data = "_beat=" + localStorage["Beat"];
        $.ajax({
            type: "GET",
            data: _data,
            dataType: "json",
            url: _url,
            contentType: "application/json; charset=utf-8",
            success: findBeatPartnersSuccess,
            error: findBeatPartnersError
        });
    }

    function findBeatPartnersSuccess(result) {
        if (result.d == "NA") {
            _buddyData = "NA";
        }
        else {
            _buddyData = $.parseJSON(result.d);
        }
    }

    function findBeatPartnersError(error) { }
	
	/*************************
	*
	* Get Drop Sites
	*
	*************************/
	
	$('#btnDropSites').click(function(){
		if(showDrops == "0"){
			findDrops();
			showDrops = "1";
		}
		else{
			var lat = localStorage["lat"];
			var lon = localStorage["lon"];
			var myCenter = new google.maps.LatLng(lat, lon);
			var mapCanvas = document.getElementById("mapDiv");
			var mapOptions = {center: myCenter, zoom:15};
			map = new google.maps.Map(mapCanvas, mapOptions);
			map.setCenter(myCenter);
			showDrops = "0";
			updatePosition();
			
		}

	});
	
	function findDrops(){
		//var shapePath = map.overlay.getPath().getArray();
		var _url = ServiceLocation + 'AJAXFSPService.svc/getDropPolygons';
		var _data = "BeatNumber=" + localStorage["Beat"];
		$.ajax({
            type: "GET",
            data: _data,
            dataType: "json",
            url: _url,
            contentType: "application/json; charset=utf-8",
            success: findDropSitesSuccess,
            error: findDropSitesError
        });
	}
	
	function findDropSitesSuccess(result){
		_dropData = $.parseJSON(result.d);
		for(var i = 0;i<_dropData.length;i++){
			
			var cDrop = [];
			for(var iGF = 0;iGF < _dropData[i].geoFence.length;iGF++){
				//cDrop.push({"lat:" + _dropData[i].geoFence[iGF].lat, "lng:" + _dropData[i].geoFence[iGF].lon});
				storeCoord(_dropData[i].geoFence[iGF].lat, _dropData[i].geoFence[iGF].lon, cDrop);
			}
			
			var dropSite = new google.maps.Polygon({
				paths: cDrop,
				strokeColor: '#FF000',
				strokeOpacity: 0.8,
				strokeWeight: 2,
				fillColor: '#FF00',
				fillOpacity: 0.35
			});
			
			dropSite.setMap(map);
		}
	}
	
	function storeCoord(lat, lng, array){
		array.push({lat: lat, lng: lng});
	}
	
	function findDropSitesError(error){
		alert(error);
	}
	
	/*************************
	*
	* Helper Functions
	*
	*************************/

    function findImage(status) {
        //var urlBase = 'C:\\MTC\\MTC Tablet Screens\\images\\';
        var urlBase = 'images/'
        var urlIcon = 'mtc_icons_v2_blue.png';
        switch (status) {
            case 'ON PATROL':
                urlIcon = 'mtc_icons_v2_green.png';
                break;
            case 'ON BREAK':
                urlIcon = 'mtc_icons_v2_lt_blue.png';
                break;
            case 'ON LUNCH':
                urlIcon = 'mtc_icons_v2_lt_blue.png';
                break;
            case 'ENROUTE':
                urlIcon = 'mtc_icons_v2_orange.png';
                break;
            case 'ON INCIDENT':
                urlIcon = 'mtc_icons_v2_blue.png';
                break;
            case 'LOGGED ON':
                urlIcon = 'mtc_icons_v2_purple.png';
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
            case 'WAITING':
                urlIcon = 'mtc_icons_v2_gray.png';
                break;
			case 'BUSY':
				urlIcon = 'mtc_icons_v2_lt_blue.png';
				break;
        }
        var url = urlBase + urlIcon;
        return url;
    }

    // function showFallR(message) { //messaging
    //     $.fallr('show', {
    //         content: '<p>' + message + '</p>',
    //         position: 'center',
    //         icon: 'lamp'
    //     });
    // }
	
	/*********************************
	*
	* Force Log Off
	*
	*********************************/

    function setLogOff() {
        localStorage["TruckStatus"] = status;
        $('#statusName').text(status);
        //tell the service truck status has changed
        var logonStatus = localStorage["logonStatus"];
        if (logonStatus != "1") {
            localStorage.clear();
            window.location.href = 'Logon.html';
        }
        else {
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
});