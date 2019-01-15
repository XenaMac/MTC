'use strict';
(function () {
    mtcApp.factory('beatSchedulesService', function ($http, $log, $q, $rootScope, $location) {
        return {
            getBeatSchedules: function () {
                console.time('Getting Beat Schedules');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/BeatSchedules/GetBeatSchedules'
                }).
                then(function (response) {
                    console.timeEnd('Getting Beat Schedules');
                    return response.data;
                });
            },
            getSchedules: function () {
                console.time('Getting Schedules');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/BeatSchedules/GetSchedules'
                }).
                then(function (response) {
                    console.timeEnd('Getting Schedules');
                    return response.data;
                });
            },
            getBeatsWithoutSchedules: function () {
                console.time('Getting Beats without Schedules');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/BeatSchedules/GetBeatsWithoutSchedules'
                }).
                then(function (response) {
                    console.timeEnd('Getting Beats without Schedules');
                    return response.data;
                });
            },
            saveBeatSchedule: function (vm) {
                console.time('Saving Beat Schedules');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/BeatSchedules/Save'
                }).
                then(function (response) {
                    console.timeEnd('Saving Beat Schedules');
                    return response.data;
                });
            },
        }

    });
}());