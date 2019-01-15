lata.FspWeb.prototype.gridViewModel = function () {

    //hub methods
    //var towTruckHub = $.connection.towTruckHub;

    var initGrid = function () {

        //$.connection.hub.start();

        ////truck collection file
        fspWeb.startTruckService();

        //handle truck collection change
        fspWeb.trucksChanged.subscribe(function () {
            //alert('Truck collection changed');    

            if (self.isSorting === false)
                self.doSort();
        })

        //handle truck selection from outside
        fspWeb.selectedId.subscribe(function () {

            //alert('follow truck with id ' + fspWeb.selectedId());

            if (fspWeb.selectedId() == null) {
                //get truck from TruckNumber
                //alert('Stop Following Truck');

                $("#towTruckGrid > tbody > tr").each(function (event) {
                    $(this).removeClass('highlight');
                })
            }
            else {

                $("#towTruckGrid > tbody > tr").each(function (event) {

                    var rowId = $(this)[0].id;

                    if (rowId === fspWeb.selectedId()) {
                        $(this).addClass('highlight').siblings().removeClass('highlight');
                    }

                })
            }
        })

    }

    var self = this;

    self.currentSort = ko.observable("beatNumber");
    self.currentSortDirection = ko.observable("Asc");

    self.columns = ko.observableArray([
         new column(self, "Beat #", "beatNumber", true),
         new column(self, "Truck #", "truckNumber", true),
         new column(self, "Driver", "driverName", true),
         new column(self, "Heading", "headingText", true),
         new column(self, "Location", "location", true),
         new column(self, "Status", "status", true),
         new column(self, "Last Status Change", "lastStatusChanged", true),
         new column(self, "Current Speed", "speed", true),
         new column(self, "Max Speed", "speedingValue", true),
         new column(self, "Speeding Time", "speedingTime", true),

         new column(self, "Contractor", "contractorName", false),
         new column(self, "Out of Bounds Message", "outOfBoundsMessage", false),
         new column(self, "Out of Bounds Time", "outOfBoundsTime", false),
         new column(self, "Last Update", "lastMessage", false)
    ]);

    self.isSorting = false;


    //row click
    self.showTruckInMap = function (item) {
        try {

            //towTruckHub.setSelectedTruckForClient(item.truckNumber);

            //alert('Current Grid User ' + fspWeb.userId)

            var userId = fspWeb.userId;


            fspWeb.towTruckHub.server.setSelectedTruck(item.truckNumber, userId);

            //var url = fspWeb.SERVICE_BASE_URL + "/Truck/SetSelectedTruck";
            //$.ajax({
            //    url: url,
            //    type: "GET",
            //    dataType: "json",
            //    data: {
            //        truckNumber: item.truckNumber,
            //        userId: userId
            //    },
            //    error: function (xhr, ajaxOptions, thrownError) {
            //        //alert(xhr.status);
            //        //alert(thrownError);
            //    },
            //    success: function () {

            //    }
            //});

        } catch (e) {

        }
    }

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


        if (currentSort() === 'truckNumber') {
            if (currentSortDirection() === 'Asc')
                fspWeb.trucks.sort(function (left, right) { return left.truckNumber == right.truckNumber ? 0 : (left.truckNumber < right.truckNumber ? -1 : 1) })
            else
                fspWeb.trucks.sort(function (left, right) { return left.truckNumber == right.truckNumber ? 0 : (left.truckNumber > right.truckNumber ? -1 : 1) })
        }
        else if (currentSort() === 'beatNumber') {
            if (currentSortDirection() === 'Asc')
                fspWeb.trucks.sort(function (left, right) { return left.beatNumber() == right.beatNumber() ? 0 : (left.beatNumber() < right.beatNumber() ? -1 : 1) })
            else
                fspWeb.trucks.sort(function (left, right) { return left.beatNumber() == right.beatNumber() ? 0 : (left.beatNumber() > right.beatNumber() ? -1 : 1) })
        }
        else if (currentSort() === 'driverName') {
            if (currentSortDirection() === 'Asc')
                fspWeb.trucks.sort(function (left, right) { return left.driverName() == right.driverName() ? 0 : (left.driverName() < right.driverName() ? -1 : 1) })
            else
                fspWeb.trucks.sort(function (left, right) { return left.driverName() == right.driverName() ? 0 : (left.driverName() > right.driverName() ? -1 : 1) })
        }
        else if (currentSort() === 'contractorName') {
            if (currentSortDirection() === 'Asc')
                fspWeb.trucks.sort(function (left, right) { return left.contractorName() == right.contractorName() ? 0 : (left.contractorName() < right.contractorName() ? -1 : 1) })
            else
                fspWeb.trucks.sort(function (left, right) { return left.contractorName() == right.contractorName() ? 0 : (left.contractorName() > right.contractorName() ? -1 : 1) })
        }
        else if (currentSort() === 'location') {
            if (currentSortDirection() === 'Asc')
                fspWeb.trucks.sort(function (left, right) { return left.location() == right.location() ? 0 : (left.location() < right.location() ? -1 : 1) })
            else
                fspWeb.trucks.sort(function (left, right) { return left.location() == right.location() ? 0 : (left.location() > right.location() ? -1 : 1) })
        }
        else if (currentSort() === 'status') {
            if (currentSortDirection() === 'Asc')
                fspWeb.trucks.sort(function (left, right) { return left.vehicleState() == right.vehicleState() ? 0 : (left.vehicleState() < right.vehicleState() ? -1 : 1) })
            else
                fspWeb.trucks.sort(function (left, right) { return left.vehicleState() == right.vehicleState() ? 0 : (left.vehicleState() > right.vehicleState() ? -1 : 1) })
        }
        else if (currentSort() === 'speed') {
            if (currentSortDirection() === 'Asc')
                fspWeb.trucks.sort(function (left, right) { return left.speed() == right.speed() ? 0 : (left.speed() < right.speed() ? -1 : 1) })
            else
                fspWeb.trucks.sort(function (left, right) { return left.speed() == right.speed() ? 0 : (left.speed() > right.speed() ? -1 : 1) })
        }
        else if (currentSort() === 'lastUpdate') {
            if (currentSortDirection() === 'Asc')
                fspWeb.trucks.sort(function (left, right) { return left.lastUpdate() == right.lastUpdate() ? 0 : (left.lastUpdate() < right.lastUpdate() ? -1 : 1) })
            else
                fspWeb.trucks.sort(function (left, right) { return left.lastUpdate() == right.lastUpdate() ? 0 : (left.lastUpdate() > right.lastUpdate() ? -1 : 1) })
        }

        else if (currentSort() === 'speedingTime') {
            if (currentSortDirection() === 'Asc')
                fspWeb.trucks.sort(function (left, right) { return left.speedingTime() == right.speedingTime() ? 0 : (left.speedingTime() < right.speedingTime() ? -1 : 1) })
            else
                fspWeb.trucks.sort(function (left, right) { return left.speedingTime() == right.speedingTime() ? 0 : (left.speedingTime() > right.speedingTime() ? -1 : 1) })
        }
        else if (currentSort() === 'speedingValue') {
            if (currentSortDirection() === 'Asc')
                fspWeb.trucks.sort(function (left, right) { return left.speedingValue() == right.speedingValue() ? 0 : (left.speedingValue() < right.speedingValue() ? -1 : 1) })
            else
                fspWeb.trucks.sort(function (left, right) { return left.speedingValue() == right.speedingValue() ? 0 : (left.speedingValue() > right.speedingValue() ? -1 : 1) })
        }
        else if (currentSort() === 'outOfBoundsMessage') {
            if (currentSortDirection() === 'Asc')
                fspWeb.trucks.sort(function (left, right) { return left.outOfBoundsMessage() == right.outOfBoundsMessage() ? 0 : (left.outOfBoundsMessage() < right.outOfBoundsMessage() ? -1 : 1) })
            else
                fspWeb.trucks.sort(function (left, right) { return left.outOfBoundsMessage() == right.outOfBoundsMessage() ? 0 : (left.outOfBoundsMessage() > right.outOfBoundsMessage() ? -1 : 1) })
        }
        else if (currentSort() === 'outOfBoundsTime') {
            if (currentSortDirection() === 'Asc')
                fspWeb.trucks.sort(function (left, right) { return left.outOfBoundsTime() == right.outOfBoundsTime() ? 0 : (left.outOfBoundsTime() < right.outOfBoundsTime() ? -1 : 1) })
            else
                fspWeb.trucks.sort(function (left, right) { return left.outOfBoundsTime() == right.outOfBoundsTime() ? 0 : (left.outOfBoundsTime() > right.outOfBoundsTime() ? -1 : 1) })
        }

        self.isSorting = false;

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

    $("#towTruckGrid").on("click", "tbody tr", function (event) {
        $(this).addClass('highlight').siblings().removeClass('highlight');
    })

});