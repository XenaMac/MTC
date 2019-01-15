(function () {
    'use strict';
    mtcApp.controller('reportAlarmsController',
        function reportAlarmsController($scope, $rootScope, reportService, generalService) {

            $scope.header = "Alarms";
            $scope.isBusy = false;
            $scope.isBusyExporting = false;
            $scope.records = [];
            $scope.drivers = [];
            $scope.beatNumbers = [];
            $scope.vehicles = [];
            $scope.callSigns = [];

            //$scope.pageSizes = [10, 20, 30, 40, 50, 100];
            //$scope.selectedPageSize = $scope.pageSizes[0];

            $scope.query = {
                startDate: '',
                endDate: '',
                alarmName: '',
                driverName: '',
                beatNumber: '',
                truckNumber: '',
                callSign: ''
            };

            $scope.busyGettingDrivers = true;
            generalService.getDrivers().then(function (results) {
                $scope.drivers = results;
                $scope.busyGettingDrivers = false;
            });
            $scope.busyGettingBeatNumbers = true;
            generalService.getBeatNumbers().then(function (results) {
                $scope.beatNumbers = results;
                $scope.busyGettingBeatNumbers = false;
            });
            $scope.busyGettingVehicles = true;
            generalService.getVehicles().then(function (results) {
                $scope.vehicles = results;
                $scope.busyGettingVehicles = false;
            });
            $scope.busyGettingCallSigns = true;
            generalService.getCallSigns().then(function (results) {
                $scope.callSigns = results;
                $scope.busyGettingCallSigns = false;
            });

            $scope.getReportData = function () {
                $scope.isBusy = true;
                $scope.query.format = 'json';
                reportService.getAlarmReport($scope.query).then(function (results) {
                    $scope.records = results;
                    $scope.isBusy = false;
                    $rootScope.redrawTable();
                });
            };

            $scope.exportReportData = function () {
                $scope.isBusyExporting = true;

                //requesting excel and not json
                $scope.query.format = 'excel';

                reportService.getAlarmReport($scope.query).then(function (results) {
                    $scope.isBusyExporting = false;
                    var element = angular.element('<a/>');
                    element.attr({
                        href: 'data:attachment/csv;charset=utf-8,' + encodeURI(results),
                        target: '_blank',
                        download: $scope.header + '.csv'
                    })[0].click();

                });
            };

            $scope.init = function (alarmName) {
                $scope.query.alarmName = alarmName;
                $scope.header = alarmName;
            }

            //$scope.$watch('selectedPageSize', function () {
            //    console.log('Updating page size');                        

            //    var tabs = document.getElementById("myTable");
            //    tabs.setAttribute('data-page-size', $scope.selectedPageSize);
            //    $('.footable').trigger('footable_initialize');
            //    $('.footable').trigger('footable_redraw');
            //    $('.footable').trigger('footable_resize');

            //});

        }
    );
}());