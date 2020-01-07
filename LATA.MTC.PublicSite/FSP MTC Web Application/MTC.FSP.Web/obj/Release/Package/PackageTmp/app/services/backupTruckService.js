(function () {
    'use strict';
    mtcApp.factory('backupTruckService', function ($http, $log, $q, $rootScope, $location) {
        return {
            getUrgentBackupValue:function(){
                console.time('Getting Urgent Back-up value');

                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/BackupTrucks/GetUrgentBackupValue'
                }).
                then(function (response) {
                    console.timeEnd('Getting Urgent Back-up value');
                    return response.data;
                });
            },
            getStandardBackupValue:function(){
                console.time('Getting Standard Back-up value');

                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/BackupTrucks/GetStandardBackupValue'
                }).
                then(function (response) {
                    console.timeEnd('Getting Standard Back-up value');
                    return response.data;
                });
            },
            getBackupReasons: function () {
                console.time('Getting Back-up Reasons');

                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/BackupTrucks/GetBackupReasons'
                }).
                then(function (response) {
                    console.timeEnd('Getting Back-up Reasons');
                    return response.data;
                });
            },
            getBackupDeclinationReasons: function () {
                console.time('Getting Back-up Declination Reasons');

                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/BackupTrucks/GetBackupDeclinationReasons'
                }).
                then(function (response) {
                    console.timeEnd('Getting Back-up Declination Reasons');
                    return response.data;
                });
            },
            getBackupCancellationReasons: function () {
                console.time('Getting Back-up Request Cancellation Reasons');

                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/BackupTrucks/GetBackupCancellationReasons'
                }).
                then(function (response) {
                    console.timeEnd('Getting Back-up Request Cancellation Reasons');
                    return response.data;
                });
            },
            getBackupAssignments: function () {
                console.time('Getting Back-up Assignments');

                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/BackupTrucks/GetBackupAssignments'
                }).
                then(function (response) {
                    console.timeEnd('Getting Back-up Assignments');
                    return response.data;
                });
            },
            getBackupProviders: function () {
                console.time('Getting Back-up Providers');

                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/BackupTrucks/GetBackupProviders'
                }).
                then(function (response) {
                    console.timeEnd('Getting Back-up Providers');
                    return response.data;
                });
            },
            getBackupBeats: function () {
                console.time('Getting Back-up Beats');

                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/BackupTrucks/GetBackupBeats'
                }).
                then(function (response) {
                    console.timeEnd('Getting Back-up Beats');
                    return response.data;
                });
            },
            getBackupRequests: function (contractorId) {
                console.time('Getting Back-up Requests');

                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/BackupTrucks/GetBackupRequests?contractorId=' + contractorId
                }).
                then(function (response) {
                    console.timeEnd('Getting Back-up Requests');
                    return response.data;
                });
            },
            getBackupRequestStatuses: function () {
                console.time('Getting Back-up Request Statuses');

                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/BackupTrucks/GetBackupRequestStatuses'
                }).
                then(function (response) {
                    console.timeEnd('Getting Back-up Request Statuses');
                    return response.data;
                });
            },
            getBackupAssignmentsByBeat: function (beatId) {
                console.time('Getting Back-up Assignments');

                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/BackupTrucks/GetBackupAssignemtByBeat?beatId=' + beatId
                }).
                then(function (response) {
                    console.timeEnd('Getting Back-up Assignments');
                    return response.data;
                });
            },
            saveBackupProvider: function (vm) {

                console.time('Saving Backup Provider');

                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/BackupTrucks/SaveBackupProvider'
                }).
                then(function (response) {
                    console.timeEnd('Saving Backup Provider');
                    return response.data;
                });
            },
            saveBackupBeat: function (vm) {

                console.time('Saving Backup Beat');

                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/BackupTrucks/SaveBackupBeat'
                }).
                then(function (response) {
                    console.timeEnd('Saving Backup Beat');
                    return response.data;
                });
            },
            saveBackupAssignment: function (vm) {

                console.time('Saving Backup Assignment');

                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/BackupTrucks/SaveBackupAssignment'
                }).
                then(function (response) {
                    console.timeEnd('Saving Backup Assignment');
                    return response.data;
                });
            },
            requestBackup: function (vm) {
                console.time('Requesting Backup');

                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/BackupTrucks/RequestBackup'
                }).
                then(function (response) {
                    console.timeEnd('Requesting Backup');
                    return response.data;
                });
            },
            responseToBackupRequest: function (vm) {
                console.time('Response to Back-up request');

                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/BackupTrucks/RespondToBackupRequest'
                }).
                then(function (response) {
                    console.timeEnd('Response to Back-up request');
                    return response.data;
                });
            },
            cancelBackupRequest: function (vm) {
                console.time('Response to Back-up request');

                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/BackupTrucks/CancelBackupRequest'
                }).
                then(function (response) {
                    console.timeEnd('Response to Back-up request');
                    return response.data;
                });
            },
            removeBackupProvider: function (id) {

                console.time('Removing Backup Provider');

                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/BackupTrucks/RemoveBackupProvider?id=' + id
                }).
                then(function (response) {
                    console.timeEnd('Removing Backup Provider');
                    return response.data;
                });
            },
            removeBackupBeat: function (id) {

                console.time('Removing Backup Beat');

                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/BackupTrucks/RemoveBackupBeat?id=' + id
                }).
                then(function (response) {
                    console.timeEnd('Removing Backup Beat');
                    return response.data;
                });
            },
            removeBackupAssignment: function (id) {

                console.time('Removing Backup Assignment');

                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/BackupTrucks/RemoveBackupAssignment?id=' + id
                }).
                then(function (response) {
                    console.timeEnd('Removing Backup Assignment');
                    return response.data;
                });
            },
            getScheduleAvailabilityForDateAndBeat: function (beatId, date) {
                console.time('Getting Schedule Availability');

                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Scheduling/GetScheduleAvailabilityForDateAndBeat?beatId=' + beatId + '&date=' + date
                }).
                then(function (response) {
                    console.timeEnd('Getting Schedule Availability');
                    return response.data;
                });
            },
        };

    });
}());