var map;
var infowindow;
var DEFAULT_MAP_CENTER_LAT = 33.6600;
var DEFAULT_MAP_CENTER_LON = -117.7900;
var DEFAULT_MAP_CENTER_LAT2 = 33.6800;
var DEFAULT_MAP_CENTER_LON2 = -117.8100;
var DEFAULT_MAP_ZOOM = 11;
var defaultMapLocation;
var marker;
var marker2;

function init() {

    defaultMapLocation = new google.maps.LatLng(DEFAULT_MAP_CENTER_LAT, DEFAULT_MAP_CENTER_LON);

    //google map configurations
    var mapOptions = {
        center: defaultMapLocation,
        zoom: DEFAULT_MAP_ZOOM,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    //init infowindow
    infowindow = new google.maps.InfoWindow({
        content: '',
        position: defaultMapLocation,
        disableAutoPan: true
    });

    map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);
    google.maps.event.trigger(map, "resize");
}

function addGoogleMarker(title, position) {

    var imageIcon = new google.maps.MarkerImage('OnAssist.png');

    //marker2 = new google.maps.Marker({
    //    id: 'myMarker',
    //    position: position,
    //    map: map,
    //    title: title,
    //    icon: imageIcon
    //    //icon: "<img id='myMarker' src='OnAssist.png' />"
    //    //icon: 'OnAssist.png',
    //    //icon: {
    //    //    path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW,
    //    //    //url: 'OnAssist.png',
    //    //    rotation: 12,
    //    //    scale: 5
    //    //}
    //});
    
    var html = "<div style='background-color:Red'><img id='myMarker' src='OnAssist.png' /><span style='font-size: 12pt; color:White; padding:10px'>OC-716</span></div>";

    marker = new RichMarker({
      
        map: map,
        position: position,
        draggable: false,
        flat: true,
        anchor: RichMarkerPosition.MIDDLE,
        content: html
    });

    //show info window
    google.maps.event.addListener(marker, 'rightclick', function () {
        alert('right click');
        //rotate truck marker icon
        $("#myMarker").rotate(60);
    });

    //show info window
    google.maps.event.addListener(marker, 'dblclick', function () {
        alert('dbl click');
        //rotate truck marker icon
        $("#myMarker").rotate(60);
    });

    //rotate truck marker icon
    $("#myMarker").rotate(60);

}

function addGoogleMarkerImage(title, position) {

    marker = new google.maps.MarkerImage({
        id: 'myMarker',
        anchor: position,
        map: map,
        url: 'OnAssist.png'
    });


    //show info window
    google.maps.event.addListener(marker, 'rightclick', function () {
        alert(this.id);
        //rotate truck marker icon
        $("#myMarker").rotate(60);
    });

    //show info window
    google.maps.event.addListener(marker, 'dblclick', function () {
        alert(this.id);
        //rotate truck marker icon
        $("#myMarker").rotate(60);
    });

    //rotate truck marker icon
    $("#myMarker").rotate(60);

}

function addCustomMarker(title, position) {

    var truckMapItem = "<div style='position: relative; left: -25%; z-index:2000;'>";
    truckMapItem += "<img id='myMarker' src='OnAssist.png' alt='' height='12px' /><div>";
    truckMapItem += "<span  style='color: blue; font-size:12px; font-weight:bold; white-space: nowrap;'>";
    truckMapItem += title;
    truckMapItem += "</span></div></div>";

    marker = new MarkerWithLabel({
        map: map,
        position: position,
        draggable: false,
        labelText: truckMapItem,
        labelVisible: true,
        title: title
    });

    //show info window
    google.maps.event.addListener(marker, 'rightclick', function () {
        alert(this.id);
        //rotate truck marker icon
        $("#myMarker").rotate(60);
    });

    //show info window
    google.maps.event.addListener(marker, 'dblclick', function () {
        alert(this.id);
        //rotate truck marker icon
        $("#myMarker").rotate(60);
    });

    //rotate truck marker icon
    $("#myMarker").rotate(60);

}

function addCustomMarker2(title, position) {

    var postion = new google.maps.LatLng(DEFAULT_MAP_CENTER_LAT, DEFAULT_MAP_CENTER_LON);
    marker = new TruckMarkerOverlay("myMarker", postion, 'OnAssist.png', map, 60);

    //show info window
    google.maps.event.addListener(marker, 'rightclick', function () {
        alert(this.id);
        //rotate truck marker icon
        $("#myMarker").rotate(60);
    });

    //show info window
    google.maps.event.addListener(marker, 'dblclick', function () {
        alert(this.id);
        //rotate truck marker icon
        $("#myMarker").rotate(60);
    });

    //rotate truck marker icon
    $("#myMarker").rotate(60);

}

$(function () {

    init();

    var position = new google.maps.LatLng(DEFAULT_MAP_CENTER_LAT, DEFAULT_MAP_CENTER_LON);

    addGoogleMarker('test', position);

    //$("#img_truck").rotate(40);

    $("#btn_rotate").click(function () {

        var position = new google.maps.LatLng(DEFAULT_MAP_CENTER_LAT2, DEFAULT_MAP_CENTER_LON2);
        marker.setPosition(position);

        //rotate truck marker icon
        $("#myMarker").rotate(60);

        //var icon = new google.maps.Symbol({
        //    path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW,
        //    url: 'OnBreak.png',
        //    rotation: 45,
        //    scale: 10
        //});

        //marker.setIcon(icon);
        //marker.setStatus('OnBreak.png');

    });

    var imgElements = document.getElementsByTagName('img'),
    i = 0;
    for (i = 0; i < imgElements.length; i += 1) {
        alert(imgElements[i].src);
    }


});


