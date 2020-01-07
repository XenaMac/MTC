(function() {
    'use strict';
    mtcApp.controller('alertDetailsController', function alertDetailsController($scope, $filter, $interval, $rootScope, $location, $routeParams, alertsService, generalService) {
        $scope.header = moment().format('MMMM DD, YYYY') + " Alarms Today";
        $scope.alertDetails = [];
        $scope.selectedAlert;
        $scope.selectedAlertType;
        $scope.excuseComments = '';
        $scope.getAlertDetails = function() {
            $scope.isBusyGettingAlertDetails = true;
            var vm = {
                beatId: $scope.selectedBeatNumberId,
                driverId: $scope.selectedDriverId,
                alertType: $scope.selectedAlert,
                isExcused: $scope.selectedExcusedState
            };
            alertsService.getAlertDetails(vm).then(function(results) {
                $scope.alertDetails = [];
                for (var i = 0; i < results.length; i++) {
                    if (moment(results[i].AlarmTime).isValid()) results[i].AlarmTime = moment(results[i].AlarmTime).format('MM/DD/YY hh:mm A');
                    $scope.alertDetails.push(results[i]);
                }
                $scope.isBusyGettingAlertDetails = false;
            });
        };
        $scope.getAlertDetails();
        $scope.updateAlertDetail = function(a) {
            console.log(a);
            //var vm = {
            //    AlertId :
            //    BeatNumber: alertDetail.BeatNumber,
            //    ContractCompanyName: alertDetail.ContractCompanyName,
            //    VehicleNumber: alertDetail.VehicleNumber,
            //    DriverName: alertDetail.DriverName,
            //    AlarmTime: alertDetail.AlarmTime,
            //    AlarmType: alertDetail.AlarmType,
            //    Comments: alertDetail.Comments,
            //    IsExcused: alertDetail.IsExcused
            //};
            alertsService.updateAlertDetail(a).then(function(results) {
                toastr.success("Alert Detail", "Successfully updated");
                $scope.getAlertDetails();
            });
        };
        $scope.reset = function() {
            $scope.selectedBeatNumberId = null;
            $scope.selectedDriverId = null;
            $scope.selectedAlert = '';
            $scope.selectedExcusedState = '';
            $scope.getAlertDetails();
        };
        $scope.selectedBeatNumberId;
        $scope.beatNumbers = [];
        $scope.busyGettingBeatNumbers = true;
        generalService.getBeatNumbers().then(function(results) {
            for (var i = 0; i < results.length; i++) {
                $scope.beatNumbers.push(results[i]);
            }
            $scope.busyGettingBeatNumbers = false;
        });
        $scope.selectedDriverId;
        $scope.drivers = [];
        $scope.busyGettingDrivers = true;
        generalService.getDrivers().then(function(results) {
            for (var i = 0; i < results.length; i++) {
                $scope.drivers.push(results[i]);
            }
            $scope.busyGettingDrivers = false;
        });
        $scope.selectedAlert;
        $scope.selectedExcusedState;
    });
}());
