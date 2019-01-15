(function() {
    'use strict';
    mtcApp.controller('backupRequestTruckController',
        function backupRequestTruckController($scope, $filter, $interval, $rootScope, $location, $routeParams, backupTruckService, generalService) {

            $scope.header = 'Request a Back-up';

            $scope.userIsContractor = false;
            $scope.userIsAdmin = false;

            $scope.busyGettingContractorBeatNumbers = false;
            $scope.isBusyGettingBackupAssignments = false;
            $scope.busyGettingBackupReasons = true;
            $scope.isBusyGettingScheduleAvailability = false;
            $scope.haveBackupsSetForContractorAndBeatSelection = false;
            $scope.isBusyRequestingBackup = false;
            $scope.busyGettingContractors = false;
            $scope.busyGettingCurrentUser = false;


            $scope.urgentPriorityInHours = '';
            $scope.standardPriorityInHours = '';

            $scope.requestPriorityIsUrgent = "false";

            $scope.selectedContractorId;
            $scope.selectedBeatId;
            $scope.selectedBackupReasonId;
            $scope.comments;
            $scope.selectedBackupAssignmentContractorId;
            $scope.selectionReason;

            $scope.contractors = [];
            $scope.contractorBeatNumbers = [];
            $scope.backupAssignements = [];
            $scope.dateAndShifts = [];
            $scope.backupReasons = [];


            $scope.init = function() {

                $scope.busyGettingContractors = true;
                generalService.getContractors().then(function(results) {

                    for (var i = 0; i < results.length; i++) {
                        $scope.contractors.push(results[i]);
                    }
                    $scope.busyGettingContractors = false;
                    $scope.busyGettingCurrentUser = true;

                    generalService.getCurrentUser().then(function(results) {
                        if (results) {
                            if (results.SelectedRoleName === 'TowContractor') {
                                $scope.selectedContractorId = results.ContractorId;
                                $scope.userIsContractor = true;
                                $scope.userIsAdmin = false;
                                $scope.getContractorBeats($scope.selectedContractorId);
                            } else if (results.SelectedRoleName === 'Admin') {
                                $scope.userIsContractor = false;
                                $scope.userIsAdmin = true;
                            } else {
                                $scope.header = "You don't have permissions.";
                            }

                            $scope.busyGettingCurrentUser = false;
                        } else {
                            $scope.header = 'Error. We were not able to retrieve your current user context. Please come back later.';
                        }
                    });
                });

            };
            $scope.init();

            $scope.getContractorBeats = function(contractorId) {
                $scope.busyGettingContractorBeatNumbers = true;
                generalService.getContractorBeats(contractorId).then(function(results) {
                    $scope.contractorBeatNumbers = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.contractorBeatNumbers.push(results[i]);
                    }
                    $scope.busyGettingContractorBeatNumbers = false;
                });
            };

            $scope.getBackupAssignments = function() {

                if ($scope.selectedBeatId != undefined) {
                    $scope.dateAndShifts = [];
                    //check date schedule
                    var date = moment().format('MM/DD/YYYY');
                    backupTruckService.getScheduleAvailabilityForDateAndBeat($scope.selectedBeatId, date).then(function(result) {
                        $scope.dateAndShifts.push(new backupRequestShiftAndDate(date, false, false, false, false, result.AMAvailable, result.MIDAvailable, result.PMAvailable));
                    });

                    $scope.isBusyGettingBackupAssignments = true;
                    backupTruckService.getBackupAssignmentsByBeat($scope.selectedBeatId).then(function(result) {
                        $scope.isBusyGettingBackupAssignments = false;
                        $scope.backupAssignements = [];
                        if (result) {
                            $scope.backupAssignements.push(new backupAssignment(result.PrimaryBackupContractorId, result.PrimaryBackupContractorName, 1));
                            $scope.backupAssignements.push(new backupAssignment(result.SecondaryBackupContractorId, result.SecondaryBackupContractorName, 2));
                            $scope.backupAssignements.push(new backupAssignment(result.TertiaryBackupContractorId, result.TertiaryBackupContractorName, 3));
                        }

                        if ($scope.backupAssignements.length > 0)
                            $scope.haveBackupsSetForContractorAndBeatSelection = true;
                        else
                            $scope.haveBackupsSetForContractorAndBeatSelection = false;
                    });
                }
            };

            $scope.toggleBeatScheduleSelection = function(id) {
                var idx = $scope.selectedBeatSchedules.indexOf(id);
                if (idx > -1) {
                    $scope.selectedBeatSchedules.splice(idx, 1);
                } else {
                    $scope.selectedBeatSchedules.push(id);
                }
            };

            $scope.updateScheduleAvailability = function(dateAndShift) {

                var date = moment(dateAndShift.BackupDate).format('MM/DD/YYYY');

                backupTruckService.getScheduleAvailabilityForDateAndBeat($scope.selectedBeatId, date).then(function(result) {

                    dateAndShift.AMAvailable = result.AMAvailable;
                    dateAndShift.MIDAvailable = result.MIDAvailable;
                    dateAndShift.PMAvailable = result.PMAvailable;

                });
            };

            $scope.addDateAndShifts = function() {

                $scope.isBusyGettingScheduleAvailability = true;
                var previousItem = $scope.dateAndShifts[$scope.dateAndShifts.length - 1];

                //get all schedules (custom, holiday, regular) whether AM, Mid, PM are availabel for this date
                var date = moment(previousItem.BackupDate).add(1, 'days').format('MM/DD/YYYY');

                backupTruckService.getScheduleAvailabilityForDateAndBeat($scope.selectedBeatId, date).then(function(result) {
                    $scope.dateAndShifts.push(new backupRequestShiftAndDate(date, false, false, false, true, result.AMAvailable, result.MIDAvailable, result.PMAvailable));
                    $scope.isBusyGettingScheduleAvailability = false;
                });
            };

            $scope.removeDateAndShifts = function(dateAndShifts) {

                if ($scope.dateAndShifts.indexOf(dateAndShifts) >= 0) {
                    var index = $scope.dateAndShifts.indexOf(dateAndShifts);
                    $scope.dateAndShifts.splice(index, 1);
                }
            };

            $scope.requestBackup = function() {

                $scope.isBusyRequestingBackup = true;

                var vm = {
                    RequestIsUrgent: $scope.requestPriorityIsUrgent,
                    ContractorId: $scope.selectedContractorId,
                    BeatId: $scope.selectedBeatId,
                    BackupDatesAndShifts: $scope.dateAndShifts,
                    BackupReasonId: $scope.selectedBackupReasonId,
                    Comments: $scope.comments,
                    SelectionReason: $scope.selectionReason,
                    SelectedBackupContractorId: $scope.selectedBackupAssignmentContractorId,
                    SelectedBackupContractorAssignmentLevel: getContractorAssignmentLevel($scope.selectedBackupAssignmentContractorId)
                };

                backupTruckService.requestBackup(vm).then(function(result) {

                    toastr.success("Back-up Request", "Success");

                    $scope.isBusyRequestingBackup = false;
                    $scope.selectedBeatId = null;
                    $scope.selectedBackupReasonId = null;
                    $scope.selectedBackupAssignmentContractorId = null;
                    $scope.backupAssignements = [];
                    $scope.contractorBeatNumbers = [];

                    if ($scope.userIsContractor) {
                        $scope.getContractorBeats($scope.selectedContractorId);
                    } else {
                        $scope.selectedContractorId = null;
                    }


                    $scope.comments = '';

                    $scope.selectionReason = '';
                    $scope.requestPriorityIsUrgent = "false";

                    $scope.dateAndShifts = [];

                    var date = moment().format('MM/DD/YYYY');
                    backupTruckService.getScheduleAvailabilityForDateAndBeat($scope.selectedBeatId, date).then(function(result) {
                        $scope.dateAndShifts.push(new backupRequestShiftAndDate(date, false, false, false, false, result.AMAvailable, result.MIDAvailable, result.PMAvailable));
                    });

                });
            };


            backupTruckService.getBackupReasons().then(function(results) {

                for (var i = 0; i < results.length; i++) {
                    $scope.backupReasons.push(results[i]);
                }

                $scope.busyGettingBackupReasons = false;
            });

            backupTruckService.getUrgentBackupValue().then(function(result) {

                var minutes = result;
                if (minutes < 60) {
                    $scope.urgentPriorityInHours = minutes + ' minutes';
                } else {
                    var hours = eval(minutes / 60);
                    $scope.urgentPriorityInHours = hours + ' hours';
                }
            });

            backupTruckService.getStandardBackupValue().then(function(result) {

                var minutes = result;
                if (minutes < 60) {
                    $scope.standardPriorityInHours = minutes + ' minutes';
                } else {
                    var hours = eval(minutes / 60);
                    $scope.standardPriorityInHours = hours + ' hours';
                }
            });


            function getContractorAssignmentLevel(contractorId) {

                for (var i = 0; i < $scope.backupAssignements.length; i++) {

                    if ($scope.backupAssignements[i].contractorId === contractorId)
                        return $scope.backupAssignements[i].assignmentLevel;
                }
            }

            function backupAssignment(contractorId, contractorCompanyName, assignmentLevel) {
                var self = this;
                self.contractorId = contractorId;
                self.contractorCompanyName = contractorCompanyName;
                self.assignmentLevel = assignmentLevel;
            }

            function backupRequestShiftAndDate(backupDate, amRequested, midRequested, pmRequested, canBeRemoved, amAvailable, midAvailable, pmAvailable) {

                var self = this;

                self.BackupDate = backupDate;
                self.AMRequested = amRequested;
                self.MIDRequested = midRequested;
                self.PMRequested = pmRequested;

                self.AMAvailable = amAvailable;
                self.MIDAvailable = midAvailable;
                self.PMAvailable = pmAvailable;

                self.AMSatisfied = false;
                self.MIDSatisfied = false;
                self.PMSatisfied = false;

                self.canBeRemoved = canBeRemoved;
            }

        }
    );
}());
