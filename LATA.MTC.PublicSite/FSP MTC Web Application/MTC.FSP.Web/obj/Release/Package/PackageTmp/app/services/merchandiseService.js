'use strict';
(function () {
    mtcApp.factory('merchandiseService', function ($http, $log, $q, $rootScope, $location) {
        return {
            getProducts: function () {
                console.time('Getting Products');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + "/MerchandiseProducts/GetProducts"
                }).
                then(function (response) {
                    console.timeEnd('Getting Products');
                    return response.data;
                });
            },
            getOrders: function () {
                console.time('Getting Orders');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + "/Merchandise/GetOrders"
                }).
                then(function (response) {
                    console.timeEnd('Getting Orders');
                    return response.data;
                });
            },
            getOrderDetails: function (id) {
                console.time('Getting Order Details');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + "/Merchandise/GetOrderDetails?id=" + id
                }).
                then(function (response) {
                    console.timeEnd('Getting Order Details');
                    return response.data;
                });
            },
            cancelOrder: function (vm) {
                console.time('Cancelling Order');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + "/Merchandise/CancelOrder"
                }).
                then(function (response) {
                    console.timeEnd('Cancelling Order');
                    return response.data;
                });
            },
            declineOrder: function (vm) {
                console.time('Declining Order');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + "/Merchandise/DeclineOrder"
                }).
                then(function (response) {
                    console.timeEnd('Declining Order');
                    return response.data;
                });
            },
            fulFillOrder: function (vm) {
                console.time('Fulfilling Order');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + "/Merchandise/FulFillOrder"
                }).
                then(function (response) {
                    console.timeEnd('Fulfilling Order');
                    return response.data;
                });
            },
            submitOrder: function (vm) {
                console.time('Submitting Merchandise Order');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + "/Merchandise/SubmitOrder"
                }).
                then(function (response) {
                    console.timeEnd('Submitting Merchandise Order');
                    return response.data;
                });
            },
            saveOrder: function (vm) {
                console.time('Saving Merchandise Order');
                return $http({
                    method: 'POST',
                    data: vm,
                    url: $(".websiteUrl").text().trim() + "/Merchandise/SaveOrder"
                }).
                then(function (response) {
                    console.timeEnd('Saving Merchandise Order');
                    return response.data;
                });
            },
            moveUp: function (productId) {
                console.time('Moving Up Product');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + "/MerchandiseProducts/MoveUp?Id=" + productId
                }).
                then(function (response) {
                    console.timeEnd('Moving Up Product');
                    return response.data;
                });
            },
            moveDown: function (productId) {
                console.time('Moving Up Product');
                return $http({
                    method: 'GET',
                    url: $(".websiteUrl").text().trim() + "/MerchandiseProducts/MoveDown?Id=" + productId
                }).
                then(function (response) {
                    console.timeEnd('Moving Up Product');
                    return response.data;
                });
            }
        }

    });
}());