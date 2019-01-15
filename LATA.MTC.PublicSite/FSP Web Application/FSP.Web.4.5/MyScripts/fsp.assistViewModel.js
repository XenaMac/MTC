/// <reference path="../Scripts/knockout-2.1.0.js" />
/// <reference path="fsp.constructor.js" />
/// <reference path="fsp.truckCollection.js" />

lata.FspWeb.prototype.assistViewModel = function () {

    var self = this;
    var init = function () {

       
        try {
                             
            self.getAssists();

            var int = setInterval(function () {
                self.getAssists();
            }, 30000);

        } catch (e) {
            alert('error get assists ' + e);
        }

    }



    self.isSorting = false;
    self.currentSortDirection = ko.observable("Asc");
    self.canSendTrucks = ko.observable(false);
    self.currentSort = ko.observable("beatNumber");

    self.columns = ko.observableArray([
         new column(self, "Beat #", "beatNumber", true),
         new column(self, "Dispatch #", "dispatchNumber", true),
         new column(self, "Assist #", "assistNumber", true),         
         new column(self, "Driver", "driver", true),
         new column(self, "Make", "make", true),
         new column(self, "Color", "color", true),
         new column(self, "Drop Zone", "dropZone", false),
         new column(self, "Plate #", "plateNumber", false),
         new column(self, "Driver Comments", "driverComments", false),
    ]);

    //show Config
    self.showConfig = function (item) {
        try {

            $("#configModal").modal('show');

        } catch (e) {

        }
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

    doSort = function () {

        self.isSorting = true;


        if (currentSort() === 'beatNumber') {
            if (currentSortDirection() === 'Asc')
                self.assists.sort(function (left, right) { return left.beatNumber() == right.beatNumber() ? 0 : (left.beatNumber() < right.beatNumber() ? -1 : 1) })
            else
                self.assists.sort(function (left, right) { return left.beatNumber() == right.beatNumber() ? 0 : (left.beatNumber() > right.beatNumber() ? -1 : 1) })
        }
        else if (currentSort() === 'dispatchNumber') {
            if (currentSortDirection() === 'Asc')
                self.assists.sort(function (left, right) { return left.dispatchNumber() == right.dispatchNumber() ? 0 : (left.dispatchNumber() < right.dispatchNumber() ? -1 : 1) })
            else
                self.assists.sort(function (left, right) { return left.dispatchNumber() == right.dispatchNumber() ? 0 : (left.dispatchNumber() > right.dispatchNumber() ? -1 : 1) })
        }
        else if (currentSort() === 'assistNumber') {
            if (currentSortDirection() === 'Asc')
                self.assists.sort(function (left, right) { return left.assistNumber() == right.assistNumber() ? 0 : (left.assistNumber() < right.assistNumber() ? -1 : 1) })
            else
                self.assists.sort(function (left, right) { return left.assistNumber() == right.assistNumber() ? 0 : (left.assistNumber() > right.assistNumber() ? -1 : 1) })
        }
        else if (currentSort() === 'driverComments') {
            if (currentSortDirection() === 'Asc')
                self.assists.sort(function (left, right) { return left.driverComments() == right.driverComments() ? 0 : (left.driverComments() < right.driverComments() ? -1 : 1) })
            else
                self.assists.sort(function (left, right) { return left.driverComments() == right.driverComments() ? 0 : (left.driverComments() > right.driverComments() ? -1 : 1) })
        }
        else if (currentSort() === 'driver') {
            if (currentSortDirection() === 'Asc')
                self.assists.sort(function (left, right) { return left.driver() == right.driver() ? 0 : (left.driver() < right.driver() ? -1 : 1) })
            else
                self.assists.sort(function (left, right) { return left.driver() == right.driver() ? 0 : (left.driver() > right.driver() ? -1 : 1) })
        }
        else if (currentSort() === 'make') {
            if (currentSortDirection() === 'Asc')
                self.assists.sort(function (left, right) { return left.make() == right.make() ? 0 : (left.make() < right.make() ? -1 : 1) })
            else
                self.assists.sort(function (left, right) { return left.make() == right.make() ? 0 : (left.make() > right.make() ? -1 : 1) })
        }
        else if (currentSort() === 'model') {
            if (currentSortDirection() === 'Asc')
                self.assists.sort(function (left, right) { return left.model() == right.model() ? 0 : (left.model() < right.model() ? -1 : 1) })
            else
                self.assists.sort(function (left, right) { return left.model() == right.model() ? 0 : (left.model() > right.model() ? -1 : 1) })
        }
        else if (currentSort() === 'color') {
            if (currentSortDirection() === 'Asc')
                self.assists.sort(function (left, right) { return left.color() == right.color() ? 0 : (left.color() < right.color() ? -1 : 1) })
            else
                self.assists.sort(function (left, right) { return left.color() == right.color() ? 0 : (left.color() > right.color() ? -1 : 1) })
        }

        else if (currentSort() === 'dropZone') {
            if (currentSortDirection() === 'Asc')
                self.assists.sort(function (left, right) { return left.dropZone() == right.dropZone() ? 0 : (left.dropZone() < right.dropZone() ? -1 : 1) })
            else
                self.assists.sort(function (left, right) { return left.dropZone() == right.dropZone() ? 0 : (left.dropZone() > right.dropZone() ? -1 : 1) })
        }
        else if (currentSort() === 'plateNumber') {
            if (currentSortDirection() === 'Asc')
                self.assists.sort(function (left, right) { return left.plateNumber() == right.plateNumber() ? 0 : (left.plateNumber() < right.plateNumber() ? -1 : 1) })
            else
                self.assists.sort(function (left, right) { return left.plateNumber() == right.plateNumber() ? 0 : (left.plateNumber() > right.plateNumber() ? -1 : 1) })
        }
     
        self.isSorting = false;

    }


    self.assists = ko.observableArray([]);

    self.getAssists = function () {

        var url = fspWeb.SERVICE_BASE_URL + "/Assist/GetAssists";

        $.ajax({
            url: url,
            type: "GET",
            dataType: "json",
            error: function (xhr, ajaxOptions, thrownError) {
                //alert(xhr.status);
                //alert(thrownError);
            },
            success: function (dbAssists) {

                for (var i = 0; i < dbAssists.length; i++) {
                    try {

                        self.addOrUpdateAssist(dbAssists[i]);

                    } catch (e) {

                    }

                }
            }
        });

    }

    self.addOrUpdateAssist = function (dbAssist) {

        try {

            var currentAssist = ko.utils.arrayFirst(self.assists(), function (i) { return i.plateNumber === dbAssist.PlateNumber; });

            if (currentAssist) {
                currentAssist.update(dbAssist);
            }
            else {
                self.assists.push(new assist(self, dbAssist));
            }


        } catch (e) {

        }
    }

    function assist(root, dbAssist) {

        var self = this;

        self.assistNumber = ko.observable(dbAssist.AssistNumber);
        self.beatNumber = ko.observable(dbAssist.BeatNumber);
        self.color = ko.observable(dbAssist.Color);
        self.dispatchNumber = ko.observable(dbAssist.DispatchNumber);
        self.driver = ko.observable(dbAssist.Driver);
        self.driverComments = ko.observable(dbAssist.DriverComments);
        self.dropZone = ko.observable(dbAssist.DropZone);
        self.make = ko.observable(dbAssist.Make);
        self.model = ko.observable(dbAssist.Model);
        self.plateNumber = ko.observable(dbAssist.PlateNumber);

        self.update = function (dbAssist) {

            self.assistNumber(dbAssist.AssistNumber);
            self.beatNumber(dbAssist.BeatNumber);
            self.color(dbAssist.Color);
            self.dispatchNumber(dbAssist.DispatchNumber);
            self.driver(dbAssist.Driver);
            self.driverComments(dbAssist.DriverComments);
            self.dropZone(dbAssist.DropZone);
            self.mak(dbAssist.Make);
            self.model(dbAssist.Model);
            self.plateNumber(dbAssist.PlateNumber);

        };
    }


    //public implementation (object literals)
    return {
        init: init      
    };

}();

$(function () {

    fspWeb = new lata.FspWeb();

    //apply binding to ko
    ko.applyBindings(fspWeb);

    fspWeb.assistViewModel.init();

});


