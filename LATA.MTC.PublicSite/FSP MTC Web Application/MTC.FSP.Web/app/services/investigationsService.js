(function () {
    'use strict';
    mtcApp.factory('investigationsService', function ($http, $log, $q, $rootScope, $location) {
        return {
            getInvestigations: function (id) {
                console.time('Getting Investigations');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Investigations/GetInvestigations'
                }).
                then(function (response) {
                    console.timeEnd('Getting Investigations');
                    return response.data;
                });
            },
            saveInvestigation: function (vm) {
                console.time('Saving Investigation');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/Investigations/SaveInvestigation'
                }).
                then(function (response) {
                    console.timeEnd('Saving Investigation');
                    return response.data;
                });
            },
            removeInvestigation: function (id) {
                console.time('Removing Investigation');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Investigations/RemoveInvestigation?id=' + id
                }).
                then(function (response) {
                    console.timeEnd('Removing Investigation');
                    return response.data;
                });
            },
        }

    });
}());