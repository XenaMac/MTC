'use strict';
(function() {
    mtcApp.factory('shiftLogService', function($http) {
        return {
            getShiftLog: function(truckNumber) {
                console.time('Getting Shift Log');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/ShiftLog/GetAll?truckNumber=' + truckNumber
                }).
                then(function(response) {
                    console.timeEnd('Getting Shift Log');
                    return response.data;
                });
            }
        }

    });
}());