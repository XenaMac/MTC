

var TT_DATA_UPDATE_INTERVAL_IN_MILLISECONDS = 10000;
var SERVICE_BASE_URL = 'http://localhost/FSP.Web';  //$("#websitePath").attr("data-websitePath");
var TRUCK_IMAGE_BASE_URL = $("#websitePath").attr("data-websitePath") + "/Content/Images/";
var DEFAULT_MAP_CENTER_LAT = 33.6600;
var DEFAULT_MAP_CENTER_LON = -117.7927;
var DEFAULT_MAP_ZOOM = 10;
//var SERVICE_BASE_URL = 'http://amyanderic.no-ip.org:801/TowTruckService.svc';





var defaultMapLocation;
var map;
var infowindow;
var myLayout;
var orangeCountryPolygon;
var currentTruckData;
var towTruckMarkersArray = [];
var beatSegmentsArray = [];

var grid;

var selectedTruckId;
var isFollowing = false;
var currentMapLocation;
var gettingTruckData = false;
var countDownCounter = 0;

$(document).ready(function () {

    $("#btnUnFollow").hide();

    $("#divDebug").hide();

    $('#towTruckGrid').tablesorter();

    $("#update").click(function () {
        $("#divDebug").toggle();
    });


    initializeScreenLayout();

    initializeGoogleMap();
   
    //auto update train data
    window.setInterval(GetTowTrucks, TT_DATA_UPDATE_INTERVAL_IN_MILLISECONDS);

    window.setInterval(UpdateCountDown, 1000);

    UpdateCountDown();

    GetTowTrucks();

    GetBeats();

    GetBeatSegments();

    window.onresize = function (event) {
        try {

            google.maps.event.trigger(map, "resize");

            var windowHeight = window.innerHeight;
            $('#mapContainer').css({ 'height': windowHeight - 100 + 'px' });

        } catch (e) {

        }
    }

    function initializeScreenLayout() {

        var windowHeight = window.innerHeight;
        $('#mapContainer').css({ 'height': windowHeight - 100 + 'px' });

        // NOW create the layout
        myLayout = $('#mapContainer').layout({
            spacing_closed: 14
        , initClosed: false
        , closable: true
        , resizable: true
        , slidable: true
        , livePaneResizing: true
        , fxName: "bounce"
        , fxSpeed: "slow"
        , fxSettings: { easing: "easeOutBounce" }
        , north__fxName: "none"
        , north__resizable: false
        , north__slidable: false
        , north__closable: false
        , north__size: 'auto'
        , north__spacing_open: 1
        , center__size: '400'
        , center__minSize: "300"
        , south__spacing_closed: 15
        , south__spacing_open: 5
        , south__resizable: true
        , south__slidable: true
        , south__closable: true
        , south__size: "300"
        , south__maxSize: "700"
        , south__minSize: "100"
        , south__togglerTip_open: "Hide Grid"
        , south__togglerTip_closed: "Show Grid"
        , south__fxName: "drop"
        , south__fxSpeed_open: "fast"
        , south__fxSettings_open: { easing: "" }
        , south__fxSpeed_closed: "slow"
        , south__fxSettings_closed: { easing: "easeOutBounce" }
        , south__onresize: function () {
            google.maps.event.trigger(map, "resize");
        }
        , south__onclose: function () {
            google.maps.event.trigger(map, "resize");
        }
        , south__onopen: function () {
            google.maps.event.trigger(map, "resize");
        }
        });

    }
    
    $("#btnUnFollow").click(function () {
        UnFollowTruck();
    });
   
    $("#toggleCountyLayerCheckBox").click(function () {
        var checkedState = $(this).is(':checked');

        if (checkedState === false) {
            //hide county
            orangeCountryPolygon.setMap(null);
        }
        else {
            //show county
            orangeCountryPolygon.setMap(map);
        }
    });

    $("#toggleBeats").click(function () {
        var checkedState = $(this).is(':checked');

        try {

            //toggle actual segments
            if (beatsArray != null && beatsArray.length > 0) {
                for (var i = 0; i < beatsArray.length; i++) {
                    if (checkedState === false) {
                        //hide all beats and segments
                        beatsArray[i].setMap(null);
                    }
                    else {
                        //show all beats and segments
                        beatsArray[i].setMap(map);
                    }
                }
            }
        } catch (e) {

        }

    });
    
    $("#btnFilter").click(function () {
        //get list of beats

        GetListOfActiveBeats();
        GetListOfActiveTowTruckContractors();
    });


    function GetListOfActiveBeats() {
        //url
        var url = SERVICE_BASE_URL + "/Map/GetListOfActiveBeats";

        $.ajax({
            url: url,
            type: "GET",
            dataType: "json",
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            },
            success: function (beatsData) {

                $("#beatsList").empty();
                for (var i = 0; i < beatsData.length; i++) {
                    var beatId = beatsData[i].BeatId;
                    var beatName = beatsData[i].BeatName;

                    var html = '<li><input type="checkbox" id="cb-1" value="1" class="corridorCheckbox" checked />&nbsp;' + beatName + '</li>';

                    $("#beatsList").append(html);
                }


            }
        });

    }

    function GetListOfActiveTowTruckContractors() {
        //url
        var url = SERVICE_BASE_URL + "/Map/GetListOfActiveTowTruckContractors";

        $.ajax({
            url: url,
            type: "GET",
            dataType: "json",
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            },
            success: function (data) {

                $("#contractorList").empty();
                for (var i = 0; i < data.length; i++) {
                    var contractorID = data[i].ContractorID;
                    var contractCompanyName = data[i].ContractCompanyName;

                    var html = '<li><input type="checkbox" id="cb-1" value="1" class="corridorCheckbox" checked />&nbsp;' + contractCompanyName + '</li>';

                    $("#contractorList").append(html);
                }


            }
        });

    }
    
});

function UpdateCountDown() {
    try {

        if (gettingTruckData === false) {

            countDownCounter += 1000;

            if (countDownCounter === TT_DATA_UPDATE_INTERVAL_IN_MILLISECONDS)
                countDownCounter = 0;

            var secondsUntilNextUpdate = (TT_DATA_UPDATE_INTERVAL_IN_MILLISECONDS - countDownCounter) / 1000;

            $("#countDownUpdate").text("(" + secondsUntilNextUpdate + ")")

        }
        else
            $("#countDownUpdate").text("(loading...)")



    } catch (e) {

    }
}


function GetTowTrucks() {

    //url
    var url = SERVICE_BASE_URL + "/Map/GetTowTrucks";
    //var url = SERVICE_BASE_URL + "/CurrentTrucks";

    gettingTruckData = true;

    $("#debugList").append("<li>Call Truck Service " + getCurrentTimeStamp() + "</li>");

    $.ajax({
        url: url,
        type: "POST",
        dataType: "json",
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        },
        success: function (truckData) {

           
            gettingTruckData = false;
            currentTruckData = truckData;

            //for each tow truck in service
            for (var i = 0; i < currentTruckData.length; i++) {
                try {
                    //tow truck properties from service
                    var truckId = currentTruckData[i].TruckId;
                    var heading = currentTruckData[i].Heading;


                    //#region MAP
                    var truckIndexToUpdate = DoesTruckLiveInGeoMap(truckId);

                    //place and or upate tow truck in map                  
                    ManageTowTruckMarker(currentTruckData[i], truckIndexToUpdate);

                    //rotate
                    $("#" + truckId).rotate(heading - 90);

                    //#endregion

                    //#region GRID

                    //ManageTowTruckGridRecord(truckId, lat, lon, state, lastUpdate);
                    ManageTowTruckGridRecord(currentTruckData[i]);

                    //#endregion

                    //if we have a truck selected and we are following it
                    if (selectedTruckId > 0 && isFollowing === true) {
                        map.setCenter(currentMapLocation);
                    }


                } catch (e) {

                }

            }

            $("#towTruckGrid").trigger("update");

            $("#debugList").append("<li>New Trucks received " + getCurrentTimeStamp() + "</li>");
            $("#lastUpdate").text("Last Update: " + getCurrentTimeStamp() + " with " + truckData.length + " tow trucks.");

        }
    });

}

function initializeGoogleMap() {


    try {

        //alert('geoMap');
        defaultMapLocation = new google.maps.LatLng(DEFAULT_MAP_CENTER_LAT, DEFAULT_MAP_CENTER_LON);

        //google map configurations
        var myOptions = {
            center: defaultMapLocation,
            zoom: DEFAULT_MAP_ZOOM,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        infowindow = new google.maps.InfoWindow({
            content: '',
            position: defaultMapLocation
        });

        google.maps.event.addListener(infowindow, 'closeclick', UnSelectTruck);

        //initialize google map      
        var mapCanvas = document.getElementById('map_canvas');
        map = new google.maps.Map(mapCanvas, myOptions);
        google.maps.event.trigger(map, "resize");


        //orange county lines        
        var orangeCountyCoords = new Array();
        var orangeCountyLatLonRaw = "-117.53781,33.45596 -117.53843,33.45564 -117.54267,33.45499 -117.5563,33.45218 -117.55649,33.45148 -117.55775,33.45159 -117.56443,33.4524 -117.56805,33.4529 -117.57268,33.45328 -117.57865,33.45329 -117.58085,33.44524 -117.58432,33.43236 -117.58882,33.41504 -117.58987,33.41041 -117.59029,33.40855 -117.59303,33.39888 -117.59364,33.39661 -117.59394,33.3953 -117.5941,33.394 -117.59464,33.39158 -117.59548,33.38783 -117.59588,33.38663 -117.59911,33.37503 -117.61404,33.33561 -117.62588,33.34211 -117.63465,33.34813 -117.63895,33.35223 -117.6444,33.35938 -117.64986,33.36401 -117.65802,33.36819 -117.66618,33.37503 -117.67129,33.37845 -117.68032,33.3845 -117.68421,33.3871 -117.68794,33.38979 -117.69067,33.39182 -117.69442,33.39459 -117.7067,33.40369 -117.71501,33.40862 -117.73534,33.40744 -117.74358,33.41215 -117.7552,33.42249 -117.76514,33.43154 -117.77077,33.43833 -117.77377,33.44218 -117.78147,33.45066 -117.78676,33.45675 -117.78937,33.4599 -117.79235,33.46535 -117.79454,33.47054 -117.79856,33.4729 -117.80012,33.47389 -117.80442,33.47662 -117.81128,33.48202 -117.81484,33.48721 -117.8166,33.491 -117.82284,33.49425 -117.82431,33.49522 -117.82632,33.49678 -117.82686,33.49708 -117.8342,33.49944 -117.83629,33.50016 -117.83732,33.5007 -117.86339,33.51437 -117.87029,33.51798 -117.87141,33.51857 -117.87178,33.51877 -117.87369,33.51977 -117.87589,33.52084 -117.87853,33.52213 -117.89076,33.52811 -117.89885,33.53206 -117.90563,33.53537 -117.90693,33.53601 -117.91086,33.53793 -117.91217,33.53857 -117.92185,33.54329 -117.93405,33.54925 -117.94957,33.55978 -117.95849,33.56583 -117.96447,33.56989 -117.97381,33.57623 -117.98261,33.58174 -117.98874,33.58557 -117.98881,33.58561 -117.98903,33.58575 -117.9891,33.58579 -117.99067,33.58677 -117.99538,33.58972 -117.99694,33.5907 -117.9972,33.59086 -117.99796,33.59133 -117.99822,33.59149 -117.99862,33.59174 -117.99982,33.5925 -118.00022,33.59275 -118.00089,33.59316 -118.00089,33.59317 -118.00089,33.59313 -118.005,33.59463 -118.01532,33.59949 -118.02497,33.60468 -118.03361,33.60935 -118.04201,33.6149 -118.04985,33.62001 -118.05612,33.62502 -118.05627,33.62514 -118.05672,33.62548 -118.05687,33.6256 -118.05773,33.62626 -118.0603,33.62823 -118.06115,33.62889 -118.06299,33.63031 -118.0685,33.63455 -118.07034,33.63596 -118.07507,33.63961 -118.08233,33.64437 -118.09647,33.65365 -118.10748,33.66878 -118.11202,33.67577 -118.12,33.68806 -118.12118,33.68988 -118.12472,33.69532 -118.1259,33.69714 -118.1259,33.69715 -118.11361,33.74681 -118.09183,33.75955 -118.0923,33.78732 -118.0767,33.81109 -118.06389,33.81931 -118.06327,33.82607 -118.06123,33.83427 -118.05647,33.84623 -118.04982,33.85361 -118.04199,33.86153 -118.03254,33.8662 -118.01824,33.87341 -118.01139,33.87821 -118.00281,33.88566 -117.9941,33.89133 -117.9853,33.89925 -117.9765,33.91917 -117.97652,33.93348 -117.97651,33.93966 -117.97648,33.94576 -117.9533,33.94591 -117.92758,33.94602 -117.90365,33.94584 -117.84442,33.94652 -117.7942,33.94654 -117.78341,33.9465 -117.78329,33.94641 -117.78189,33.94541 -117.78153,33.94516 -117.77824,33.94285 -117.77576,33.9411 -117.76939,33.93661 -117.76785,33.93549 -117.76547,33.93376 -117.75934,33.92933 -117.7505,33.9231 -117.74773,33.92119 -117.73949,33.91586 -117.73684,33.91414 -117.73546,33.91325 -117.7305,33.90996 -117.72557,33.90669 -117.72357,33.90538 -117.71896,33.90237 -117.71428,33.89931 -117.70729,33.89481 -117.69862,33.88922 -117.69319,33.88573 -117.69148,33.88462 -117.67946,33.87649 -117.67679,33.87351 -117.67394,33.87101 -117.67375,33.87083 -117.67311,33.87025 -117.67498,33.86811 -117.66241,33.8575 -117.64685,33.84101 -117.611,33.80147 -117.58582,33.77474 -117.57509,33.76658 -117.54905,33.76026 -117.53608,33.75183 -117.53568,33.74693 -117.53473,33.72114 -117.5341,33.71166 -117.51911,33.70836 -117.46957,33.69971 -117.45594,33.68899 -117.42404,33.66702 -117.41396,33.65758 -117.4212,33.64853 -117.43922,33.62577 -117.46539,33.59225 -117.4927,33.55771 -117.50167,33.54516 -117.51,33.52306 -117.50921,33.52053 -117.50338,33.51564 -117.50972,33.50534 -117.50972,33.50502 -117.50938,33.49185 -117.50857,33.47045 -117.51425,33.46779";
        var orangeCountyLatLonRawArray = orangeCountyLatLonRaw.split(" ");

        for (var i = 0; i < orangeCountyLatLonRawArray.length; i++) {
            var lat = orangeCountyLatLonRawArray[i].split(",")[1];
            var lon = orangeCountyLatLonRawArray[i].split(",")[0];
            orangeCountyCoords.push(new google.maps.LatLng(lat, lon));
        }

        orangeCountryPolygon = new google.maps.Polygon({
            paths: orangeCountyCoords,
            strokeColor: '#FF0000',
            strokeOpacity: 0.8,
            strokeWeight: 2,
            fillColor: '#FF0000',
            fillOpacity: 0.1
        });

        orangeCountryPolygon.setMap(map);


    } catch (e) {
        alert('error geo map ' + e);
    }

}

function FollowTruck() {
    try {
        //close info window
        if (infowindow) infowindow.close();

        //set following to trye
        isFollowing = true;

        //zoom into map and center on selected truck
        map.setZoom(15);
        map.setCenter(currentMapLocation);

        //show unfollow button and set its text
        $("#btnUnFollow").show();
        $("#btnUnFollow").text('Stop following ' + selectedTruckId);
    } catch (e) {

    }

}

function UnSelectTruck() {

    try {

    } catch (e) {

    }

}

function UnFollowTruck() {

    try {
        //center map back to original
        map.setCenter(defaultMapLocation);

        isFollowing = false;
        selectedTruckId = 0;
        map.setZoom(DEFAULT_MAP_ZOOM);

        $("#btnUnFollow").hide();

    } catch (e) {

    }

}

function DoesTruckLiveInGeoMap(carId) {

    var truckIndex;

    try {
        if (towTruckMarkersArray != null && towTruckMarkersArray.length > 0) {
            for (var ii = 0; ii < towTruckMarkersArray.length; ii++) {
                if (towTruckMarkersArray[ii].id === carId) {
                    truckIndex = ii;
                }
            }
        }
    } catch (e) {

    }

    return truckIndex;
}

function ManageTowTruckMarker(singleTruckData, truckIndexToUpdate) {

    var image;
    var trainUpdated = false;
    var imageSource;
    try {

        var truckId = singleTruckData.TruckId;
        var speed = singleTruckData.Speed;
        var lat = singleTruckData.Lat;
        var lon = singleTruckData.Lon;
        var heading = singleTruckData.Heading;
        var state = singleTruckData.State;
        var lastUpdate = singleTruckData.LastUpdate;
        var location = new google.maps.LatLng(lat, lon);

        imageSource = TRUCK_IMAGE_BASE_URL + "/" + state + ".png";
        image = "<img id='" + truckId + "' src='" + imageSource + "' alt=''height='9px' /><div><span style='color: blue; font-size:20px; width:50px; font-weight:bold;'>" + truckId + "</span></div>";


        if (truckIndexToUpdate >= 0 && truckIndexToUpdate != undefined) {

            //#region Update Existing Train
            towTruckMarkersArray[truckIndexToUpdate].setPosition(location);
            towTruckMarkersArray[truckIndexToUpdate].setTitle(truckId + ' ' + state);
            towTruckMarkersArray[truckIndexToUpdate].theLabel_.labelDiv_.innerHTML = image;
            //#endregion

        }
        else {

            //#region New Train
            var left = "-21px";

            var towTruckMarker = new MarkerWithLabel({
                position: location,
                id: truckId,
                draggable: false,
                map: map,
                labelText: image,
                labelClass: "labels", // the CSS class for the label       
                labelStyle: { top: "2px", left: left, opacity: 0.95 },
                labelVisible: true,
                title: truckId + ' ' + state
            });

            google.maps.event.addListener(towTruckMarker, "click", function () {
                if (infowindow) infowindow.close();

                //set selected truck/location values
                selectedTruckId = this.id;
                currentMapLocation = location;

                //center the map over selected train
                map.setCenter(location);

                infowindow.setPosition(location);
                infowindow.maxWidth = 600;
                infowindow.setContent(BuildTruckDetailsWindow(singleTruckData));

                infowindow.open(map, this);

            });

            towTruckMarkersArray.push(towTruckMarker);

            //#endregion
        }


    } catch (e) {
        alert("Manage Geo Map: " + e);
    }

}

function BuildTruckDetailsWindow(singleTruckData) {

    var content;

    try {

        var truckId = singleTruckData.TruckId;
        var speed = singleTruckData.Speed;
        var lat = singleTruckData.Lat;
        var lon = singleTruckData.Lon;
        var heading = singleTruckData.Heading;
        var state = singleTruckData.State;
        var lastUpdate = singleTruckData.LastUpdate;

        var dtLastUpdate = new Date(parseInt(lastUpdate.substr(6)));
        var lastUpdateString = parseDate(dtLastUpdate);


        content = "<div class='demo'>";
        content += "<div id='infoWindow' class='tabs'>";

        content += "<div id='tabs-1'>";
        content += "<table style='font-size:12px;' width='100%'>";
        content += "<tr>"
        content += "<td align='left' style='color:#cccccc' colspan='2'>"
        content += "<input onclick='FollowTruck();' type='button' class='btn btn-regular' value='Follow' />&nbsp;"
        content += "</td>";
        content += "</tr>";

        content += "<tr>";
        content += "<td>";
        content += "<br/>";
        content += "</td>";
        content += "</tr>";

        var imageSource = TRUCK_IMAGE_BASE_URL + "/" + state + ".png";

        content += "<tr>";
        content += "<td style='font-size:20px; font-weight:bold' colspan='2' valign='center'>";
        content += "<img src='" + imageSource + "' height='15px' />&nbsp;" + truckId + " (" + state + ")";
        content += "</td>";
        content += "</tr>";

        content += "<tr>";
        content += "<td>";
        content += "<br />";
        content += "</td>";
        content += "</tr>";

        content += "<tr>"
        content += "<td align='right' class='detailsWindowFieldCaption'>Speed: </td><td style='width:100%'><b>" + speed + "</b></td>";
        content += "</tr>";
        content += "<tr>"
        content += "<td align='right' class='detailsWindowFieldCaption'>State: </td><td><b><span id='selectedTrainMileMarker'>" + state + "</span></b></td>";
        content += "</tr>";
        content += "<tr>"
        content += "<td align='right' class='detailsWindowFieldCaption'>Last Update: </td><td><b><span id='selectedTrainSpeed'>" + lastUpdateString + "</span></b></td>";
        content += "</tr>";
        content += "</table>";
        content += "</div>";
        content += "</div>";
        content += "</div>";
    } catch (e) {

    }


    return content;
}

function SelectTruckInMap(singleTruckData) {

    try {

        var truckId = singleTruckData.TruckId;
        var speed = singleTruckData.Speed;
        var lat = singleTruckData.Lat;
        var lon = singleTruckData.Lon;
        var heading = singleTruckData.Heading;
        var state = singleTruckData.State;
        var lastUpdate = singleTruckData.LastUpdate;
        var location = new google.maps.LatLng(lat, lon);


        if (towTruckMarkersArray != null && towTruckMarkersArray.length > 0) {
            for (var ii = 0; ii < towTruckMarkersArray.length; ii++) {
                if (towTruckMarkersArray[ii].id === truckId) {

                    selectedTruckId = truckId;
                    currentMapLocation = location;

                    if (infowindow) infowindow.close();

                    //center the map over selected train
                    map.setCenter(currentMapLocation);

                    infowindow.setPosition(currentMapLocation);
                    infowindow.maxWidth = 600;
                    infowindow.setContent(BuildTruckDetailsWindow(singleTruckData));

                    infowindow.open(map, towTruckMarkersArray[ii]);
                }
            }
        }
    } catch (e) {

    }
}

function GetBeats() {

    //url
    var url = SERVICE_BASE_URL + "/Map/GetBeats";

    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        },
        success: function (beatsData) {

            for (var i = 0; i < beatsData.length; i++) {
                try {

                    var name = beatsData[i].Name;
                    var points = beatsData[i].Points;

                    var coords = new Array();
                    for (var ii = 0; ii < points.length; ii++) {

                        var lat = points[ii].Lat;
                        var lon = points[ii].Lon;

                        coords.push(new google.maps.LatLng(lat, lon));
                    }

                    //beat polygon
                    var polygon = new google.maps.Polygon({
                        paths: coords,
                        strokeColor: '#0000ff',
                        strokeOpacity: 0.8,
                        strokeWeight: 2,
                        fillColor: '#0000ff',
                        fillOpacity: 0.1
                    });

                    google.maps.event.addListener(polygon, 'mouseover', function () {
                        tooltip.show(this.id);
                    });
                    google.maps.event.addListener(polygon, 'mouseout', function () {
                        tooltip.hide();
                    });

                    polygon.setMap(map);
                    beatSegmentsArray.push(polygon);


                } catch (e) {

                }

            }
        }
    });
}

function GetBeatSegments() {

    //url
    var url = SERVICE_BASE_URL + "/Map/GetBeatSegments";

    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        },
        success: function (beatsData) {

            for (var i = 0; i < beatsData.length; i++) {
                try {

                    var name = beatsData[i].Name;
                    var points = beatsData[i].Points;

                    var coords = new Array();
                    for (var ii = 0; ii < points.length; ii++) {

                        var lat = points[ii].Lat;
                        var lon = points[ii].Lon;

                        coords.push(new google.maps.LatLng(lat, lon));
                    }

                    var polygon = new google.maps.Polygon({
                        id: name,
                        paths: coords,
                        strokeColor: '#339933',
                        strokeOpacity: 0.8,
                        strokeWeight: 2,
                        fillColor: '#339933',
                        fillOpacity: 0.1
                    });

                    google.maps.event.addListener(polygon, 'mouseover', function () {
                        tooltip.show(this.id);
                    });
                    google.maps.event.addListener(polygon, 'mouseout', function () {
                        tooltip.hide();
                    });

                    polygon.setMap(map);
                    beatSegmentsArray.push(polygon);

                } catch (e) {

                }

            }
        }
    });
}


function ManageTowTruckGridRecord(singleTruckData) {

    var addRow = 'true';
    var rowCounter = 0;
    var rowId = 0;
    var gridTruckId = 0;
    var rowToUpdate = null;

    var truckId = singleTruckData.TruckId;
    var speed = singleTruckData.Speed;
    var lat = singleTruckData.Lat;
    var lon = singleTruckData.Lon;
    var heading = singleTruckData.Heading;
    var state = singleTruckData.State;
    var lastUpdate = singleTruckData.LastUpdate;


    var towTruckGrid = document.getElementById('towTruckGrid');
  


    try {

        var dtLastUpdate = new Date(parseInt(lastUpdate.substr(6)));
        var lastUpdateString = parseDate(dtLastUpdate);
        var imageSource = TRUCK_IMAGE_BASE_URL + "/" + state + ".png";


        //start at 1 to skip header
        for (var i = 0; i < towTruckGrid.rows.length; i++) {
            try {
                gridTruckId = $(towTruckGrid.rows[i].cells[0]).text();
                if (gridTruckId.toString() === truckId.toString()) {
                    addRow = 'false';

                    $(lmtGrid.rows[i].cells[1]).html("<a class='GridTruckId' id='TruckId-" + truckId + "' href='#' >" + truckId + "</a>");
                    $(lmtGrid.rows[i].cells[2]).text(speed);
                    $(lmtGrid.rows[i].cells[3]).text("<img src=" + imageSource + " alt=" + truckId + " height='15px'/>&nbsp;" + state);
                    $(lmtGrid.rows[i].cells[4]).text(lastUpdateString);

                    $("#debugList").append("<li>Updating truck  " + gridTruckId + "</li>");
                }

            } catch (e) {

            }
        }

        if (addRow === 'true') {
            var row = "<tr>";            
            row += "<td style='display:none'>" + truckId + "</td>";
            row += "<td><a class='GridTruckId' id='TruckId-" + truckId + "' href='#' >" + truckId + "</a></td>";
            row += "<td>" + speed + "</td>";
            row += "<td><img src=" + imageSource + " alt=" + truckId + " height='15px'/>&nbsp;" + state + "</td>";
            row += "<td>" + lastUpdateString + "</td>";
            row += "</tr>";

            $("#debugList").append("<li>Adding truck  " + gridTruckId + "</li>");
            $("#towTruckGrid").append(row);
        }

        
        //start at 1 to skip header
        for (var ii = 1; ii <= towTruckGrid.rows.length; ii++) {
            try {
                var row = $(towTruckGrid.rows[ii]);
                gridTruckId = $(towTruckGrid.rows[ii].cells[0]).text();

                var rowIsStillValid = 'false';

                //now let's check if htmlTableCarId is in the server feed
                for (var iii = 0; iii < currentTruckData.length; iii++) {
                    var truckId = currentTruckData[iii].TruckId;

                    if (truckId === gridTruckId) {
                        //the row exists in the feed
                        rowIsStillValid = 'true';
                    }
                }


                if (rowIsStillValid === 'false') {
                    //REMOVE TRAIN

                    $("#debugList").append("<li>Removing truck " + gridTruckId + "</li>");
                    row.remove();

                    //if (updateMap) {
                    //    $("#debugList").append("<li>Removing train from Map " + htmlTableCarId + "</li>");
                    //    RemoveTrainFromMap(htmlTableCarId);
                    //}

                    //if (updateRoute) {
                    //    $("#debugList").append("<li>Removing train from Route " + htmlTableCarId + "</li>");
                    //    RemoveTrainFromMapArray(htmlTableCarId);
                    //}
                }



            } catch (e) {

            }
        }
               

    } catch (e) {

    }

}




$(".GridTruckId").live("click", function (e) {
    var id = $(this).attr('id');
    var truckId = id.replace('TruckId-', '');

    if (currentTruckData != null && currentTruckData.length > 0) {
        for (var i = 0; i < currentTruckData.length; i++) {
            if (currentTruckData[i].TruckId.toString() === truckId) {
                SelectTruckInMap(currentTruckData[i]);
            }
        }
    }

    e.preventDefault()
});



function DrawPolygon(latLon, stroke, fill, name) {

    //orange county lines        
    var coords = new Array();
    var latLonRaw = "-117.53781,33.45596 -117.53843,33.45564 -117.54267,33.45499 -117.5563,33.45218 -117.55649,33.45148 -117.55775,33.45159 -117.56443,33.4524 -117.56805,33.4529 -117.57268,33.45328 -117.57865,33.45329 -117.58085,33.44524 -117.58432,33.43236 -117.58882,33.41504 -117.58987,33.41041 -117.59029,33.40855 -117.59303,33.39888 -117.59364,33.39661 -117.59394,33.3953 -117.5941,33.394 -117.59464,33.39158 -117.59548,33.38783 -117.59588,33.38663 -117.59911,33.37503 -117.61404,33.33561 -117.62588,33.34211 -117.63465,33.34813 -117.63895,33.35223 -117.6444,33.35938 -117.64986,33.36401 -117.65802,33.36819 -117.66618,33.37503 -117.67129,33.37845 -117.68032,33.3845 -117.68421,33.3871 -117.68794,33.38979 -117.69067,33.39182 -117.69442,33.39459 -117.7067,33.40369 -117.71501,33.40862 -117.73534,33.40744 -117.74358,33.41215 -117.7552,33.42249 -117.76514,33.43154 -117.77077,33.43833 -117.77377,33.44218 -117.78147,33.45066 -117.78676,33.45675 -117.78937,33.4599 -117.79235,33.46535 -117.79454,33.47054 -117.79856,33.4729 -117.80012,33.47389 -117.80442,33.47662 -117.81128,33.48202 -117.81484,33.48721 -117.8166,33.491 -117.82284,33.49425 -117.82431,33.49522 -117.82632,33.49678 -117.82686,33.49708 -117.8342,33.49944 -117.83629,33.50016 -117.83732,33.5007 -117.86339,33.51437 -117.87029,33.51798 -117.87141,33.51857 -117.87178,33.51877 -117.87369,33.51977 -117.87589,33.52084 -117.87853,33.52213 -117.89076,33.52811 -117.89885,33.53206 -117.90563,33.53537 -117.90693,33.53601 -117.91086,33.53793 -117.91217,33.53857 -117.92185,33.54329 -117.93405,33.54925 -117.94957,33.55978 -117.95849,33.56583 -117.96447,33.56989 -117.97381,33.57623 -117.98261,33.58174 -117.98874,33.58557 -117.98881,33.58561 -117.98903,33.58575 -117.9891,33.58579 -117.99067,33.58677 -117.99538,33.58972 -117.99694,33.5907 -117.9972,33.59086 -117.99796,33.59133 -117.99822,33.59149 -117.99862,33.59174 -117.99982,33.5925 -118.00022,33.59275 -118.00089,33.59316 -118.00089,33.59317 -118.00089,33.59313 -118.005,33.59463 -118.01532,33.59949 -118.02497,33.60468 -118.03361,33.60935 -118.04201,33.6149 -118.04985,33.62001 -118.05612,33.62502 -118.05627,33.62514 -118.05672,33.62548 -118.05687,33.6256 -118.05773,33.62626 -118.0603,33.62823 -118.06115,33.62889 -118.06299,33.63031 -118.0685,33.63455 -118.07034,33.63596 -118.07507,33.63961 -118.08233,33.64437 -118.09647,33.65365 -118.10748,33.66878 -118.11202,33.67577 -118.12,33.68806 -118.12118,33.68988 -118.12472,33.69532 -118.1259,33.69714 -118.1259,33.69715 -118.11361,33.74681 -118.09183,33.75955 -118.0923,33.78732 -118.0767,33.81109 -118.06389,33.81931 -118.06327,33.82607 -118.06123,33.83427 -118.05647,33.84623 -118.04982,33.85361 -118.04199,33.86153 -118.03254,33.8662 -118.01824,33.87341 -118.01139,33.87821 -118.00281,33.88566 -117.9941,33.89133 -117.9853,33.89925 -117.9765,33.91917 -117.97652,33.93348 -117.97651,33.93966 -117.97648,33.94576 -117.9533,33.94591 -117.92758,33.94602 -117.90365,33.94584 -117.84442,33.94652 -117.7942,33.94654 -117.78341,33.9465 -117.78329,33.94641 -117.78189,33.94541 -117.78153,33.94516 -117.77824,33.94285 -117.77576,33.9411 -117.76939,33.93661 -117.76785,33.93549 -117.76547,33.93376 -117.75934,33.92933 -117.7505,33.9231 -117.74773,33.92119 -117.73949,33.91586 -117.73684,33.91414 -117.73546,33.91325 -117.7305,33.90996 -117.72557,33.90669 -117.72357,33.90538 -117.71896,33.90237 -117.71428,33.89931 -117.70729,33.89481 -117.69862,33.88922 -117.69319,33.88573 -117.69148,33.88462 -117.67946,33.87649 -117.67679,33.87351 -117.67394,33.87101 -117.67375,33.87083 -117.67311,33.87025 -117.67498,33.86811 -117.66241,33.8575 -117.64685,33.84101 -117.611,33.80147 -117.58582,33.77474 -117.57509,33.76658 -117.54905,33.76026 -117.53608,33.75183 -117.53568,33.74693 -117.53473,33.72114 -117.5341,33.71166 -117.51911,33.70836 -117.46957,33.69971 -117.45594,33.68899 -117.42404,33.66702 -117.41396,33.65758 -117.4212,33.64853 -117.43922,33.62577 -117.46539,33.59225 -117.4927,33.55771 -117.50167,33.54516 -117.51,33.52306 -117.50921,33.52053 -117.50338,33.51564 -117.50972,33.50534 -117.50972,33.50502 -117.50938,33.49185 -117.50857,33.47045 -117.51425,33.46779";
    var latLonRawArray = latLonRaw.split(" ");

    for (var i = 0; i < latLonRawArray.length; i++) {
        var lat = latLonRawArray[i].split(",")[1];
        var lon = latLonRawArray[i].split(",")[0];
        coords.push(new google.maps.LatLng(lat, lon));
    }

    var polygon = new google.maps.Polygon({
        paths: coords,
        strokeColor: '#FF0000',
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: '#FF0000',
        fillOpacity: 0.1
    });

    polygon.setMap(map);

}

function parseDate(myDate) {
    try {
        var Month = myDate.getMonth();
        Month += 1;
        var Day = myDate.getDate();
        var Year = myDate.getFullYear();
        var Hour = myDate.getHours();
        var Minutes = myDate.getMinutes();
        var Seconds = myDate.getSeconds();
        var cDate = PadZeros(Month) + "/" + PadZeros(Day) + "/" + Year + " " + PadZeros(Hour) + ":" + PadZeros(Minutes) + ":" + PadZeros(Seconds);
        return cDate;
    } catch (e) {

    }
}

function PadZeros(val) {
    var str = '' + val;
    while (str.length < 2) {
        str = '0' + str;
    }
    return str;
}

function getCurrentTimeStamp() {
    var currentTime = new Date()
    var seconds = currentTime.getSeconds();
    var minutes = currentTime.getMinutes();
    var hours = currentTime.getHours();

    return hours + ":" + minutes + ":" + seconds;
}



