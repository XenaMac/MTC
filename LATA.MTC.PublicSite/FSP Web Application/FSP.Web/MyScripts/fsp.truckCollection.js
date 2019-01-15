
lata.FspWeb.prototype.startTruckService = function () {
   
    //hub methods
    var towTruckHub = $.connection.towTruckHub;

    towTruckHub.addOrUpdateTruck = function (dbTruck) {
        //fspWeb.trucks.push(new fspWeb.truck(dbTruck));

        try {

            var currentTruck = ko.utils.arrayFirst(fspWeb.trucks(), function (i) { return i.truckNumber === dbTruck.TruckNumber; });

            if (currentTruck) {
                currentTruck.update(dbTruck);

                //DO TO CHECK IF WE HAVE A SECLECTED TRUCK AND UPDATE IT ALSO
            }
            else {
                fspWeb.trucks.push(new fspWeb.truck(dbTruck));                
            }

        } catch (e) {

        }


    };

    towTruckHub.updateLastUpdateTime = function (truckNumber, lastUpdate, lastMessage) {
        try {
            var currentTruck = ko.utils.arrayFirst(fspWeb.trucks(), function (i) { return i.truckNumber === truckNumber; });
            if (currentTruck) {               
                currentTruck.lastUpdate(lastUpdate + ' seconds ago');

                //DO TO CHECK IF WE HAVE A SECLECTED TRUCK AND UPDATE IT ALSO
            }
        } catch (e) {

        }
       
    };

    towTruckHub.deleteTruck = function (truckId) {
       
    };

    $.connection.hub.start(function () {
        towTruckHub.initialize();
    });
   
};