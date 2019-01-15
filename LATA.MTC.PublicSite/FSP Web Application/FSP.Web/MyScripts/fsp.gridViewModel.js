lata.FspWeb.prototype.gridViewModel = function () {
   
    var initGrid = function () {

        ////truck collection file
        fspWeb.startTruckService(); 
    }

    var self = this;

    self.currentSort = ko.observable("truckNumber");
    self.currentSortDirection = ko.observable("Asc");

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

    //row click
    self.showDetails = function (item) {
        try {

            fspWeb.selectedTruck(item);           
            $("#truckDetails").modal('show');
        } catch (e) {

        }
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

    //public implementation (object literals)
    return {
        initGrid: initGrid
    };

}();


//wait for browser to load (jquery command)
$(function () {

    fspWeb = new lata.FspWeb();

    //apply binding to ko
    ko.applyBindings(fspWeb);

    //init
    fspWeb.gridViewModel.initGrid();

});