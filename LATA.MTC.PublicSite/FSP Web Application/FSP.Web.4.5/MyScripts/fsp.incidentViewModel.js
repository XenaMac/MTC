/// <reference path="../Scripts/knockout-2.1.0.js" />
/// <reference path="fsp.constructor.js" />
/// <reference path="fsp.truckCollection.js" />

lata.FspWeb.prototype.incidentViewModel = function () {

    var self = this;

    self.isSorting = false;
    self.currentSortDirection = ko.observable("Asc");
    self.canSendTrucks = ko.observable(false);
    self.currentSort = ko.observable("beatNumber");
    self.incidents = ko.observableArray([]);
    self.numberOfDispatchedAndAcked = ko.computed(function () {

        var retValue = 0;

        for (var i = 0; i < incidents().length; i++) {
            if (incidents()[i].isAcked() === false) {
                retValue++;
            }
        }
        return retValue;
    });
    self.numberOfActive = ko.computed(function () {

        var retValue = 0;

        for (var i = 0; i < incidents().length; i++) {
            if (incidents()[i].isAcked() === true && incidents()[i].isIncidentComplete() === false) {
                retValue++;
            }
        }
        return retValue;
    });
    self.numberOfCompleted = ko.computed(function () {

        var retValue = 0;

        for (var i = 0; i < incidents().length; i++) {
            if (incidents()[i].isIncidentComplete() === true) {
                retValue++;
            }
        }
        return retValue;
    });

    var init = function () {

        try {

            self.getIncidents();


            setTimeout(function updateIncidentsTimer() {
                console.log('Calling Server For New Incidents');
                self.getIncidents();
                setTimeout(updateIncidentsTimer, 10000);
            }, 10000);


            //var int = setInterval(function () {
            //    self.getIncidents();
            //}, 30000);

        } catch (e) {
            console.error('error get incidents %s ', e);
        }

    }

    self.columns = ko.observableArray([
         new column(self, "Beat #", "beatNumber", true),
         new column(self, "Truck #", "truckNumber", true),
         new column(self, "Driver", "driverName", true),
         new column(self, "Dispatch Summary Message", "dispatchComments", true),
         new column(self, "Time", "timeStamp", true),
         new column(self, "Dispatch #", "dispatchNumber", true),
         new column(self, "State", "state", false)
    ]);

    self.doSort = function () {

        self.isSorting = true;


        if (currentSort() === 'truckNumber') {
            if (currentSortDirection() === 'Asc')
                self.incidents.sort(function (left, right) { return left.truckNumber == right.truckNumber ? 0 : (left.truckNumber < right.truckNumber ? -1 : 1) })
            else
                self.incidents.sort(function (left, right) { return left.truckNumber == right.truckNumber ? 0 : (left.truckNumber > right.truckNumber ? -1 : 1) })
        }
        else if (currentSort() === 'beatNumber') {
            if (currentSortDirection() === 'Asc')
                self.incidents.sort(function (left, right) { return left.beatNumber() == right.beatNumber() ? 0 : (left.beatNumber() < right.beatNumber() ? -1 : 1) })
            else
                self.incidents.sort(function (left, right) { return left.beatNumber() == right.beatNumber() ? 0 : (left.beatNumber() > right.beatNumber() ? -1 : 1) })
        }
        else if (currentSort() === 'driverName') {
            if (currentSortDirection() === 'Asc')
                self.incidents.sort(function (left, right) { return left.driverName() == right.driverName() ? 0 : (left.driverName() < right.driverName() ? -1 : 1) })
            else
                self.incidents.sort(function (left, right) { return left.driverName() == right.driverName() ? 0 : (left.driverName() > right.driverName() ? -1 : 1) })
        }
        else if (currentSort() === 'dispatchComments') {
            if (currentSortDirection() === 'Asc')
                self.incidents.sort(function (left, right) { return left.dispatchComments() == right.dispatchComments() ? 0 : (left.dispatchComments() < right.dispatchComments() ? -1 : 1) })
            else
                self.incidents.sort(function (left, right) { return left.dispatchComments() == right.dispatchComments() ? 0 : (left.dispatchComments() > right.dispatchComments() ? -1 : 1) })
        }
        else if (currentSort() === 'timestamp') {
            if (currentSortDirection() === 'Asc')
                self.incidents.sort(function (left, right) { return left.timestamp() == right.timestamp() ? 0 : (left.timestamp() < right.timestamp() ? -1 : 1) })
            else
                self.incidents.sort(function (left, right) { return left.timestamp() == right.timestamp() ? 0 : (left.timestamp() > right.timestamp() ? -1 : 1) })
        }
        else if (currentSort() === 'state') {
            if (currentSortDirection() === 'Asc')
                self.incidents.sort(function (left, right) { return left.state() == right.state() ? 0 : (left.state() < right.state() ? -1 : 1) })
            else
                self.incidents.sort(function (left, right) { return left.state() == right.state() ? 0 : (left.state() > right.state() ? -1 : 1) })
        }
        else if (currentSort() === 'dispatchNumber') {
            if (currentSortDirection() === 'Asc')
                self.incidents.sort(function (left, right) { return left.dispatchNumber() == right.dispatchNumber() ? 0 : (left.dispatchNumber() < right.dispatchNumber() ? -1 : 1) })
            else
                self.incidents.sort(function (left, right) { return left.dispatchNumber() == right.dispatchNumber() ? 0 : (left.dispatchNumber() > right.dispatchNumber() ? -1 : 1) })
        }


        self.isSorting = false;

    }

    self.getIncidents = function () {

        var url = fspWeb.SERVICE_BASE_URL + "/Incident/GetIncidents";

        $.ajax({
            url: url,
            type: "GET",
            dataType: "json",
            error: function (xhr, ajaxOptions, thrownError) {
                console.error(xhr);
            },
            success: function (dbIncidents) {


                self.incidents([]);

                for (var i = 0; i < dbIncidents.length; i++) {
                    try {

                        self.addOrUpdateIncident(dbIncidents[i]);

                    } catch (e) {

                    }

                }
            }
        });

    }

    self.addOrUpdateIncident = function (dbIncident) {

        try {

            var currentIncident = ko.utils.arrayFirst(self.incidents(), function (i) { return i.incidentID === dbIncident.IncidentID; });

            if (currentIncident) {
                currentIncident.update(dbIncident);
            }
            else {
                self.incidents.push(new incident(self, dbIncident));
            }

        } catch (e) {
            Console.error(e);
        }
    }

    //show Config
    self.showConfig = function (item) {
        try {

            $("#configModal").modal('show');

        } catch (e) {

        }
    }

    function incident(root, dbIncident) {

        var self = this;

        self.incidentID = dbIncident.IncidentID;
        self.incidentNumber = ko.observable(dbIncident.IncidentNumber);
        self.beatNumber = ko.observable(dbIncident.BeatNumber);
        self.truckNumber = ko.observable(dbIncident.TruckNumber);
        self.driverName = ko.observable(dbIncident.DriverName);
        self.dispatchComments = ko.observable(dbIncident.DispatchComments);
        self.timeStamp = ko.observable(dbIncident.Timestamp);
        self.state = ko.observable(dbIncident.State);
        self.dispatchNumber = ko.observable(dbIncident.DispatchNumber);
        self.isIncidentComplete = ko.observable(dbIncident.IsIncidentComplete);
        self.isAcked = ko.observable(dbIncident.IsAcked);

        self.update = function (dbIncident) {

            self.incidentNumber(dbIncident.IncidentNumber);
            self.beatNumber(dbIncident.BeatNumber);
            self.truckNumber(dbIncident.TruckNumber);
            self.driverName(dbIncident.DriverName);
            self.dispatchComments(dbIncident.DispatchComments);
            self.timeStamp(dbIncident.Timestamp);
            self.state(dbIncident.State);
            self.dispatchNumber(dbIncident.DispatchNumber);

        };
    }

    //column
    function column(root, name, value, isVisible) {
        var self = this;

        self.name = ko.observable(name);
        self.value = ko.observable(value);
        self.isVisible = ko.observable(isVisible);

        self.sort = function (item) {

            root.currentSort(item.value());

            if (root.currentSortDirection() === 'Asc')
                root.currentSortDirection('Desc');
            else
                root.currentSortDirection('Asc');

            doSort();

        };
    }

    return {
        init: init
    };


}();

$(function () {

    fspWeb = new lata.FspWeb();

    //apply binding to ko
    ko.applyBindings(fspWeb);

    fspWeb.incidentViewModel.init();

});


