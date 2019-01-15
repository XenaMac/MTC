'use strict';
(function () {
    mtcApp.factory('beatsService', function ($http, $log, $q, $rootScope, $location) {
        return {
            getBeatCallSigns: function () {
                console.time('Getting Beat Call Signs');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/BeatCallSigns/GetAll'
                }).
                then(function (response) {
                    console.timeEnd('Getting Beat Call Signs');
                    return response.data;
                });
            },
            getBeatCallSign: function (id) {
                console.time('Getting Beat Call Sign');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/BeatCallSigns/GetOne?BeatId=' + id
                }).
                then(function (response) {
                    console.timeEnd('Getting Beat Call Sign');
                    return response.data;
                });
            },
            saveBeatCallSign: function (vm) {
                console.time('Saving Beat Call Sign');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/BeatCallSigns/Save'
                }).
                then(function (response) {
                    console.timeEnd('Saving Beat Call Sign');
                    return response.data;
                });
            }
        }

    });
}());