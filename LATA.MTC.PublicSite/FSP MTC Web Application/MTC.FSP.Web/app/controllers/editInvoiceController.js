(function () {
    'use strict';
    mtcApp.controller('editInvoiceController',
        function editInvoiceController($scope, $filter, $interval, $rootScope, $location, $routeParams, editInvoiceService, generalService) {

            $scope.header = 'Invoice';
            $scope.isBusy = false;
        }
    );
}());