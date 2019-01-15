

//namespace
var lata = lata || {};
var fspWeb;

lata.FspWeb = function () {


    var thisFsp = this;

    thisFsp.SERVICE_BASE_URL = $("#websitePath").attr("data-websitePath");
    thisFsp.TRUCK_IMAGE_BASE_URL = $("#websitePath").attr("data-websitePath") + "/Content/Images/";

    thisFsp.map = null;
    thisFsp.trucks = ko.observableArray([]);
    thisFsp.selectedTrucks = ko.observableArray([]);
    thisFsp.selectedTruck = ko.observable(null);

    //truck object
    thisFsp.truck = function truck(dbTruck) {
        var self = this;

        self.truckNumber = dbTruck.TruckNumber;
        self.vehicleStateIconUrl = ko.observable(fspWeb.TRUCK_IMAGE_BASE_URL + '/' + dbTruck.VehicleStateIconUrl);
        self.vehicleState = ko.observable(dbTruck.VehicleState);
        self.heading = ko.observable(dbTruck.Heading);
        self.beatNumber = ko.observable(dbTruck.BeatNumber);
        self.beatSegmentNumber = ko.observable(dbTruck.BeatSegmentNumber);
        self.contractorId = ko.observable(dbTruck.ContractorId);
        self.lat = ko.observable(dbTruck.Lat);
        self.lon = ko.observable(dbTruck.Lon);
        self.speed = ko.observable(dbTruck.Speed);
        self.driverName = ko.observable(dbTruck.DriverName);
        self.contractorName = ko.observable(dbTruck.ContractorName);
        self.location = ko.observable(dbTruck.Location);
        self.lastUpdate = ko.observable(dbTruck.LastUpdate + ' seconds ago');
        self.lastMessage = ko.observable(dbTruck.LastMessage);
        self.background = ko.observable('');
       
        ////hold reference to map marker
        self.mapMarker;

        self.update = function (dbTruck) {

            self.vehicleStateIconUrl = ko.observable(fspWeb.TRUCK_IMAGE_BASE_URL + '/' + dbTruck.VehicleStateIconUrl);
            self.vehicleState(dbTruck.VehicleState);
            self.heading(dbTruck.Heading);
            self.beatNumber(dbTruck.BeatNumber);
            self.beatSegmentNumber(dbTruck.BeatSegmentNumber);
            self.contractorId(dbTruck.ContractorId);
            self.lat(dbTruck.Lat);
            self.lon(dbTruck.Lon);
            self.speed(dbTruck.Speed);
            self.lastUpdate(dbTruck.LastUpdate + ' seconds ago');
            self.lastMessage(dbTruck.LastMessage);
            self.driverName(dbTruck.DriverName);
            self.contractorName(dbTruck.ContractorName);
            self.location(dbTruck.Location);
            self.lastMessage(dbTruck.LastMessage);
            self.background = ko.observable('');
        };
      
        self.toggleTruckSelection = function (item) {

            var truckIsSelected = ko.utils.arrayFirst(fspWeb.selectedTrucks(), function (i) { return i.truckNumber === item.truckNumber; });
            if (!truckIsSelected)
                fspWeb.selectedTrucks.push(item);
            else
                fspWeb.selectedTrucks.remove(function (j) { return j.truckNumber === item.truckNumber; });

        }
        self.unSelectTruck = function (item) {

            var truckIsSelected = ko.utils.arrayFirst(fspWeb.selectedTrucks(), function (i) { return i.truckNumber === item.truckNumber; });
            if (truckIsSelected)
                fspWeb.selectedTrucks.remove(function (j) { return j.truckNumber === item.truckNumber; });          
        }

    };

}



