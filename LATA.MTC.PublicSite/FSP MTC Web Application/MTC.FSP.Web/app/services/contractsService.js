(function () {
    'use strict';
    mtcApp.factory('contractsService', function ($http, $log, $q, $rootScope, $location) {
        return {
            getContracts: function () {
                console.time('Getting Contracts');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Contracts/GetContracts'
                }).
                then(function (response) {
                    console.timeEnd('Getting Contracts');
                    return response.data;
                });
            },
            getContract: function (id) {
                console.time('Getting Contract');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Contracts/GetContract?id=' + id
                }).
                then(function (response) {
                    console.timeEnd('Getting Contract');
                    return response.data;
                });
            },
            saveContract: function (vm) {
                console.time('Saving Contract');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/Contracts/SaveContract'
                }).
                then(function (response) {
                    console.timeEnd('Saving Contract');
                    return response.data;
                });
            },           
            getAvailableBeatNumbers: function () {
                console.time('Getting BeatNumbers');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Contracts/GetAvailableBeats'
                }).
                then(function (response) {
                    console.timeEnd('Getting BeatNumbers');
                    return response.data;
                });
            }
        }

    });
}());