/// <reference path="../Scripts/knockout-2.1.0.js" />

var SERVICE_BASE_URL = $("#websitePath").attr("data-websitePath");
var viewModel;


var MyViewModel = function () {

    var self = this;

    self.dispatchs = ko.observableArray();
    self.searchValue = ko.observable('');
    //self.selectedDispatch = ko.observable(new dispatch(self, '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''));
    self.selectedDispatch = ko.observable(null);
    self.currentSort = ko.observable("beatNumber");
    self.currentSortDirection = ko.observable("Asc");

    self.columns = ko.observableArray([
        new column(self, "Beat #", "beatNumber"),
        new column(self, "Truck #", "truckNumber"),
        new column(self, "Contractor", "contractorName"),
        new column(self, "Driver", "driverName"),
        new column(self, "Service Date", "serviceDate"),
        new column(self, "Beat Start", "beatStartTime"),
        new column(self, "Status", "status"),
        new column(self, "Last Update", "lastUpdateTime"),
        new column(self, "Alarms", "alarms"),
        new column(self, "Freeway", "freeway"),
        new column(self, "Direction", "direction"),
        new column(self, "Last Location", "lastLocation"),
        new column(self, "Assist Description", "assistDescription"),
        new column(self, "Incident Code", "incidentCode"),
        new column(self, "Service Code", "serviceCode"),
        new column(self, "Vehicle Desc.", "vehicleDescription"),
        new column(self, "vehicle Lic. #", "vehicleLicensePlateNumber"),
        new column(self, "Tow Location", "towLocation"),
    ]);

    //================================== EVENTS =====================================================================

    //event handler to handle category changed event
    self.searchValue.subscribe(function () {
        self.doSearch();
    });


    //===================================== METHODS ======================================================================

    //set selected
    self.setSelectedDispatch = function (dispatch) {
        //alert(dispatch);
        self.selectedDispatch(dispatch);
        $("#detailsModal").modal('show');
    };

    //add or update 
    self.addDispatch = function (dbDispatch) {

        self.dispatchs.push(new dispatch(self, dbDispatch.BeatNumber, dbDispatch.TruckNumber, dbDispatch.ContractorName, dbDispatch.DriverName, dbDispatch.ServiceDate, dbDispatch.BeatStartTime, dbDispatch.Status, dbDispatch.LastUpdateTime, dbDispatch.Alarms, dbDispatch.Freeway, dbDispatch.Direction, dbDispatch.LastLocation, dbDispatch.AssistDescription, dbDispatch.IncidentCode, dbDispatch.ServiceCode, dbDispatch.VehicleDescription, dbDispatch.VehicleLicensePlateNumber, dbDispatch.TowLocation));

        $("#gridNumberOfRows").text(self.towTrucks().length + ' trucks');
    };

    //search
    self.doSearch = function () {

        for (var i = 0; i < self.dispatchs().length; i++) {

            var d = self.dispatchs()[i];

            if (d.beatNumber().toLowerCase().indexOf(self.searchValue().toLowerCase()) >= 0
                || d.truckNumber().toLowerCase().indexOf(self.searchValue().toLowerCase()) >= 0
                || d.contractorName().toLowerCase().indexOf(self.searchValue().toLowerCase()) >= 0
                || d.driverName().toLowerCase().indexOf(self.searchValue().toLowerCase()) >= 0
                || d.serviceDate().toLowerCase().indexOf(self.searchValue().toLowerCase()) >= 0
                || d.beatStartTime().toLowerCase().indexOf(self.searchValue().toLowerCase()) >= 0
                || d.status().toLowerCase().indexOf(self.searchValue().toLowerCase()) >= 0
                || d.lastUpdateTime().toLowerCase().indexOf(self.searchValue().toLowerCase()) >= 0
                || d.alarms().toLowerCase().indexOf(self.searchValue().toLowerCase()) >= 0
                || d.freeway().toLowerCase().indexOf(self.searchValue().toLowerCase()) >= 0
                || d.direction().toLowerCase().indexOf(self.searchValue().toLowerCase()) >= 0
                || d.lastLocation().toLowerCase().indexOf(self.searchValue().toLowerCase()) >= 0
                || d.assistDescription().toLowerCase().indexOf(self.searchValue().toLowerCase()) >= 0
                || d.incidentCode().toLowerCase().indexOf(self.searchValue().toLowerCase()) >= 0
                || d.serviceCode().toLowerCase().indexOf(self.searchValue().toLowerCase()) >= 0
                || d.vehicleDescription().toLowerCase().indexOf(self.searchValue().toLowerCase()) >= 0
                || d.vehicleLicensePlateNumber().toLowerCase().indexOf(self.searchValue().toLowerCase()) >= 0
                || d.towLocation().toLowerCase().indexOf(self.searchValue().toLowerCase()) >= 0)
                d.isVisible(true);
            else
                d.isVisible(false);


        }

    };

    //===================================== CLASS ===========================================================================

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


            if (root.currentSort() === 'beatNumber') {
                if (root.currentSortDirection() === 'Asc')
                    root.dispatchs.sort(function (left, right) { return left.beatNumber() == right.beatNumber() ? 0 : (left.beatNumber() < right.beatNumber() ? -1 : 1) })
                else
                    root.dispatchs.sort(function (left, right) { return left.beatNumber() == right.beatNumber() ? 0 : (left.beatNumber() > right.beatNumber() ? -1 : 1) })
            }
            else if (root.currentSort() === 'truckNumber') {
                if (root.currentSortDirection() === 'Asc')
                    root.dispatchs.sort(function (left, right) { return left.truckNumber() == right.truckNumber() ? 0 : (left.truckNumber() < right.truckNumber() ? -1 : 1) })
                else
                    root.dispatchs.sort(function (left, right) { return left.truckNumber() == right.truckNumber() ? 0 : (left.truckNumber() > right.truckNumber() ? -1 : 1) })
            }
            else if (root.currentSort() === 'contractorName') {
                if (root.currentSortDirection() === 'Asc')
                    root.dispatchs.sort(function (left, right) { return left.contractorName() == right.contractorName() ? 0 : (left.contractorName() < right.contractorName() ? -1 : 1) })
                else
                    root.dispatchs.sort(function (left, right) { return left.contractorName() == right.contractorName() ? 0 : (left.contractorName() > right.contractorName() ? -1 : 1) })
            }
            else if (root.currentSort() === 'driverName') {
                if (root.currentSortDirection() === 'Asc')
                    root.dispatchs.sort(function (left, right) { return left.driverName() == right.driverName() ? 0 : (left.driverName() < right.driverName() ? -1 : 1) })
                else
                    root.dispatchs.sort(function (left, right) { return left.driverName() == right.driverName() ? 0 : (left.driverName() > right.driverName() ? -1 : 1) })
            }
            else if (root.currentSort() === 'serviceDate') {
                if (root.currentSortDirection() === 'Asc')
                    root.dispatchs.sort(function (left, right) { return left.serviceDate() == right.serviceDate() ? 0 : (left.serviceDate() < right.serviceDate() ? -1 : 1) })
                else
                    root.dispatchs.sort(function (left, right) { return left.serviceDate() == right.serviceDate() ? 0 : (left.serviceDate() > right.serviceDate() ? -1 : 1) })
            }
            else if (root.currentSort() === 'beatStartTime') {
                if (root.currentSortDirection() === 'Asc')
                    root.dispatchs.sort(function (left, right) { return left.beatStartTime() == right.beatStartTime() ? 0 : (left.beatStartTime() < right.beatStartTime() ? -1 : 1) })
                else
                    root.dispatchs.sort(function (left, right) { return left.beatStartTime() == right.beatStartTime() ? 0 : (left.beatStartTime() > right.beatStartTime() ? -1 : 1) })
            }
            else if (root.currentSort() === 'status') {
                if (root.currentSortDirection() === 'Asc')
                    root.dispatchs.sort(function (left, right) { return left.status() == right.status() ? 0 : (left.status() < right.status() ? -1 : 1) })
                else
                    root.dispatchs.sort(function (left, right) { return left.status() == right.status() ? 0 : (left.status() > right.status() ? -1 : 1) })
            }
            else if (root.currentSort() === 'lastUpdateTime') {
                if (root.currentSortDirection() === 'Asc')
                    root.dispatchs.sort(function (left, right) { return left.lastUpdateTime() == right.lastUpdateTime() ? 0 : (left.lastUpdateTime() < right.lastUpdateTime() ? -1 : 1) })
                else
                    root.dispatchs.sort(function (left, right) { return left.lastUpdateTime() == right.lastUpdateTime() ? 0 : (left.lastUpdateTime() > right.lastUpdateTime() ? -1 : 1) })
            }
            else if (root.currentSort() === 'alarms') {
                if (root.currentSortDirection() === 'Asc')
                    root.dispatchs.sort(function (left, right) { return left.alarms() == right.alarms() ? 0 : (left.alarms() < right.alarms() ? -1 : 1) })
                else
                    root.dispatchs.sort(function (left, right) { return left.alarms() == right.alarms() ? 0 : (left.alarms() > right.alarms() ? -1 : 1) })
            }
            else if (root.currentSort() === 'freeway') {
                if (root.currentSortDirection() === 'Asc')
                    root.dispatchs.sort(function (left, right) { return left.freeway() == right.freeway() ? 0 : (left.freeway() < right.freeway() ? -1 : 1) })
                else
                    root.dispatchs.sort(function (left, right) { return left.freeway() == right.freeway() ? 0 : (left.freeway() > right.freeway() ? -1 : 1) })
            }
            else if (root.currentSort() === 'direction') {
                if (root.currentSortDirection() === 'Asc')
                    root.dispatchs.sort(function (left, right) { return left.direction() == right.direction() ? 0 : (left.direction() < right.direction() ? -1 : 1) })
                else
                    root.dispatchs.sort(function (left, right) { return left.direction() == right.direction() ? 0 : (left.direction() > right.direction() ? -1 : 1) })
            }
            else if (root.currentSort() === 'lastLocation') {
                if (root.currentSortDirection() === 'Asc')
                    root.dispatchs.sort(function (left, right) { return left.lastLocation() == right.lastLocation() ? 0 : (left.lastLocation() < right.lastLocation() ? -1 : 1) })
                else
                    root.dispatchs.sort(function (left, right) { return left.lastLocation() == right.lastLocation() ? 0 : (left.lastLocation() > right.lastLocation() ? -1 : 1) })
            }
            else if (root.currentSort() === 'assistDescription') {
                if (root.currentSortDirection() === 'Asc')
                    root.dispatchs.sort(function (left, right) { return left.assistDescription() == right.assistDescription() ? 0 : (left.assistDescription() < right.assistDescription() ? -1 : 1) })
                else
                    root.dispatchs.sort(function (left, right) { return left.assistDescription() == right.assistDescription() ? 0 : (left.assistDescription() > right.assistDescription() ? -1 : 1) })
            }
            else if (root.currentSort() === 'incidentCode') {
                if (root.currentSortDirection() === 'Asc')
                    root.dispatchs.sort(function (left, right) { return left.incidentCode() == right.incidentCode() ? 0 : (left.truckNumber() < right.truckNumber() ? -1 : 1) })
                else
                    root.dispatchs.sort(function (left, right) { return left.incidentCode() == right.incidentCode() ? 0 : (left.truckNumber() > right.truckNumber() ? -1 : 1) })
            }
            else if (root.currentSort() === 'serviceCode') {
                if (root.currentSortDirection() === 'Asc')
                    root.dispatchs.sort(function (left, right) { return left.serviceCode() == right.serviceCode() ? 0 : (left.serviceCode() < right.serviceCode() ? -1 : 1) })
                else
                    root.dispatchs.sort(function (left, right) { return left.serviceCode() == right.serviceCode() ? 0 : (left.serviceCode() > right.serviceCode() ? -1 : 1) })
            }
            else if (root.currentSort() === 'vehicleDescription') {
                if (root.currentSortDirection() === 'Asc')
                    root.dispatchs.sort(function (left, right) { return left.vehicleDescription() == right.vehicleDescription() ? 0 : (left.vehicleDescription() < right.vehicleDescription() ? -1 : 1) })
                else
                    root.dispatchs.sort(function (left, right) { return left.vehicleDescription() == right.vehicleDescription() ? 0 : (left.vehicleDescription() > right.vehicleDescription() ? -1 : 1) })
            }
            else if (root.currentSort() === 'vehicleLicensePlateNumber') {
                if (root.currentSortDirection() === 'Asc')
                    root.dispatchs.sort(function (left, right) { return left.vehicleLicensePlateNumber() == right.vehicleLicensePlateNumber() ? 0 : (left.vehicleLicensePlateNumber() < right.vehicleLicensePlateNumber() ? -1 : 1) })
                else
                    root.dispatchs.sort(function (left, right) { return left.vehicleLicensePlateNumber() == right.vehicleLicensePlateNumber() ? 0 : (left.vehicleLicensePlateNumber() > right.vehicleLicensePlateNumber() ? -1 : 1) })
            }
            else if (root.currentSort() === 'towLocation') {
                if (root.currentSortDirection() === 'Asc')
                    root.dispatchs.sort(function (left, right) { return left.towLocation() == right.towLocation() ? 0 : (left.towLocation() < right.towLocation() ? -1 : 1) })
                else
                    root.dispatchs.sort(function (left, right) { return left.towLocation() == right.towLocation() ? 0 : (left.towLocation() > right.towLocation() ? -1 : 1) })
            }


        };
    }


    //truck object
    function dispatch(root, beatNumber, truckNumber, contractorName, driverName, serviceDate, beatStartTime, status, lastUpdateTime, alarms, freeway, direction, lastLocation, assistDescription, incidentCode, serviceCode, vehicleDescription, vehicleLicensePlateNumber, towLocation) {

        var self = this;

        self.beatNumber = ko.observable(beatNumber);
        self.truckNumber = ko.observable(truckNumber);
        self.contractorName = ko.observable(contractorName);
        self.driverName = ko.observable(driverName);
        self.serviceDate = ko.observable(serviceDate);
        self.beatStartTime = ko.observable(beatStartTime);
        self.status = ko.observable(status);
        self.lastUpdateTime = ko.observable(lastUpdateTime);
        self.alarms = ko.observable(alarms);
        self.freeway = ko.observable(freeway);
        self.direction = ko.observable(direction);
        self.lastLocation = ko.observable(lastLocation);
        self.assistDescription = ko.observable(assistDescription);
        self.incidentCode = ko.observable(incidentCode);
        self.serviceCode = ko.observable(serviceCode);
        self.vehicleDescription = ko.observable(vehicleDescription);
        self.vehicleLicensePlateNumber = ko.observable(vehicleLicensePlateNumber);
        self.towLocation = ko.observable(towLocation);

        self.isVisible = ko.observable(true);
    };

};

$(function () {

    viewModel = new MyViewModel();

    ko.applyBindings(viewModel);

    GetDispatchs();

    //set selected row
    $("#dispatchGrid tbody tr").live("click", function () {
        $(this).addClass("success").siblings().removeClass("success");
    });

});

function GetDispatchs() {

    //url
    var url = SERVICE_BASE_URL + "/Dispatch/GetDispatchs";

    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        },
        success: function (data) {

            for (var i = 0; i < data.length; i++) {
                try {

                    viewModel.addDispatch(data[i]);

                } catch (e) {

                }

            }
        }
    });



}



