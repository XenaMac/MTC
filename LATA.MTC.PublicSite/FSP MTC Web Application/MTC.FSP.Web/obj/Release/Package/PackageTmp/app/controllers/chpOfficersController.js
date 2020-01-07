'use strict';
(function () {
    mtcApp.controller('chpOfficersController',
        function chpOfficersController($scope, $filter, $rootScope, $location, $routeParams, chpOfficerService) {

            $scope.header = 'CHP Officers';
            $scope.chpOfficers = [];

            var getChpOfficers = function () {

                $scope.isBusy = true;

                chpOfficerService.getCHPOfficers().then(function (results) {
                    for (var i = 0; i < results.length; i++) {
                        $scope.chpOfficers.push(results[i]);
                    }

                    $scope.isBusy = false;

                });
            };

            getChpOfficers();

        }
    );
}());