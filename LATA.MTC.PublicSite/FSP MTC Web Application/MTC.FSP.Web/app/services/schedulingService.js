(function () {
    'use strict';
    mtcApp.factory('schedulingService', function ($http, $log, $q, $rootScope, $location) {
        return {
            getHolidayDates: function () {
                console.time('Getting Holiday Dates');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Scheduling/GetHolidayDates'
                }).
                then(function (response) {
                    console.timeEnd('Getting Holiday Dates');
                    return response.data;
                });
            },
            getCustomDates: function () {
                console.time('Getting Custom Dates');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Scheduling/GetCustomDates'
                }).
                then(function (response) {
                    console.timeEnd('Getting Custom Dates');
                    return response.data;
                });
            },
            getSchedules: function () {
                console.time('Getting Schedules');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Scheduling/GetSchedules'
                }).
                then(function (response) {
                    console.timeEnd('Getting Schedules');
                    return response.data;
                });
            },
            getHolidaySchedules: function () {
                console.time('Getting Holiday Schedules');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Scheduling/GetHolidaySchedules'
                }).
                then(function (response) {
                    console.timeEnd('Getting Holiday Schedules');
                    return response.data;
                });
            },
            getCustomSchedules: function () {
                console.time('Getting Custom Schedules');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Scheduling/GetCustomSchedules'
                }).
                then(function (response) {
                    console.timeEnd('Getting Custom Schedules');
                    return response.data;
                });
            },
            getHolidayScheduleTimes: function (holidayDateId) {
                console.time('Getting Holiday Schedule Times...');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Scheduling/GetHolidayScheduleTimes?holidayDateId=' + holidayDateId
                }).
                then(function (response) {
                    console.timeEnd('Getting Holiday Schedule Times...');
                    return response.data;
                });
            },
            getCustomScheduleTimes: function (customDateId) {
                console.time('Getting Custom Schedule Times...');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Scheduling/GetCustomScheduleTimes?customDateId=' + customDateId
                }).
                then(function (response) {
                    console.timeEnd('Getting Custom Schedule Times...');
                    return response.data;
                });
            },
            getBeatSchedules: function () {
                console.time('Getting Beat Schedules...');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Scheduling/GetBeatSchedules'
                }).
                then(function (response) {
                    console.timeEnd('Getting Beat Schedules...');
                    return response.data;
                });
            },

            saveHolidayDate: function (vm) {
                console.time('Saving Holiday Date');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/Scheduling/SaveHolidayDate'
                }).
                then(function (response) {
                    console.timeEnd('Saving Holiday Dates');
                    return response.data;
                });
            },
            saveCustomDate: function (vm) {
                console.time('Saving Custom Date');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/Scheduling/SaveCustomDate'
                }).
                then(function (response) {
                    console.timeEnd('Saving Custom Dates');
                    return response.data;
                });
            },
            saveHolidaySchedule: function (vm) {
                console.time('Saving Holiday Schedule');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/Scheduling/SaveHolidaySchedule'
                }).
                then(function (response) {
                    console.timeEnd('Saving Holiday Schedule');
                    return response.data;
                });
            },
            saveCustomSchedule: function (vm) {
                console.time('Saving Custom Schedule');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/Scheduling/SaveCustomSchedule'
                }).
                then(function (response) {
                    console.timeEnd('Saving Custom Schedule');
                    return response.data;
                });
            },            
            saveSchedule: function (vm) {
                console.time('Saving Schedule');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/Scheduling/SaveSchedule'
                }).
                then(function (response) {
                    console.timeEnd('Saving Schedule');
                    return response.data;
                });
            },
            saveBeatSchedule: function (vm) {
                console.time('Saving Beat Schedule');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/Scheduling/SaveBeatSchedule'
                }).
                then(function (response) {
                    console.timeEnd('Saving Beat Schedule');
                    return response.data;
                });
            },


            removeHolidayDate: function (id) {
                console.group('Removing Holiday Date');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Scheduling/RemoveHolidayDate?id=' + id
                }).
                then(function (response) {
                    console.log(response);
                    console.groupEnd('Removing Holiday Dates');
                    return response.data;
                });
            },
            removeHolidaySchedule: function (id) {
                console.time('Removing Holiday Schedule');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Scheduling/RemoveHolidaySchedule?id=' + id
                }).
                then(function (response) {
                    console.timeEnd('Removing Holiday Schedule');
                    return response.data;
                });
            },

            removeCustomDate: function (id) {
                console.time('Removing Custom Date');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Scheduling/RemoveCustomDate?id=' + id
                }).
                then(function (response) {
                    console.timeEnd('Removing Custom Dates');
                    return response.data;
                });
            },          
            removeCustomSchedule: function (id) {
                console.time('Removing Custom Schedule');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Scheduling/RemoveCustomSchedule?id=' + id
                }).
                then(function (response) {
                    console.timeEnd('Removing Custom Schedule');
                    return response.data;
                });
            },

            removeSchedule: function (id) {
                console.time('Removing Schedule');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Scheduling/RemoveSchedule?id=' + id
                }).
                then(function (response) {
                    console.timeEnd('Removing Schedule');
                    return response.data;
                });
            }           
        }

    });
}());