(function () {
    'use strict';
    mtcApp.factory('chpOfficerService', function ($http, $log, $q, $rootScope, $location) {
        return {
            getCHPOfficers: function () {
                console.time('Getting CHP Officers');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/CHPOfficers/GetCHPOfficers'
                }).
                then(function (response) {
                    console.timeEnd('Getting CHP Officers');
                    return response.data;
                });
            },
            getCHPOfficer: function (id) {
                console.time('Getting CHP Officer');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/CHPOfficers/GetCHPOfficer?id=' + id
                }).
                then(function (response) {
                    console.timeEnd('Getting CHP Officer');
                    return response.data;
                });
            },
            save: function (vm) {
                console.time('Saving CHP Officer');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/CHPOfficers/Save'
                }).
                then(function (response) {
                    console.timeEnd('Saving CHP Officer');
                    return response.data;
                });
            },
            getAvailableBeatNumbers: function () {
                console.time('Getting BeatNumbers');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/CHPOfficers/GetAvailableBeats'
                }).
                then(function (response) {
                    console.timeEnd('Getting BeatNumbers');
                    return response.data;
                });
            }
        }

    });
}());