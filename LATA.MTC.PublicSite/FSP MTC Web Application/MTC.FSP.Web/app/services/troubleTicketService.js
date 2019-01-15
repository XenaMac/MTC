'use strict';
(function () {
    mtcApp.factory('troubleTicketService', function ($http, $log, $q, $rootScope, $location) {
        return {
            getTroubleTickets: function () {
                console.time('Getting Trouble Tickets');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/TroubleTickets/GetTroubleTickets'
                }).
                then(function (response) {
                    console.timeEnd('Getting Trouble Tickets');
                    return response.data;
                });
            },
            getUnResolvedTroubleTickets: function () {
                console.time('Getting Unresolved Trouble Tickets');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/TroubleTickets/GetUnResolvedTroubleTickets'
                }).
                then(function (response) {
                    console.timeEnd('Getting Unresolved Trouble Tickets');
                    return response.data;
                });
            },
            getTowContractorsTroubleTickets: function (contractorId, contractorType) {
                console.time('Getting Tow Contractors Trouble Tickets');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/TroubleTickets/GetTowContractorsTroubleTickets?contractorId=' + contractorId + '&contractorType=' + contractorType
                }).
                then(function (response) {
                    console.timeEnd('GettingTow Contractors Trouble Tickets');
                    return response.data;
                });
            },
            getTroubleTicketProblems: function () {
                console.time('Getting Trouble Ticket Problems');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/TroubleTickets/GetTroubleTicketProblems'
                }).
                then(function (response) {
                    console.timeEnd('Getting Trouble Ticket Problems');
                    return response.data;
                });
            },
            getTroubleTicketComponentIssues: function () {
                console.time('Getting Trouble Ticket Component Issues');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/TroubleTickets/GetTroubleTicketComponentIssues'
                }).
                then(function (response) {
                    console.timeEnd('Getting Trouble Ticket Component Issues');
                    return response.data;
                });
            },
            getTroubleTicketLATATraxIssues: function () {
                console.time('Getting Trouble Ticket LATATrax Issues');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/TroubleTickets/GetTroubleTicketLATATraxIssues'
                }).
                then(function (response) {
                    console.timeEnd('Getting Trouble Ticket LATATrax Issues');
                    return response.data;
                });
            },
            getTroubleTicketsSnapshot: function () {
                console.time('Getting Trouble Tickets Snapshot');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/TroubleTickets/GetTroubleTicketsSnapshot'
                }).
                then(function (response) {
                    console.timeEnd('Getting Trouble Tickets Snapshot');
                    return response.data;
                });
            },
            saveTroubleTicket: function (vm) {
                console.time('Saving Trouble Ticket');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/TroubleTickets/SaveTroubleTicket'
                }).
                then(function (response) {
                    console.timeEnd('Saving Trouble Ticket');
                    return response.data;
                });
            },
            saveTroubleTicketReplacementData: function (vm) {
                console.time('Saving Trouble Ticket Replacement Data');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/TroubleTickets/SaveTroubleTicketReplacementData'
                }).
                then(function (response) {
                    console.timeEnd('Saving Trouble Ticket Replacement Data');
                    return response.data;
                });
            },
            saveTroubleTicketMaintenanceData: function (vm) {
                console.time('Saving Trouble Ticket Maintenance Data');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/TroubleTickets/SaveTroubleTicketMaintenanceData'
                }).
                then(function (response) {
                    console.timeEnd('Saving Trouble Ticket Maintenance Data');
                    return response.data;
                });
            },
            resolveTroubleTicketAdmin: function (vm) {
                console.time('Resolving Trouble Ticket');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + '/TroubleTickets/ResolveTroubleTicketAdmin'
                }).
                then(function (response) {
                    console.timeEnd('Resolving Trouble Ticket');
                    return response.data;
                });
            },
            resolveTroubleTicket: function (Id) {
                console.time('Resolving Trouble Ticket');
                return $http({
                    method: 'GET',                    
                    url: $(".websiteUrl").text().trim() + '/TroubleTickets/ResolveTroubleTicket?Id=' + Id
                }).
                then(function (response) {
                    console.timeEnd('Resolving Trouble Ticket');
                    return response.data;
                });
            }
        }

    });
}());