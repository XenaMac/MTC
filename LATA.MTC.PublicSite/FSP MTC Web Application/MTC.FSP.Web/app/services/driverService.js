(function () {
    'use strict';
    mtcApp.factory('driverService', function ($http, $log, $q, $rootScope, $location) {
        return {
            getDriver: function (id) {
                console.time('Getting Driver');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Drivers/Get?id=' + id
                }).
                then(function (response) {
                    console.timeEnd('Getting Driver');
                    return response.data;
                });
            },
            saveDriver: function (vm) {
                console.time('Saving Driver');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/Drivers/Save'
                }).
                then(function (response) {
                    console.timeEnd('Saving Driver');
                    return response.data;
                });
            },
            removeDriver: function (id) {
                console.time('Removing Driver');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Drivers/Remove?driverId=' + id
                }).
                then(function (response) {
                    console.timeEnd('Removing Driver');
                    return response.data;
                });
            },
        }

    });
}());