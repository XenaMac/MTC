(function () {
    'use strict';
    mtcApp.factory('contractorService', function ($http, $log, $q, $rootScope, $location) {
        return {           
            getContractor: function (id) {
                console.time('Getting Contractor');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Contractors/GetContractor?id=' + id
                }).
                then(function (response) {
                    console.timeEnd('Getting Contractor');
                    return response.data;
                });
            },
            getContractorTypes: function (id) {
                console.time('Getting Contractor Types');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Contractors/GetContractorTypes'
                }).
                then(function (response) {
                    console.timeEnd('Getting Contractor Types');
                    return response.data;
                });
            },
            saveContractor: function (vm) {
                console.time('Saving Contractor');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/Contractors/SaveContractor'
                }).
                then(function (response) {
                    console.timeEnd('Saving Contractor');
                    return response.data;
                });
            },
          
        }

    });
}());