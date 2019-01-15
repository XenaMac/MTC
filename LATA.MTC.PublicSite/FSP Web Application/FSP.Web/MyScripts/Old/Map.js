/// <reference path="../Scripts/knockout-2.1.0.js" />
/// <reference path="http://maps.googleapis.com/maps/api/js?key=AIzaSyDVGMBMCxemD0_VM3UjKVKNxfC1lstlTW4&sensor=false" type="text/javascript" />"
var map;
var infowindow;

$(function () {

    var SERVICE_BASE_URL = $("#websitePath").attr("data-websitePath");
    var TRUCK_IMAGE_BASE_URL = $("#websitePath").attr("data-websitePath") + "/Content/Images/";
    var DEFAULT_MAP_CENTER_LAT = 33.6600;
    var DEFAULT_MAP_CENTER_LON = -117.7927;
    var DEFAULT_MAP_ZOOM = 11;
    var defaultMapLocation;
    var currentMapLocation;

    var markersArray = [];

    var beatSegmentsArray = [];
    var beatSegmentsLabels = [];
   
    var beatPolygons = [];
    var beatLabels = [];
   
    var callBoxArray = [];


    //size mape to window height  
    $('#map').css({ 'height': window.innerHeight - 150 + 'px' });

    initializeGoogleMap();

    GetBeatSegments();

    GetBeats();

    GetCallBoxes();

    window.onresize = function (event) {
        try {
            $('#map').css({ 'height': window.innerWidth - 150 + 'px' });
        } catch (e) {

        }
    }

    $(".newTowTruckWindow").click(function (event) {

        try {
            var url = $(this).attr("href");
            var windowName = "Tow Truck Grid";
            var windowSize = 'width=800,height=800';
            window.open(url, windowName, windowSize);
            event.preventDefault();
        } catch (e) {

        }


    });

    //toggle beats
    $("#toggleBeats").click(function () {
        var checkedState = $(this).is(':checked');

        try {

            //toggle actual segments
            if (beatPolygons != null && beatPolygons.length > 0) {
                for (var i = 0; i < beatPolygons.length; i++) {
                    if (checkedState === false) {
                        //hide all beats and segments
                        beatPolygons[i].setMap(null);
                        beatLabels[i].setMap(null);
                    }
                    else {
                        //show all beats and segments
                        beatPolygons[i].setMap(map);
                        beatLabels[i].setMap(map);
                    }
                }
            }
        } catch (e) {

        }

    });

    //toggle beat segments
    $("#toggleSegments").click(function () {
        var checkedState = $(this).is(':checked');

        try {

            //toggle actual segments
            if (beatSegmentsArray != null && beatSegmentsArray.length > 0) {
                for (var i = 0; i < beatSegmentsArray.length; i++) {
                    if (checkedState === false) {
                        //hide all beats and segments
                        beatSegmentsArray[i].setMap(null);
                        beatSegmentsLabels[i].setMap(null);
                    }
                    else {
                        //show all beats and segments
                        beatSegmentsArray[i].setMap(map);
                        beatSegmentsLabels[i].setMap(map);
                    }
                }
            }
        } catch (e) {

        }

    });

    //toggle call boxes
    $("#toggleCallBoxes").click(function () {
        var checkedState = $(this).is(':checked');

        //toggle actual segments
        if (callBoxArray != null && callBoxArray.length > 0) {
            for (var i = 0; i < callBoxArray.length; i++) {
                if (checkedState === false) {
                    //hide all beats and segments
                    callBoxArray[i].setMap(null);
                }
                else {
                    //show all beats and segments
                    callBoxArray[i].setMap(map);
                }
            }
        }
    });

    //beat Number autocomplete
    $("#beatsFilter").autocomplete({
        source: function (request, response) {

            var url =  SERVICE_BASE_URL +  "/Map/GetBeatNumbers";

            $.ajax({
                url: url,
                dataType: "json",
                data: {
                    featureClass: "P",
                    style: "full",
                    maxRows: 12,
                    name_startsWith: request.term
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Number,
                            value: item.Number,
                            myValue: item.Number
                        }
                    }));
                },
                error: function (data) {
                    alert(data);
                }
            });
        },
        minLength: 0,
        select: function (event, ui) {
            try {
                //find the beat with this number and zoom to it

                //toggle actual segments
                if (beatLabels != null && beatLabels.length > 0) {
                    for (var i = 0; i < beatLabels.length; i++) {
                        var beatNumber = beatLabels[i].id;

                        if (beatNumber === ui.item.value) {
                            //zoom to beat/label?                          
                            map.setCenter(beatLabels[i].pos);
                            map.setZoom(13);

                            //close model
                            $("#filterModal").modal('hide');
                        }


                       
                    }
                }
            } catch (e) {

            }                      
        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });

    function GetBeats() {

        //url
        var url = SERVICE_BASE_URL + "/Map/GetBeats";

        $.ajax({
            url: url,
            type: "GET",
            dataType: "json",
            error: function (xhr, ajaxOptions, thrownError) {
                //alert(xhr.status);
                //alert(thrownError);
            },
            success: function (beatsData) {


                for (var i = 0; i < beatsData.length; i++) {
                    try {

                        var number = beatsData[i].Number;
                        var description = beatsData[i].Description;
                       
                        var points = beatsData[i].Points;
                        var labelPosition;

                        var coords = new Array();
                        for (var ii = 0; ii < points.length; ii++) {

                            var lat = points[ii].Lat;
                            var lon = points[ii].Lon;

                            if (ii == 5) { //Math.ceil(points.length / 2)) {
                                //place label in the middle of the beats
                                labelPosition = new google.maps.LatLng(lat, lon);
                            }

                            coords.push(new google.maps.LatLng(lat, lon));
                        }

                        //beat polygon
                        var beatPolygon = new google.maps.Polygon({
                            id: number,
                            desc: description,
                            paths: coords,
                            strokeColor: '#0000ff',
                            strokeOpacity: 0.8,
                            strokeWeight: 2,
                            fillColor: 'Transparent',
                            fillOpacity: 0.1,
                            hoverColor: 'Red',
                            baseColor: '#0000ff'
                        });

                        var beatLabelContent = customTxt = "<div>" + number + "</div>";

                        var beatLabel = new TxtOverlay(
                            number,
                            labelPosition,
                            beatLabelContent,
                            description,
                            map,
                            "visible",
                            "position: absolute; border: 1px solid black; border-radius: 3px; background: #efefef; padding: 3px; white-space: nowrap; z-index:-1; opacity: 0.9 "
                        );

                        //beat label events
                        google.maps.event.addListener(beatLabel, 'click', function () {

                            //zoom to beat/label?                          
                            map.setCenter(this.pos);
                            map.setZoom(13);

                        });
                        google.maps.event.addListener(beatLabel, 'mouseover', function () {
                            tooltip.show(this.desc);

                            //toggle actual segments
                            if (beatPolygons != null && beatPolygons.length > 0) {
                                for (var i = 0; i < beatPolygons.length; i++) {
                                    if (beatPolygons[i].id === this.id) {
                                        beatPolygons[i].setOptions({ fillColor: beatPolygons[i].hoverColor });
                                        beatPolygons[i].setOptions({ fillOpacity: 1 });
                                    }
                                }
                            }

                        });
                        google.maps.event.addListener(beatLabel, 'mouseout', function () {
                            tooltip.hide();

                            //toggle actual segments
                            if (beatPolygons != null && beatPolygons.length > 0) {
                                for (var i = 0; i < beatPolygons.length; i++) {
                                    if (beatPolygons[i].id === this.id) {
                                        beatPolygons[i].setOptions({ fillColor: 'Transparent' });
                                        beatPolygons[i].setOptions({ fillOpacity: 1 });
                                    }
                                }
                            }

                        });

                        //beat polygon events
                        google.maps.event.addListener(beatPolygon, 'mouseover', function () {
                            tooltip.show(this.desc);
                        });
                        google.maps.event.addListener(beatPolygon, 'mouseout', function () {
                            tooltip.hide();
                        });

                        beatPolygon.setMap(map);
                        beatPolygons.push(beatPolygon);
                        beatLabels.push(beatLabel);                      

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
                //alert(xhr.status);
                //alert(thrownError);
            },
            success: function (beatsData) {

                for (var i = 0; i < beatsData.length; i++) {
                    try {
                        var number = beatsData[i].Number;
                        var description = beatsData[i].Description;
                        var points = beatsData[i].Points;


                        var labelPosition;

                        var coords = new Array();
                        for (var ii = 0; ii < points.length; ii++) {

                            var lat = points[ii].Lat;
                            var lon = points[ii].Lon;

                            if (ii == 0) {//Math.ceil(points.length / 2)) {
                                //place label in the middle of the beats
                                labelPosition = new google.maps.LatLng(lat, lon);
                            }

                            coords.push(new google.maps.LatLng(lat, lon));
                        }

                        var polygon = new google.maps.Polygon({
                            id: description,
                            paths: coords,
                            strokeColor: 'Black',
                            strokeOpacity: 0.8,
                            strokeWeight: 2,
                            fillColor: 'Transparent',
                            //fillOpacity: 0.1
                        });


                        //var mapLabel = new MapLabel({
                        //    text: number,
                        //    position: labelPosition,
                        //    map: map,
                        //    fontSize: 12,
                        //    align: 'right',
                        //    visible: false
                        //});

                        var beatSegmentLabelContent = customTxt = "<div>" + number + "</div>";
                        var beatSegmentLabel = new TxtOverlay(
                            number,
                            labelPosition,
                            beatSegmentLabelContent,
                            description,
                            map,
                            "hidden",
                            "position: absolute; border: 1px solid black; border-radius: 3px; background: #ffffff; padding: 1px; white-space: nowrap; z-index:-1; opacity: 0.9 "
                        );

                        google.maps.event.addListener(polygon, 'mouseover', function () {
                            tooltip.show(this.id);
                        });
                        google.maps.event.addListener(polygon, 'mouseout', function () {
                            tooltip.hide();
                        });

                        polygon.setMap(map);
                        beatSegmentsArray.push(polygon);
                        beatSegmentsLabels.push(beatSegmentLabel);

                    } catch (e) {

                    }

                }
            }
        });
    }

    function GetCallBoxes() {

        //url
        var url = SERVICE_BASE_URL + "/Map/GetCallBoxes";

        $.ajax({
            url: url,
            type: "GET",
            dataType: "json",
            error: function (xhr, ajaxOptions, thrownError) {
                //alert(xhr.status);
                //alert(thrownError);
            },
            success: function (callBoxData) {

                for (var i = 0; i < callBoxData.length; i++) {
                    try {

                        var callBoxId = callBoxData[i].CallBoxId;
                        var phoneNumber = callBoxData[i].TelephoneNumber;
                        var freewayId = callBoxData[i].FreewayId;
                        var locationDescription = callBoxData[i].LocationDescription;
                        var lat = callBoxData[i].Lat;
                        var lon = callBoxData[i].Lon;

                        var position = new google.maps.LatLng(lat, lon);


                        var image = SERVICE_BASE_URL + '/content/images/callBox.png';
                        var callBox = new google.maps.Marker({
                            position: position,
                            map: map,
                            icon: image
                        });

                        callBoxArray.push(callBox);


                    } catch (e) {

                    }

                }
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

            //initialize google map      
            var mapCanvas = document.getElementById('map');
            map = new google.maps.Map(mapCanvas, myOptions);
            google.maps.event.trigger(map, "resize", function () {

                alert('map resized');

            });


            var listener = google.maps.event.addListener(map, 'zoom_changed', function () {

                try {
                    var visibility = "hidden";
                    if (map.zoom > 12)
                        visibility = "visible";

                    //toggle actual segments
                    if (beatSegmentsLabels != null && beatSegmentsLabels.length > 0) {
                        for (var i = 0; i < beatSegmentsLabels.length; i++) {
                            beatSegmentsLabels[i].toggle(visibility);
                        }
                    }
                } catch (e) {

                }


            });


            ////orange county lines        
            //var orangeCountyCoords = new Array();
            //var orangeCountyLatLonRaw = "-117.53781,33.45596 -117.53843,33.45564 -117.54267,33.45499 -117.5563,33.45218 -117.55649,33.45148 -117.55775,33.45159 -117.56443,33.4524 -117.56805,33.4529 -117.57268,33.45328 -117.57865,33.45329 -117.58085,33.44524 -117.58432,33.43236 -117.58882,33.41504 -117.58987,33.41041 -117.59029,33.40855 -117.59303,33.39888 -117.59364,33.39661 -117.59394,33.3953 -117.5941,33.394 -117.59464,33.39158 -117.59548,33.38783 -117.59588,33.38663 -117.59911,33.37503 -117.61404,33.33561 -117.62588,33.34211 -117.63465,33.34813 -117.63895,33.35223 -117.6444,33.35938 -117.64986,33.36401 -117.65802,33.36819 -117.66618,33.37503 -117.67129,33.37845 -117.68032,33.3845 -117.68421,33.3871 -117.68794,33.38979 -117.69067,33.39182 -117.69442,33.39459 -117.7067,33.40369 -117.71501,33.40862 -117.73534,33.40744 -117.74358,33.41215 -117.7552,33.42249 -117.76514,33.43154 -117.77077,33.43833 -117.77377,33.44218 -117.78147,33.45066 -117.78676,33.45675 -117.78937,33.4599 -117.79235,33.46535 -117.79454,33.47054 -117.79856,33.4729 -117.80012,33.47389 -117.80442,33.47662 -117.81128,33.48202 -117.81484,33.48721 -117.8166,33.491 -117.82284,33.49425 -117.82431,33.49522 -117.82632,33.49678 -117.82686,33.49708 -117.8342,33.49944 -117.83629,33.50016 -117.83732,33.5007 -117.86339,33.51437 -117.87029,33.51798 -117.87141,33.51857 -117.87178,33.51877 -117.87369,33.51977 -117.87589,33.52084 -117.87853,33.52213 -117.89076,33.52811 -117.89885,33.53206 -117.90563,33.53537 -117.90693,33.53601 -117.91086,33.53793 -117.91217,33.53857 -117.92185,33.54329 -117.93405,33.54925 -117.94957,33.55978 -117.95849,33.56583 -117.96447,33.56989 -117.97381,33.57623 -117.98261,33.58174 -117.98874,33.58557 -117.98881,33.58561 -117.98903,33.58575 -117.9891,33.58579 -117.99067,33.58677 -117.99538,33.58972 -117.99694,33.5907 -117.9972,33.59086 -117.99796,33.59133 -117.99822,33.59149 -117.99862,33.59174 -117.99982,33.5925 -118.00022,33.59275 -118.00089,33.59316 -118.00089,33.59317 -118.00089,33.59313 -118.005,33.59463 -118.01532,33.59949 -118.02497,33.60468 -118.03361,33.60935 -118.04201,33.6149 -118.04985,33.62001 -118.05612,33.62502 -118.05627,33.62514 -118.05672,33.62548 -118.05687,33.6256 -118.05773,33.62626 -118.0603,33.62823 -118.06115,33.62889 -118.06299,33.63031 -118.0685,33.63455 -118.07034,33.63596 -118.07507,33.63961 -118.08233,33.64437 -118.09647,33.65365 -118.10748,33.66878 -118.11202,33.67577 -118.12,33.68806 -118.12118,33.68988 -118.12472,33.69532 -118.1259,33.69714 -118.1259,33.69715 -118.11361,33.74681 -118.09183,33.75955 -118.0923,33.78732 -118.0767,33.81109 -118.06389,33.81931 -118.06327,33.82607 -118.06123,33.83427 -118.05647,33.84623 -118.04982,33.85361 -118.04199,33.86153 -118.03254,33.8662 -118.01824,33.87341 -118.01139,33.87821 -118.00281,33.88566 -117.9941,33.89133 -117.9853,33.89925 -117.9765,33.91917 -117.97652,33.93348 -117.97651,33.93966 -117.97648,33.94576 -117.9533,33.94591 -117.92758,33.94602 -117.90365,33.94584 -117.84442,33.94652 -117.7942,33.94654 -117.78341,33.9465 -117.78329,33.94641 -117.78189,33.94541 -117.78153,33.94516 -117.77824,33.94285 -117.77576,33.9411 -117.76939,33.93661 -117.76785,33.93549 -117.76547,33.93376 -117.75934,33.92933 -117.7505,33.9231 -117.74773,33.92119 -117.73949,33.91586 -117.73684,33.91414 -117.73546,33.91325 -117.7305,33.90996 -117.72557,33.90669 -117.72357,33.90538 -117.71896,33.90237 -117.71428,33.89931 -117.70729,33.89481 -117.69862,33.88922 -117.69319,33.88573 -117.69148,33.88462 -117.67946,33.87649 -117.67679,33.87351 -117.67394,33.87101 -117.67375,33.87083 -117.67311,33.87025 -117.67498,33.86811 -117.66241,33.8575 -117.64685,33.84101 -117.611,33.80147 -117.58582,33.77474 -117.57509,33.76658 -117.54905,33.76026 -117.53608,33.75183 -117.53568,33.74693 -117.53473,33.72114 -117.5341,33.71166 -117.51911,33.70836 -117.46957,33.69971 -117.45594,33.68899 -117.42404,33.66702 -117.41396,33.65758 -117.4212,33.64853 -117.43922,33.62577 -117.46539,33.59225 -117.4927,33.55771 -117.50167,33.54516 -117.51,33.52306 -117.50921,33.52053 -117.50338,33.51564 -117.50972,33.50534 -117.50972,33.50502 -117.50938,33.49185 -117.50857,33.47045 -117.51425,33.46779";
            //var orangeCountyLatLonRawArray = orangeCountyLatLonRaw.split(" ");

            //for (var i = 0; i < orangeCountyLatLonRawArray.length; i++) {
            //    var lat = orangeCountyLatLonRawArray[i].split(",")[1];
            //    var lon = orangeCountyLatLonRawArray[i].split(",")[0];
            //    orangeCountyCoords.push(new google.maps.LatLng(lat, lon));
            //}

            //orangeCountryPolygon = new google.maps.Polygon({
            //    paths: orangeCountyCoords,
            //    strokeColor: '#FF0000',
            //    strokeOpacity: 0.8,
            //    strokeWeight: 2,
            //    fillColor: '#FF0000',
            //    fillOpacity: 0.1
            //});

            //orangeCountryPolygon.setMap(map);


            ////test circle

            //var callBox = new google.maps.Circle({
            //    map: map,
            //    center: defaultMapLocation,
            //    fillColor: '#000000',
            //    radius: 100
            //});

            //callBoxArray.push(callBox);


        } catch (e) {
            alert('error geo map ' + e);
        }

    }

});



