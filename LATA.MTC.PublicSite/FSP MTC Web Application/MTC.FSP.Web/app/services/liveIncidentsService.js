'use strict';
(function() {
    mtcApp.factory('liveIncidentsService', function($http, $log, $q, $rootScope, $location) {
        return {
            getLiveIncidentsRefreshRate: function() {
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/LiveIncidents/GetLiveIncidentRefreshInterval'
                }).
                then(function(response) {
                    return response.data;
                });
            },
            getLiveIncidents: function() {
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + "/LiveIncidents/GetIncidents"
                }).
                then(function(response) {
                    return response.data;
                });
            }
        }

    });
}());