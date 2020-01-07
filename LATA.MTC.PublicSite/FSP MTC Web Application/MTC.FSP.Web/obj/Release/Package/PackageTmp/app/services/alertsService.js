'use strict';
(function () {
    mtcApp.factory('alertsService', function ($http, $log, $q, $rootScope, $location) {
        return {
            getAlertsRefreshRate: function () {
                console.time('Getting Alerts Refresh Rate');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Alerts/GetAlertsRefreshRate'
                }).
                then(function (response) {
                    console.timeEnd('Getting Alerts Refresh Rate');
                    return response.data;
                });
            },
            getAlerts: function () {
                console.time('Getting Alerts');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Alerts/GetAlerts'
                }).
                then(function (response) {
                    console.timeEnd('Getting Alerts');
                    return response.data;
                });
            },
            clearAlert: function (vm) {
                console.time('Clearing Alert');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/Alerts/ClearAlert'
                }).
                then(function (response) {
                    console.timeEnd('Clearing Alerts');
                    return response.data;
                });
            },
            excuseAlert: function (vm) {
                console.time('Excusing Alert');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/Alerts/ExcuseAlert'
                }).
                then(function (response) {
                    console.timeEnd('Excusing Alerts');
                    return response.data;
                });
            },

            getAlertDetails: function (vm) {
                console.time('Getting Alerts Details');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/Alerts/GetAlertDetails'
                }).
                then(function (response) {
                    console.timeEnd('Getting Alerts Details');
                    return response.data;
                });
            },
            updateAlertDetail: function (vm) {
                console.time('Update Alert Details');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/Alerts/UpdateAlertDetail'
                }).
                then(function (response) {
                    console.timeEnd('Update Alert Details');
                    return response.data;
                });
            },

            getDriverAlertComments: function (vm) {
                console.time('Getting Driver Alert Comments');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/Alerts/GetDriverAlertComments'
                }).
                then(function (response) {
                    console.timeEnd('Getting Driver Alert Comments');
                    return response.data;
                });
            },
        }

    });
}());