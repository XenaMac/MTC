'use strict';
(function () {
    mtcApp.factory('invoiceService', function ($http, $log, $q, $rootScope, $location) {
        return {
            getInvoiceSummary: function (month) {
                console.time('Getting Schedule Test');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Invoice/FillInvoiceSummaryGrid?month=' + month
                }).
                then(function (response) {
                    console.timeEnd('Getting Schedule Test');
                    return response.data;
                });
            },
            fillMonthDropDown: function (month) {
                console.time('Filling Month Drop Down');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Invoice/fillMonthDropDown'
                }).
                then(function (response) {
                    console.timeEnd('Filled Month Drop Down');
                    return response.data;
                });
            },
            getBeatContractorInfo: function (beatid, month, contractorid) {
                console.time('Getting Beat Contractor Info');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Invoice/getBeatContractorInfo?beatid=' + beatid + '&month=' + month + '&contractorid=' + contractorid
                }).
                then(function (response) {
                    console.timeEnd('Beat Contractor Info Retreived');
                    return response.data;
                });
            },
            getServiceSummary: function (beatid, month, contractorid) {
                console.time('Getting Service Summary');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Invoice/GetServiceSummary?beatid=' + beatid + '&month=' + month + '&contractorid=' + contractorid
                }).
                then(function (response) {
                    console.timeEnd('Survice Summary Retreived');
                    return response.data;
                });
            },
            getServiceSummary2: function (beatid, month, contractorid) {
                console.time('Getting Service Summary');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Invoice/GetServiceSummary2?beatid=' + beatid + '&month=' + month + '&contractorid=' + contractorid
                }).
                then(function (response) {
                    console.timeEnd('Survice Summary Retreived');
                    return response.data;
                });
            },
            getBaseRate: function (beatid) {
                console.time('Getting Base Rate for Beat');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Invoice/GetBaseRate?beatid=' + beatid
                }).
                then(function (response) {
                    console.timeEnd('Base Rate Retreived');
                    return response.data;
                });
            },
            getInvoiceAdditions: function (beatid, month, contractorid) {
                console.time('Getting additions for invoice');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Invoice/GetInvoiceAdditions?beatid=' + beatid + '&month=' + month + '&contractorid=' + contractorid
                }).
                then(function (response) {
                    console.timeEnd('Invoice Additions Retreived');
                    return response.data;
                });
            },
            getInvoiceDeductions: function (beatid, month, contractorid) {
                console.time('Getting deductions for invoice');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Invoice/GetInvoiceDeductions?beatid=' + beatid + '&month=' + month + '&contractorid=' + contractorid
                }).
                then(function (response) {
                    console.timeEnd('Invoice Deductions Retreived');
                    return response.data;
                });
            },
            saveInvoice: function (invoice) {
                console.time('Saving Invoice');
                return $http({
                    method: 'POST',
                    data: invoice,
                    url: $(".websiteUrl").text().trim() + '/Invoice/SaveInvoice'
                }).
                then(function (response) {
                    console.timeEnd('Invoice Saved');
                    return response.data;
                });
            },
            invoiceExists: function (invoiceNumber) {
                console.time('Saving Invoice');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Invoice/InvoiceExists?invoiceNumber=' + invoiceNumber,
                }).
                then(function (response) {
                    console.timeEnd('Invoice Saved');
                    return response.data;
                });
            },
            getInvoices: function () {
                console.time('Getting Invoice');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Invoice/GetInvoices',
                }).
                then(function (response) {
                    console.timeEnd('Invoices Retreived');
                    return response.data;
                });
            },
            getInvoice: function (invoiceNumber) {
                console.time('Getting Invoice');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Invoice/GetInvoice?invoiceNumber=' + invoiceNumber,
                }).
                then(function (response) {
                    console.timeEnd('Invoice Retreived');
                    return response.data;
                });
            },
            getBeatContractors: function (beatid) {
                console.time('Getting Beat Contractors');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Invoice/GetBeatContractors?beatid=' + beatid,
                }).
                then(function (response) {
                    console.timeEnd('Beat Contractors Retreived');
                    return response.data;
                });
            },
            getInvoiceAnamolies: function (beatid, month, contractorid) {
                console.time('Getting Invoice Anamolies');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Invoice/GetAnamolies?beatid=' + beatid + '&month=' + month + '&contractorid=' + contractorid
                }).
                then(function (response) {
                    console.timeEnd('Invoice Anamolies Retreived');
                    return response.data;
                });
            },
            deleteInvoice: function (invoiceNumber) {
                console.time('Deleting Invoice');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + '/Invoice/DeleteInvoice?invoiceNumber=' + invoiceNumber
                }).
                then(function (response) {
                    console.timeEnd('Invoice Deleted');
                    return response.data;
                });
            }
        }

    });
}());