'use strict';
(function () {
    mtcApp.factory('dispatchingService', function ($http, $log, $q, $rootScope, $location) {
        return {
            doDispatch: function (vm) {
                console.time('Dispatching...');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/Dispatching/DoDispatch'
                }).
                then(function (response) {
                    console.timeEnd('Dispatching...');
                    return response.data;
                });
            },
            getDispatchCodes: function () {
                console.time('Getting Dispatch Codes');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/DispatchCodes/GetAll'
                }).
                then(function (response) {
                    console.timeEnd('Getting Dispatch Codes');
                    return response.data;
                });
            }
        }

    });
}());