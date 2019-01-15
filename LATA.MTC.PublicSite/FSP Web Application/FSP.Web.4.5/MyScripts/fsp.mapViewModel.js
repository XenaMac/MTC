/// <reference path="fsp.constructor.js" />
/// <reference path="fsp.truckCollection.js" />


lata.FspWeb.prototype.mapViewModel = function () {

    var self = this;

    //https://maps.google.com/?ll=33.779932,-117.873902&spn=0.032781,0.063386&t=m&z=15

    var DEFAULT_MAP_CENTER_LAT = 33.739660;
    var DEFAULT_MAP_CENTER_LON = -117.832146;
    var DEFAULT_MAP_ZOOM = 11;
    var defaultMapLocation;
    var currentMapLocation;

    var infowindow = null;
    var callBoxWindow = null;

    var beatSegmentPolygons = [];
    var beatSegmentsLabels = [];

    var beatPolygons = [];
    var beatLabels = [];

    var callBoxArray = [];
    var dropZoneArray = [];

    //observable UI Properties
    unFollowText = ko.observable('Stop following');
    filterText = ko.observable('Filter');
    followingTruck = ko.observable(false);
    beatsVisible = ko.observable(false);
    beatsLabelsVisible = ko.observable(false);
    beatSegmentsVisible = ko.observable(false);
    beatSegmentsLabelsVisible = ko.observable(false);
    callBoxesVisible = ko.observable(false);
    dropZonesVisible = ko.observable(false);
    debugItems = ko.observableArray([]);
    showOnPatrol = ko.observable(true);
    showDriverLoggedOn = ko.observable(true);
    showOnAssist = ko.observable(true);
    showOnRollOutIn = ko.observable(true);
    showOnBreakLunch = ko.observable(true);
    showNotLoggedIn = ko.observable(true);
    checkAllTruckStates = ko.observable(true);
    trucksAreFiltered = ko.observable(false);
    markersArray = ko.observableArray([]);
    showDebug = ko.observable(false);
    contractorNameFilter = ko.observable('');

    //testing
    truckBeatToRemove = ko.observable('');

    var initMap = function () {

        ////truck collection file
        fspWeb.startTruckService();

        try {

            //alert('geoMap');
            defaultMapLocation = new google.maps.LatLng(DEFAULT_MAP_CENTER_LAT, DEFAULT_MAP_CENTER_LON);

            //google map configurations
            var myOptions = {
                center: defaultMapLocation,
                zoom: DEFAULT_MAP_ZOOM,
                mapTypeId: google.maps.MapTypeId.ROADMAP   //ROADMAP //SATELLITE //HYBRID //TERRAIN
            };

            //init infowindow
            infowindow = new google.maps.InfoWindow({
                content: '',
                position: defaultMapLocation,
                disableAutoPan: true,
                shadowStyle: 0,
                closeBoxMargin: "2px 2px 2px 2px",
                infoBoxClearance: new google.maps.Size(1, 1),
                pixelOffset: new google.maps.Size(-13, -10)
            });

            //init infowindow
            callBoxWindow = new google.maps.InfoWindow({
                content: '',
                position: defaultMapLocation,
                disableAutoPan: true,
                shadowStyle: 0
            });


            google.maps.event.addListener(infowindow, 'domready', function () {
                $('#map').css('cursor', 'auto');
            });

            //google.maps.event.clearListeners(infowindow, 'domready');

            //initialize google map      
            var mapCanvas = document.getElementById('map');
            fspWeb.map = new google.maps.Map(mapCanvas, myOptions);
            google.maps.event.trigger(fspWeb.map, "resize");

            //local calls           
            getBeats();
            getBeatSegments();
            getCallBoxes();
            getDropZones();


            //show beat segments label only at certain zoom
            var listener = google.maps.event.addListener(fspWeb.map, 'zoom_changed', function () {
                try {
                    var visibility = "hidden";
                    if (fspWeb.map.zoom > 12 && beatSegmentsLabelsVisible() === true)
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

            //handle truck selection from outside
            fspWeb.selectedId.subscribe(function () {

                //get truck from TruckNumber
                //alert('Selected truck in grid with trucknumber ' + fspWeb.selectedTruckNumber());

                var currentTruck = ko.utils.arrayFirst(fspWeb.trucks(), function (i) { return i.id === fspWeb.selectedId(); });
                if (currentTruck) {

                    self.followingTruck(true);
                    fspWeb.selectedTruck(currentTruck);

                    var position = new google.maps.LatLng(fspWeb.selectedTruck().lat(), fspWeb.selectedTruck().lon());

                    fspWeb.map.setCenter(position);
                    fspWeb.map.setZoom(15);

                    if (infowindow) infowindow.close();
                    infowindow.setPosition(position);
                    infowindow.maxWidth = 600;
                    infowindow.setContent($("#infoWindowContent").html());
                    infowindow.open(fspWeb.map, currentTruck.mapMarker);
                }
                else {
                    fspWeb.map.setZoom(DEFAULT_MAP_ZOOM);
                    fspWeb.selectedTruck(null);
                    self.followingTruck(false);
                    if (infowindow) infowindow.close();
                }
            })

            //handle truck selection from outside
            fspWeb.selectedIdForDeletion.subscribe(function () {
                writeToDebug('Request to remove truck from map: ' + fspWeb.selectedIdForDeletion());
                removeTruck(fspWeb.selectedIdForDeletion());
            })

            var int = setInterval(function () {

                writeToDebug('Updating map');

                try {
                    //make sure truck meets filter conditions
                    if (fspWeb.trucks != null && fspWeb.trucks().length > 0) {
                        for (var i = 0; i < fspWeb.trucks().length; i++) {
                            var truck = fspWeb.trucks()[i];
                            var truckState = truck.vehicleState();
                            var contractorName = truck.contractorName();
                            determineSingleTruckVisibility(truckState, truck.id, contractorName);
                        }
                    }
                } catch (e) {

                }


                try {
                    //make sure no duplicate trucks
                    if (markersArray != null && markersArray().length > 0) {
                        for (var ii = 0; ii < markersArray().length; ii++) {
                            var truckMarker = markersArray()[ii];
                            var numberOfTrucksWithThisId = getTruckMarkerCountByTruckId(truckMarker.id);

                            if (numberOfTrucksWithThisId > 1) {
                                writeToDebug('Duplicate truck found: ' + truckMarker.id);
                                //that means a truck with this ID already exists. remove it         
                                removeTruck(truckMarker.id);
                            }
                        }
                    }

                } catch (e) {

                }

                try {
                    var numberOfTrucksInCollection = fspWeb.trucks().length;
                    var numberOfTrucksInMap = markersArray().length;

                    writeToDebug('Number Of Trucks in Collection ' + numberOfTrucksInCollection);
                    writeToDebug('Number Of Trucks in Map ' + numberOfTrucksInMap);

                    if (numberOfTrucksInCollection > numberOfTrucksInMap) {

                        //show missing trucks (when map count less than collection count)
                        for (var iii = 0; iii < numberOfTrucksInCollection; iii++) {
                            try {
                                var collectionTruck = fspWeb.trucks()[iii];

                                var truckFoundInMap = false;

                                for (var j = 0; j < numberOfTrucksInMap; j++) {
                                    var mapTruck = markersArray[j];
                                    if (mapTruck.id === collectionTruck.id) {
                                        truckFoundInMap = true;
                                    }
                                }

                                if (truckFoundInMap === false)
                                    writeToDebug('MISSING Truck In Map ' + collectionTruck.id);
                            } catch (e) {

                            }
                        }
                    }


                    if (numberOfTrucksInCollection < numberOfTrucksInMap) {
                        //show exessive trucks (when map count more than collection count)
                        for (var jj = 0; jj < numberOfTrucksInMap; jj++) {
                            try {
                                var mapTruck = markersArray[jj];

                                var truckFoundInCollection = false;

                                for (var l = 0; l < numberOfTrucksInCollection; l++) {
                                    var collectionTruck = fspWeb.trucks()[l];

                                    if (mapTruck.id === collectionTruck.id) {
                                        truckFoundInCollection = true;
                                    }
                                }

                                if (truckFoundInCollection === false)
                                    writeToDebug('EXESSIVE Truck In Map ' + mapTruck.id);
                            } catch (e) {

                            }
                        }

                    }
                } catch (e) {

                }





            }, 10000);

        } catch (e) {
            alert('error geo map ' + e);
        }

    },
    getBeats = function () {

        try {
            //url
            var url = fspWeb.SERVICE_BASE_URL + "/Truck/GetBeats";

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
                            var color = beatsData[i].Color;

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
                                strokeColor: color,
                                strokeOpacity: 0.8,
                                strokeWeight: 2,
                                fillColor: color,
                                fillOpacity: 0.8,
                                hoverColor: 'Red',
                                baseColor: color
                            });

                            var beatLabelContent = customTxt = "<div style='z-index:1'><b>" + number + "</b></div>";

                            var beatLabel = new TxtOverlay(
                                number,
                                labelPosition,
                                beatLabelContent,
                                description,
                                fspWeb.map,
                                "visible",
                                "position: absolute; border: 1px solid black; border-radius: 3px; background: " + color + "; padding: 3px; white-space: nowrap; z-index:1; opacity: 0.9 "
                            );


                            //beat label events
                            google.maps.event.addListener(beatLabel, 'click', function () {

                                //zoom to beat/label?                          
                                fspWeb.map.setCenter(this.pos);
                                fspWeb.map.setZoom(13);

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

                            beatPolygon.setMap(fspWeb.map);
                            beatPolygons.push(beatPolygon);
                            beatLabels.push(beatLabel);


                            toggleBeatsVisibility();
                            toggleBeatLabelsVisibility();

                        } catch (e) {

                        }

                    }
                }
            });
        } catch (e) {

        }

    },
    getBeatsArcGIS = function () {

        try {
            //url
            var url = "http://38.124.164.212:6080/arcgis/rest/services/BeatPolygons/MapServer/0/query?where=1%3D1&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=BeatNumber%2C+BeatDescription%2CBeatColor&returnGeometry=true&maxAllowableOffset=&geometryPrecision=&outSR=&returnIdsOnly=false&returnCountOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&returnDistinctValues=false&f=pjson";

            $.ajax({
                url: url,
                type: "GET",
                dataType: "jsonp",
                error: function (xhr, ajaxOptions, thrownError) {
                    //alert(xhr.status);
                    //alert(thrownError);
                },
                success: function (beatsData) {

                    //console.log('Beat Data:' + beatsData);
                    //console.log('Beat Features:' + beatsData.features);

                    for (var i = 0; i < beatsData.features.length; i++) {

                        var feature = beatsData.features[i];


                        //console.log('Feature:' + feature);

                        for (var ii = 0; ii < feature.geometry.rings.length; ii++) {

                            //console.log('Ring:' + feature.geometry.rings[ii]);

                            try {

                                var number = feature.attributes.BeatNumber;
                                var description = feature.attributes.BeatDescription;
                                var color = feature.attributes.BeatColor;

                                var points = feature.geometry.rings[ii];
                                var labelPosition;

                                var coords = new Array();
                                for (var iii = 0; iii < points.length; iii++) {

                                    var lon = points[iii][0];
                                    var lat = points[iii][1];
                                   

                                    if (iii == 5) { //Math.ceil(points.length / 2)) {
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
                                    strokeColor: color,
                                    strokeOpacity: 0.8,
                                    strokeWeight: 2,
                                    fillColor: color,
                                    fillOpacity: 0.8,
                                    hoverColor: 'Red',
                                    baseColor: color
                                });

                                var beatLabelContent = customTxt = "<div style='z-index:1'><b>" + number + "</b></div>";

                                var beatLabel = new TxtOverlay(
                                    number,
                                    labelPosition,
                                    beatLabelContent,
                                    description,
                                    fspWeb.map,
                                    "visible",
                                    "position: absolute; border: 1px solid black; border-radius: 3px; background: " + color + "; padding: 3px; white-space: nowrap; z-index:1; opacity: 0.9 "
                                );


                                //beat label events
                                google.maps.event.addListener(beatLabel, 'click', function () {

                                    //zoom to beat/label?                          
                                    fspWeb.map.setCenter(this.pos);
                                    fspWeb.map.setZoom(13);

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

                                beatPolygon.setMap(fspWeb.map);
                                beatPolygons.push(beatPolygon);
                                beatLabels.push(beatLabel);


                                toggleBeatsVisibility();
                                toggleBeatLabelsVisibility();

                            } catch (e) {

                            }


                        }





                    }
                }
            });
        } catch (e) {

        }

    },
    getBeatSegments = function () {

        try {

            //url
            var url = fspWeb.SERVICE_BASE_URL + "/Truck/GetBeatSegments";

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

                            var beatSegmentPolygon = new google.maps.Polygon({
                                id: description,
                                paths: coords,
                                strokeColor: 'Black',
                                strokeOpacity: 0.8,
                                strokeWeight: 2,
                                fillColor: 'Transparent',
                                //fillOpacity: 0.1
                            });

                            var beatSegmentLabelContent = customTxt = "<div>" + number + "</div>";
                            var beatSegmentLabel = new TxtOverlay(
                                number,
                                labelPosition,
                                beatSegmentLabelContent,
                                description,
                                fspWeb.map,
                                "hidden",
                                "position: absolute; border: 1px solid black; border-radius: 3px; background: #ffffff; padding: 1px; white-space: nowrap; z-index:-1; opacity: 0.9 "
                            );

                            google.maps.event.addListener(beatSegmentPolygon, 'mouseover', function () {
                                tooltip.show(this.id);
                            });
                            google.maps.event.addListener(beatSegmentPolygon, 'mouseout', function () {
                                tooltip.hide();
                            });

                            beatSegmentPolygon.setMap(fspWeb.map);
                            beatSegmentPolygons.push(beatSegmentPolygon);
                            beatSegmentsLabels.push(beatSegmentLabel);


                            toggleBeatSegmentsVisibility();

                        } catch (e) {

                        }

                    }
                }
            });

        } catch (e) {

        }

    },
    getCallBoxes = function () {
        try {

            //url
            var url = fspWeb.SERVICE_BASE_URL + "/Truck/GetCallBoxes";

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
                            var signNumber = callBoxData[i].SignNumber;
                            var siteType = callBoxData[i].SiteType;
                            var comments = callBoxData[i].Comments;

                            var position = new google.maps.LatLng(lat, lon);

                            var image = fspWeb.SERVICE_BASE_URL + '/Images/callBox.png';

                            var callBox = new google.maps.Marker({
                                position: position,
                                map: fspWeb.map,
                                icon: image,
                                phoneNumber: phoneNumber,
                                freewayId: freewayId,
                                locationDescription: locationDescription,
                                signNumber: signNumber,
                                siteType: siteType,
                                comments: comments
                            });

                            callBoxArray.push(callBox);

                            toggleCallBoxesVisibility();

                            //callBox polygon events
                            google.maps.event.addListener(callBox, 'rightclick', function () {

                                if (callBoxWindow) callBoxWindow.close();

                                callBoxWindow.setPosition(this.position);
                                callBoxWindow.setContent('<table>' +
                                    '<tr><td class="hoverDetailsFields">Sign Number:</td><td class="hoverDetails">' + this.signNumber + '</td></tr>' +
                                    '<tr><td class="hoverDetailsFields">Phone Number:</td><td class="hoverDetails">' + this.phoneNumber + '</td></tr>' +
                                    '<tr><td class="hoverDetailsFields">Comments:</td><td class="hoverDetails">' + this.comments + '</td></tr>' +
                                    '<tr><td class="hoverDetailsFields">Location:</td><td class="hoverDetails">' + this.locationDescription + '</td></tr>' +
                                    '<tr><td class="hoverDetailsFields">Site Type:</td><td class="hoverDetails">' + this.siteType + '</td></tr>');
                                callBoxWindow.open(fspWeb.map, this);

                            });


                        } catch (e) {

                        }

                    }
                }
            });
        } catch (e) {

        }
    },
    getDropZones = function () {
        try {

            //url
            var url = fspWeb.SERVICE_BASE_URL + "/Truck/GetDropZones";

            $.ajax({
                url: url,
                type: "GET",
                dataType: "json",
                error: function (xhr, ajaxOptions, thrownError) {
                    //alert(xhr.status);
                    //alert(thrownError);
                },
                success: function (dropZoneData) {

                    for (var i = 0; i < dropZoneData.length; i++) {
                        try {

                            var dropZoneID = dropZoneData[i].DropZoneID;
                            var dropZoneNumber = dropZoneData[i].DropZoneNumber;
                            var comments = dropZoneData[i].Comments;
                            var dropZoneDescription = dropZoneData[i].DropZoneDescription;
                            var points = dropZoneData[i].PolygonPoints;

                            var coords = new Array();
                            for (var ii = 0; ii < points.length; ii++) {

                                var lat = points[ii].Lat;
                                var lon = points[ii].Lon;
                                coords.push(new google.maps.LatLng(lat, lon));
                            }

                            //beat polygon
                            var dropZonePolygon = new google.maps.Polygon({
                                id: dropZoneNumber,
                                desc: dropZoneDescription,
                                paths: coords,
                                strokeColor: '999999',
                                strokeOpacity: 0.8,
                                strokeWeight: 2,
                                fillColor: '999999',
                                fillOpacity: 0.8,

                            });

                            google.maps.event.addListener(dropZonePolygon, 'mouseover', function () {
                                tooltip.show(this.desc);
                            });
                            google.maps.event.addListener(dropZonePolygon, 'mouseout', function () {
                                tooltip.hide();
                            });

                            dropZonePolygon.setMap(fspWeb.map);
                            dropZoneArray.push(dropZonePolygon);

                            toggleDropZonesVisibility();

                        } catch (e) {

                        }

                    }
                }
            });
        } catch (e) {

        }
    },
    zoomToBeat = function (selectedBeatNumber) {
        try {

            //toggle actual segments
            if (beatLabels != null && beatLabels.length > 0) {
                for (var i = 0; i < beatLabels.length; i++) {
                    var beatNumber = beatLabels[i].id;

                    if (beatNumber === selectedBeatNumber) {
                        //zoom to beat/label?                          
                        fspWeb.map.setCenter(beatLabels[i].pos);
                        fspWeb.map.setZoom(13);

                        //close model
                        $("#filterModal").modal('hide');
                    }
                }
            }
        } catch (e) {

        }

    },
    rightClickTruck = function (item) {
        try {

            fspWeb.selectedTruck(null);
            infowindow.setContent($("#infoWindowContent").html());
            $("#" + item.truckNumber).css('cursor', 'wait');

            var position = new google.maps.LatLng(item.lat(), item.lon());

            if (infowindow) infowindow.close();
            infowindow.setPosition(position);
            infowindow.maxWidth = 600;
            infowindow.open(fspWeb.map, item.mapMarker);

            var int = setInterval(function () {
                $("#" + item.truckNumber).css('cursor', 'auto');
                fspWeb.selectedTruck(item);
                infowindow.setContent($("#infoWindowContent").html());
                clearInterval(int);
            }, 500);

        } catch (e) {

        }
    },
    updateInfoWindowContent = function (item) {
        try {
            infowindow.setContent($("#infoWindowContent").html());
        } catch (e) {

        }
    },
    getTruckMarkerByTruckId = function (id) {

        var returnItem;

        try {
            return ko.utils.arrayFirst(markersArray(), function (i) { return i.id === id; });
        } catch (e) {
            writeToDebug('Error getting truck marker ' + e);
        }
        return returnItem;
    },
    getTruckMarkerCountByTruckId = function (id) {

        var counter = 0;

        try {
            if (markersArray != null && markersArray.length > 0) {
                for (var i = 0; i < markersArray.length; i++) {
                    var truckMarker = markersArray[i];
                    if (truckMarker.id === id) {
                        counter += 1;
                    }
                }
            }
        } catch (e) {

        }

        return counter;
    },
    showTruck = function (id) {
        try {
            var truckMarker = getTruckMarkerByTruckId(id);
            if (truckMarker != undefined) {
                if (truckMarker.map == null)
                    truckMarker.setMap(fspWeb.map);
            }
        } catch (e) {
            writeToDebug('Error unhidding truck ' + e);
        }


    },
    hideTruck = function (id) {
        try {
            var truckMarker = getTruckMarkerByTruckId(id);
            if (truckMarker != undefined) {
                truckMarker.setMap(null);
                writeToDebug('Hiding truck ' + id);
            }
            else
                writeToDebug('Truck Marker not found to hide ' + id);
        } catch (e) {
            writeToDebug('Error hidding truck ' + e);
        }


    },
    removeTruck = function (id) {

        writeToDebug('Attempt to remove truck: ' + id);

        try {
            //find marker to set it's map reference to null
            var currentMarker = ko.utils.arrayFirst(markersArray(), function (i) { return i.id === id; });
            if (currentMarker) {
                currentMarker.setMap(null);
                writeToDebug('Removed truck in map: ' + currentMarker.id);
            }

            markersArray.remove(function (i) { return i.id === id; });
            writeToDebug('Removed truck in array: ');

        } catch (e) {
            writeToDebug('Error removing truck: ' + e);
        }



    },
    zoomToTruckAndSelect = function () {
        try {

            self.followingTruck(true);

            var position = new google.maps.LatLng(fspWeb.selectedTruck().lat(), fspWeb.selectedTruck().lon());
            fspWeb.map.setCenter(position);
            fspWeb.map.setZoom(15);

            if (infowindow) infowindow.close();
            infowindow.setPosition(position);
            infowindow.maxWidth = 600;
            infowindow.setContent($("#infoWindowContent").html());
            infowindow.open(fspWeb.map, fspWeb.selectedTruck().mapMarker);


            //send follow signal to other windows
            var userId = fspWeb.userId;

            fspWeb.towTruckHub.server.SetSelectedTruck(fspWeb.selectedTruck().truckNumber, userId);

            //var url = fspWeb.SERVICE_BASE_URL + "/Truck/SetSelectedTruck";
            //$.ajax({
            //    url: url,
            //    type: "GET",
            //    dataType: "json",
            //    data: {
            //        truckNumber: fspWeb.selectedTruck().truckNumber,
            //        userId: userId
            //    },
            //    error: function (xhr, ajaxOptions, thrownError) {
            //        //alert(xhr.status);
            //        //alert(thrownError);
            //    },
            //    success: function () {

            //    }
            //});



        } catch (e) {

        }
    },
    toggleBeatsVisibility = function () {

        //toggle actual segments
        if (beatPolygons != null && beatPolygons.length > 0) {
            for (var i = 0; i < beatPolygons.length; i++) {
                if (beatsVisible() === true) {
                    //show all beats and segments
                    beatPolygons[i].setMap(fspWeb.map);
                }
                else {
                    //hide all beats and segments
                    beatPolygons[i].setMap(null);
                }
            }
        }

    },
    toggleBeatLabelsVisibility = function () {

        //toggle actual segments
        if (beatLabels != null && beatLabels.length > 0) {
            for (var i = 0; i < beatLabels.length; i++) {
                if (beatsLabelsVisible() === true) {
                    //show all beats and segments                     
                    beatLabels[i].setMap(fspWeb.map);
                }
                else {
                    //hide all beats and segments                     
                    beatLabels[i].setMap(null);
                }
            }
        }

    },
    toggleBeatSegmentsVisibility = function () {

        //toggle actual segments
        if (beatSegmentPolygons != null && beatSegmentPolygons.length > 0) {
            for (var i = 0; i < beatSegmentPolygons.length; i++) {
                if (beatSegmentsVisible() === true) {
                    //show all beats and segments
                    beatSegmentPolygons[i].setMap(fspWeb.map);
                }
                else {
                    //hide all beats and segments
                    beatSegmentPolygons[i].setMap(null);
                }
            }
        }

    },
    toggleBeatSegmentLabelsVisibility = function () {

        //toggle actual segments
        if (beatSegmentsLabels != null && beatSegmentsLabels.length > 0) {
            for (var i = 0; i < beatSegmentsLabels.length; i++) {
                if (beatSegmentsLabelsVisible() === true) {
                    //show all beats and segments                   
                    beatSegmentsLabels[i].setMap(fspWeb.map);
                }
                else {
                    beatSegmentsLabels[i].setMap(null);
                }
            }
        }

    },
    toggleCallBoxesVisibility = function () {

        try {
            if (callBoxArray != null && callBoxArray.length > 0) {
                for (var i = 0; i < callBoxArray.length; i++) {
                    if (callBoxesVisible() === true) {
                        callBoxArray[i].setMap(fspWeb.map);
                    }
                    else {
                        callBoxArray[i].setMap(null);
                    }
                }
            }
        } catch (e) {

        }

    },
    toggleDropZonesVisibility = function () {

        try {
            if (dropZoneArray != null && dropZoneArray.length > 0) {
                for (var i = 0; i < dropZoneArray.length; i++) {
                    if (dropZonesVisible() === true) {
                        dropZoneArray[i].setMap(fspWeb.map);
                    }
                    else {
                        dropZoneArray[i].setMap(null);
                    }
                }
            }
        } catch (e) {

        }

    },
    toggleTruckVisibility = function () {

        try {
            if (fspWeb.trucks != null && fspWeb.trucks().length > 0) {
                for (var i = 0; i < fspWeb.trucks().length; i++) {
                    var truck = fspWeb.trucks()[i];
                    var truckState = truck.vehicleState();
                    var contractorName = truck.contractorName();
                    determineSingleTruckVisibility(truckState, truck.id, contractorName);
                }
            }

            //determine is trucks are filtered
            if (showNotLoggedIn() && showOnPatrol() && showDriverLoggedOn() && showOnAssist() && showOnBreakLunch() && showOnRollOutIn() && self.contractorNameFilter().length === 0)
                trucksAreFiltered(false);
            else
                trucksAreFiltered(true);

        } catch (e) {
        }
    },
    determineSingleTruckVisibility = function (truckState, id, contractorName) {

        var passedContractorFilter = true;

        if (self.contractorNameFilter().length > 0) {
            if (contractorName != self.contractorNameFilter())
                passedContractorFilter = false;
        }

        if (truckState === 'Waiting for Driver Login') {
            if (showNotLoggedIn() && passedContractorFilter)
                showTruck(id);
            else
                hideTruck(id);
        }
        else if (truckState === 'Driver Logged On') {
            if (showDriverLoggedOn() && passedContractorFilter)
                showTruck(id);
            else
                hideTruck(id);
        }
        else if (truckState === 'On Patrol') {
            if (showOnPatrol() && passedContractorFilter)
                showTruck(id);
            else
                hideTruck(id);
        }
        else if (truckState === 'On Incident') {
            if (showOnAssist() && passedContractorFilter)
                showTruck(id);
            else
                hideTruck(id);
        }
        else if (truckState === 'On Break' || truckState === 'On Lunch') {
            if (showOnBreakLunch() && passedContractorFilter)
                showTruck(id);
            else
                hideTruck(id);
        }
        else if (truckState === 'Roll In' || truckState === 'Roll Out') {
            if (showOnRollOutIn() && passedContractorFilter)
                showTruck(id);
            else
                hideTruck(id);
        }



    },
    writeToDebug = function (message) {
        try {
            //debugItems.push(getCurrentTimeStamp() + ': ' + message);
        } catch (e) {

        }
    },
    isFollowing = function () {
        return self.followingTruck();
    }

    stopFollowing = function () {

        try {
            var userId = fspWeb.userId;

            //center map back to original
            fspWeb.map.setCenter(defaultMapLocation);
            fspWeb.towTruckHub.server.stopFollowingTruck(userId);
            fspWeb.map.setZoom(DEFAULT_MAP_ZOOM);
            fspWeb.selectedTruck(null);
            self.followingTruck(false);
            if (infowindow) infowindow.close();

            //var url = fspWeb.SERVICE_BASE_URL + "/Truck/StopFollowingTruck";
            //$.ajax({
            //    url: url,
            //    type: "GET",
            //    dataType: "json",
            //    data: {
            //        userId: userId
            //    },
            //    error: function (xhr, ajaxOptions, thrownError) {
            //        //alert(xhr.status);
            //        //alert(thrownError);
            //    },
            //    success: function () {

            //        fspWeb.map.setZoom(DEFAULT_MAP_ZOOM);
            //        fspWeb.selectedTruck(null);
            //        self.followingTruck(false);
            //        if (infowindow) infowindow.close();

            //    }
            //});           


        } catch (e) {

        }
    }

    showFilter = function () {
        $("#filterModal").modal('show');
        $("#beatsFilter").focus();
    }

    //showTruckList = function () {
    //    try {
    //        var url = fspWeb.SERVICE_BASE_URL + "/Truck/Grid";
    //        var windowName = "Tow Truck Grid";
    //        var windowSize = 'width=1000,height=800';
    //        window.open(url, '', windowSize);
    //        event.preventDefault();
    //    } catch (e) {

    //    }
    //}

    beatsVisible.subscribe(function () {
        toggleBeatsVisibility();
    })
    beatsLabelsVisible.subscribe(function () {
        toggleBeatLabelsVisibility();
    })
    beatSegmentsVisible.subscribe(function () {
        toggleBeatSegmentsVisibility();
    })
    beatSegmentsLabelsVisible.subscribe(function () {
        toggleBeatSegmentLabelsVisibility();
    })
    callBoxesVisible.subscribe(function () {
        toggleCallBoxesVisibility();

    })
    dropZonesVisible.subscribe(function () {
        toggleDropZonesVisibility();

    })

    showOnPatrol.subscribe(function () {
        toggleTruckVisibility();
    })

    showDriverLoggedOn.subscribe(function () {
        toggleTruckVisibility();
    })

    showOnAssist.subscribe(function () {
        toggleTruckVisibility();
    })

    showOnRollOutIn.subscribe(function () {
        toggleTruckVisibility();
    })

    showOnBreakLunch.subscribe(function () {
        toggleTruckVisibility();
    })

    showNotLoggedIn.subscribe(function () {
        toggleTruckVisibility();
    })

    checkAllTruckStates.subscribe(function () {

        showOnPatrol(checkAllTruckStates());
        showOnAssist(checkAllTruckStates());
        showOnRollOutIn(checkAllTruckStates());
        showOnBreakLunch(checkAllTruckStates());
        showNotLoggedIn(checkAllTruckStates());
        showDriverLoggedOn(checkAllTruckStates());
    })

    filterTrucksByContractor = function (contractorName) {
        self.contractorNameFilter(contractorName);
        toggleTruckVisibility();
    }

    clearContractorNameFilter = function () {
        self.contractorNameFilter('');
        toggleTruckVisibility();
    }

    toggleDebug = function () {

        if (showDebug() === true)
            showDebug(false);
        else
            showDebug(true);

    }

    forceBeatRemoval = function () {
        removeTruck(truckBeatToRemove());
    }

    function getCurrentTimeStamp() {
        var currentTime = new Date()
        var seconds = currentTime.getSeconds();
        var minutes = currentTime.getMinutes();
        var hours = currentTime.getHours();

        return hours + ":" + minutes + ":" + seconds;
    }

    //public implementation (object literals)
    return {
        initMap: initMap,
        zoomToBeat: zoomToBeat,
        rightClickTruck: rightClickTruck,
        zoomToTruckAndSelect: zoomToTruckAndSelect,
        updateInfoWindowContent: updateInfoWindowContent,
        isFollowing: isFollowing,
        toggleTruckVisibility: toggleTruckVisibility,
        determineSingleTruckVisibility: determineSingleTruckVisibility,
        getTruckMarkerByTruckId: getTruckMarkerByTruckId,
        writeToDebug: writeToDebug,
        filterTrucksByContractor: filterTrucksByContractor
    };
}();

//wait for browser to load (jquery command)
$(function () {

    fspWeb = new lata.FspWeb();

    //apply binding to ko
    ko.applyBindings(fspWeb);

    //init
    fspWeb.mapViewModel.initMap();

    //map sizing
    $('#map').css({ 'height': window.innerHeight - 150 + 'px' });
    window.onresize = function (event) {
        try {
            $('#map').css({ 'height': window.innerHeight - 150 + 'px' });
        } catch (e) {

        }
    }

    //beat Number autocomplete
    $("#contractorsFilter").autocomplete({
        source: function (request, response) {
            var url = fspWeb.SERVICE_BASE_URL + "/Truck/GetTowTruckContractors";
            $.ajax({
                url: url,
                dataType: "json",
                data: {
                    featureClass: "P",
                    style: "full",
                    maxRows: 5,
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
            //fspWeb.mapViewModel.zoomToBeat(ui.item.value);
            fspWeb.mapViewModel.filterTrucksByContractor(ui.item.value);
        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });

    $("#followButton").click(function () {
        alert('');
    });
         
});

function follow() {
    fspWeb.mapViewModel.zoomToTruckAndSelect();
}

//binding trucks to map
ko.bindingHandlers.map = {
    init: function (element, valueAccessor, allBindingsAccessor, truck) {

        try {

            var lat = allBindingsAccessor().lat();
            var lon = allBindingsAccessor().lon();
            var id = allBindingsAccessor().id;
            var truckNumber = allBindingsAccessor().truckNumber;
            var truckState = allBindingsAccessor().vehicleState();
            var beatNumber = allBindingsAccessor().beatNumber();
            var vehicleStateIconUrl = allBindingsAccessor().vehicleStateIconUrl();
            var heading = allBindingsAccessor().heading();


            //check if there is already a truck with same ID on map;
            var currentTruckMarker = fspWeb.mapViewModel.getTruckMarkerByTruckId(id);

            if (currentTruckMarker == null) {
                var position = new google.maps.LatLng(lat, lon);

                //var displayNumber = beatNumber.substring(beatNumber.indexOf("-") + 1);
                var displayNumber = beatNumber;

                if (displayNumber === 'Not set' || displayNumber === 'Not Assigned')
                    displayNumber = truckNumber;


                var html = "<div style='cursor: pointer'><img id='" + id + "' src='" + vehicleStateIconUrl + "' /><span style='color: blue; font-size:16px; font-weight:bold; white-space: nowrap;'>" + displayNumber + "</span></div>";

                marker = new RichMarker({
                    id: id,
                    map: fspWeb.map,
                    position: position,
                    draggable: false,
                    flat: true,
                    anchor: RichMarkerPosition.MIDDLE,
                    content: html,
                    truckState: truckState
                });

                //show info window
                google.maps.event.addListener(marker, 'click', function () {
                    fspWeb.mapViewModel.rightClickTruck(truck);
                });

                //add truck marker to main list 
                self.markersArray.push(marker);
                fspWeb.mapViewModel.writeToDebug('Adding Truck In Map ' + id);


                //associated truck marker with truck object in truck list
                truck.mapMarker = marker;

                //fspWeb.mapViewModel.writeToDebug('Adding truck ' + truckNumber);
                fspWeb.mapViewModel.toggleTruckVisibility();

                //rotate truck marker icon
                $("#" + id).rotate({
                    angle: heading - 90
                });
            }
            else {
                fspWeb.mapViewModel.writeToDebug('Attempting to add duplicate truck: ' + id);
            }

        } catch (e) {

        }

    },
    update: function (element, valueAccessor, allBindingsAccessor, truck) {

        try {

            var lat = truck.lat();
            var lon = truck.lon();
            var id = truck.id;
            var truckNumber = truck.truckNumber;
            var contractorName = truck.contractorName();
            var truckState = truck.vehicleState();
            var beatNumber = truck.beatNumber();
            var vehicleStateIconUrl = truck.vehicleStateIconUrl();
            var heading = truck.heading();
            var speed = truck.speed();


            //update position
            var latlng = new google.maps.LatLng(lat, lon);
            truck.mapMarker.setPosition(latlng);

            //update icon/text
            var displayNumber = beatNumber;
            if (displayNumber === 'Not set' || displayNumber === 'Not Assigned')
                displayNumber = truckNumber;
            var html = "<div style='cursor: pointer'><img id='" + id + "' src='" + vehicleStateIconUrl + "' /><span style='color: blue; font-size:16px; font-weight:bold; white-space: nowrap;'>" + displayNumber + "</span></div>";
            truck.mapMarker.setContent(html);

            //rotate
            $("#" + id).rotate({
                angle: heading - 90
            });

            //only toggle visibily if the truck states has changed
            if (truck.mapMarker.truckState != truckState)
                fspWeb.mapViewModel.determineSingleTruckVisibility(truckState, id, contractorName);

            try {
                if (fspWeb.selectedTruck() != undefined) {

                    if (id === fspWeb.selectedTruck().id) {

                        //set the truck info    
                        fspWeb.selectedTruck(truck);


                        if (fspWeb.mapViewModel.isFollowing() === true) {
                            //only center if "following"                            
                            var position = new google.maps.LatLng(lat, lon);
                            fspWeb.map.setCenter(position);
                        }

                        fspWeb.mapViewModel.updateInfoWindowContent(truck);

                    }
                }
            } catch (e) {

            }

        } catch (e) {

        }


    }
};
