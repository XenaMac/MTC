/// <reference path="../Scripts/knockout-2.1.0.js" />

//namespace
var octa = octa || {};

var fsp;

octa.FSP = function () {
    this.map = null;
    this.persons = ko.observableArray([]);
    this.persons1 = ko.observableArray([]);
    this.header = ko.observable('Testing Test');;
}

//octa.FSP.prototype = {
//    initializeMap: function () {

//        //map variables       
//        var DEFAULT_MAP_CENTER_LAT = 33.6600;
//        var DEFAULT_MAP_CENTER_LON = -117.7927;
//        var DEFAULT_MAP_ZOOM = 10;
//        var defaultMapLocation;
//        var currentMapLocation;


//        try {

//            //alert('geoMap');
//            defaultMapLocation = new google.maps.LatLng(DEFAULT_MAP_CENTER_LAT, DEFAULT_MAP_CENTER_LON);

//            //google map configurations
//            var myOptions = {
//                center: defaultMapLocation,
//                zoom: DEFAULT_MAP_ZOOM,
//                mapTypeId: google.maps.MapTypeId.ROADMAP
//            };

//            //initialize google map      
//            var mapCanvas = document.getElementById('map');
//            this.map = new google.maps.Map(mapCanvas, myOptions);


//        } catch (e) {
//            alert('error geo map ' + e);
//        }
//    },
//    setPersons: function () {
//    this.persons = ko.observableArray([
//          new person(self, "Tolga", "Koseoglu"),
//          new person(self, "Charli", "Koseoglu"),
//          new person(self, "Jen", "Koseoglu"),
//    ]);

//    function person(root, firstName, lastName) {

//        var self = this;

//        self.firstName = ko.observable(firstName);
//        self.lastName = ko.observable(lastName);

//        self.updatePerson = function (firstName, lastName) {

//            self.firstName(firstName);
//            self.lastName(lastName);

//        };

//    }
//}
//};

//$(function () {

//    try {
//        fsp = new octa.FSP();

//        fsp.initializeMap();
//        fsp.setPersons();

//        //apply binding to ko
//        ko.applyBindings(fsp);
//    } catch (e) {

//    }
//});



