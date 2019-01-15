/// <reference path="../Scripts/knockout-2.1.0.js" />
/// <reference path="../Scripts/jquery.signalR-0.5.3.js" />


$(function () {

    var SERVICE_BASE_URL = $("#websitePath").attr("data-websitePath");
    var TRUCK_IMAGE_BASE_URL = $("#websitePath").attr("data-websitePath") + "/Content/Images/";
    var DEFAULT_MAP_CENTER_LAT = 33.6600;
    var DEFAULT_MAP_CENTER_LON = -117.7927;
    var DEFAULT_MAP_ZOOM = 10;
    var defaultMapLocation;
    var currentMapLocation;     
    var myLayout;
    var markersArray = [];
  
    var viewModel;

    var MyViewModel = function () {

        var self = this;

        self.towTrucks = ko.observableArray();
        self.selectedTowTruck = ko.observable(new uiTruckItem(self, 0, '', '', 0, 0, 0, 0, 0, ''));
        self.unFollowText = ko.observable('Stop following');
        self.followingTruck = ko.observable(false);

        self.currentSort = ko.observable("truckId");
        self.currentSortDirection = ko.observable("Asc");

        self.columns = ko.observableArray([
          new column(self, "Truck #", "truckId"),
          new column(self, "Beat #", "beatNumber"),
          new column(self, "Beat Segment #", "beatSegmentNumber"),
          new column(self, "Status", "status"),
          new column(self, "Speed", "speed"),
          new column(self, "Last Update", "lastUpdate")
        ]);


        //column
        function column(root, name, value) {
            var self = this;

            self.name = ko.observable(name);
            self.value = ko.observable(value);

            self.sort = function (item) {

                root.currentSort(item.value());

                if (root.currentSortDirection() === 'Asc')
                    root.currentSortDirection('Desc');
                else
                    root.currentSortDirection('Asc');


                if (root.currentSort() === 'truckId') {
                    if (root.currentSortDirection() === 'Asc')
                        root.towTrucks.sort(function (left, right) { return left.truckId == right.truckId ? 0 : (left.truckId < right.truckId ? -1 : 1) })
                    else
                        root.towTrucks.sort(function (left, right) { return left.truckId == right.truckId ? 0 : (left.truckId > right.truckId ? -1 : 1) })
                }
                else if (root.currentSort() === 'beatNumber') {
                    if (root.currentSortDirection() === 'Asc')
                        root.towTrucks.sort(function (left, right) { return left.beatNumber() == right.beatNumber() ? 0 : (left.beatNumber() < right.beatNumber() ? -1 : 1) })
                    else
                        root.towTrucks.sort(function (left, right) { return left.beatNumber() == right.beatNumber() ? 0 : (left.beatNumber() > right.beatNumber() ? -1 : 1) })
                }
                else if (root.currentSort() === 'beatSegmentNumber') {
                    if (root.currentSortDirection() === 'Asc')
                        root.towTrucks.sort(function (left, right) { return left.beatSegmentNumber() == right.beatSegmentNumber() ? 0 : (left.beatSegmentNumber() < right.beatSegmentNumber() ? -1 : 1) })
                    else
                        root.towTrucks.sort(function (left, right) { return left.beatSegmentNumber() == right.beatSegmentNumber() ? 0 : (left.beatSegmentNumber() > right.beatSegmentNumber() ? -1 : 1) })
                }
                else if (root.currentSort() === 'status') {
                    if (root.currentSortDirection() === 'Asc')
                        root.towTrucks.sort(function (left, right) { return left.status() == right.status() ? 0 : (left.status() < right.status() ? -1 : 1) })
                    else
                        root.towTrucks.sort(function (left, right) { return left.status() == right.status() ? 0 : (left.status() > right.status() ? -1 : 1) })
                }
                else if (root.currentSort() === 'speed') {
                    if (root.currentSortDirection() === 'Asc')
                        root.towTrucks.sort(function (left, right) { return left.speed() == right.speed() ? 0 : (left.speed() < right.speed() ? -1 : 1) })
                    else
                        root.towTrucks.sort(function (left, right) { return left.speed() == right.speed() ? 0 : (left.speed() > right.speed() ? -1 : 1) })
                }
                else if (root.currentSort() === 'lastUpdate') {
                    if (root.currentSortDirection() === 'Asc')
                        root.towTrucks.sort(function (left, right) { return left.lastUpdate() == right.lastUpdate() ? 0 : (left.lastUpdate() < right.lastUpdate() ? -1 : 1) })
                    else
                        root.towTrucks.sort(function (left, right) { return left.lastUpdate() == right.lastUpdate() ? 0 : (left.lastUpdate() > right.lastUpdate() ? -1 : 1) })
                }

            };
        }

        //truck object
        function uiTruckItem(root, truckId, vehicleStateIconUrl, vehicleState, heading, beatNumber, beatSegmentNumber, contractorId, lat, lon, speed, lastUpdate, lastMessage) {
            var self = this;

            self.truckId = truckId;
            self.vehicleStateIconUrl = ko.observable(TRUCK_IMAGE_BASE_URL + '/' + vehicleStateIconUrl);
            self.vehicleState = ko.observable(vehicleState);
            self.heading = ko.observable(heading);
            self.beatNumber = ko.observable(beatNumber);
            self.beatSegmentNumber = ko.observable(beatSegmentNumber);
            self.contractorId = ko.observable(contractorId);
            self.lat = ko.observable(lat);
            self.lon = ko.observable(lon);
            self.speed = ko.observable(speed);
            self.lastUpdate = ko.observable(lastUpdate + ' seconds ago');
            self.lastMessage = ko.observable(lastMessage);

            //hold reference to map marker
            self.mapMarker;

            self.update = function (vehicleStateIconUrl, vehicleState, heading, beatNumber, beatSegmentNumber, contractorId, lat, lon, speed, lastUpdate, lastMessage) {

                self.vehicleStateIconUrl = ko.observable(TRUCK_IMAGE_BASE_URL + '/' + vehicleStateIconUrl);
                self.vehicleState(vehicleState);
                self.heading(heading);
                self.beatId(beatId);
                self.contractorId(contractorId);
                self.lat(lat);
                self.lon(lon);
                self.speed(speed);
                self.lastUpdate(lastUpdate + ' seconds ago');
                self.lastMessage(lastMessage);

            };

            //row click
            self.showDetails = function (item) {
                try {

                    root.selectedTowTruck(item);

                    //var position = new google.maps.LatLng(item.lat(), item.lon());
                    //map.setCenter(position);

                    //if (infowindow) infowindow.close();

                    //infowindow.setPosition(position);
                    //infowindow.maxWidth = 600;
                    //infowindow.setContent($("#infoWindowContent").html());

                    //infowindow.open(map, self.mapMarker);

                    $("#gridTruckDetailsModal").modal('show');


                } catch (e) {

                }
            }

            //marker hover
            self.showInfoWindow = function (item) {
                try {

                    root.selectedTowTruck(item);

                    var position = new google.maps.LatLng(item.lat(), item.lon());
                   
                    if (infowindow) infowindow.close();

                    infowindow.setPosition(position);
                    infowindow.maxWidth = 600;
                    infowindow.setContent($("#infoWindowContent").html());

                    infowindow.open(map, self.mapMarker);

                } catch (e) {

                }
            }

            //marker click
            self.showInfoWindowAndZoomToTruck = function (item) {
                try {

                    root.selectedTowTruck(item);

                    var position = new google.maps.LatLng(item.lat(), item.lon());
                    map.setCenter(position);
                    map.setZoom(15);

                    if (infowindow) infowindow.close();

                    infowindow.setPosition(position);
                    infowindow.maxWidth = 600;
                    infowindow.setContent($("#infoWindowContent").html());

                    infowindow.open(map, self.mapMarker);

                } catch (e) {

                }
            }

            //row click
            self.hideInfoWindow = function () {
                try {                   
                    if (infowindow) infowindow.close();

                } catch (e) {

                }
            }

            self.follow = function (item) {

                map.setZoom(15);
                map.setCenter(new google.maps.LatLng(item.lat(), item.lon()));

                root.followingTruck(true);
                root.unFollowText('Stop following ' + item.truckId);

            };

        };

        //add or update truck object
        self.addOrUpdate = function (truckId, vehicleStateIconUrl, vehicleState, heading, beatNumber, beatSegmentNumber, contractorId, lat, lon, speed, lastUpdate, lastMessage) {
            var currentTruck = ko.utils.arrayFirst(self.towTrucks(), function (i) { return i.truckId === truckId; });

            if (currentTruck) {
                currentTruck.update(vehicleStateIconUrl, vehicleState, heading, beatNumber, beatSegmentNumber, contractorId, lat, lon, speed, lastUpdate, lastMessage);
                $("#grid-" + truckId).flash('255,216,0', 2000);

                //update selected truck            
                if (self.selectedTowTruck != undefined) {
                    if (currentTruck.truckId === self.selectedTowTruck().truckId) {

                        self.selectedTowTruck(currentTruck);

                        ////update info window
                        //if (infowindow) {
                        //    infowindow.setContent($("#infoWindowContent").html());
                        //}

                        //if following 
                        if (self.followingTruck() === true) {
                            map.setCenter(new google.maps.LatLng(currentTruck.lat(), currentTruck.lon()));
                        }

                    }
                }
            }
            else {
                self.towTrucks.push(new uiTruckItem(self, truckId, vehicleStateIconUrl, vehicleState, heading, beatNumber, beatSegmentNumber, contractorId, lat, lon, speed, lastUpdate, lastMessage));
                $("#grid-" + truckId).flash('154,240,117', 2000);

            }

            $("#gridNumberOfRows").text(self.towTrucks().length + ' trucks');
        };

        //update last update time. Nothing else has changed for this truck
        self.updateLastUpdateTime = function (truckId, lastUpdate, lastMessage) {

            var currentTruck = ko.utils.arrayFirst(self.towTrucks(), function (i) { return i.truckId === truckId; });
            if (currentTruck) {
                //update lmt in grid
                currentTruck.lastUpdate(lastUpdate + ' seconds ago');

                //update selected truck            
                if (self.selectedTowTruck != undefined) {
                    if (currentTruck.truckId === self.selectedTowTruck().truckId) {
                        self.selectedTowTruck().lastUpdate(lastUpdate + ' seconds ago');

                        ////update info window
                        //if (infowindow) {
                        //    infowindow.setContent($("#infoWindowContent").html());
                        //}

                    }
                }

            }
        };

        //remove truck
        self.remove = function (truckId) {
            var currentTruck = ko.utils.arrayFirst(self.towTrucks(), function (i) { return i.truckId === truckId; });
            if (currentTruck) {
                $("#grid-" + truckId).flash('255,148,148', 2000);
                self.towTrucks.remove(function (item) { return item.truckId === truckId; });
            }
            $("#gridNumberOfRows").text(self.towTrucks().length + ' trucks');
        };

        //unfollow truck
        self.unFollow = function () {

            try {
                //center map back to original
                map.setCenter(defaultMapLocation);
                map.setZoom(DEFAULT_MAP_ZOOM);

                self.unFollowText('Stop following');
                self.followingTruck(false);

            } catch (e) {

            }

        };


        self.showFilter = function () {
            
            $("#filterModal").modal('show');
            $("#beatsFilter").focus();

        };
        //=======================================================================================

        function uiIncidentType(root, incidentTypeId, incidentTypeName, incidentTypeCode) {
            var self = this;

            self.incidentTypeId = incidentTypeId;
            self.incidentTypeName = ko.observable(incidentTypeName);
            self.incidentTypeCode = ko.observable(incidentTypeCode);
        };

        self.incidentTypes = ko.observableArray([
            new uiIncidentType(self, 1, 'Test', 'Test Code'),
            new uiIncidentType(self, 2, 'Test 2', 'Test2 Code')
        ]);

    };

    viewModel = new MyViewModel();
      
    ko.applyBindings(viewModel);
   
    //hub methods
    var hub = $.connection.towTruck;

    hub.addOrUpdateTruck = function (item) {
        viewModel.addOrUpdate(item.TruckId, item.VehicleStateIconUrl, item.VehicleState, item.Heading, item.BeatNumber, item.BeatSegmentNumber, item.ContractorId, item.Lat, item.Lon, item.Speed, item.LastUpdate, item.LastMessage);
    };

    hub.updateLastUpdateTime = function (truckId, lastUpdate, lastMessage) {
        viewModel.updateLastUpdateTime(truckId, lastUpdate, lastMessage);
    };

    hub.deleteTruck = function (truckId) {
        viewModel.remove(truckId);

        //remove truck from map
        try {
            if (markersArray != null && markersArray.length > 0) {
                for (var i = 0; i < markersArray.length; i++) {

                    if (markersArray[i].id === truckId) {
                        markersArray[i].setMap(null);
                    }
                }
            }
        } catch (e) {

        }

    };

    $.connection.hub.start().done(function(){      
        //new
        var url = SERVICE_BASE_URL + '/Map/StartTruckUpdate'
        $.get(url,
            function (isAdmin) {
            }, "json");

    });




    // A simple background color flash effect that uses jQuery Color plugin
    jQuery.fn.flash = function (color, duration) {
        var current = this.css('backgroundColor');
        this.animate({ backgroundColor: 'rgb(' + color + ')' }, duration / 2)
            .animate({ backgroundColor: current }, duration / 2);
    }

    ko.bindingHandlers.map = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {


            var lat = allBindingsAccessor().latitude();
            var lon = allBindingsAccessor().longitude();
            var truckId = allBindingsAccessor().truckId;
            var vehicleStateIconUrl = allBindingsAccessor().vehicleStateIconUrl();
            var heading = allBindingsAccessor().heading();
            //var map = allBindingsAccessor().map;

            var position = new google.maps.LatLng(lat, lon);

            var truckMapItem = "<div style='z-index:1000;'><img id='" + truckId + "' src='" + vehicleStateIconUrl + "' alt=''height='9px' /><div><span style='color: blue; font-size:20px; width:50px; font-weight:bold;'>" + truckId + "</span></div></div>";
            var left = "-15px";

            var marker = new MarkerWithLabel({
                id: truckId,
                map: map,
                position: position,
                draggable: false,
                labelText: truckMapItem,
                labelClass: "labels", // the CSS class for the label       
                labelStyle: { top: "2px", left: left, opacity: 0.95, zindex: 1000 },
                labelVisible: true,
                title: truckId.toString()
            });

            //var image = 'Content/Images/NIS.png';
            //var marker = new new google.maps.Marker({
            //    id: truckId,
            //    icon: image,
            //    map: map,
            //    position: position,
            //    title: truckId.toString()
            //});


            //marker click
            google.maps.event.addListener(marker, 'click', function () {
                viewModel.showInfoWindowAndZoomToTruck(viewModel);
            });
            //marker click
            google.maps.event.addListener(marker, 'mouseover', function () {
                viewModel.showInfoWindow(viewModel);
            });
            //marker click
            google.maps.event.addListener(marker, 'mouseout', function () {
                //viewModel.hideInfoWindow();
            });

            markersArray.push(marker);
            viewModel.mapMarker = marker;

            //rotate
            $("#" + truckId).rotate(heading - 90);


        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {

            var lat = allBindingsAccessor().latitude();
            var lon = allBindingsAccessor().longitude();
            var truckId = allBindingsAccessor().truckId;
            //var map = allBindingsAccessor().map;
            var vehicleStateIconUrl = allBindingsAccessor().vehicleStateIconUrl();
            var heading = allBindingsAccessor().heading();

            var latlng = new google.maps.LatLng(lat, lon);

            viewModel.mapMarker.setPosition(latlng);

            //rotate
            $("#" + truckId).rotate(heading - 90);

        }
    };

    //this is a work-around because knockoutjs's click binding does not seem to work inside google map infowindow
    function FollowSelectedTruck() {
        viewModel.selectedTowTruck().follow(viewModel.selectedTowTruck());
    }
             
});


