mtcApp.directive('mapSegment', function(mapService, utilService, generalService) {
    return {
        restrict: 'E',
        templateUrl: $(".websiteUrl").text().trim() + '/app/templates/mapSegmentTemplate.html',
        scope: {
            resetMap: "&",
            setMapLocation: "&",
            displayMapData: "&",
            hideMapData: "&",
            setEditPolygon: "&",
            setCancelEditPolygon: "&",
            setNewPolygon: "&",
            makeAllPolygonsUneditable: "&",

            selectedPolygon: "=",
            visible: "="
        },
        link: function(scope) {

            var selectedSegmentId = 0;

            scope.isEditing = false;
            scope.isAdding = false;
            scope.isBusyGetting = false;
            scope.isBusySaving = false;
            scope.isBusyDeleting = false;

            scope.segments = [];
            scope.polygons = [];
            scope.markers = [];
            scope.selectedSegment = "";

            function buildDetailsContent(segment) {
                var content = "<table>";
                content += "<tr>";
                content += "<td>ID:</td>";
                content += "<td><strong>" + segment.segmentID + "</strong></td>";
                content += "</tr>";
                content += "<tr>";
                content += "<td>Description:</td>";
                content += "<td><strong>" + segment.segmentDescription + "</strong></td>";
                content += "</tr>";
                content += "</table>";
                return content;
            }

            function buildPolygons(segment) {
                var cleanLatLng = [];
                if (segment.geoFence !== null) {
                    segment.geoFence.forEach(function(geoFence) {
                        cleanLatLng.push({
                            lat: geoFence.lat,
                            lng: geoFence.lon
                        });
                    });
                }

                var segmentPolygon = new google.maps.Polygon({
                    id: "segmentPolygon" + segment.segmentID,
                    paths: cleanLatLng,
                    strokeColor: segment.Color,
                    strokeOpacity: 0.8,
                    strokeWeight: 2,
                    fillColor: segment.Color,
                    fillOpacity: 0.35,
                    editable: false
                });

                scope.polygons.push(segmentPolygon);
            }

            function buildMarkers(segment) {

                var latDelta = (segment.maxLat - segment.minLat) / 2;
                var middleLat = segment.minLat + latDelta;
                var lonDelta = (segment.maxLon - segment.minLon) / 2;
                var middleLon = segment.minLon + lonDelta;

                var segmentMarker = new MarkerWithLabel({
                    id: "segmentMarker" + segment.segmentID,
                    animation: google.maps.Animation.DROP,
                    position: new google.maps.LatLng(middleLat, middleLon),
                    draggable: false,
                    labelContent: segment.segmentID,
                    labelAnchor: new google.maps.Point(25, 40),
                    labelClass: "googleMapMarkerLabel", // the CSS class for the label
                    labelStyle: { opacity: 0.75 }
                });

                var infowindow = new google.maps.InfoWindow({
                    title: "Segment Details",
                    content: buildDetailsContent(segment)
                });
                segmentMarker.addListener('click', function() {
                    infowindow.open(scope.map, segmentMarker);
                });
                scope.markers.push(segmentMarker);
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

            scope.triggerResetMap = function() {
                scope.resetMap();
            };

            scope.triggerMakeAllPolygonsUneditable = function() {
                scope.makeAllPolygonsUneditable();
            };

            scope.triggerSetMapLocation = function(lat, lon, zoom) {
                scope.setMapLocation({
                    lat: lat,
                    lon: lon,
                    zoom: zoom
                });
            };

            scope.triggerSetEditPolygon = function(id) {
                scope.setEditPolygon({
                    id: id
                });
            };

            scope.triggerSetCancelEditPolygon = function(id, color) {
                scope.setCancelEditPolygon({
                    id: id,
                    color: color
                });
            };

            scope.triggerSetNewPolygon = function(color) {
                scope.setNewPolygon({
                    color: color
                });
            };

            scope.$watch("visible", function(isVisible) {
                if (isVisible !== undefined) {

                    if (isVisible) {
                        if (scope.segments.length === 0) {
                            scope.getSegments(true);
                            scope.getBeats();
                        } else {
                            scope.triggerDisplayMapData();
                        }
                    } else {
                        scope.selectedSegment = "";
                    }
                }
            });

            scope.setSelectedSegment = function() {
                console.log(scope.selectedSegment);

                if (scope.selectedSegment) {
                    if (scope.selectedSegment === null) {
                        scope.triggerResetMap();
                        return;
                    }

                    selectedSegmentId = scope.selectedSegment.ID;
                    var latDelta = (scope.selectedSegment.maxLat - scope.selectedSegment.minLat) / 2;
                    var middleLat = scope.selectedSegment.minLat + latDelta;
                    var lonDelta = (scope.selectedSegment.maxLon - scope.selectedSegment.minLon) / 2;
                    var middleLon = scope.selectedSegment.minLon + lonDelta;
                    
                    scope.triggerSetMapLocation(middleLat, middleLon, 16);

                } else {
                    scope.triggerHideMapData();
                    scope.triggerResetMap();
                }

            };

            scope.getSegments = function(triggerMapUpdate) {
                scope.isBusyGettingSegments = true;
                mapService.getSegmentPolygons().then(function(result) {
                    scope.isBusyGettingSegments = false;
                    if (!result) {
                        toastr.error('Failed to retrieve Segments', 'Error');
                    } else {
                        console.log('%i segments found', scope.segments.length);
                        scope.segments = result;
                        scope.polygons = [];
                        scope.markers = [];
                        scope.segments.forEach(function(segment) {
                            buildPolygons(segment);
                            buildMarkers(segment);
                        });

                        if (selectedSegmentId) {
                            scope.selectedSegment = utilService.findArrayElement(scope.segments, "ID", selectedSegmentId);
                        }

                        if (triggerMapUpdate)
                            scope.triggerDisplayMapData();
                    }

                });
            };

            scope.setEdit = function() {
                scope.isEditing = true;
                console.log("Edit segment %s", scope.selectedSegment.segmentID);
                scope.triggerSetEditPolygon("segmentPolygon" + scope.selectedSegment.segmentID);
            };

            scope.cancelEdit = function() {
                scope.isEditing = false;
                console.log("Cancel edit segment %s", scope.selectedSegment.segmentID);
                scope.triggerSetCancelEditPolygon("segmentPolygon" + scope.selectedSegment.segmentID, scope.selectedSegment.Color);
            };

            scope.save = function() {
                console.log("Saving segment...");
                if (scope.selectedPolygon)
                    scope.selectedSegment.geoFence = utilService.getPolygonCoords(scope.selectedPolygon);

                scope.isBusySaving = true;
                mapService.saveSegment(scope.selectedSegment).then(function(result) {
                    scope.isBusySaving = false;
                    if (!result) {
                        console.error("Save Segment");
                        toastr.error('Failed to save Segment', 'Error');
                    } else {
                        console.log("Save Segment Success");
                        toastr.success('Segment Saved', 'Success');
                        scope.cancelEdit();
                        scope.triggerMakeAllPolygonsUneditable();
                    }
                });
            };

            scope.reduceSegmentPolygonPoints = function() {
                var selectedSegmentPolygon = utilService.findArrayElement(scope.polygons, "id", "segmentPolygon" + scope.selectedSegment.segmentID);
                if (!selectedSegmentPolygon) return;
                utilService.clearPolygonCoords(selectedSegmentPolygon);
            };

            scope.delete = function() {
                if (confirm("Ok to delete this Segment?")) {
                    scope.isBusyDeleting = true;
                    mapService.deleteSegment(scope.selectedSegment.ID).then(function(result) {
                        scope.isBusyDeleting = false;
                        scope.isEditing = false;
                        if (!result) {
                            console.error("Delete Segment");
                            toastr.error('Failed to delete Segment', 'Error');
                        } else {
                            console.log("Delete Segment Success");
                            toastr.success('Segment Deleted', 'Success');
                            selectedSegmentId = 0;
                            scope.selectedSegment = "";
                            scope.triggerMakeAllPolygonsUneditable();
                            scope.triggerHideMapData();
                            scope.getSegments(true);
                        }
                    });
                }
            };

            scope.prepareNew = function() {
                scope.selectedSegment = {
                    BeatID: "",
                    Color: "#000000",
                    geoFence: []
                };
                scope.isAdding = true;
                scope.triggerSetNewPolygon(scope.selectedSegment.color);
            };

            scope.cancelAdd = function() {
                scope.isAdding = false;
                scope.selectedSegment = "";
                scope.triggerHideMapData();
                scope.triggerDisplayMapData();
            };

            scope.add = function() {
                console.log("Adding Segment...");
                if (scope.selectedPolygon)
                    scope.selectedSegment.geoFence = utilService.getPolygonCoords(scope.selectedPolygon);

                scope.isBusyAdding = true;
                mapService.addSegment(scope.selectedSegment).then(function(result) {
                    scope.isBusyAdding = false;
                    scope.isAdding = false;
                    if (!result) {
                        console.error("Add Segment");
                        toastr.error('Failed to add Segment', 'Error');
                    } else {
                        console.log("Add Segment Success");
                        toastr.success('Segment Added', 'Success');
                        scope.triggerMakeAllPolygonsUneditable();
                        scope.triggerHideMapData();
                        scope.getSegments(true);
                    }
                });
            };

            scope.getBeats = function() {
                generalService.getBeatNumbers().then(function(result) {
                    scope.beats = result;
                });
            };

        }
    };
});