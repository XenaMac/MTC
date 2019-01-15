mtcApp.directive('mapTowYard', function(mapService, utilService, generalService) {
    return {
        restrict: 'E',
        templateUrl: $(".websiteUrl").text().trim() + '/app/templates/mapTowYardTemplate.html',
        scope: {
            resetMap: "&",
            setMapLocation: "&",
            displayMapData: "&",
            hideMapData: "&",
            setEditPolygon: "&",
            setCancelEditPolygon: "&",
            setNewPolygon: "&",

            selectedPolygon: "=",
            visible: "="
        },
        link: function(scope) {

            var selectedYardId = 0;
            scope.yards = [];
            scope.selectedYard = "";


            scope.isEditing = false;
            scope.isAdding = false;
            scope.isBusyGetting = false;
            scope.isBusySaving = false;
            scope.isBusyDeleting = false;

            scope.markers = [];
            scope.polygons = [];

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

            function buildDetailsContent(yard) {

                console.log("Yard Details %O", yard);

                var content = "<table>";
                content += "<tr>";
                content += "<td>ID:</td>";
                content += "<td><strong>" + yard.YardID + "</strong></td>";
                content += "</tr>";
                content += "<tr>";
                content += "<td>Contractor:</td>";
                content += "<td><strong>" + yard.Contractor + "</strong></td>";
                content += "</tr>";
                content += "<tr>";
                content += "<td>Address:</td>";
                content += "<td><strong>" + yard.Address + "</strong></td>";
                content += "</tr>";
                content += "<tr>";
                content += "<td>City:</td>";
                content += "<td><strong>" + yard.City + "</strong></td>";
                content += "</tr>";
                content += "<tr>";
                content += "<td>Zip:</td>";
                content += "<td><strong>" + yard.Zip + "</strong></td>";
                content += "</tr>";
                content += "<tr>";
                content += "<td>Phone:</td>";
                content += "<td><strong>" + yard.Phone + "</strong></td>";
                content += "</tr>";
                content += "<tr>";
                content += "<td>Beat ID:</td>";
                content += "<td><strong>" + yard.BeatID + "</strong></td>";
                content += "</tr>";
                content += "</table>";
                return content;
            }

            function buildPolygons(yard) {

                var cleanLatLng = [];
                yard.geoFence.forEach(function(geoFence) {
                    cleanLatLng.push({
                        lat: geoFence.lat,
                        lng: geoFence.lon
                    });
                });

                var yardPolygon = new google.maps.Polygon({
                    id: "yardPolygon" + yard.YardID,
                    paths: cleanLatLng,
                    strokeColor: yard.Color,
                    strokeOpacity: 0.8,
                    strokeWeight: 2,
                    fillColor: yard.Color,
                    fillOpacity: 0.35
                });
                scope.polygons.push(yardPolygon);

            }

            function buildMarkers(yard) {
                var latDelta = (yard.maxLat - yard.minLat) / 2;
                var middleLat = yard.minLat + latDelta;
                var lonDelta = (yard.maxLon - yard.minLon) / 2;
                var middleLon = yard.minLon + lonDelta;

                var yardMarker = new MarkerWithLabel({
                    id: "yardMarker" + yard.YardID,
                    animation: google.maps.Animation.DROP,
                    position: new google.maps.LatLng(middleLat, middleLon),
                    draggable: false,
                    labelContent: yard.YardID,
                    labelAnchor: new google.maps.Point(25, 40),
                    labelClass: "googleMapMarkerLabel", // the CSS class for the label
                    labelStyle: { opacity: 0.75 }
                });

                var infowindow = new google.maps.InfoWindow({
                    title: "Tow Truck Yard Details",
                    content: buildDetailsContent(yard)
                });

                yardMarker.addListener('click', function() {
                    infowindow.open(scope.map, yardMarker);
                });
                scope.markers.push(yardMarker);
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
                        if (scope.yards.length === 0) {
                            scope.getYards(true);
                            scope.getBeats();
                        } else {
                            scope.triggerDisplayMapData();
                        }
                    } else {
                        scope.selectedYard = "";
                    }
                }
            });

            scope.setSelectedYard = function() {
                console.log(scope.selectedYard);

                if (scope.selectedYard) {
                    if (scope.selectedYard === null) {
                        scope.triggerResetMap();
                        return;
                    }

                    selectedYardId = scope.selectedYard.ID;
                    if (scope.selectedYard.maxLat === undefined) return;
                    var latDelta = (scope.selectedYard.maxLat - scope.selectedYard.minLat) / 2;
                    var middleLat = scope.selectedYard.minLat + latDelta;
                    var lonDelta = (scope.selectedYard.maxLon - scope.selectedYard.minLon) / 2;
                    var middleLon = scope.selectedYard.minLon + lonDelta;
                                      
                    scope.triggerSetMapLocation(middleLat, middleLon, 17);

                } else {
                    scope.triggerHideMapData();
                    scope.triggerResetMap();
                }
                
            };

            scope.getYards = function(triggerMapUpdate) {
                scope.isBusyGettingYards = true;
                mapService.getYardPolygons().then(function(result) {
                    scope.isBusyGettingYards = false;
                    if (!result) {
                        toastr.error('Failed to retrieve Tow Yards', 'Error');
                    } else {
                        console.log('%i yards found', scope.yards.length);
                        scope.yards = result;
                        scope.polygons = [];
                        scope.markers = [];
                        scope.yards.forEach(function(yard) {
                            buildPolygons(yard);
                            buildMarkers(yard);
                        });

                        if (selectedYardId) {
                            scope.selectedYard = utilService.findArrayElement(scope.yards, "ID", selectedYardId);
                        }

                        if (triggerMapUpdate)
                            scope.triggerDisplayMapData();
                    }

                });
            };

            scope.setEdit = function() {
                scope.isEditing = true;
                console.log("Edit yard %s", scope.selectedYard.YardID);
                scope.triggerSetEditPolygon("yardPolygon" + scope.selectedYard.YardID);
            };

            scope.cancelEdit = function() {
                scope.isEditing = false;
                console.log("Cancel edit yard %s", scope.selectedYard.YardID);
                scope.triggerSetCancelEditPolygon("yardPolygon" + scope.selectedYard.YardID, scope.selectedYard.Color);
            };

            scope.save = function() {
                console.log("Saving yard...");
                if (scope.selectedPolygon)
                    scope.selectedYard.geoFence = utilService.getPolygonCoords(scope.selectedPolygon);

                scope.isBusySaving = true;
                mapService.saveYard(scope.selectedYard).then(function(result) {
                    scope.isBusySaving = false;
                    if (!result) {
                        console.error("Save Tow Yard");
                        toastr.error('Failed to save Tow Yard', 'Error');
                    } else {
                        console.log("Save Tow Yard Success");
                        toastr.success('Tow Yard Saved', 'Success');
                        scope.cancelEdit();
                    }
                });
            };

            scope.delete = function() {
                if (confirm("Ok to delete this Tow Truck Yard?")) {
                    scope.isBusyDeleting = true;
                    mapService.deleteYard(scope.selectedYard.ID).then(function(result) {
                        scope.isBusyDeleting = false;
                        scope.isEditing = false;
                        if (!result) {
                            console.error("Delete Tow Yard");
                            toastr.error('Failed to delete Tow Yard', 'Error');
                        } else {
                            console.log("Delete Tow Yard Success");
                            selectedYardId = 0;
                            scope.selectedYard = "";
                            toastr.success('Tow Yard Deleted', 'Success');
                            scope.triggerHideMapData();
                            scope.triggerResetMap();
                            scope.getYards(true);
                        }
                    });
                }
            };

            scope.prepareNew = function() {
                scope.selectedYard = {
                    Address: "",
                    BeatID: "",
                    City: "",
                    Zip: "",
                    Color: "#000000",
                    Contractor: "",
                    Phone: "",
                    geoFence: []
                };
                scope.isAdding = true;
                scope.triggerSetNewPolygon(scope.selectedYard.color);
            };

            scope.cancelAdd = function() {
                scope.isAdding = false;
                scope.selectedYard = "";
                scope.triggerHideMapData();
                scope.triggerDisplayMapData();
            };

            scope.add = function() {
                console.log("Adding yard...");
                if (scope.selectedPolygon)
                    scope.selectedYard.geoFence = utilService.getPolygonCoords(scope.selectedPolygon);

                if (scope.selectedYard.geoFence.length === 0) {
                    toastr.error('Polygon data missing. Please try again.', 'Error');
                    return;
                }
                console.log(scope.selectedYard);

                scope.isBusyAdding = true;
                scope.triggerHideMapData();
                mapService.addYard(scope.selectedYard).then(function(result) {
                    scope.isBusyAdding = false;
                    scope.isAdding = false;
                    if (!result) {
                        console.error("Add Tow Yard");
                        toastr.error('Failed to add Tow Yard', 'Error');
                    } else {
                        console.log("Add Tow Yard Success");
                        toastr.success('Tow Yard Added', 'Success');
                        scope.getYards(true);
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