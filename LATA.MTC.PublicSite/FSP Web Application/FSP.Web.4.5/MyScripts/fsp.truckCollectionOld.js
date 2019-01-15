
lata.FspWeb.prototype.startTruckService = function () {

    //hub methods
    fspWeb.towTruckHub = $.connection.towTruckHub;

    fspWeb.towTruckHub.client.addOrUpdateTruck = function (dbTruck) {
        //fspWeb.trucks.push(new fspWeb.truck(dbTruck));

        try {

            var currentTruck = ko.utils.arrayFirst(fspWeb.trucks(), function (i) { return i.truckNumber === dbTruck.TruckNumber; });

            if (currentTruck) {
                currentTruck.update(dbTruck);
                if (fspWeb.trucksChanged() === true)
                    fspWeb.trucksChanged(false);
                else
                    fspWeb.trucksChanged(true);
            }
            else {
                fspWeb.trucks.push(new fspWeb.truck(dbTruck));
            }

        } catch (e) {

        }


    };

    fspWeb.towTruckHub.client.updateLastUpdateTime = function (truckNumber, lastUpdate, lastMessage) {
        try {
            var currentTruck = ko.utils.arrayFirst(fspWeb.trucks(), function (i) { return i.truckNumber === truckNumber; });
            if (currentTruck) {
                currentTruck.lastUpdate(lastUpdate + ' seconds ago');
                if (fspWeb.trucksChanged() === true)
                    fspWeb.trucksChanged(false);
                else
                    fspWeb.trucksChanged(true);
            }
        } catch (e) {

        }

    };

    fspWeb.towTruckHub.client.deleteTruck = function (truckNumber) {
        var currentTruck = ko.utils.arrayFirst(fspWeb.trucks(), function (i) { return i.truckNumber === truckNumber; });

        if (currentTruck) {
            try {
                currentTruck.lastUpdate('Deleted');

                //removed it from collection
                fspWeb.trucks.remove(function (i) { return i.id === currentTruck.id; });

                if (fspWeb.trucksChanged() === true)
                    fspWeb.trucksChanged(false);
                else
                    fspWeb.trucksChanged(true);

                //remove it from map
                fspWeb.selectedIdForDeletion(currentTruck.id);

            } catch (e) {

            }
        }

    };

    fspWeb.towTruckHub.client.setSelectedTruck = function (truckNumber, userId) {

        var currentTruck = ko.utils.arrayFirst(fspWeb.trucks(), function (i) { return i.truckNumber === truckNumber; });
        if (currentTruck) {
            try {

                //alert('Current UserId ' + fspWeb.userId + '\nRequesting UserId ' + userId);

                if (userId === fspWeb.userId) {
                    //alert('Setting truck to: ' + truckNumber);
                    fspWeb.selectedId(currentTruck.id);
                    //alert('Setting truck to: ' + truckNumber);
                }
            } catch (e) {

            }
        }
    };

    fspWeb.towTruckHub.client.stopFollowingTruck = function (userId) {

        try {
            //alert('Current UserId ' + fspWeb.userId + ' requesting UserId ' + userId);

            if (userId === fspWeb.userId) {
                //alert('Stop folling truck');
                fspWeb.selectedId(null);
                //alert('Setting truck to: ' + truckNumber);
            }
        } catch (e) {

        }

    };

    fspWeb.towTruckHub.client.updateUserId = function (userId) {
        //alert('New UserId ' + userId);
        fspWeb.userId = userId;
    };

    $.connection.hub.start(function () {
        fspWeb.towTruckHub.server.initialize();
    });


};


