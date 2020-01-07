(function () {
    'use strict';
    mtcApp.factory('violationsService', function ($http, $log, $q, $rootScope, $location) {
        return {
            getViolations: function (vm) {
                console.time('Getting Violations');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/Violations/GetViolations'
                }).
                then(function (response) {
                    console.timeEnd('Getting Violations');
                    return response.data;
                });
            },
            saveViolation: function (vm) {
                console.time('Saving Violation');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/Violations/SaveViolation'
                }).
                then(function (response) {
                    console.timeEnd('Saving Violation');
                    return response.data;
                });
            },
            removeViolation: function (id) {
                console.time('Removing Violation');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Violations/RemoveViolation?id=' + id
                }).
                then(function (response) {
                    console.timeEnd('Removing Violation');
                    return response.data;
                });
            }
        }

    });
}());