(function() {
    'use strict';
    mtcApp.factory('generalService', function($http, $log, $q, $rootScope, $location) {
        return {
            getDrivers: function() {
                console.time('Getting Drivers');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/GetDrivers'
                }).
                then(function(response) {
                    console.timeEnd('Getting Drivers');
                    return response.data;
                });
            },
            getCHPOfficers: function() {
                console.time('Getting CHP Officers');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/GetCHPOfficers'
                }).
                then(function(response) {
                    console.timeEnd('Getting CHP Officers');
                    return response.data;
                });
            },
            getContractorDrivers: function(contractorId) {
                console.time('Getting Contractor Drivers');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/GetContractorDrivers?contractorId=' + contractorId
                }).
                then(function(response) {
                    console.timeEnd('Getting Contractor Drivers');
                    return response.data;
                });
            },
            getContractors: function(contractorTypeName) {
                console.time('Getting Contractors');

                var url = '';
                if (contractorTypeName !== undefined)
                    url = $(".websiteUrl").text().trim() + '/Home/GetContractors?contractorTypeName=' + contractorTypeName;
                else
                    url = $(".websiteUrl").text().trim() + '/Home/GetContractors';

                return $http({
                    method: 'GET',
                    url: url
                }).
                then(function(response) {
                    console.timeEnd('Getting Contractors');
                    return response.data;
                });
            },
            getBackupProvidingContractors: function() {
                console.time('Getting Backup Providers');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/GetBackupProvidingContractors'
                }).
                then(function(response) {
                    console.timeEnd('Getting Backup Providers');
                    return response.data;
                });
            },
            getVehicles: function() {
                console.time('Getting Vehicles');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/GetVehicles'
                }).
                then(function(response) {
                    console.timeEnd('Getting Vehicles');
                    return response.data;
                });
            },
            getCallSigns: function() {
                console.time('Getting Call-Signs');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/GetCallSigns'
                }).
                then(function(response) {
                    console.timeEnd('Getting Call-Signs');
                    return response.data;
                });
            },
            getAlarmTypes: function() {
                console.time('Getting Alarm Types');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/GetAlarmTypes'
                }).
                then(function(response) {
                    console.timeEnd('Getting Alarm Types');
                    return response.data;
                });
            },
            getContractorVehicles: function(contractorId) {
                console.time('Getting Contractor Vehicles');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/GetContractorVehicles?contractorId=' + contractorId
                }).
                then(function(response) {
                    console.timeEnd('Getting Contractor Vehicles');
                    return response.data;
                });
            },
            getContractorBeats: function(contractorId) {
                console.time('Getting Contractor Beats');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/GetContractorBeats?contractorId=' + contractorId
                }).
                then(function(response) {
                    console.timeEnd('Getting Contractor Beats');
                    return response.data;
                });
            },
            getBeatNumbers: function() {
                console.time('Getting BeatNumbers');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/GetBeatNumbers'
                }).
                then(function(response) {
                    console.timeEnd('Getting BeatNumbers');
                    return response.data;
                });
            },
            getIncidentTypes: function() {
                console.time('Getting IncidentTypes');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/GetIncidentTypes'
                }).
                then(function(response) {
                    console.timeEnd('Getting IncidentTypes');
                    return response.data;
                });
            },
            getTrafficSpeeds: function() {
                console.time('Getting TrafficSpeeds');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/GetTrafficSpeeds'
                }).
                then(function(response) {
                    console.timeEnd('Getting TrafficSpeeds');
                    return response.data;
                });
            },
            getFreeways: function() {
                console.time('Getting Freeways');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/GetFreeways'
                }).
                then(function(response) {
                    console.timeEnd('Getting Freeways');
                    return response.data;
                });
            },
            getBeatsFreewaysByBeat: function(beat) {
                console.time('Getting Freeways');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/getBeatsFreewaysByBeat?beatNumber=' + beat
                }).
                then(function(response) {
                    console.timeEnd('Getting Freeways');
                    return response.data;
                });
            },
            getVehiclePositions: function() {
                console.time('Getting VehiclePositions');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/GetVehiclePositions'
                }).
                then(function(response) {
                    console.timeEnd('Getting VehiclePositions');
                    return response.data;
                });
            },
            getVehicleTypes: function() {
                console.time('Getting VehicleTypes');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/GetVehicleTypes'
                }).
                then(function(response) {
                    console.timeEnd('Getting VehicleTypes');
                    return response.data;
                });
            },
            getTowLocations: function() {
                console.time('Getting TowLocations');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/GetTowLocations'
                }).
                then(function(response) {
                    console.timeEnd('Getting TowLocations');
                    return response.data;
                });
            },
            getServiceTypes: function() {
                console.time('Getting ServiceTypes');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/GetServiceTypes'
                }).
                then(function(response) {
                    console.timeEnd('Getting ServiceTypes');
                    return response.data;
                });
            },
            getAssistEntryUrl: function() {
                console.time('Getting Assist Entry URL');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/GetAssistEntryUrl'
                }).
                then(function(response) {
                    console.timeEnd('Getting Assist Entry URL');
                    return response.data;
                });
            },
            getBeatSchedules: function() {
                console.time('Getting Beat Schedules');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/GetBeatSchedules'
                }).
                then(function(response) {
                    console.timeEnd('Getting Beat Schedules');
                    return response.data;
                });
            },
            getViolationTypes: function() {
                console.time('Getting ViolationTypes');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/ViolationTypes/GetAll'
                }).
                then(function(response) {
                    console.timeEnd('Getting ViolationTypes');
                    return response.data;
                });
            },
            getViolationStatusTypes: function(id) {
                console.time('Getting Violation Status Types');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/ViolationStatusTypes/GetAll'
                }).
                then(function(response) {
                    console.timeEnd('Getting Violation Status Types');
                    return response.data;
                });
            },
            getCurrentUser: function() {
                console.time('Getting Current User');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/GetCurrentUser'
                }).
                then(function(response) {
                    console.timeEnd('Getting Current User');
                    return response.data;
                });
            },
            getCanEdit: function(currentControllerName) {
                console.time('Getting Can Edit');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Home/CanEdit?currentControllerName=' + currentControllerName
                }).
                then(function(response) {
                    console.timeEnd('Getting Can Edit');
                    return response.data;
                });
            }
        };
    });
}());