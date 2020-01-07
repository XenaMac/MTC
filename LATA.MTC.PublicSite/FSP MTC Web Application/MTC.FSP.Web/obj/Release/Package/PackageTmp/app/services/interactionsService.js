'use strict';
(function () {
    mtcApp.factory('interactionsService', function ($http, $log, $q, $rootScope, $location) {
        return {
            getInteractions: function (vm) {
                console.time('Getting Interactions');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/DriverInteractions/GetInteractions'
                }).
                then(function (response) {
                    console.timeEnd('Getting Interactions');
                    return response.data;
                });
            },
        }
    });
}());