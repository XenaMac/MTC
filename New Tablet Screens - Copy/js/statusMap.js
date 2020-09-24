$(document).ready(function () {

    //dojo.require("dijit.layout.BorderContainer");
    //dojo.require("dijit.layout.ContentPane");
    //dojo.require("esri.map");
    //dojo.require("esri.layers.agsdynamic");
    //dojo.require("esri.layers.FeatureLayer");
    //dojo.require("esri.tasks.query");
    var map; //main map
    var _buddyData = "NA"; //list of other trucks on same beat
    var dropSitesLayer; //drop sites layer
    var dropSitesFeatureLayer; //feature layer
    //var infoTemplate = new esri.InfoTemplate();
    var gl; //graphics layer
    var glBuds; //beat buddies
    var p; //map point
    var pFind; //find me button
    var s; //picture marker symbol;
    var g; //graphic
    var t; //textobject
    var urlBase; //where the icons are located
    var myLon = 0.0;
    var myLat = 0.0;
    var showDrops = 'F';
    loadMap();
    getPosition();
    //set up for repeat checking
    var positionTimer;
    positionTimer = setInterval(function () { getPosition() }, 20000);
    var buddyTimer;
    buddyTimer = setInterval(function () { findBeatPartners() }, 20000);

    /********************
	*
	* Map Init, run once
	*
	********************/
    function loadMap() {
        require([
			"esri/map",
            "esri/basemaps",
			"esri/geometry/Point",
			"esri/symbols/SimpleMarkerSymbol",
			"esri/graphic",
			"esri/layers/GraphicsLayer",
			"esri/layers/ArcGISDynamicMapServiceLayer",
			"esri/layers/ImageParameters",
			"dojo/domReady!"
        ], 
		function (Map, Point, SimpleMarkerSymbol, Graphic, GraphicsLayer, ArcGISDynamicMapServiceLayer) {
		    map = new Map("mapDiv", {
		        center: [-122.3000, 37.8000],
		        zoom: 12,
		        basemap: "streets"
		    });

		    $(window).load(function () {
		        $(".loader").fadeOut("slow");
		    })
		});
    }

    /************************
	*
	* Show / Hide Drop Sites
	*
	************************/
    function addSites() {
        require([
			"esri/map",
			"esri/geometry/Point",
			"esri/symbols/SimpleMarkerSymbol",
			"esri/graphic",
			"esri/layers/GraphicsLayer",
			"esri/layers/ArcGISDynamicMapServiceLayer",
			"esri/layers/ImageParameters",
			"esri/layers/FeatureLayer",
			"dojo/domReady!"
        ], function (Map, Point, SimpleMarkerSymbol, Graphic, GraphicsLayer, ArcGISDynamicMapServiceLayer, FeatureLayer, InfoTemplate) {
            if (map != null) {
				dropSitesLayer = new ArcGISDynamicMapServiceLayer("http://services4.geopowered.com/arcgis/rest/services/LATA/DropSites2015/MapServer");
                //dropSitesLayer = new ArcGISDynamicMapServiceLayer("http://38.124.164.214:6080/arcgis/rest/services/DropZones/MapServer");
                //dropSitesLayer = new ArcGISDynamicMapServiceLayer("http://38.124.164.214:6080/arcgis/rest/services/DropZones/FeatureServer");
                /*
				var infoTemplate = new InfoTemplate();
				infoTemplate.setTitle("${Name}");
				infoTemplate.setContent("<b>Name: </b>${Name}");
				dropSitesFeatureLayer = new esri.layers.FeatureLayer("http://38.124.164.214:6080/arcgis/rest/services/DropZones/FeatureServer/0",{
					mode: esri.layers.FeatureLayer.MODE_ONDEMAND,
					infoTemplate: infoTemplate,
					outFields: ["*"]
				});
				*/
                map.addLayer(dropSitesLayer);
                //map.addLayer(dropSitesFeatureLayer);
            }
            else {
                alert('no map');
            }
        });
    }

    function removeSites() {
        require([
			"esri/map",
			"esri/geometry/Point",
			"esri/symbols/SimpleMarkerSymbol",
			"esri/graphic",
			"esri/layers/GraphicsLayer",
			"esri/layers/ArcGISDynamicMapServiceLayer",
			"esri/layers/ImageParameters",
			"esri/layers/FeatureLayer",
			"dojo/domReady!"
        ], function (Map, Point, SimpleMarkerSymbol, Graphic, GraphicsLayer, ArcGISDynamicMapServiceLayer, FeatureLayer) {
            if (map != null) {
                map.removeLayer(dropSitesLayer);
                //map.removeLayer(dropSitesFeatureLayer);
            }
            else {
                alert('no map');
            }
        });
    }

    /*********************************
	*
	* Get Vehicle Lat/Lon from service
	*
	**********************************/

    function getPosition() {
        var logonStatus = localStorage["logonStatus"];
        if (logonStatus != "1") {
            localStorage.clear();
            window.location.href = 'Logon.html';
        }
        else {
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
        /*
		else{
			var _data = $.parseJSON(result.d);
			//truckNumber, lat, lon, heading
			require([
			"esri/map",
			"esri/geometry/Point",
			"esri/symbols/SimpleMarkerSymbol",
			"esri/graphic", 
			"esri/layers/GraphicsLayer",
			"esri/layers/ArcGISDynamicMapServiceLayer",
			"esri/layers/ImageParameters",
			"dojo/domReady!"
			], function(Map, Point, SimpleMarkerSymbol, Graphic, GraphicsLayer, ArcGISDynamicMapServiceLayer){
			
				if(map.graphics!=null){
					var layrIDs = map.graphicsLayerIds;
					if(layrIDs.length > 1){
						for(var i = 2;i<layrIDs.length;i++){
							map.removeLayer(map.getLayer(map.graphicsLayerIds[i]));
						}
					}
				}
				if(map.layers!=null){
					var lr = map.layerIds;
				}
				if(gl==null){
					glBuds = new GraphicsLayer({id:"buddies"});
				}
				else{
					glBuds.clear();
				}
				var url = findImage('LOGGED ON'); //load the correct icon
				for (var i = 0; i < _data.length; i++) {
					var pBud = new Point(_data[i].lon, _data[i].lon);
					var sBud = new esri.symbol.PictureMarkerSymbol({
						"angle":_data[i].heading,
						"xoffset":0,
						"yoffset":0,
						"type":"esriPMS",
						"url":url,
						"contentType":"image/png",
						"width":24,
						"height":24
					});
					var gBud = new Graphic(pBud, sBud);
					glBuds.add(gBud);
					map.resize();
					map.addLayer(glBuds);
				}
			});

		}*/
    }

    function findBeatPartnersError(error) { }

    /*************************
	*
	* Update Position On Map
	*
	*************************/

    function addPosition(lon, lat, status, head) {
        if (map == null) {
            //alert('no map');
        } //end if
        else {
            var url = findImage(status);
            //alert(url);

            //place the pin
            require([
			"esri/map",
			"esri/geometry/Point",
			"esri/symbols/SimpleMarkerSymbol",
			"esri/graphic",
			"esri/layers/GraphicsLayer",
			"esri/layers/ArcGISDynamicMapServiceLayer",
			"esri/layers/ImageParameters",
			"esri/symbols/TextSymbol",
			"dojo/domReady!"
            ], function (Map, Point, SimpleMarkerSymbol, Graphic, GraphicsLayer, ArcGISDynamicMapServiceLayer, TextSymbol) {

                if (map.graphics != null) {
                    var layrIDs = map.graphicsLayerIds;
                    if (layrIDs.length > 0) {
                        map.removeLayer(map.getLayer(map.graphicsLayerIds[0]));
                    }
                }
                if (map.layers != null) {
                    var lr = map.layerIds;
                    //map.removeLayers();
                    //map.graphicsLayers.clear();
                    //map.layers = null;
                    //map.removeAllLayers();
                }
                if (gl == null) {
                    gl = new GraphicsLayer({ id: "truck" });
                    //gl.clear();
                }
                else {
                    gl.clear();
                }
                if (p != null) { p = null; }
                p = new Point(lon, lat);
                myLon = lon;
                myLat = lat;
                if (s != null) { s = null; }
                s = new esri.symbol.PictureMarkerSymbol({
                    "angle": head,
                    "xoffset": 0,
                    "yoffset": 0,
                    "type": "esriPMS",
                    "url": url,
                    "contentType": "image/png",
                    "width": 24,
                    "height": 24
                });
                if (_buddyData != "NA") {
                    var burl = findImage('WAITING'); //load the correct icon
                    if (glBuds == null) {
                        glBuds = new GraphicsLayer({ id: "buddies" });
                    }
                    else {
                        glBuds.clear();
                    }
                    for (var i = 0; i < _buddyData.length; i++) {
                        var stat = _buddyData[i].status;
                        burl = findImage(stat);
                        var pBud = new Point(_buddyData[i].lon, _buddyData[i].lat);
                        //var pBudtext = new Point(_buddyData[i].lon, _buddyData[i].lat); //point holder for text label for bro
                        var sBud = new esri.symbol.PictureMarkerSymbol({
                            "angle": _buddyData[i].heading,
                            "xoffset": 0,
                            "yoffset": 0,
                            "type": "esriPMS",
                            "url": burl,
                            "contentType": "image/png",
                            "width": 24,
                            "height": 24
                        });
                        /*
						var sBudText = new esri.symbol.TextSymbol({
							"text":_buddyData[i].truckNumber,
							"xoffset":12,
							"yoffset":12
						});
						*/
                        var gBud = new Graphic(pBud, sBud);

                        glBuds.add(gBud);
                        var bfont = new esri.symbol.Font();
                        bfont.setSize("14pt");
                        bfont.setWeight(esri.symbol.Font.WEIGHT_BOLD);
                        var tNumB = _buddyData[i].truckNumber;
                        var lastTwoB = '';
                        if (tNumB != null && tNumB.length > 2) {
                            lastTwoB = tNumB.substring(tNumB.length - 2, tNumB.length);
                        }
                        else {
                            lastTwoB = 'UN';
                        }
                        glBuds.add(new esri.Graphic(pBud, new esri.symbol.TextSymbol(lastTwoB).setOffset(0, 18).setFont(bfont)));
                    }
                }
                g = new Graphic(p, s);
                gl.add(g);
                var font = new esri.symbol.Font();
                font.setSize("14pt");
                font.setWeight(esri.symbol.Font.WEIGHT_BOLD);
                var tNum = localStorage["trucknumber"];
                var lastTwo = '';
                if (tNum != null && tNum.length > 2) {
                    lastTwo = tNum.substring(tNum.length - 2, tNum.length);
                }
                else {
                    lastTwo = 'UN';
                }
                gl.add(new esri.Graphic(p, new esri.symbol.TextSymbol(lastTwo).setOffset(0, 18).setFont(font)));
                if (_buddyData != "NA") {
                    map.addLayer(glBuds);
                }
                map.resize();
                //map.refresh();
                map.addLayer(gl);
                //map.centerAt(p);
            });

        } //end else
    } //end f

    /*****************************
	*
	* Button Clicks
	*
	*****************************/

    $('#btnDropSites').click(function () {
        if (showDrops == 'F') {
            addSites();
            showDrops = 'T';
            //$('#btnDropSites').css("background-color", "green");
            // /images/
            $('#btnDropSites').attr('src', 'images/MTC - Tablet Buttons_v4_DROP_SITES_GREEN.png');
        }
        else {
            removeSites();
            showDrops = 'F';
            //$('#btnDropSites').css("background-color", "black");
            $('#btnDropSites').attr('src', 'images/MTC - Tablet Buttons_v4_DROP_SITES.png');
        }
    });


    /*****************************
	*
	* Test button Clicks
	*
	*****************************/

    $('#btnLat1').click(function () {
        var lat = $('#lat1').val();
        var lon = $('#lon1').val();
        addPosition(lon, lat, 'ON PATROL', 182);
    });

    $('#btnLat2').click(function () {
        var lat = $('#lat2').val();
        var lon = $('#lon2').val();
        addPosition(lon, lat, 'ON PATROL', 250);
    });

    $('#btnAddSites').click(function () {
        addSites();
    });

    $('#btnRemoveSites').click(function () {
        removeSites();
    });

    $('#btnFindMe').click(function () {
        require([
			"esri/map",
			"esri/geometry/Point",
			"dojo/domReady!"
        ], function (Map, Point) {
            if (pFind != null) { pFind = null; }
            pFind = new Point(myLon, myLat);
            map.resize();
            map.centerAt(p);
        });
    });

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

    function showFallR(message) { //messaging
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