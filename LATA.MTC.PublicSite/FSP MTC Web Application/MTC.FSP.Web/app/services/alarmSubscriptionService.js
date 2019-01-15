'use strict';
(function () {
    mtcApp.factory('alarmSubscriptionService', function ($http, $log, $q, $rootScope, $location) {
        return {
            save: function (vm) {
                console.time('Saving Alarm Subscription');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/AlarmSubscriptions/SaveMyAlarms'
                }).
                then(function (response) {
                    console.timeEnd('Saving Alarm Subscription');
                    return response.data;
                });
            },
            getMyAlarms: function (vm) {
                console.time('Getting My Alarms');
                return $http({
                    method: 'GET',                    
                    url: $(".websiteUrl").text().trim() + '/AlarmSubscriptions/GetMyAlarms'
                }).
                then(function (response) {
                    console.timeEnd('Getting My Alarms');
                    return response.data;
                });
            }
        }

    });
}());