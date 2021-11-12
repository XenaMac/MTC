(function() {
    'use strict';
    mtcApp.controller('schedulingController', function schedulingController($scope, $filter, $interval, $rootScope, $location, $routeParams, schedulingService, generalService) {
        $scope.header = 'Scheduling';
        $scope.canEdit = false;
        $scope.selectedContractorId;
        $scope.contractors = [];
        $scope.isWeekday = false;
        $scope.isBusyGettingContractorBeats = false;
        $scope.isBusyGettingCustomScheduleTimes = false;
        $scope.isBusyGettingHolidayScheduleTimes = false;
        $scope.holidayDates = [];
        $scope.holidayDate;
        $scope.isBusyWithHolidayDates = false;
        $scope.customDates = [];
        $scope.customDate;
        $scope.isBusyWithCustomDates = false;
        $scope.schedules = [];
        $scope.beatSchedules = [];
        $scope.selectedBeatSchedule;
        $scope.schedule;
        $scope.isBusyWithSchedules = false;
        $scope.holidaySchedules = [];
        $scope.holidaySchedule = {
            ContractorId: '',
            BeatId: '',
            HolidayDateId: '',
            HolidayScheduleId: '',
            NumberOfTrucks: 1
        };
        $scope.isBusyWithHolidaySchedules = false;
        $scope.holidayScheduleTimes = [];
        $scope.customSchedules = [];
        $scope.customSchedule = {
            ContractorId: '',
            BeatId: '',
            CustomDateId: '',
            CustomScheduleId: '',
            NumberOfTrucks: 1
        };
        $scope.isBusyWithCustomSchedules = false;
        $scope.customScheduleTimes = [];
        generalService.getCanEdit('Scheduling').then(function(result) {
            $scope.canEdit = result;
        });
        //holiday dates
        $scope.getHolidayDates = function() {
            $scope.isBusyWithHolidayDates = true;
            schedulingService.getHolidayDates().then(function(results) {
                $scope.holidayDates = [];
                for (var i = 0; i < results.length; i++) {
                    results[i].DateFriendly = moment(results[i].Date).format('MM/DD/YYYY');
                    results[i].DateTicks = moment(results[i].Date).format('X');
                    if (results[i].Times.length === 0) {
                        results[i].Times = [{
                            'ScheduleName': 'AM',
                            'StartTime': '',
                            'EndTime': ''
                            }, {
                            'ScheduleName': 'MID',
                            'StartTime': '',
                            'EndTime': ''
                            }, {
                            'ScheduleName': 'PM',
                            'StartTime': '',
                            'EndTime': ''
                            }];
                    } else {
                        for (var ii = 0; ii < results[i].Times.length; ii++) {
                            if (results[i].Times[ii].StartTime) {
                                results[i].Times[ii].StartTime = $rootScope.createDateFromTimespan(results[i].Times[ii].StartTime);
                            }
                            if (results[i].Times[ii].EndTime) {
                                results[i].Times[ii].EndTime = $rootScope.createDateFromTimespan(results[i].Times[ii].EndTime);
                            }
                        }
                    }
                    $scope.holidayDates.push(results[i]);
                }
                $scope.isBusyWithHolidayDates = false;
                $rootScope.redrawTable();
            });
        };
        $scope.getHolidayDates();
        $scope.manageHolidayDate = function(h) {
            $scope.holidayDate = h;
            $("#manageHolidayDateModal").modal('show');
        };
        $scope.addHolidayDate = function() {
            $scope.holidayDate = {
                'Id': '0',
                'Name': '',
                'Abbreviation': '',
                'Date': '',
                'Times': [{
                    'ScheduleName': 'AM',
                    'StartTime': $rootScope.createDate(6, 0),
                    'EndTime': $rootScope.createDate(10, 0)
                    }, {
                    'ScheduleName': 'MID',
                    'StartTime': $rootScope.createDate(10, 0),
                    'EndTime': $rootScope.createDate(14, 0)
                    }, {
                    'ScheduleName': 'PM',
                    'StartTime': $rootScope.createDate(14, 0),
                    'EndTime': $rootScope.createDate(18, 0)
                    }]
            };
            $("#manageHolidayDateModal").modal('show');
        };
        $scope.saveHolidayDate = function() {
            $scope.isBusyWithHolidayDates = true;
            for (var i = 0; i < $scope.holidayDate.Times.length; i++) {
                $scope.holidayDate.Times[i].StartTime = moment($scope.holidayDate.Times[i].StartTime).format('HH:mm');
                $scope.holidayDate.Times[i].EndTime = moment($scope.holidayDate.Times[i].EndTime).format('HH:mm');
            }
            var vm = {
                Id: $scope.holidayDate.Id,
                Name: $scope.holidayDate.Name,
                Abbreviation: $scope.holidayDate.Abbreviation,
                Date: $scope.holidayDate.DateFriendly,
                Times: $scope.holidayDate.Times
            };
            schedulingService.saveHolidayDate(vm).then(function(result) {
                $("#manageHolidayDateModal").modal('hide');
                toastr.success('Scheduling', 'Holiday Date saved');
                $scope.getHolidayDates();
                $scope.isBusyWithHolidayDates = false;
            });
        };
        $scope.removeHolidayDate = function(h) {
            if (confirm("Are you sure to remove this 'Holiday Date'? This will delete all associated 'Holiday Schedules'!")) {
                $scope.isBusyWithHolidayDates = true;
                schedulingService.removeHolidayDate(h.Id).then(function(response) {
                    if (response.HasError) {
                        toastr.error("Error removing Holiday Date " + response.Message, "Scheduling");
                    } else {
                        toastr.warning("Holiday Date removed", "Scheduling");
                        $scope.getHolidayDates();
                        $scope.getHolidaySchedules();
                    }
                    $scope.isBusyWithHolidayDates = false;
                });
            };
        };
        //holiday schedules
        $scope.getHolidaySchedules = function() {
            $scope.isBusyWithHolidaySchedules = true;
            schedulingService.getHolidaySchedules().then(function(results) {
                $scope.holidaySchedules = [];
                for (var i = 0; i < results.length; i++) {
                    results[i].StartDateFriendly = moment(results[i].StartDate).format('MM/DD/YY');
                    results[i].EndDateFriendly = moment(results[i].EndDate).format('MM/DD/YY');
                    var startTime = '2000-01-01 ' + results[i].StartTime.Hours + ":" + results[i].StartTime.Minutes;
                    var endTime = '2000-01-01 ' + results[i].EndTime.Hours + ":" + results[i].EndTime.Minutes;
                    results[i].StartTimeFriendly = moment(startTime).format('hh:mm A');
                    results[i].EndTimeFriendly = moment(endTime).format('hh:mm A');
                    $scope.holidaySchedules.push(results[i]);
                }
                $scope.isBusyWithHolidaySchedules = false;
                $rootScope.redrawTable();
            });
        };
        $scope.getHolidaySchedules();
        $scope.getHolidayScheduleTimes = function() {
            $scope.isBusyGettingHolidayScheduleTimes = true;
            schedulingService.getHolidayScheduleTimes($scope.holidaySchedule.HolidayDateId).then(function(results) {
                $scope.holidayScheduleTimes = [];
                for (var i = 0; i < results.length; i++) {
                    $scope.holidayScheduleTimes.push(results[i]);
                }
                $scope.isBusyGettingHolidayScheduleTimes = false;
            });
        };
        $scope.manageHolidaySchedule = function(s) {
            $scope.holidaySchedule = s;
            $scope.getContractorBeats(s.ContractorId);
            $scope.getHolidayScheduleTimes();
            $("#manageHolidayScheduleModal").modal('show');
        };
        $scope.addHolidaySchedule = function() {
            $scope.holidaySchedule = angular.copy($scope.holidaySchedule);
            $("#manageHolidayScheduleModal").modal('show');
        };
        $scope.saveHolidaySchedule = function() {
            $scope.isBusyWithHolidaySchedules = true;
            var vm = {
                Id: $scope.holidaySchedule.Id,
                holidayScheduleId: $scope.holidaySchedule.HolidayScheduleId,
                beatId: $scope.holidaySchedule.BeatId,
                numberOfTrucks: $scope.holidaySchedule.NumberOfTrucks
            };
            schedulingService.saveHolidaySchedule(vm).then(function(result) {
                $("#manageHolidayScheduleModal").modal('hide');
                toastr.success('Scheduling', 'Holiday Schedule saved');
                $scope.getHolidaySchedules();
                $scope.holidaySchedule = {};
                $scope.isBusyWithHolidaySchedules = false;
            });
        };
        $scope.removeHolidaySchedule = function(s) {
            if (confirm("Are you sure to remove this 'Holiday Schedule'?")) {
                $scope.isBusyWithHolidaySchedules = true;
                schedulingService.removeHolidaySchedule(s.Id).then(function(response) {
                    if (response.HasError) {
                        toastr.error("Error removing Holiday Schedule " + response.Message, "Scheduling");
                    } else {
                        toastr.warning("Holiday Schedule removed", "Scheduling");
                        $scope.getHolidaySchedules();
                    }
                    $scope.isBusyWithHolidaySchedules = false;
                });
            };
        };
        //regular schedules
        $scope.getSchedules = function() {
            $scope.isBusyWithSchedules = true;
            schedulingService.getSchedules().then(function(results) {
                $scope.schedules = [];
                for (var i = 0; i < results.length; i++) {
                    if (results[i].StartDate) {
                        results[i].StartDateFriendly = moment(results[i].StartDate).format('MM/DD/YY');
                        if (results[i].StartTime) {
                            results[i].StartTime = $rootScope.createDateFromTimespan(results[i].StartTime);
                            results[i].StartTimeFriendly = moment(results[i].StartTime).format('hh:mm A');
                        }
                    }
                    if (results[i].EndDate) {
                        results[i].EndDateFriendly = moment(results[i].EndDate).format('MM/DD/YY');
                        if (results[i].EndTime) {
                            results[i].EndTime = $rootScope.createDateFromTimespan(results[i].EndTime);
                            results[i].EndTimeFriendly = moment(results[i].EndTime).format('hh:mm A');
                        }
                    }
                    if (results[i].Saturday === true || results[i].Sunday === true) results[i].IsWeekday = false;
                    else results[i].IsWeekday = true;
                    $scope.schedules.push(results[i]);
                }
                $scope.isBusyWithSchedules = false;
                $rootScope.redrawTable();
            });
        };
        $scope.getSchedules();
        $scope.manageSchedule = function(s, isWeekday) {
            $scope.isWeekday = isWeekday;
            $scope.schedule = s;
            $scope.getContractorBeats($scope.schedule.ContractorId);
            $("#manageScheduleModal").modal('show');
        };
        $scope.addSchedule = function(isWeekday) {
            $scope.isWeekday = isWeekday;
            $scope.schedule = null;
            $("#manageScheduleModal").modal('show');
        };
        $scope.saveSchedule = function() {
            $scope.schedule.StartTime = moment($scope.schedule.StartTime).format('HH:mm');
            $scope.schedule.EndTime = moment($scope.schedule.EndTime).format('HH:mm');
            $scope.isBusyWithSchedules = true;
            schedulingService.saveSchedule($scope.schedule).then(function(result) {
                $("#manageScheduleModal").modal('hide');
                toastr.success('Scheduling', 'Schedule saved');
                $scope.getSchedules();
                $scope.isBusyWithSchedules = false;
            });
        };
        $scope.removeSchedule = function(s) {
            if (confirm("Are you sure to remove this schedule?")) {
                $scope.isBusyWithSchedules = true;
                schedulingService.removeSchedule(s.Id).then(function(response) {
                    if (response.HasError) {
                        toastr.error("Error removing Schedule " + response.Message, "Scheduling");
                    } else {
                        toastr.warning("Schedule removed", "Scheduling");
                        $scope.getSchedules();
                    }
                    $scope.isBusyWithSchedules = false;
                });
            };
        };
        //custom dates
        $scope.getCustomDates = function() {
            $scope.isBusyWithCustomDates = true;
            schedulingService.getCustomDates().then(function(results) {
                $scope.customDates = [];
                for (var i = 0; i < results.length; i++) {
                    results[i].DateFriendly = moment(results[i].Date).format('MM/DD/YYYY');
                    results[i].DateTicks = moment(results[i].Date).format('X');
                    if (moment(results[i].EndDate).isValid()) {
                        results[i].EndDateFriendly = moment(results[i].EndDate).format('MM/DD/YYYY');
                        results[i].EndDateTicks = moment(results[i].EndDate).format('X');
                    }
                    if (results[i].Times.length === 0) {
                        results[i].Times = [{
                            'ScheduleName': 'AM',
                            'StartTime': '',
                            'EndTime': ''
                            }, {
                            'ScheduleName': 'MID',
                            'StartTime': '',
                            'EndTime': ''
                            }, {
                            'ScheduleName': 'PM',
                            'StartTime': '',
                            'EndTime': ''
                            }];
                    } else {
                        for (var ii = 0; ii < results[i].Times.length; ii++) {
                            if (results[i].Times[ii].StartTime) {
                                results[i].Times[ii].StartTime = $rootScope.createDateFromTimespan(results[i].Times[ii].StartTime);
                            }
                            if (results[i].Times[ii].EndTime) {
                                results[i].Times[ii].EndTime = $rootScope.createDateFromTimespan(results[i].Times[ii].EndTime);
                            }
                        }
                    }
                    $scope.customDates.push(results[i]);
                }
                $scope.isBusyWithCustomDates = false;
                $rootScope.redrawTable();
            });
        };
        $scope.getCustomDates();
        $scope.manageCustomDate = function(h) {
            $scope.customDate = h;
            $("#manageCustomDateModal").modal('show');
        };
        $scope.addCustomDate = function() {
            $scope.customDate = {
                'Id': '0',
                'Name': '',
                'Abbreviation': '',
                'Date': '',
                'EndDate': '',
                'Times': [{
                    'ScheduleName': 'AM',
                    'StartTime': $rootScope.createDate(6, 0),
                    'EndTime': $rootScope.createDate(10, 0)
                    }, {
                    'ScheduleName': 'MID',
                    'StartTime': $rootScope.createDate(10, 0),
                    'EndTime': $rootScope.createDate(14, 0)
                    }, {
                    'ScheduleName': 'PM',
                    'StartTime': $rootScope.createDate(14, 0),
                    'EndTime': $rootScope.createDate(18, 0)
                    }]
            };
            $("#manageCustomDateModal").modal('show');
        };
        $scope.saveCustomDate = function() {
            $scope.isBusyWithCustomDates = true;
            for (var i = 0; i < $scope.customDate.Times.length; i++) {
                $scope.customDate.Times[i].StartTime = moment($scope.customDate.Times[i].StartTime).format('HH:mm');
                $scope.customDate.Times[i].EndTime = moment($scope.customDate.Times[i].EndTime).format('HH:mm');
            }
            var vm = {
                Id: $scope.customDate.Id,
                Name: $scope.customDate.Name,
                Abbreviation: $scope.customDate.Abbreviation,
                Date: $scope.customDate.DateFriendly,
                EndDate: $scope.customDate.EndDateFriendly,
                Times: $scope.customDate.Times
            };
            schedulingService.saveCustomDate(vm).then(function(result) {
                $("#manageCustomDateModal").modal('hide');
                toastr.success('Scheduling', 'Custom Date saved');
                $scope.getCustomDates();
                $scope.isBusyWithCustomDates = false;
            });
        };
        $scope.removeCustomDate = function(h) {
            if (confirm("Are you sure to remove this 'Custom Date'? This will delete all associated 'Custom Schedules'!")) {
                $scope.isBusyWithCustomDates = true;
                schedulingService.removeCustomDate(h.Id).then(function(response) {
                    if (response.HasError) {
                        toastr.error("Error removing Custom Date " + response.Message, "Scheduling");
                    } else {
                        toastr.warning("Custom Date removed", "Scheduling");
                        $scope.getCustomDates();
                        $scope.getCustomSchedules();
                    }
                    $scope.isBusyWithCustomDates = false;
                });
            };
        };
        //custom schedules
        $scope.getCustomSchedules = function() {
            $scope.isBusyWithCustomSchedules = true;
            schedulingService.getCustomSchedules().then(function(results) {
                $scope.customSchedules = [];
                for (var i = 0; i < results.length; i++) {
                    results[i].StartDateFriendly = moment(results[i].StartDate).format('MM/DD/YY');
                    results[i].EndDateFriendly = moment(results[i].EndDate).format('MM/DD/YY');
                    var startTime = '2000-01-01 ' + results[i].StartTime.Hours + ":" + results[i].StartTime.Minutes;
                    var endTime = '2000-01-01 ' + results[i].EndTime.Hours + ":" + results[i].EndTime.Minutes;
                    results[i].StartTimeFriendly = moment(startTime).format('hh:mm A');
                    results[i].EndTimeFriendly = moment(endTime).format('hh:mm A');
                    $scope.customSchedules.push(results[i]);
                }
                $scope.isBusyWithCustomSchedules = false;
                $rootScope.redrawTable();
            });
        };
        $scope.getCustomSchedules();
        $scope.getCustomScheduleTimes = function() {
            $scope.isBusyGettingCustomScheduleTimes = true;
            schedulingService.getCustomScheduleTimes($scope.customSchedule.CustomDate.Id).then(function(results) {
                $scope.customScheduleTimes = [];
                for (var i = 0; i < results.length; i++) {
                    $scope.customScheduleTimes.push(results[i]);
                }
                $scope.isBusyGettingCustomScheduleTimes = false;
            });
        };
        $scope.manageCustomSchedule = function(s) {
            $scope.customSchedule = s;
            $scope.getContractorBeats();
            $scope.getCustomScheduleTimes();
            $("#manageCustomScheduleModal").modal('show');
        };
        $scope.addCustomSchedule = function() {
            $scope.customSchedule = angular.copy($scope.customSchedule);
            $("#manageCustomScheduleModal").modal('show');
        };
        $scope.saveCustomSchedule = function() {
            $scope.isBusyWithCustomSchedules = true;
            var vm = {
                Id: $scope.customSchedule.Id,
                customScheduleId: $scope.customSchedule.CustomScheduleId,
                beatId: $scope.customSchedule.BeatId,
                numberOfTrucks: $scope.customSchedule.NumberOfTrucks
            };
            schedulingService.saveCustomSchedule(vm).then(function(result) {
                $("#manageCustomScheduleModal").modal('hide');
                toastr.success('Scheduling', 'Custom Schedule saved');
                $scope.getCustomSchedules();
                $scope.customSchedule = {};
                $scope.isBusyWithCustomSchedules = false;
            });
        };
        $scope.removeCustomSchedule = function(s) {
            if (confirm("Are you sure to remove this Custom schedule?")) {
                $scope.isBusyWithCustomSchedules = true;
                schedulingService.removeCustomSchedule(s.Id).then(function(response) {
                    if (response.HasError) {
                        toastr.error("Error removing Custom Schedule " + response.Message, "Scheduling");
                    } else {
                        toastr.warning("Custom Schedule removed", "Scheduling");
                        $scope.getCustomSchedules();
                    }
                    $scope.isBusyWithCustomSchedules = false;
                });
            };
        };
        //common
        generalService.getContractors().then(function(results) {
            for (var i = 0; i < results.length; i++) {
                $scope.contractors.push(results[i]);
            }
        });
        $scope.getContractorBeats = function(contractorId) {
            $scope.isBusyGettingContractorBeats = true;
            if (contractorId != undefined) {
                generalService.getContractorBeats(contractorId).then(function(results) {
                    $scope.contractorBeatNumbers = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.contractorBeatNumbers.push(results[i]);
                    }
                    $scope.isBusyGettingContractorBeats = false;
                });
            }
        };
    });
}());
