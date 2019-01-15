(function() {
    'use strict';
    mtcApp.controller('mapController', function alertDetailsController($scope, $rootScope, $interval, $window, $compile, trucksService, mapService, utilService) {

        var DEFAULT_MAP_CENTER_LAT = 37.880;
        var DEFAULT_MAP_CENTER_LON = -122.000;

        var ZOOM_9 = 9;
        var ZOOM_12 = 12;
        var ZOOM_13 = 13;
        var ZOOM_14 = 14;
        var ZOOM_15 = 15;
        var ZOOM_16 = 16;
        var ZOOM_17 = 17;
        var ZOOM_18 = 18;
        var ZOOM_20 = 20;
        var IS_SELECTED_COLOR = "#FF9900";
        var IS_EDITING_COLOR = "#FF0000";

        $scope.map;
        $scope.infowindow = new google.maps.InfoWindow({
            content: ''
        });
        $scope.mapPolygon = new google.maps.Polygon({
            strokeColor: '#FF0000',
            strokeOpacity: 0.8,
            strokeWeight: 2,
            fillColor: '#FF0000',
            fillOpacity: 0.35
        });
        $scope.generalMarker = new google.maps.Marker({});

        $scope.towTruckYardsVisible = false;
        $scope.segmentsVisible = false;
        $scope.beatsVisible = false;
        $scope.dropSitesVisible = false;
        $scope.callSignsVisible = false;

        $scope.polygons = [];
        $scope.selectedPolygon = {};
        $scope.markers = [];

        $scope.trucks = [];
        $scope.truckMarkers = [];
        $scope.selectedTruck = "";
        $scope.selectedTruckMarker = "";
        $scope.truckToBeFollowed = "";

        // var markerClusterer;
        // var markerClusterOptions = {
        //     imagePath: 'https://raw.githubusercontent.com/googlemaps/js-marker-clusterer/gh-pages/images/m'
        // };

        function sizeMap() {
            var menuHeight = 100;
            var sideMenuWidth = 300;
            var wHeight = ($window.innerHeight - menuHeight) + 'px';
            var wWidth = ($window.innerWidth - sideMenuWidth) + 'px';
            $("#googleMap").height(wHeight);
            $("#googleMap").width(wWidth);

            $("#googleMapSideNavigation").height(wHeight);
        }

        function buildDetailsContent(truck) {
            var content = "<table>";
            content += "<tr>";
            content += "<td>Last Msg:</td>";
            content += "<td><strong>" + truck.lastMessage + "</strong></td>";
            content += "</tr>";
            content += "<tr>";
            content += "<td>CS:</td>";
            content += "<td><strong>" + truck.callSign + "</strong></td>";
            content += "</tr>";
            content += "<tr>";
            content += "<td>Driver:</td>";
            content += "<td><strong>" + truck.driverName + "</strong></td>";
            content += "</tr>";
            content += "<tr>";
            content += "<td>Truck Number:</td>";
            content += "<td><strong>" + truck.truckNumber + "</strong></td>";
            content += "</tr>";
            content += "<tr>";
            content += "<td>Contractor:</td>";
            content += "<td><strong>" + truck.contractorName + "</strong></td>";
            content += "</tr>";
            content += "<tr>";
            content += "<td>Status:</td>";
            content += "<td><strong>" + truck.vehicleState + "</strong></td>";
            content += "</tr>";
            content += "<tr>";
            content += "<td>Speed:</td>";
            content += "<td><strong>" + truck.speed + "mph</td>";
            content += "</tr>";
            content += "<tr>";
            content += "<td><button ng-click='follow(" + truck.id + ")'>Follow</button></td>";
            content += "<td><button ng-click='zoomTo(" + truck.id + ")'>Zoom In</button></td>";
            content += "</tr>";
            content += "</table>";
            return content;
        }

        function setTruckMarker(truck) {

            var detailsContent = $compile(buildDetailsContent(truck))($scope);

            var truckMarker = utilService.findArrayElement($scope.truckMarkers, "id", truck.id);
            if (!truckMarker) {
                console.log("New truck marker %O", truck);
                truckMarker = new RichMarker({
                    id: truck.id,
                    map: $scope.map,
                    animation: google.maps.Animation.DROP,
                    draggable: false,
                    flat: true,
                    anchor: RichMarkerPosition.MIDDLE,
                    detailsContent: detailsContent,
                    content: "<div style='cursor: pointer'><img id='truckIcon" + truck.id + "' src='" + truck.vehicleStateIconUrl + "' class='truckIcon' /></div>"
                });

                google.maps.event.addListener(truckMarker, 'click', (function(truckMarker, scope) {
                    return function() {
                        scope.selectedTruckMarker = truckMarker;
                        scope.infowindow.setContent(truckMarker.detailsContent[0]);
                        scope.infowindow.open(scope.map, truckMarker);
                    };
                })(truckMarker, $scope));

                $scope.truckMarkers.push(truckMarker);
            }
            truckMarker.detailsContent = detailsContent;

            var latlng = new google.maps.LatLng(truck.lat, truck.lon);
            truckMarker.setPosition(latlng);
            truckMarker.setContent("<img id='truckIcon" + truck.id + "' src='" + truck.vehicleStateIconUrl + "' class='truckIcon' /><br /><span style='color: black;background-color: white;border: 1px solid white;font-size: 12px;text-align: center;white-space: normal;margin: -60px 5px 0px 0px;padding: 0px;opacity: .75;pointer-events: none;width: 60px;'>" + truck.truckNumber + "</span>");

            $("#truckIcon" + truck.id).rotate({
                angle: truck.heading - 90
            });

            if ($scope.selectedTruckMarker && $scope.selectedTruckMarker.id === truckMarker.id) {
                $scope.infowindow.setContent(truckMarker.detailsContent[0]);
            }

        }

        function updateMap(latlng, zoom) {
            $scope.map.panTo(latlng);
            $scope.map.setZoom(zoom);
        }

        function cleanupTruckMarkers() {
            for (var i = 0; i < $scope.truckMarkers.length; i++) {
                var truckMarker = $scope.truckMarkers[i];
                var truck = utilService.findArrayElement($scope.trucks, "id", truckMarker.id);
                if (!truck) {
                    truckMarker.setMap(null);
                }
            }
        }

        function setMapControls() {

            var controlDiv = document.createElement('div');

            // Set CSS for the control border.
            var controlUI = document.createElement('div');
            controlUI.style.backgroundColor = '#fff';
            controlUI.style.border = '2px solid #fff';
            controlUI.style.borderRadius = '3px';
            controlUI.style.boxShadow = '0 2px 6px rgba(0,0,0,.3)';
            controlUI.style.cursor = 'pointer';
            controlUI.style.marginBottom = '22px';
            controlUI.style.textAlign = 'center';
            controlUI.title = 'Click to recenter the map';
            controlDiv.appendChild(controlUI);

            // Set CSS for the control interior.
            var controlText = document.createElement('div');
            controlText.style.color = 'rgb(25,25,25)';
            controlText.style.fontFamily = 'Roboto,Arial,sans-serif';
            controlText.style.fontSize = '12px';
            controlText.style.lineHeight = '38px';
            controlText.style.paddingLeft = '5px';
            controlText.style.paddingRight = '5px';
            controlText.innerHTML = 'Center Map';
            controlUI.appendChild(controlText);

            // Setup the click event listeners: simply set the map to Chicago.
            controlUI.addEventListener('click', function() {
                updateMap(new google.maps.LatLng(DEFAULT_MAP_CENTER_LAT, DEFAULT_MAP_CENTER_LON), ZOOM_9);
            });

            controlDiv.index = 1;
            $scope.map.controls[google.maps.ControlPosition.TOP_CENTER].push(controlDiv);

        }

        function getTrucksRefreshRate() {
            trucksService.getTrucksRefreshRate().then(function(results) {
                getTrucks();
                $interval(function() {
                    getTrucks();
                }, eval(results));
            });
        }

        function getTrucks() {
            trucksService.getTrucks().then(function(results) {
                for (var i = 0; i < results.length; i++) {
                    var exists = false;
                    for (var ii = 0; ii < $scope.trucks.length; ii++) {
                        if ($scope.trucks[ii].ipAddress === results[i].IPAddress) {
                            exists = true;
                            $scope.trucks[ii].update(results[i]);
                        }
                    }
                    if (!exists) $scope.trucks.push(new $rootScope.mtcTruck(results[i]));
                }
                drawTruckMarkers();
                cleanupTruckMarkers();
                if ($scope.truckToBeFollowed) {
                    updateMap(new google.maps.LatLng($scope.truckToBeFollowed.lat, $scope.truckToBeFollowed.lon), ZOOM_14);
                }
            });
        }

        function drawTruckMarkers() {
            $scope.trucks.forEach(function(truck) {
                setTruckMarker(truck);
            });
        }

        $('#towTruckYards').on('show.bs.collapse', function() {
            console.log("tow truck yards visible");
            $scope.hideMapData();
            $scope.resetMap();
            setTimeout(function() {
                $scope.towTruckYardsVisible = true;
                $scope.$apply();
            }, 250);
        });

        $('#towTruckYards').on('hidden.bs.collapse', function() {
            console.log("tow truck yards invisible");
            $scope.towTruckYardsVisible = false;
            $scope.$apply();
            // $scope.hideMapData();
            // $scope.resetMap();            
        });

        $('#segments').on('show.bs.collapse', function() {
            console.log("segments visible");
            $scope.hideMapData();
            $scope.resetMap();
            setTimeout(function() {
                $scope.segmentsVisible = true;
                $scope.$apply();
            }, 250);
        });

        $('#segments').on('hidden.bs.collapse', function() {
            console.log("segments invisible");
            $scope.segmentsVisible = false;
            $scope.$apply();
            // $scope.hideMapData();
            // $scope.resetMap();            
        });

        $('#beats').on('show.bs.collapse', function() {
            console.log("beats visible");
            $scope.hideMapData();
            $scope.resetMap();
            setTimeout(function() {
                $scope.beatsVisible = true;
                $scope.$apply();
            }, 250);
        });

        $('#beats').on('hidden.bs.collapse', function() {
            console.log("beats invisible");
            $scope.beatsVisible = false;
            $scope.$apply();                    
        });

        $('#dropSites').on('show.bs.collapse', function() {
            console.log("drop sites visible");
            $scope.hideMapData();
            $scope.resetMap();
            setTimeout(function() {
                $scope.dropSitesVisible = true;
                $scope.$apply();
            }, 250);
        });

        $('#dropSites').on('hidden.bs.collapse', function() {
            console.log("drop sites invisible");
            $scope.dropSitesVisible = false;
            $scope.$apply();
            //$scope.hideMapData();
            // $scope.resetMap();            
        });

        $('#callBoxes').on('show.bs.collapse', function() {
            console.log("call boxes visible");
            $scope.hideMapData();
            $scope.resetMap();
            setTimeout(function() {
                $scope.callSignsVisible = true;
                $scope.$apply();
            }, 250);
        });

        $('#callBoxes').on('hidden.bs.collapse', function() {
            console.log("call boxes invisible");
            $scope.callSignsVisible = false;
            $scope.$apply();                  
        });

        //map actions

        function removeAllMapEvents() {
            google.maps.event.clearListeners($scope.map, 'dblclick');
            google.maps.event.clearListeners($scope.map, 'click');
            $scope.polygons.forEach(function(polygon) {
                polygon.setEditable(false);
                google.maps.event.clearListeners(polygon, 'dblclick');
            });
        }

        $scope.resetMap = function() {
            console.log("resetMap");
            removeAllMapEvents();
            updateMap(new google.maps.LatLng(DEFAULT_MAP_CENTER_LAT, DEFAULT_MAP_CENTER_LON), ZOOM_9);
        };

        $scope.displayMapData = function(polygons, markers) {
            console.log("displayMapData, number of polygons: %i, number of markers: %i", polygons.length, markers.length);

            if (polygons !== undefined) {
                $scope.polygons = polygons;
                $scope.polygons.forEach(function(polygon) {
                    polygon.setMap($scope.map);
                });
            }
            if (markers !== undefined) {
                $scope.markers = markers;
                $scope.markers.forEach(function(marker) {
                    marker.setMap($scope.map);
                });
                //markerClusterer = new MarkerClusterer($scope.map, $scope.markers, markerClusterOptions);
            }
        };

        $scope.hideMapData = function() {
            console.log("hideMapData");
            removeAllMapEvents();
            $scope.polygons.forEach(function(polygon) {
                polygon.setMap(null);
            });
            $scope.markers.forEach(function(marker) {
                marker.setMap(null);
            });
            // if (markerClusterer !== null && markerClusterer !== undefined)
            //     markerClusterer.clearMarkers();

            $scope.polygons = [];
            $scope.markers = [];
            $scope.selectedPolygon = {};
        };

        $scope.hideMapMarkers = function() {
          console.log("hideMapMarkers");          
          $scope.markers.forEach(function(marker) {
              marker.setMap(null);
          });                    
          $scope.markers = [];          
      };

        $scope.setMapLocation = function(lat, lon, zoom) {
            console.log("setMapLocation");
            updateMap(new google.maps.LatLng(lat, lon), zoom);
        };

        $scope.setEditPolygon = function(id) {
            console.log("setEditPolygon");
            $scope.selectedPolygon = utilService.findArrayElement($scope.polygons, "id", id);
            if (!$scope.selectedPolygon) return;

            google.maps.event.addListener($scope.selectedPolygon, 'dblclick', function(e) {
                if (e.vertex === undefined) {
                    return;
                }
                $scope.selectedPolygon.getPath().removeAt(e.vertex);
            });
            google.maps.event.addListener($scope.map, 'click', function(e) {
                var path = $scope.selectedPolygon.getPath();
                if (path === undefined)
                    path = [];
                path.push(e.latLng);
            });

            $scope.selectedPolygon.setEditable(true);
            $scope.selectedPolygon.setOptions({
                strokeColor: IS_EDITING_COLOR,
                fillColor: IS_EDITING_COLOR
            });
        };

        $scope.setEditPolygons = function() {
            console.log("setEditPolygons");

            $scope.polygons.forEach(function(polygon) {

                google.maps.event.addListener(polygon, 'dblclick', function(e) {
                    if (e.vertex === undefined) {
                        return;
                    }
                    polygon.getPath().removeAt(e.vertex);
                });
                google.maps.event.addListener($scope.map, 'click', function(e) {
                    var path = polygon.getPath();
                    if (path === undefined)
                        path = [];
                    path.push(e.latLng);
                });

                polygon.setEditable(true);
                polygon.setOptions({
                    strokeColor: IS_EDITING_COLOR,
                    fillColor: IS_EDITING_COLOR
                });
            });

        };

        $scope.setCancelEditPolygon = function(id, color) {
            console.log("setCancelEditPolygon");
            $scope.selectedPolygon = utilService.findArrayElement($scope.polygons, "id", id);
            if (!$scope.selectedPolygon) return;
            $scope.selectedPolygon.setEditable(false);
            $scope.selectedPolygon.setOptions({
                strokeColor: color,
                fillColor: color
            });
            var polygon = $scope.selectedPolygon;
            removeAllMapEvents();
        };

        $scope.setCancelEditPolygons = function() {
            console.log("setCancelEditPolygons");
            removeAllMapEvents();
            $scope.polygons.forEach(function(polygon) {
                polygon.setEditable(false);
                polygon.setOptions({
                    strokeColor: polygon.Color,
                    fillColor: polygon.Color
                });
            });
        };

        $scope.setNewPolygon = function(color) {
            console.log("setNewPolygon");
            $scope.hideMapData();
            $scope.selectedPolygon = {};

            $scope.selectedPolygon = new google.maps.Polygon({
                strokeColor: color,
                strokeOpacity: 0.8,
                strokeWeight: 2,
                fillColor: color,
                fillOpacity: 0.35,
                editable: true
            });
            $scope.selectedPolygon.setMap($scope.map);

            google.maps.event.addListener($scope.selectedPolygon, 'dblclick', function(e) {
                if (e.vertex === undefined) {
                    return;
                }
                $scope.selectedPolygon.getPath().removeAt(e.vertex);
            });
            google.maps.event.addListener($scope.map, 'click', function(e) {
                var path = $scope.selectedPolygon.getPath();
                path.push(e.latLng);
            });

            $scope.polygons.push($scope.selectedPolygon);
        };

        $scope.makeAllPolygonsUneditable = function() {
            console.log("makeAllPolygonsUneditable");
            $scope.polygons.forEach(function(polygon) {
                polygon.setEditable(false);
            });
        };

        //truck actions
        $scope.follow = function(truckId) {
            var tId = truckId.toString();
            console.log("Follow %s", tId);
            $scope.truckToBeFollowed = utilService.findArrayElement($scope.trucks, "id", tId);
            if ($scope.truckToBeFollowed === undefined) return;

            updateMap(new google.maps.LatLng($scope.truckToBeFollowed.lat, $scope.truckToBeFollowed.lon), ZOOM_14);
        };

        $scope.zoomTo = function(truckId) {
            var tId = truckId.toString();
            console.log("Zoom To %s", tId);
            var truckToBeZoomedTo = utilService.findArrayElement($scope.trucks, "id", tId);
            if (truckToBeZoomedTo === undefined) return;

            updateMap(new google.maps.LatLng(truckToBeZoomedTo.lat, truckToBeZoomedTo.lon), ZOOM_14);
        };

        $scope.initMap = function() {

            var defaultMapLocation = new google.maps.LatLng(DEFAULT_MAP_CENTER_LAT, DEFAULT_MAP_CENTER_LON);

            var myOptions = {
                center: defaultMapLocation,
                zoom: ZOOM_9,
                mapTypeId: google.maps.MapTypeId.ROADMAP //ROADMAP //SATELLITE //HYBRID //TERRAIN
            };

            var mapElement = document.getElementById('googleMap');
            $scope.map = new google.maps.Map(mapElement, myOptions);
            google.maps.event.trigger($scope.map, "resize");

            google.maps.event.addListener($scope.infowindow, 'closeclick', function() {
                $scope.selectedTruckMarker = "";
                if ($scope.truckToBeFollowed)
                    $scope.truckToBeFollowed = "";
                $scope.map.panTo(defaultMapLocation);
                $scope.map.setZoom(ZOOM_9);
            });

            sizeMap();
            setMapControls();
        };

        $scope.showLegend = function() {
            console.log("show legend");
            $("#legendModal").modal("show");
        };

        getTrucksRefreshRate();

        $scope.initMap();

        angular.element($window).bind('resize', function() {
            sizeMap();
        });

    });
}());