'use strict';
(function () {
    mtcApp.factory('inspectionsService', function ($http, $log, $q, $rootScope, $location) {
        return {
            getInspections: function (vm) {
                console.time('Getting Inspections');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/CHPInspections/GetInspections'
                }).
                then(function (response) {
                    console.timeEnd('Getting Inspections');
                    return response.data;
                });
            },
        }

    });
}());