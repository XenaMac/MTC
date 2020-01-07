(function () {
    'use strict';
    mtcApp.factory('fleetVehicleService', function ($http, $log, $q, $rootScope, $location) {
        return {
            getFleetVehicle: function (id) {
                console.time('Getting Fleet Vehicle');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/FleetVehicles/Get?id=' + id
                }).
                then(function (response) {
                    console.timeEnd('Getting Fleet Vehicle');
                    return response.data;
                });
            },
            saveFleetVehicle: function (vm) {
                console.time('Saving Fleet Vehicle');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/FleetVehicles/Save'
                }).
                then(function (response) {
                    console.timeEnd('Saving Fleet Vehicle');
                    return response.data;
                });
            },
            removeFleetVehicle: function (id) {
                console.time('Removing Fleet Vehicle');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/FleetVehicles/RemoveFleetVehicle?fleetVehicleId=' + id
                }).
                then(function (response) {
                    console.timeEnd('Removing Fleet Vehicle');
                    return response.data;
                });
            },
        }

    });
}());