(function() {
    'use strict';
    mtcApp.controller('shiftLogController',
        function shiftLogController($scope, $rootScope, shiftLogService) {

            $scope.shiftLogs = [];
            $scope.isBusyGettingShiftLog = false;

            $scope.init = function(truckNumber) {
                console.log('TruckNumber %s', truckNumber);
                if (truckNumber) {
                    $scope.truckNumber = truckNumber;
                    $scope.getShiftLog();
                }
            };

            $scope.getShiftLog = function() {
                $scope.isBusyGettingShiftLog = true;
                shiftLogService.getShiftLog($scope.truckNumber).then(function(results) {
                    $scope.shiftLogs = results;
                    $scope.isBusyGettingShiftLog = false;
                    $rootScope.redrawTable();
                });
            };
        }
    );
}());