'use strict';
(function () {
    mtcApp.controller('driverAlertCommentsController',
        function driverAlertCommentsController($scope, $filter, $interval, $rootScope, $location, $routeParams, alertsService, generalService) {

            $scope.header = moment().format('MMMM, DD YYYY') + ' Driver\'s Alert Comments';
            $scope.driverAlertComments = [];

            $scope.selectedAlert;
            $scope.selectedAlertType;
            $scope.excuseComments = '';

            $scope.columns = [];
            $scope.columns.push(new $rootScope.mtcColumn("Beat #", "BeatNumber", true, true));
            $scope.columns.push(new $rootScope.mtcColumn("Vehicle", "VehicleNumber", true, true));
            $scope.columns.push(new $rootScope.mtcColumn("Driver Name", "DriverFullName", true, true));
            $scope.columns.push(new $rootScope.mtcColumn("Time", "Datestamp", true, true));
            $scope.columns.push(new $rootScope.mtcColumn("Comment", "Comment", true, true, true));
            $scope.columns.push(new $rootScope.mtcColumn("Type", "ExceptionType", true, true));

            $scope.getDriverAlertComments = function () {

                $scope.isBusyGettingDriverAlertComments = true;

                var vm = {
                    beat: $scope.selectedBeatNumberId,
                    driver: $scope.selectedDriverId,
                    alertType: $scope.selectedAlert,
                };

                alertsService.getDriverAlertComments(vm).then(function (results) {

                    $scope.driverAlertComments = [];

                    for (var i = 0; i < results.length; i++) {
                        $scope.driverAlertComments.push(results[i]);
                    }

                    $scope.isBusyGettingDriverAlertComments = false;
                });
            };
            $scope.getDriverAlertComments();
            
            $scope.reset = function () {

                $scope.selectedBeatNumberId = null;
                $scope.selectedDriverId = null;
                $scope.selectedAlert = '';
            
                $scope.getDriverAlertComments();

            };

            $scope.selectedBeatNumberId;
            $scope.beatNumbers = [];
            $scope.busyGettingBeatNumbers = true;
            generalService.getBeatNumbers().then(function (results) {
                for (var i = 0; i < results.length; i++) {
                    $scope.beatNumbers.push(results[i]);
                }
                $scope.busyGettingBeatNumbers = false;
            });

            $scope.selectedDriverId;
            $scope.drivers = [];
            $scope.busyGettingDrivers = true;
            generalService.getDrivers().then(function (results) {
                for (var i = 0; i < results.length; i++) {
                    $scope.drivers.push(results[i]);
                }
                $scope.busyGettingDrivers = false;
            });

            $scope.selectedAlert;

           
        }
    );
}());