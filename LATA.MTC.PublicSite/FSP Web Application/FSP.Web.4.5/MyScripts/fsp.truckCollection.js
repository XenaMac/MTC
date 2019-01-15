
lata.FspWeb.prototype.startTruckService = function () {

    //hub methods
    fspWeb.towTruckHub = $.connection.towTruckHub;

    fspWeb.getTrucks = function () {
        //new
        var truckCollectionUrl = fspWeb.SERVICE_BASE_URL + '/Truck/UpdateAllTrucks';

        console.time('Loading trucks');

        $.get(truckCollectionUrl,
                function (trucks) {

                    console.timeEnd('Loading trucks');                    
                    console.time('Rendering trucks');

                    $.each(trucks, function (i, truck) {
                        fspWeb.addOrUpdateTruck(truck);
                    });
              
                    for (var i = 0; i < fspWeb.trucks().length; i++) {
                        var uiTruck = fspWeb.trucks()[i];
                        var truckIsGood = false;

                        for (var ii = 0; ii < trucks.length; ii++) {
                            var serverTruck = trucks[ii];

                            if (serverTruck.TruckNumber === uiTruck.truckNumber) {
                                truckIsGood = true;
                            }

                        }

                        if (truckIsGood === false) {
                            fspWeb.trucks.remove(function (item) { return item.truckNumber === uiTruck.truckNumber; });
                        }
                    }

                    console.timeEnd('Rendering trucks');


                }, "json");

    }

    //add or update lmt in corridor lmt list
    fspWeb.addOrUpdateTruck = function (dbTruck) {

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

                //var addTruck = true;

                ////check for user contractor association
                //if (dbTruck.UserContractorName != '') {
                //    if (dbTruck.UserContractorName === dbTruck.ContractorName)
                //        addTruck = true;
                //    else
                //        addTruck = false;
                //}


                //if (addTruck)

                fspWeb.trucks.push(new fspWeb.truck(dbTruck));
            }

        } catch (e) {

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

        fspWeb.getTrucks();

        //var truckUpdateInterval = setInterval(function () {
        //    fspWeb.getTrucks();
        //}, 3000);

        //recursive setTimeout Pattern
        //to avoid concurrent calls. this patterns finishes processing first before executing next call

        var clientRefreshRate = 1000;

        setTimeout(function updateTrucks() {           
            fspWeb.getTrucks();
            setTimeout(updateTrucks, clientRefreshRate);
        }, clientRefreshRate);

        fspWeb.towTruckHub.server.initialize();
    });


};


