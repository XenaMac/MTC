mtcApp.directive('mapBeat', function(mapService, utilService, generalService) {
    return {
        restrict: 'E',
        templateUrl: $(".websiteUrl").text().trim() + '/app/templates/mapBeatTemplate.html',
        scope: {
            resetMap: "&",
            setMapLocation: "&",
            displayMapData: "&",
            hideMapData: "&",
            hideMapMarkers: "&",
            setEditPolygons: "&",
            setCancelEditPolygons: "&",
            setNewPolygon: "&",
            makeAllPolygonsUneditable: "&",

            selectedPolygon: "=",
            visible: "="
        },
        link: function(scope) {

            var selectedBeatId = 0;

            scope.isEditing = false;
            scope.isAdding = false;
            scope.isBusyGetting = false;
            scope.isBusySaving = false;
            scope.isBusyDeleting = false;
            scope.isBusyGettingBeatSegments = false;
            scope.isBusyGettingSegments = false;

            scope.beats = [];
            scope.selectedBeat = "";

            scope.segments = [];
            scope.beatSegments = [];
            scope.polygons = [];
            scope.markers = [];

            scope.colorPickerSettings = {
                control: 'hue', // control: 'brightness',
                theme: 'bootstrap', //  theme: 'default'
                position: 'top left', // position: 'bottom left',
                animationSpeed: 50,
                animationEasing: 'swing',
                change: null,
                changeDelay: 0,
                dataUris: true,
                defaultValue: '#ffffff',
                hide: null,
                hideSpeed: 100,
                inline: false,
                letterCase: 'uppercase', //lowercase
                opacity: false,
                show: null,
                showSpeed: 100
            };

            function buildDetailsContent(beatSegmentData) {
                var content = "<table>";
                content += "<tr>";
                content += "<td>ID:</td>";
                content += "<td><strong>" + beatSegmentData.segmentID + "</strong></td>";
                content += "</tr>";
                content += "<tr>";
                content += "<td>Description:</td>";
                content += "<td><strong>" + beatSegmentData.segmentDescription + "</strong></td>";
                content += "</tr>";
                content += "</table>";
                return content;
            }

            function buildPolygons(beatSegmentPolygonData) {
                var cleanLatLng = [];
                if (beatSegmentPolygonData.geoFence !== null && beatSegmentPolygonData.geoFence !== undefined) {
                    beatSegmentPolygonData.geoFence.forEach(function(geoFence) {
                        cleanLatLng.push({
                            lat: geoFence.lat,
                            lng: geoFence.lon
                        });
                    });
                }

                if (cleanLatLng.length > 0) {
                    var beatSegmentPolygon = new google.maps.Polygon({
                        id: "beatSegmentPolygon" + beatSegmentPolygonData.ID,
                        paths: cleanLatLng,
                        strokeColor: beatSegmentPolygonData.Color,
                        strokeOpacity: 0.8,
                        strokeWeight: 2,
                        fillColor: beatSegmentPolygonData.Color,
                        fillOpacity: 0.35,
                        editable: false
                    });
                    scope.polygons.push(beatSegmentPolygon);
                }
            }

            function buildMarkers(segmentPolygonData, lat, lon) {

                if (segmentPolygonData.geoFence !== null && segmentPolygonData.geoFence !== undefined) {

                    var beatSegmentMarker = new MarkerWithLabel({
                        id: "beatSegmentMarker" + segmentPolygonData.ID,
                        animation: google.maps.Animation.DROP,
                        position: new google.maps.LatLng(lat, lon),
                        draggable: false,
                        labelContent: segmentPolygonData.segmentID,
                        labelAnchor: new google.maps.Point(25, 40),
                        labelClass: "googleMapMarkerLabel", // the CSS class for the label
                        labelStyle: { opacity: 0.75 }
                    });

                    var infowindow = new google.maps.InfoWindow({
                        title: "Beat Segment Details",
                        content: buildDetailsContent(segmentPolygonData)
                    });
                    beatSegmentMarker.addListener('click', function() {
                        infowindow.open(scope.map, beatSegmentMarker);
                    });
                    scope.markers.push(beatSegmentMarker);
                }

            }

            scope.triggerDisplayMapData = function() {
                scope.displayMapData({
                    polygons: scope.polygons,
                    markers: scope.markers
                });
            };

            scope.triggerHideMapData = function() {
                scope.hideMapData();
            };

            scope.triggerHideMapMarkers = function() {
              scope.hideMapMarkers();
          };

            scope.triggerResetMap = function() {
                scope.resetMap();
            };

            scope.triggerSetMapLocation = function(lat, lon, zoom) {
                scope.setMapLocation({
                    lat: lat,
                    lon: lon,
                    zoom: zoom
                });
            };

            scope.triggerSetEditPolygons = function() {
                scope.setEditPolygons();
            };

            scope.triggerSetCancelEditPolygons = function() {
                scope.setCancelEditPolygons();
            };

            scope.triggerSetNewPolygon = function(color) {
                scope.setNewPolygon({
                    color: color
                });
            };

            scope.$watch("visible", function(isVisible) {
                if (isVisible !== undefined) {
                    if (isVisible) {
                        if (scope.beats.length === 0) {
                            scope.getBeats();
                        } else {
                            scope.triggerDisplayMapData();
                        }
                    } else {
                        scope.selectedBeat = "";
                    }
                }
            });

            scope.setSelectedBeat = function() {
                console.log(scope.selectedBeat);
                scope.beatSegments = [];
                if (scope.selectedBeat) {
                    if (scope.selectedBeat === null) {
                        scope.triggerResetMap();
                        return;
                    }
                    selectedBeatId = scope.selectedBeat.ID;
                    scope.getBeatSegments();
                } else {
                    scope.triggerHideMapData();
                    scope.triggerResetMap();
                }

            };

            scope.getBeatSegments = function() {
                scope.isBusyGettingBeatSegments = true;
                console.log("Getting beat segments...");
                mapService.getBeatSegmentPolygons(scope.selectedBeat.BeatID).then(function(result) {
                    scope.isBusyGettingBeatSegments = false;
                    scope.triggerHideMapData();

                    if (!result || result.length === 0) {
                        console.log("Beat has no segments");
                        scope.beatSegments = [];
                        return;
                    }

                    console.log("Beat Segment Polygons %O", result);

                    scope.beatSegments = result;
                    scope.polygons = [];
                    scope.markers = [];

                    var totalLat = 0;
                    var totalLon = 0;

                    scope.beatSegments.forEach(function(segmentPolygonData) {
                        buildPolygons(segmentPolygonData);
                        var latDelta = (segmentPolygonData.maxLat - segmentPolygonData.minLat) / 2;
                        var middleLat = segmentPolygonData.minLat + latDelta;
                        var lonDelta = (segmentPolygonData.maxLon - segmentPolygonData.minLon) / 2;
                        var middleLon = segmentPolygonData.minLon + lonDelta;
                        //buildMarkers(segmentPolygonData, middleLat, middleLon);
                        totalLat += middleLat;
                        totalLon += middleLon;
                    });

                    scope.triggerDisplayMapData();

                    var mapLat = totalLat / result.length;
                    var mapLon = totalLon / result.length;
                    scope.triggerSetMapLocation(mapLat, mapLon, 13);

                });
            };

            scope.getSegments = function() {
                if (scope.segments.length === 0) {
                    scope.isBusyGettingSegments = true;
                    mapService.getSegmentPolygons().then(function(result) {
                        scope.isBusyGettingSegments = false;
                        scope.segments = result;
                    });
                }
            };

            scope.addSegment = function() {
                if (scope.selectedSegment) {
                    var isExisting = utilService.findArrayElement(scope.beatSegments, "ID", scope.selectedSegment.ID);
                    if (!isExisting) {
                        scope.beatSegments.push(scope.selectedSegment);
                    } else {
                        console.log("Beat already contains this segment");
                    }
                }
            };

            scope.getBeats = function() {
                scope.isBusyGettingBeats = true;
                mapService.getBeats().then(function(result) {
                    scope.isBusyGettingBeats = false;
                    if (!result) {
                        console.error("GetBeats");
                        toastr.error('Failed to retrieve Beat', 'Error');
                    } else {
                        console.log('beats %O', scope.beats);
                        scope.beats = result;
                    }
                });
            };

            scope.setEdit = function() {
                scope.isEditing = true;
                scope.getSegments();

                scope.beatSegments.forEach(function(segmentPolygonData) {
                    var latDelta = (segmentPolygonData.maxLat - segmentPolygonData.minLat) / 2;
                    var middleLat = segmentPolygonData.minLat + latDelta;
                    var lonDelta = (segmentPolygonData.maxLon - segmentPolygonData.minLon) / 2;
                    var middleLon = segmentPolygonData.minLon + lonDelta;
                    buildMarkers(segmentPolygonData, middleLat, middleLon);
                });
                scope.triggerDisplayMapData();
            };

            scope.cancelEdit = function() {
                if (scope.beatSegments.length !== scope.selectedBeat.beatSegmentList.length)
                    scope.getBeatSegments();
                scope.isEditing = false;
                scope.triggerHideMapMarkers();
            };

            scope.triggerMakeAllPolygonsUneditable = function() {
                scope.makeAllPolygonsUneditable();
            };

            scope.save = function() {
                var newBeatSegmentList = [];
                scope.beatSegments.forEach(function(beatSegment) {
                    newBeatSegmentList.push(beatSegment.segmentID);
                });
                scope.selectedBeat.beatSegmentList = newBeatSegmentList;
                scope.isBusySaving = true;

                console.log("Saving beat...%O", scope.selectedBeat);
                scope.triggerHideMapData();

                mapService.saveBeat(scope.selectedBeat).then(function(result) {
                    scope.isBusySaving = false;
                    scope.isEditing = false;
                    if (!result) {
                        console.error("Save Beat");
                        toastr.error('Failed to save Beat', 'Error');
                    } else {
                        console.log("Save Beat Success");
                        toastr.success('Beat Saved', 'Success');
                        scope.getBeatSegments();
                    }
                });
            };

            scope.reduceBeatPolygonPoints = function() {
                var selectedBeatPolygon = utilService.findArrayElement(scope.polygons, "id", "beatPolygon" + scope.selectedBeat.BeatID);
                if (!selectedBeatPolygon) return;
                utilService.clearPolygonCoords(selectedBeatPolygon);
            };

            scope.delete = function() {
                if (confirm("Ok to delete this Beat?")) {
                    scope.isBusyDeleting = true;
                    mapService.deleteBeat(scope.selectedBeat.ID).then(function(result) {
                        scope.isBusyDeleting = false;
                        scope.isEditing = false;
                        if (!result) {
                            console.error("Delete Beat");
                            toastr.error('Failed to delete Beat', 'Error');
                        } else {
                            console.log("Delete Beat Success");
                            toastr.success('Beat Deleted', 'Success');
                            selectedBeatId = 0;
                            scope.selectedBeat = "";
                            scope.triggerHideMapData();
                            scope.triggerResetMap();
                            scope.getBeats();
                        }
                    });
                }
            };

            scope.prepareNew = function() {
                scope.selectedBeat = {
                    BeatID: "",
                    BeatDescription: "",
                    Color: "#000000",
                    geoFence: []
                };
                scope.isAdding = true;
                scope.triggerSetNewPolygon(scope.selectedBeat.color);
            };

            scope.cancelAdd = function() {
                scope.isAdding = false;
                scope.selectedBeat = "";
                scope.triggerHideMapData();
                scope.triggerDisplayMapData();
            };

            scope.add = function() {
                console.log("Adding beat...");
                console.log(scope.selectedBeat);
                scope.isBusyAdding = true;
                scope.triggerHideMapData();
                mapService.addBeat(scope.selectedBeat).then(function(result) {
                    scope.isBusyAdding = false;
                    scope.isAdding = false;
                    if (!result) {
                        console.error("Add Beat");
                        toastr.error('Failed to add Beat', 'Error');
                    } else {
                        console.log("Add Beat Success");
                        scope.selectedBeat = "";
                        toastr.success('Beat Added', 'Success');
                        scope.getBeats();
                    }

                });
            };

        }
    };
});