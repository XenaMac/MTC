/// <reference path="../Testing/ELabel.js" />
var map;

var SERVICE_BASE_URL = $("#websitePath").attr("data-websitePath");
var TRUCK_IMAGE_BASE_URL = $("#websitePath").attr("data-websitePath") + "/Content/Images/";
var DEFAULT_MAP_CENTER_LAT = 33.6600;
var DEFAULT_MAP_CENTER_LON = -117.7927;
var DEFAULT_MAP_ZOOM = 10;
var defaultMapLocation;
var currentMapLocation;

$(function () {

    initializeGoogleMap();


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
            google.maps.event.trigger(map, "resize");

            createLabel(defaultMapLocation);


        } catch (e) {
            alert('error geo map ' + e);
        }

    }

    function createLabel(location) {

        //marker = new google.maps.Marker({
        //    position: location,
        //    map: map
        //});

        //var label = new Label({
        //    map: map
        //});
        //label.bindTo('position', location);
        //label.bindTo('text', marker, 'position');
        //label.bindTo('visible', marker);
        //label.bindTo('clickable', marker);
        //label.bindTo('zIndex', marker);

        customTxt = "<div class='beatLabel'>Tolga</div>";

        //var label = new TxtOverlay(location, customTxt, "customBox", map);      
        var label = new TxtOverlay(
            '123',
            location,
            customTxt,
            "Red",
            map
        );

        //marker click
        google.maps.event.addListener(label, 'click', function () {

            alert(label.id);

        });

        //var label = new Label(location, customTxt, "customBox", map);

    };
});