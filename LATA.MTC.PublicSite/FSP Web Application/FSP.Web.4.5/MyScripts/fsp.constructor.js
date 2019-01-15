/// <reference path="../Scripts/knockout-2.2.1.js" />


//namespace
var lata = lata || {};
var fspWeb;

lata.FspWeb = function () {


    var thisFsp = this;

    test = ko.observable(false);

    thisFsp.SERVICE_BASE_URL = $("#websitePath").attr("data-websitePath");
    thisFsp.TRUCK_IMAGE_BASE_URL = $("#websitePath").attr("data-websitePath") + "/Images/";

    thisFsp.userId = '';
    thisFsp.map = null;
    thisFsp.trucks = ko.observableArray([]);
    thisFsp.availableTrucks = ko.computed(function () {
        return ko.utils.arrayFilter(thisFsp.trucks(), function (i) {
            return (i.vehicleState() === "On Patrol");
        });
    });
    thisFsp.loggedOnTrucks = ko.computed(function () {
        return ko.utils.arrayFilter(thisFsp.trucks(), function (i) {
            return (i.vehicleState() != "On Patrol" && i.vehicleState() != "Waiting for Driver Login");
        });
    });
    thisFsp.notLoggedOnTrucks = ko.computed(function () {
        return ko.utils.arrayFilter(thisFsp.trucks(), function (i) {
            return (i.vehicleState() === "Waiting for Driver Login");
        });
    });


    thisFsp.trucksChanged = ko.observable(false);
    thisFsp.selectedTrucks = ko.observableArray([]);
    thisFsp.selectedTruck = ko.observable(null);
    thisFsp.selectedId = ko.observable();
    thisFsp.selectedIdForDeletion = ko.observable();
    thisFsp.towTruckHub;


    //truck object
    thisFsp.truck = function truck(dbTruck) {
        var self = this;

        self.id = dbTruck.TruckNumber.replace(' ', '').replace('-', '').replace('_', '');
        self.truckNumber = dbTruck.TruckNumber;
        self.vehicleStateIconUrl = ko.observable(fspWeb.TRUCK_IMAGE_BASE_URL + dbTruck.VehicleStateIconUrl);
        self.vehicleState = ko.observable(dbTruck.VehicleState);
        self.heading = ko.observable(dbTruck.Heading);
        self.headingText = ko.computed(function () {

            if (self.heading() > 315 && self.heading() <= 360)
                return 'North';
            else if (self.heading() >= 0 && self.heading() <= 45)
                return 'North';
            else if (self.heading() > 45 && self.heading() <= 135)
                return 'East';
            else if (self.heading() > 135 && self.heading() <= 225)
                return 'South';
            else if (self.heading() > 225 && self.heading() <= 315)
                return 'West';

        });
        self.beatNumber = ko.observable(dbTruck.BeatNumber.substring(dbTruck.BeatNumber.indexOf("-") + 1));
        self.beatNumberString = self.beatNumber();
        self.beatSegmentNumber = ko.observable(dbTruck.BeatSegmentNumber);
        self.contractorId = ko.observable(dbTruck.ContractorId);
        self.lat = ko.observable(dbTruck.Lat);
        self.lon = ko.observable(dbTruck.Lon);
        self.speed = ko.observable(dbTruck.Speed);
        self.driverName = ko.observable(dbTruck.DriverName);

        self.driverNameShort = ko.computed(function () {

            if (self.driverName().length < 12)
                return self.driverName();
            else
                return self.driverName().substring(0, 12) + '...';

        });


        self.contractorName = ko.observable(dbTruck.ContractorName);

        self.contractorNameShort = ko.computed(function () {

            if (self.contractorName().length < 12)
                return self.contractorName();
            else
                return self.contractorName().substring(0, 12) + '...';

        });

        self.location = ko.observable(dbTruck.Location);
        self.lastUpdate = ko.observable(dbTruck.LastUpdate + ' seconds ago');
        self.lastMessage = ko.observable(dbTruck.LastMessage);
        self.lastStatusChanged = ko.observable(dbTruck.LastStatusChanged);

        self.speedingTime = ko.observable(dbTruck.SpeedingTime);
        self.speedingValue = ko.observable(dbTruck.SpeedingValue);
        self.outOfBoundsMessage = ko.observable(dbTruck.OutOfBoundsMessage);
        self.outOfBoundsTime = ko.observable(dbTruck.OutOfBoundsTime);
        self.hasAlarm = ko.observable(dbTruck.HasAlarm);

        ////hold reference to map marker
        self.mapMarker;

        self.update = function (dbTruck) {

            self.vehicleStateIconUrl(fspWeb.TRUCK_IMAGE_BASE_URL + dbTruck.VehicleStateIconUrl);
            self.vehicleState(dbTruck.VehicleState);
            self.heading(dbTruck.Heading);

            self.headingText = ko.computed(function () {

                if (self.heading() > 315 && self.heading() <= 360)
                    return 'North';
                else if (self.heading() >= 0 && self.heading() <= 45)
                    return 'North';
                else if (self.heading() > 45 && self.heading() <= 135)
                    return 'East';
                else if (self.heading() > 135 && self.heading() <= 225)
                    return 'South';
                else if (self.heading() > 225 && self.heading() <= 315)
                    return 'West';


            });


            self.beatNumber(dbTruck.BeatNumber.substring(dbTruck.BeatNumber.indexOf("-") + 1));
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
            self.lastStatusChanged(dbTruck.LastStatusChanged);


            self.speedingTime(dbTruck.SpeedingTime);
            self.speedingValue(dbTruck.SpeedingValue);
            self.outOfBoundsMessage(dbTruck.OutOfBoundsMessage);
            self.outOfBoundsTime(dbTruck.OutOfBoundsTime);
            self.hasAlarm(dbTruck.HasAlarm);
        };

    };

    //thisFsp.checkForAlerts = function () {
    //    var url = thisFsp.SERVICE_BASE_URL + '/Truck/HaveAlarms';
    //    $.get(url,
    //            function (value) {
    //                if (value === true) {
    //                    $("#alertScreenLink").attr('class', 'btn btn-danger');
    //                    $("#monitoringTab").css('color', 'red');
    //                }
    //                else {
    //                    $("#alertScreenLink").attr('class', 'btn btn-info');
    //                    $("#monitoringTab").css('color', '#999999');
    //                }

    //            }, "json");
    //}

    //thisFsp.checkForAlerts();
    //setTimeout(function checkForAlertsFunction() {
    //    console.log('Checking for Alerts');
    //    thisFsp.checkForAlerts();
    //    setTimeout(checkForAlertsFunction, 10000);
    //}, 10000);
}



