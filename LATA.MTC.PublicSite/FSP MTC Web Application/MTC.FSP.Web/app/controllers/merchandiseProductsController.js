'use strict';
(function() {
    mtcApp.controller('merchandiseProductsController', function merchandiseProductsController($scope, $filter, $interval, $rootScope, $location, $routeParams, merchandiseService) {
        $scope.header = 'Products';
        $scope.products = [];
        $scope.columns = [];
        $scope.columns.push(new $rootScope.mtcColumn("ID", "ID", true, true));
        $scope.columns.push(new $rootScope.mtcColumn("Name", "DisplayName", true, true));
        $scope.columns.push(new $rootScope.mtcColumn("Order", "OrderNumber", true, true));
        $scope.columns.push(new $rootScope.mtcColumn("Unit Cost", "UnitCost", true, true));
        $scope.columns.push(new $rootScope.mtcColumn("Units In Stock", "UnitsInStock", true, true));
        $scope.columns.push(new $rootScope.mtcColumn("Size", "Size", true, true));
        $scope.columns.push(new $rootScope.mtcColumn("Description", "Description", true, true));
        $scope.isBusyGettingProducts = true;
        $scope.getProducts = function() {
            merchandiseService.getProducts().then(function(results) {
                $scope.products = [];
                for (var i = 0; i < results.length; i++) {
                    $scope.products.push(results[i]);
                }
                $scope.isBusyGettingProducts = false;
            });
        };
        $scope.moveUp = function(product) {
            product.isBusy = true;
            merchandiseService.moveUp(product.Id).then(function(results) {
                $scope.getProducts();
                product.isBusy = false;
                toastr.success("Complete", "Sort");
            });
        };
        $scope.moveDown = function(product) {
            product.isBusy = true;
            merchandiseService.moveDown(product.Id).then(function(results) {
                $scope.getProducts();
                product.isBusy = false;
                toastr.success("Complete", "Sort");
            });
        };
        $scope.getProducts();
    });
}());
