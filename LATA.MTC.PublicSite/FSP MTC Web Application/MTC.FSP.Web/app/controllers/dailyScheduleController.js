'use strict';
(function () {
    mtcApp.controller('dailyScheduleController',
        function dailyScheduleController($scope, $filter, $rootScope, $interval, $location, $routeParams, scheduleService, trucksService) {

            $scope.header = 'FSP Schedule for ' + moment().format('MMMM DD, YYYY');
            $scope.isBusy = false;
            $scope.schedules = [];

            $scope.getDailySchedule = function () {
                $scope.isBusy = true;
                scheduleService.getDailySchedule().then(function (results) {
                    $scope.schedules = [];
                    $scope.isBusy = false;
                    for (var i = 0; i < results.length; i++) {

                        results[i].BeatNumber = eval(results[i].BeatNumber);

                        if (results[i].StartTime) {
                            var startTime = '2000-01-01 ' + results[i].StartTime.Hours + ":" + results[i].StartTime.Minutes;
                            results[i].StartTimeFriendly = moment(startTime).format('hh:mm A');
                        }

                        if (results[i].EndTime) {
                            var endTime = '2000-01-01 ' + results[i].EndTime.Hours + ":" + results[i].EndTime.Minutes;
                            results[i].EndTimeFriendly = moment(endTime).format('hh:mm A');
                        }

                        results[i].TruckNumber = '';
                        results[i].VehicleState = '';

                        $scope.schedules.push(results[i]);
                    }

                    $rootScope.redrawTable();
                });

            };
            $scope.getDailySchedule();
           
        }
    );
}());