mtcApp.directive('mapDropSite', function(mapService, utilService, generalService) {
    return {
        restrict: 'E',
        templateUrl: $(".websiteUrl").text().trim() + '/app/templates/mapDropSiteTemplate.html',
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


            var selectedDropSiteId = 0;
            scope.dropSites = [];
            scope.selectedDropSite = "";

            scope.isEditing = false;
            scope.isAdding = false;
            scope.isBusyGetting = false;
            scope.isBusySaving = false;
            scope.isBusyDeleting = false;

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

            function buildDetailsContent(dropSite) {
                var content = "<table>";
                content += "<tr>";
                content += "<td>Drop Site ID:</td>";
                content += "<td><strong>" + dropSite.dropSiteID + "</strong></td>";
                content += "</tr>";
                content += "<tr>";
                content += "<td>Drop Site Description:</td>";
                content += "<td><strong>" + dropSite.dropSiteDescription + "</strong></td>";
                content += "</tr>";
                content += "<tr>";
                content += "<td>Beat ID:</td>";
                content += "<td><strong>" + dropSite.beatID + "</strong></td>";
                content += "</tr>";
                content += "</table>";
                return content;
            }

            function buildPolygons(dropSite) {

                var cleanLatLng = [];
                dropSite.geoFence.forEach(function(geoFence) {
                    cleanLatLng.push({
                        lat: geoFence.lat,
                        lng: geoFence.lon
                    });
                });
                var dropSitePolygon = new google.maps.Polygon({
                    id: "dropsitePolygon" + dropSite.dropSiteID,
                    paths: cleanLatLng,
                    strokeColor: dropSite.Color,
                    strokeOpacity: 0.8,
                    strokeWeight: 2,
                    fillColor: dropSite.Color,
                    fillOpacity: 0.35
                });
                scope.polygons.push(dropSitePolygon);

            }

            function buildMarkers(dropSite) {
                var latDelta = (dropSite.maxLat - dropSite.minLat) / 2;
                var middleLat = dropSite.minLat + latDelta;
                var lonDelta = (dropSite.maxLon - dropSite.minLon) / 2;
                var middleLon = dropSite.minLon + lonDelta;
                var dropSiteMarker = new MarkerWithLabel({
                    id: "dropSite" + dropSite.dropSiteID,
                    animation: google.maps.Animation.DROP,
                    position: new google.maps.LatLng(middleLat, middleLon),
                    draggable: false,
                    labelContent: dropSite.dropSiteID,
                    labelAnchor: new google.maps.Point(25, 40),
                    labelClass: "googleMapMarkerLabel", // the CSS class for the label
                    labelStyle: { opacity: 0.75 }
                });
                var infowindow = new google.maps.InfoWindow({
                    title: "Drop Site Details",
                    content: buildDetailsContent(dropSite)
                });
                dropSiteMarker.addListener('click', function() {
                    infowindow.open(scope.map, dropSiteMarker);
                });
                scope.markers.push(dropSiteMarker);
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
                        if (scope.dropSites.length === 0) {
                            scope.getDropSites(true);
                            scope.getBeats();
                        } else {
                            scope.triggerDisplayMapData();
                        }
                    } else {
                        scope.selectedDropSite = "";
                    }
                }
            });

            scope.setSelected = function() {
                console.log(scope.selectedDropSite);

                if (scope.selectedDropSite) {
                    if (scope.selectedDropSite === null) {
                        scope.triggerResetMap();
                        return;
                    }

                    selectedDropSiteId = scope.selectedDropSite.ID;
                    var latDelta = (scope.selectedDropSite.maxLat - scope.selectedDropSite.minLat) / 2;
                    var middleLat = scope.selectedDropSite.minLat + latDelta;
                    var lonDelta = (scope.selectedDropSite.maxLon - scope.selectedDropSite.minLon) / 2;
                    var middleLon = scope.selectedDropSite.minLon + lonDelta;
                    
                    scope.triggerSetMapLocation(middleLat, middleLon, 18);

                } else {
                    scope.triggerHideMapData();
                    scope.triggerResetMap();
                }

            };

            scope.getDropSites = function(triggerMapUpdate) {
                scope.isBusyGettingDropSites = true;
                mapService.getDropSitePolygons().then(function(result) {
                    scope.isBusyGettingDropSites = false;

                    if (!result) {
                        console.error("Get Drop Sites");
                        toastr.error('Failed to retrieve Drop Sites', 'Error');
                    } else {
                        console.log('%i drop sites found', scope.dropSites.length);
                        scope.dropSites = result;

                        scope.polygons = [];
                        scope.markers = [];
                        scope.dropSites.forEach(function(dropSite) {
                            buildPolygons(dropSite);
                            buildMarkers(dropSite);
                        });

                        if (selectedDropSiteId) {
                            scope.selectedDropSite = utilService.findArrayElement(scope.dropSites, "ID", selectedDropSiteId);
                        }
                        if (triggerMapUpdate)
                            scope.triggerDisplayMapData();
                    }


                });
            };

            scope.setEdit = function() {
                scope.isEditing = true;
                console.log("Edit drop site %s", scope.selectedDropSite.dropSiteID);
                scope.triggerSetEditPolygon("dropsitePolygon" + scope.selectedDropSite.dropSiteID);
            };

            scope.cancelEdit = function() {
                scope.isEditing = false;
                console.log("Cancel edit dropsite %s", scope.selectedDropSite.dropSiteID);
                scope.triggerSetCancelEditPolygon("dropsitePolygon" + scope.selectedDropSite.dropSiteID, scope.selectedDropSite.Color);
            };

            scope.save = function() {
                console.log("Saving drop site...");
                if (scope.selectedDropSite)
                    scope.selectedDropSite.geoFence = utilService.getPolygonCoords(scope.selectedPolygon);
                scope.isBusySaving = true;
                mapService.saveDropSite(scope.selectedDropSite).then(function(result) {
                    scope.isBusySaving = false;
                    if (!result) {
                        console.error("Save Drop Site");
                        toastr.error('Failed to save Drop Sites', 'Error');
                    } else {
                        console.log("Save Drop Site Success");
                        toastr.success('Drop Site Saved', 'Success');
                        scope.cancelEdit();
                    }
                });
            };

            scope.delete = function() {
                if (confirm("Ok to delete this Drop Site?")) {
                    scope.isBusyDeleting = true;
                    mapService.deleteDropSite(scope.selectedDropSite.ID).then(function(result) {
                        scope.isBusyDeleting = false;
                        scope.isEditing = false;
                        if (!result) {
                            console.error("Delete Drop Site");
                            toastr.error('Failed to delete Drop Site', 'Error');
                        } else {
                            console.log("Delete Drop Site Success");
                            selectedDropSiteId = 0;
                            scope.selectedDropSite = "";
                            toastr.success('Drop Site Deleted', 'Success');
                            scope.triggerHideMapData();
                            scope.triggerResetMap();
                            scope.getDropSites(true);
                        }
                    });
                }
            };

            scope.prepareNew = function() {
                scope.selectedDropSite = {
                    dropSiteID: "",
                    dropSiteDescription: "",
                    beatID: "",
                    Color: "#000000",
                    geoFence: []
                };
                scope.isAdding = true;
                scope.triggerSetNewPolygon(scope.selectedDropSite.color);
            };

            scope.cancelAdd = function() {
                scope.isAdding = false;
                scope.selectedDropSite = "";
                scope.triggerHideMapData();
                scope.triggerDisplayMapData();
            };

            scope.add = function() {
                console.log("Adding drop site...");
                if (scope.selectedDropSite)
                    scope.selectedDropSite.geoFence = utilService.getPolygonCoords(scope.selectedPolygon);

                if (scope.selectedDropSite.geoFence.length === 0) {
                    toastr.error('Polygon data missing. Please try again.', 'Error');
                    return;
                }
                console.log(scope.selectedDropSite);
                scope.isBusyAdding = true;
                scope.triggerHideMapData();
                mapService.addDropSite(scope.selectedDropSite).then(function(result) {
                    scope.isBusyAdding = false;
                    scope.isAdding = false;
                    if (!result) {
                        console.error("Save Drop Site");
                        toastr.error('Failed to add Drop Site', 'Error');
                    } else {
                        console.log("Add Drop Site Success");
                        toastr.success('Drop Site Added', 'Success');
                        scope.getDropSites(true);
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