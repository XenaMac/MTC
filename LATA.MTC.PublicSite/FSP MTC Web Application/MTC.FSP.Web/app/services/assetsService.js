'use strict';
(function () {
    mtcApp.factory('assetsService', function ($http, $log, $q, $rootScope, $location) {
        return {
            getAssetsAndLocations: function () {
                console.time('Getting Assets & Locations');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/AssetStatusLocations/GetAll'
                }).
                then(function (response) {
                    console.timeEnd('Getting Assets & Locations');
                    return response.data;
                });
            },
            getAssetsWarranties: function () {
                console.time('Getting Assets Warranties');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/AssetWarranties/GetAll'
                }).
                then(function (response) {
                    console.timeEnd('Getting Assets Warranties');
                    return response.data;
                });
            },
        }

    });
}());