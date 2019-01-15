/// <reference path="fsp.constructor.js" />
/// <reference path="fsp.truckCollection.js" />


lata.FspWeb.prototype.mapViewModel = function () {

    var self = this;

    var DEFAULT_MAP_CENTER_LAT = 33.6600;
    var DEFAULT_MAP_CENTER_LON = -117.7900;
    var DEFAULT_MAP_ZOOM = 11;
    var defaultMapLocation;
    var currentMapLocation;

    var infowindow = null;
    self.markersArray = [];

    var beatSegmentPolygons = [];
    var beatSegmentsLabels = [];

    var beatPolygons = [];
    var beatLabels = [];

    var callBoxArray = [];
    var dropZoneArray = [];

    //observable UI Properties
    unFollowText = ko.observable('Stop following');
    followingTruck = ko.observable(false);
    beatsVisible = ko.observable(true);
    beatsLabelsVisible = ko.observable(true);
    beatSegmentsVisible = ko.observable(true);
    beatSegmentsLabelsVisible = ko.observable(true);
    callBoxesVisible = ko.observable(true);
    dropZonesVisible = ko.observable(true);

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
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            //init infowindow
            infowindow = new google.maps.InfoWindow({
                content: '',
                position: defaultMapLocation
            });

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
                    if (fspWeb.map.zoom > 12)
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

                            //var beatLabel = new Label({
                            //    map: fspWeb.map,
                            //    position: labelPosition,
                            //    text: '123'
                            //});


                            var beatLabelContent = customTxt = "<div style='z-index:1'>" + number + "</div>";

                            var beatLabel = new TxtOverlay(
                                number,
                                labelPosition,
                                beatLabelContent,
                                description,
                                fspWeb.map,
                                "visible",
                                "position: absolute; border: 1px solid black; border-radius: 3px; background: " + color + "; padding: 3px; white-space: nowrap; z-index:1; opacity: 0.9 "
                            );

                            //var beatLabel = new TxtOverlay(
                            //{
                            //    id: number,
                            //    map: fspWeb.map,
                            //    position: labelPosition,
                            //    text: number,
                            //    color: color,
                            //    desc: description
                            //});


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

                        } catch (e) {

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

                            var position = new google.maps.LatLng(lat, lon);

                            var image = fspWeb.SERVICE_BASE_URL + '/content/images/callBox.png';
                            var callBox = new google.maps.Marker({
                                position: position,
                                map: fspWeb.map,
                                icon: image
                            });

                            callBoxArray.push(callBox);

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

                         } catch (e) {

                         }

                     }
                 }
             });
         } catch (e) {

         }
     },
    unFollowTruck = function () {

        try {
            ////center map back to original
            //map.setCenter(defaultMapLocation);
            //map.setZoom(DEFAULT_MAP_ZOOM);

            //self.unFollowText('Stop following');
            //self.followingTruck(false);

        } catch (e) {

        }

    };
    showFilter = function () {
        $("#filterModal").modal('show');
        $("#beatsFilter").focus();
    }
    showTruckList = function () {
        try {
            var url = fspWeb.SERVICE_BASE_URL + "/Truck/Grid";;
            var windowName = "Tow Truck Grid";
            var windowSize = 'width=1000,height=800';
            window.open(url, '', windowSize);
            event.preventDefault();
        } catch (e) {

        }

    }
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
    showInfoWindow = function (item) {
        try {

            fspWeb.selectedTruck(item);

            var position = new google.maps.LatLng(item.lat(), item.lon());

            if (infowindow) infowindow.close();

            infowindow.setPosition(position);
            infowindow.maxWidth = 600;
            infowindow.setContent($("#infoWindowContent").html());

            infowindow.open(fspWeb.map, item.mapMarker);

        } catch (e) {

        }
    },
    showInfoWindowAndZoomToTruck = function (item) {
        try {

            fspWeb.selectedTruck(item);

            var position = new google.maps.LatLng(item.lat(), item.lon());
            fspWeb.map.setCenter(position);
            fspWeb.map.setZoom(15);

            if (infowindow) infowindow.close();

            infowindow.setPosition(position);
            infowindow.maxWidth = 600;
            infowindow.setContent($("#infoWindowContent").html());

            infowindow.open(fwpWeb.map, item.mapMarker);

        } catch (e) {

        }
    },
    beatsVisible.subscribe(function () {
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
    }),
    beatsLabelsVisible.subscribe(function () {
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
    }),
    beatSegmentsVisible.subscribe(function () {
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
    }),
    beatSegmentsLabelsVisible.subscribe(function () {
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
    }),
    callBoxesVisible.subscribe(function () {
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

    }),
    dropZonesVisible.subscribe(function () {
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

     })

    //public implementation (object literals)
    return {
        initMap: initMap,
        zoomToBeat: zoomToBeat
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
    $("#beatsFilter").autocomplete({
        source: function (request, response) {
            var url = fspWeb.SERVICE_BASE_URL + "/Truck/GetBeatNumbers";
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
            fspWeb.mapViewModel.zoomToBeat(ui.item.value);
        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });

});


//binding trucks to map
ko.bindingHandlers.map = {
    init: function (element, valueAccessor, allBindingsAccessor, truck) {

        try {

            var lat = allBindingsAccessor().lat();
            var lon = allBindingsAccessor().lon();
            var truckNumber = allBindingsAccessor().truckNumber;
            var beatNumber = allBindingsAccessor().beatNumber();
            var vehicleStateIconUrl = allBindingsAccessor().vehicleStateIconUrl();
            var heading = allBindingsAccessor().heading();

            var position = new google.maps.LatLng(lat, lon);

            var truckMapItem = "<div style='position: relative; left: -25%; z-index:2000;'><img id='" + truckNumber + "' src='" + vehicleStateIconUrl + "' alt='' height='12px' /><div>";
            truckMapItem += "<span  style='color: blue; font-size:12px; font-weight:bold; white-space: nowrap;'>";
            truckMapItem += beatNumber;
            truckMapItem += "</span></div></div>";
          
            var marker = new MarkerWithLabel({
                id: truckNumber,
                map: fspWeb.map,
                position: position,
                draggable: false,
                labelText: truckMapItem,
                labelVisible: true,
                title: beatNumber.toString(),
                //zIndex: google.maps.Marker.MAX_ZINDEX + 1
            });

            //var marker = new google.maps.Marker({
            //    position: position,
            //    map: fspWeb.map,
            //    title: beatNumber.toString(),
            //    zIndex: 6000
            //});

            //marker click
            google.maps.event.addListener(marker, 'click', function () {
                self.showInfoWindowAndZoomToTruck(truck);
            });
            //marker click
            google.maps.event.addListener(marker, 'mouseover', function () {
                self.showInfoWindow(truck);
            });
            //marker click
            google.maps.event.addListener(marker, 'mouseout', function () {
                //viewModel.hideInfoWindow();
            });

            //add truck marker to main list 
            self.markersArray.push(marker);

            //associated truck marker with truck object in truck list
            truck.mapMarker = marker;

            //rotate truck marker icon
            $("#" + truckNumber).rotate(heading - 90);

        } catch (e) {

        }

    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {

        try {
            var lat = allBindingsAccessor().lat();
            var lon = allBindingsAccessor().lon();
            var truckNumber = allBindingsAccessor().truckNumber;
            var beatNumber = allBindingsAccessor().beatNumber;
            var vehicleStateIconUrl = allBindingsAccessor().vehicleStateIconUrl();
            var heading = allBindingsAccessor().heading();

            var latlng = new google.maps.LatLng(lat, lon);

            fspWeb.mapViewModel.mapMarker.setPosition(latlng);
            //fspWeb.mapViewModel.mapMarker.setZIndex(google.maps.Marker.MAX_ZINDEX + 1);

            //rotate
            $("#" + truckNumber).rotate(heading - 90);
        } catch (e) {

        }


    }
};





