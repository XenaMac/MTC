mtcApp.directive('mapCallSign', function(mapService, utilService, generalService) {
    return {
        restrict: 'E',
        templateUrl: $(".websiteUrl").text().trim() + '/app/templates/mapCallSignTemplate.html',
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

            var selectedCallSignId = 0;
            scope.callSigns = [];
            scope.selectedCallSign = "";


            scope.isEditing = false;
            scope.isAdding = false;
            scope.isBusyGetting = false;
            scope.isBusySaving = false;
            scope.isBusyDeleting = false;

            scope.markers = [];
            scope.polygons = [];

            function buildMarkers(callSign) {

                var csMarker = new google.maps.Marker({
                    position: new google.maps.LatLng(callSign.Lat, callSign.Lon)
                });

                var infowindow = new google.maps.InfoWindow({
                    title: "Callbox Details",
                    content: "ID: <strong>" + callSign.CallBoxNumber + "</strong><div>Location: " + callSign.Location + "</div>"
                });

                csMarker.addListener('click', function() {
                    infowindow.open(scope.map, csMarker);
                });
                scope.markers.push(csMarker);

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
                        if (scope.callSigns.length === 0) {
                            scope.getCallSigns(true);
                        } else {
                            scope.triggerDisplayMapData();
                        }
                    } else {
                        scope.selectedCallSign = "";
                    }
                }
            });

            scope.setSelectedCallSign = function() {
                console.log(scope.selectedCallSign);

                if (scope.selectedCallSign) {
                    if (scope.selectedCallSign === null) {
                        scope.triggerResetMap();
                        return;
                    }

                    selectedCallSignId = scope.selectedCallSign.ID;

                    buildMarkers(scope.selectedCallSign);
                    scope.triggerDisplayMapData();

                    scope.triggerSetMapLocation(scope.selectedCallSign.Lat, scope.selectedCallSign.Lon, 20);

                } else {
                    scope.triggerHideMapData();
                    scope.triggerResetMap();
                }

            };

            scope.getCallSigns = function(triggerMapUpdate) {
                scope.isBusyGettingCallSigns = true;
                mapService.getCallSigns().then(function(result) {
                    scope.isBusyGettingCallSigns = false;
                    scope.callSigns = result;
                    console.log('%i callsigns found', scope.callSigns.length);

                    scope.polygons = [];
                    scope.markers = [];
                    // scope.callSigns.forEach(function(callSign) {
                    //     buildMarkers(callSign);
                    // });

                    if (selectedCallSignId) {
                        scope.selectedCallSign = utilService.findArrayElement(scope.callSigns, "ID", selectedCallSignId);
                    }

                    // if (triggerMapUpdate)
                    //     scope.triggerDisplayMapData();
                });
            };

        }
    };
});