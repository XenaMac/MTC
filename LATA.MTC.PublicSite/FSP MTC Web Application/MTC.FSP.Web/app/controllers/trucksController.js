'use strict';
(function() {
    mtcApp.controller('trucksController', function trucksController($scope, $interval, $rootScope, $location, $routeParams, trucksService) {
        $scope.trucks = [];
        $scope.selectAllAvailable = false;
        var getTrucksRefreshRate = function() {
            trucksService.getTrucksRefreshRate().then(function(results) {
                getTrucks();
                $interval(function() {
                    console.log('getting new trucks');
                    getTrucks();
                }, eval(results));
            });
        };
        var getTrucks = function() {
            trucksService.getTrucks().then(function(results) {
                for (var i = 0; i < results.length; i++) {
                    var exists = false;
                    for (var ii = 0; ii < $scope.trucks.length; ii++) {
                        if ($scope.trucks[ii].ipAddress === results[i].IPAddress) {
                            exists = true;
                            $scope.trucks[ii].update(results[i]);
                        }
                    }
                    if (!exists) $scope.trucks.push(new $rootScope.mtcTruck(results[i]));
                }
                $rootScope.redrawTable();
            });
        };
        $scope.selectTruck = function(truck) {
            $rootScope.$broadcast('selectedTruck', {
                selectedTruck: truck
            });
            trucksService.setSelectedTruck(truck.ipAddress).then(function(result) {
                console.log('Truck selected');
            });
        };
        $scope.selectAll = function() {
            for (var i = 0; i < $scope.trucks.length; i++) {
                $rootScope.$broadcast('selectedTruck', {
                    selectedTruck: $scope.trucks[i]
                });
            }
        };
        getTrucksRefreshRate();
    });
}());
