(function () {
    'use strict';
    mtcApp.factory('shiftEntryService', function ($http, $log, $q, $rootScope, $location) {
        return {
            getCallSigns: function (BeatID) {
                console.time('Getting Call Signs');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/GetCallSigns?BeatID=' + BeatID
                }).
                then(function (response) {
                    console.timeEnd('Returning CallSigns');
                    return response.data;
                });
            },
            saveIncident: function (incident) {
                console.time('Saving Incident');
                return $http({
                    method: 'POST',
                    data: incident,
                    url: $(".websiteUrl").text().trim() + '/ShiftEntry/SaveIncident'
                }).
                then(function (response) {
                    console.timeEnd('Saving Incident');
                    return response.data;
                });
            }
        }

    });
}());