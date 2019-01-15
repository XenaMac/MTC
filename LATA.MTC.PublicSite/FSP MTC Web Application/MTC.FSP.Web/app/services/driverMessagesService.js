'use strict';
(function () {
    mtcApp.factory('driverMessagesService', function ($http, $log, $q, $rootScope, $location) {
        return {
            sendDriverMessage: function (vm) {
                console.time('Sending Driver Message');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/DriverMessages/SendDriverMessage'
                }).
                then(function (response) {
                    console.timeEnd('Sending Driver Message');
                    return response.data;
                });
            },
            getMessages: function (id) {
                console.time('Getting Messages');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/DriverMessages/GetTodaysSummary'
                }).
                then(function (response) {
                    console.timeEnd('Getting Messages');
                    return response.data;
                });
            }
        }

    });
}());