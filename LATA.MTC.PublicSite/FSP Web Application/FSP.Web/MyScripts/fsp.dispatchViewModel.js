/// <reference path="../Scripts/knockout-2.1.0.js" />
/// <reference path="fsp.constructor.js" />
/// <reference path="fsp.truckCollection.js" />

lata.FspWeb.prototype.dispatchViewModel = function () {

    var self = this;

    self.fspDirection = ko.observable('');
    self.fspFreeway = ko.observable('');
    self.fspLocation = ko.observable('');
    self.fspCrossStreet = ko.observable('');
    self.fspCrossStreet2 = ko.observable('');
    self.comments = ko.observable('');
    self.resultingBeats = ko.observableArray([]);
    self.currentSort = ko.observable("truckNumber");
    self.currentSortDirection = ko.observable("Asc");
    self.canSendTrucks = ko.observable(false);

    self.columns = ko.observableArray([
         new column(self, "Truck #", "truckNumber"),
         new column(self, "Beat #", "beatNumber"),
         new column(self, "Driver", "driverName"),
         new column(self, "Contractor", "contractorName"),
         new column(self, "Location", "location"),
         new column(self, "Status", "status"),
         new column(self, "Speed", "speed"),
         new column(self, "Last Update", "lastUpdate")
    ]);

    var initDispatch = function () {

        ////truck collection file
        fspWeb.startTruckService();

    },
    getResultingBeats = function () {
        if (fspDirection().length > 0 && fspFreeway().length > 0 && fspLocation().length > 0 && fspCrossStreet().length > 0) {

            try {
                //url
                var url = fspWeb.SERVICE_BASE_URL + "/Dispatch/GetBeatsFromDispatchInputForm";

                self.resultingBeats([]);
                self.canSendTrucks(false);

                $.ajax({
                    url: url,
                    type: "GET",
                    dataType: "json",
                    data:
                    {
                        beatSegmentDescription: self.fspCrossStreet()
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        //alert(xhr.status);
                        //alert(thrownError);
                    },
                    success: function (beatsData) {


                        for (var i = 0; i < beatsData.length; i++) {
                            try {

                                var beatNumber = beatsData[i];
                                self.resultingBeats.push(beatNumber);

                                //now highlight trucks in filter

                            } catch (e) {

                            }
                        }

                        if (self.resultingBeats().length > 0)
                            self.canSendTrucks(true);

                    }
                });
            } catch (e) {

            }
        }
        else {
            self.canSendTrucks(false);
        }
    }

    self.fspDirection.subscribe(function () {
        getResultingBeats();
    })
    self.fspFreeway.subscribe(function () {
        getResultingBeats();
    })
    self.fspLocation.subscribe(function () {
        self.fspCrossStreet2('');
        getResultingBeats();
    })
    self.fspCrossStreet.subscribe(function () {
        getResultingBeats();
    })

    self.updateFspDirection = function (newDirection) {
        self.fspDirection(newDirection);
    }
    self.updateFspFreeway = function (newFreeway) {
        self.fspFreeway(newFreeway);
    }
    self.updateFspLocation = function (newLocation) {
        self.fspLocation(newLocation);
    }
    self.updateFspCrossStreet = function (newCrossStreet) {
        self.fspCrossStreet(newCrossStreet);
    }
    self.updateFspCrossStreet2 = function (newCrossStreet) {
        self.fspCrossStreet2(newCrossStreet);
    }

    self.clearBeats = function () {

        self.fspDirection('');
        self.fspFreeway('');
        self.fspLocation('');
        self.fspCrossStreet('');
        self.comments('');
        self.resultingBeats([]);

        self.canSendTrucks(false);
    }
    self.dispatchTrucks = function () {

        alert("Trucks dispatched");

    }

    //column
    function column(root, name, value) {
        var self = this;

        self.name = ko.observable(name);
        self.value = ko.observable(value);

        self.sort = function (item) {

            root.currentSort(item.value());

            if (root.currentSortDirection() === 'Asc')
                root.currentSortDirection('Desc');
            else
                root.currentSortDirection('Asc');

            if (root.currentSort() === 'truckNumber') {
                if (root.currentSortDirection() === 'Asc')
                    fspWeb.trucks.sort(function (left, right) { return left.truckNumber == right.truckId ? 0 : (left.truckNumber < right.truckNumber ? -1 : 1) })
                else
                    fspWeb.trucks.sort(function (left, right) { return left.truckNumber == right.truckId ? 0 : (left.truckNumber > right.truckNumber ? -1 : 1) })
            }
            else if (root.currentSort() === 'beatNumber') {
                if (root.currentSortDirection() === 'Asc')
                    fspWeb.trucks.sort(function (left, right) { return left.beatNumber() == right.beatNumber() ? 0 : (left.beatNumber() < right.beatNumber() ? -1 : 1) })
                else
                    fspWeb.trucks.sort(function (left, right) { return left.beatNumber() == right.beatNumber() ? 0 : (left.beatNumber() > right.beatNumber() ? -1 : 1) })
            }
            else if (root.currentSort() === 'driverName') {
                if (root.currentSortDirection() === 'Asc')
                    fspWeb.trucks.sort(function (left, right) { return left.driverName() == right.driverName() ? 0 : (left.driverName() < right.driverName() ? -1 : 1) })
                else
                    fspWeb.trucks.sort(function (left, right) { return left.driverName() == right.driverName() ? 0 : (left.driverName() > right.driverName() ? -1 : 1) })
            }
            else if (root.currentSort() === 'contractorName') {
                if (root.currentSortDirection() === 'Asc')
                    fspWeb.trucks.sort(function (left, right) { return left.contractorName() == right.contractorName() ? 0 : (left.contractorName() < right.contractorName() ? -1 : 1) })
                else
                    fspWeb.trucks.sort(function (left, right) { return left.contractorName() == right.contractorName() ? 0 : (left.contractorName() > right.contractorName() ? -1 : 1) })
            }
            else if (root.currentSort() === 'location') {
                if (root.currentSortDirection() === 'Asc')
                    fspWeb.trucks.sort(function (left, right) { return left.location() == right.location() ? 0 : (left.location() < right.location() ? -1 : 1) })
                else
                    fspWeb.trucks.sort(function (left, right) { return left.location() == right.location() ? 0 : (left.location() > right.location() ? -1 : 1) })
            }
            else if (root.currentSort() === 'status') {
                if (root.currentSortDirection() === 'Asc')
                    fspWeb.trucks.sort(function (left, right) { return left.vehicleState() == right.vehicleState() ? 0 : (left.vehicleState() < right.vehicleState() ? -1 : 1) })
                else
                    fspWeb.trucks.sort(function (left, right) { return left.vehicleState() == right.vehicleState() ? 0 : (left.vehicleState() > right.vehicleState() ? -1 : 1) })
            }
            else if (root.currentSort() === 'speed') {
                if (root.currentSortDirection() === 'Asc')
                    fspWeb.trucks.sort(function (left, right) { return left.speed() == right.speed() ? 0 : (left.speed() < right.speed() ? -1 : 1) })
                else
                    fspWeb.trucks.sort(function (left, right) { return left.speed() == right.speed() ? 0 : (left.speed() > right.speed() ? -1 : 1) })
            }
            else if (root.currentSort() === 'lastUpdate') {
                if (root.currentSortDirection() === 'Asc')
                    fspWeb.trucks.sort(function (left, right) { return left.lastUpdate() == right.lastUpdate() ? 0 : (left.lastUpdate() < right.lastUpdate() ? -1 : 1) })
                else
                    fspWeb.trucks.sort(function (left, right) { return left.lastUpdate() == right.lastUpdate() ? 0 : (left.lastUpdate() > right.lastUpdate() ? -1 : 1) })
            }

        };
    }

    return {
        initDispatch: initDispatch,
        updateFspDirection: updateFspDirection,
        updateFspFreeway: updateFspFreeway,
        updateFspLocation: updateFspLocation,
        updateFspCrossStreet: updateFspCrossStreet,
        updateFspCrossStreet2: updateFspCrossStreet2
    };

}();



$(function () {

    fspWeb = new lata.FspWeb();

    //apply binding to ko
    ko.applyBindings(fspWeb);

    //init
    fspWeb.dispatchViewModel.initDispatch();

    $("#fspDirection").focus();

    //auto-completes    
    $("#fspDirection").autocomplete({
        source: function (request, response) {
            var url = fspWeb.SERVICE_BASE_URL + "/Dispatch/GetDirections";
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
                            label: item,
                            value: item,
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
            fspWeb.dispatchViewModel.updateFspDirection(ui.item.value);
        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });
    $("#fspFreeway").autocomplete({
        source: function (request, response) {
            var url = fspWeb.SERVICE_BASE_URL + "/Dispatch/GetFreewaysByDirection";
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
                            label: item,
                            value: item,
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
            fspWeb.dispatchViewModel.updateFspFreeway(ui.item.value);
        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });
    $("#fspLocation").autocomplete({
        source: function (request, response) {
            var url = fspWeb.SERVICE_BASE_URL + "/Dispatch/GetLocations";
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
                            label: item,
                            value: item,
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
            fspWeb.dispatchViewModel.updateFspLocation(ui.item.value);
        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });
    $("#fspCrossStreet").autocomplete({
        source: function (request, response) {
            var url = fspWeb.SERVICE_BASE_URL + "/Dispatch/GetCrossStreetFromBeatSegmentDescription";
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
                            label: item,
                            value: item,
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
            fspWeb.dispatchViewModel.updateFspCrossStreet(ui.item.value);
        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });
    $("#fspCrossStreet2").autocomplete({
        source: function (request, response) {
            var url = fspWeb.SERVICE_BASE_URL + "/Dispatch/GetCrossStreetFromBeatSegmentDescription";
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
                            label: item,
                            value: item,
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
            fspWeb.dispatchViewModel.updateFspCrossStreet2(ui.item.value);
        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });

});





