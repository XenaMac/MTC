/// <reference path="../../Scripts/knockout-2.1.0.js" />
$(function () {

      
    defaultMapLocation = new google.maps.LatLng(33.6600, -117.7900);
    defaultMapLocation2 = new google.maps.LatLng(33.6800, -117.8000);

    //google map configurations
    var myOptions = {
        center: defaultMapLocation,
        zoom: 10,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
   
    //initialize google map      
    var mapCanvas = document.getElementById('map');
    map = new google.maps.Map(mapCanvas, myOptions);
  
 
    //var beatLabel = new TxtOverlay(
    //    '123',
    //    defaultMapLocation2,
    //    '123',
    //    '123',
    //    map,
    //    "visible",
    //    "position: absolute; border: 1px solid black; border-radius: 3px; background:#efefef; padding: 3px; white-space: nowrap; z-index:1; opacity: 0.9 "
    //);

   // var beatLabel = new TxtOverlay(
   //    '123',
   //    defaultMapLocation2,
   //    '123',
   //    '123',
   //    map,
   //    "visible",
   //    "position: relative; left: -50%; top: -8px; white-space: nowrap; border: 1px solid blue;padding: 2px; background-color: white "
   //);

    var marker = new google.maps.Marker({
        position: defaultMapLocation,
        map: map,
        title: 'Marker',
        zIndex: 6000
    });

    var label = new Label({
        map: map,
        position: defaultMapLocation2,
        text: '123'
    });
    //label.bindTo('position', marker, 'position');
    //label.bindTo('text', marker, 'title');
       
         

});





