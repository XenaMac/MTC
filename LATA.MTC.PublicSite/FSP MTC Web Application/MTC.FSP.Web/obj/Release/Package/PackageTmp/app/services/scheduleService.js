'use strict';
(function () {
    mtcApp.factory('scheduleService', function ($http, $log, $q, $rootScope, $location) {
        return {
            getDailySchedule: function () {
                console.time('Getting Daily Schedule...');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/DailySchedule/GetDailySchedule'
                }).
                then(function (response) {
                    console.timeEnd('Getting Daily Schedule...');
                    return response.data;
                });
            }
          
        }

    });
}());