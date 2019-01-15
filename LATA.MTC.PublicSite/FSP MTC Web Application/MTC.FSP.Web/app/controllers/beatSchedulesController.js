'use strict';
(function () {
    mtcApp.controller('beatSchedulesController',
        function beatSchedulesController($scope, $filter, $rootScope, $location, $routeParams, beatSchedulesService) {

            $scope.header = 'Beat Schedules';
            $scope.beatSchedules = [];
            $scope.schedules = [];
            $scope.selectedSchedules = [];
            $scope.beatsWithoutSchedule = [];            
            $scope.selectedBeat;
            $scope.isEdit = false;

            var getBeatSchedules = function () {

                $scope.isBusyGettingBeatSchedules = true;
               
                beatSchedulesService.getBeatSchedules().then(function (results) {
                    $scope.beatSchedules = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.beatSchedules.push(results[i]);
                    }
                    $scope.isBusyGettingBeatSchedules = false;

                });
            };
            var getSchedules = function () {

                $scope.isBusyGettingSchedules = true;

                beatSchedulesService.getSchedules().then(function (results) {
                    for (var i = 0; i < results.length; i++) {
                        $scope.schedules.push(new schedule(results[i]));
                    }
                    $scope.isBusyGettingSchedules = false;

                });
            };

            getBeatSchedules();
            getSchedules();

            $scope.edit = function (beatSchedule) {

                $scope.isEdit = true;
                $scope.selectedBeat = beatSchedule;
                $("#beatScheduleModal").modal('show');

                for (var ii = 0; ii < $scope.schedules.length; ii++) {
                    for (var i = 0; i < beatSchedule.Schedule.length; i++) {
                        if (beatSchedule.Schedule[i].ScheduleName === $scope.schedules[ii].scheduleName)
                            $scope.schedules[ii].isSelected = true;
                    }
                }

                $scope.validate();
            }
            $scope.new = function () {

                $scope.isEdit = false;                
                $("#beatScheduleModal").modal('show');

                $scope.beatsWithoutSchedule = [];

                beatSchedulesService.getBeatsWithoutSchedules().then(function (results) {
                    for (var i = 0; i < results.length; i++) {
                        $scope.beatsWithoutSchedule.push(results[i]);
                    }                   

                });

            };


            function schedule(dbSchedule) {

                var self = this;
                self.beatScheduleId = dbSchedule.BeatScheduleId;
                self.scheduleId = dbSchedule.ScheduleID;
                self.scheduleId = dbSchedule.ScheduleID;
                self.scheduleName = dbSchedule.ScheduleName;
                self.onPatrol = dbSchedule.OnPatrol;
                self.rollIn = dbSchedule.RollIn;
                self.weekday = dbSchedule.Weekday;
                self.isSelected = false;
                self.isEnabled = true;
                self.start = moment(dbSchedule.Start).format('MM/DD/YYYY');
                self.end = moment(dbSchedule.End).format('MM/DD/YYYY');
            }
            $scope.checked = function (selectedSchedule) {
                console.log('Checked Schedule: %s, OnPatrol: %s, RollIn: %s', selectedSchedule.scheduleName, selectedSchedule.onPatrol.Hours, selectedSchedule.rollIn.Hours);
                console.log('Number of selected schedules %s', $scope.selectedSchedules.length);
                $scope.validate();
            };
            $scope.validate = function () {

                //enable all first
                for (var iii = 0; iii < $scope.schedules.length; iii++) {
                    $scope.schedules[iii].isEnabled = true;
                }

                var selectedSchedules = [];
                for (var v = 0; v < $scope.schedules.length; v++) {
                    if ($scope.schedules[v].isSelected)
                        selectedSchedules.push($scope.schedules[v]);
                }

                //let's disable all other schedule that would conflict or enable schedules that don't
                for (var i = 0; i < selectedSchedules.length; i++) {

                    var selectedSavedSchedule = selectedSchedules[i];

                    for (var ii = 0; ii < $scope.schedules.length; ii++) {

                        var schedule = $scope.schedules[ii];

                        if ((selectedSavedSchedule.rollIn.Hours < schedule.rollIn.Hours) && (selectedSavedSchedule.weekday === schedule.weekday)) {
                            if ((selectedSavedSchedule.rollIn.Hours > schedule.onPatrol.Hours) && (selectedSavedSchedule.weekday === schedule.weekday)) {
                                if (schedule.isSelected === false)
                                    schedule.isEnabled = false;
                            }
                        }
                        else {
                            if ((selectedSavedSchedule.onPatrol.Hours < schedule.rollIn.Hours) && (selectedSavedSchedule.weekday === schedule.weekday)) {
                                if (schedule.isSelected === false)
                                    schedule.isEnabled = false;
                            }
                        }
                    }
                }
            };
            $scope.save = function () {

                $("#beatScheduleModal").modal('hide');

                var selectedSchedules = [];
                for (var v = 0; v < $scope.schedules.length; v++) {
                    if ($scope.schedules[v].isSelected)
                        selectedSchedules.push($scope.schedules[v]);
                }

                var vm = {
                    BeatId: $scope.selectedBeat.BeatID,
                    SelectedSchedules: selectedSchedules
                };

                beatSchedulesService.saveBeatSchedule(vm).then(function (result) {

                    toastr.success('Successfully saved beat schedule changes \"' + $scope.selectedBeat.Beat + "\"");
                    getBeatSchedules();

                }, function (status) {
                    console.log(status);
                });

            };
        }
    );
}());