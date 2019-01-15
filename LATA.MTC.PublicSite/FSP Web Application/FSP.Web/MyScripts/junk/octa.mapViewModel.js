/// <reference path="../Scripts/knockout-2.1.0.js" />
/// <reference path="octa.truckCollection.js" />
/// <reference path="octa.constants.js" />

octa.FSP.prototype = {
    initializeMap: function () {
       
        //map variables       
        var DEFAULT_MAP_CENTER_LAT = 33.6600;
        var DEFAULT_MAP_CENTER_LON = -117.7927;
        var DEFAULT_MAP_ZOOM = 10;
        var defaultMapLocation;
        var currentMapLocation;


        try {            
            defaultMapLocation = new google.maps.LatLng(DEFAULT_MAP_CENTER_LAT, DEFAULT_MAP_CENTER_LON);

            //google map configurations
            var myOptions = {
                center: defaultMapLocation,
                zoom: DEFAULT_MAP_ZOOM,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            //initialize google map      
            var mapCanvas = document.getElementById('map');
            this.map = new google.maps.Map(mapCanvas, myOptions);


        } catch (e) {
            alert('error geo map ' + e);
        }
    }
}();





