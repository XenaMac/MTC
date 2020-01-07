(function () {
    'use strict';
    mtcApp.controller('backupResponseTruckController',
        function backupResponseTruckController($scope, $filter, $interval, $rootScope, $location, $routeParams, backupTruckService, generalService) {

            $scope.header = 'Back-up Response';
            $scope.subHeader = '';

            $scope.userIsContractor = false;
            $scope.userIsAdmin = false;

            $scope.busyGettingBackupRequests = false;
            $scope.busyGettingBackupRequests = false;
            $scope.busyGettingDeclinationReasons = false;
            $scope.busyGettingContactors = false;
            $scope.busyGettingCurrentUser = false;

            $scope.selectedContractorId = null;
            $scope.selectedBackupRequestId = null;
            $scope.selectedBackupRequest = null;
            $scope.selectedBackupDeclinationReasonId = null;

            $scope.contractors = [];
            $scope.backupRequests = [];
            $scope.backupDeclinationReasons = [];


            //1 get any screen data
            $scope.init = function () {

                $scope.busyGettingDeclinationReasons = true;
                backupTruckService.getBackupDeclinationReasons().then(function (results) {

                    $scope.backupDeclinationReasons = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.backupDeclinationReasons.push(results[i]);
                    }
                    $scope.busyGettingDeclinationReasons = false;
                });

                //Are you Admin or not?
                $scope.busyGettingContactors = true;
                $scope.busyGettingCurrentUser = true;

                generalService.getBackupProvidingContractors().then(function (results) {

                    $scope.contractors = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.contractors.push(results[i]);
                    }

                    $scope.busyGettingContactors = false;

                    generalService.getCurrentUser().then(function (results) {
                        if (results) {
                            if (results.SelectedRoleName === "Admin") {
                                $scope.userIsContractor = false;
                                $scope.userIsAdmin = true;
                            }
                            else if (results.SelectedRoleName === 'TowContractor') {
                                $scope.selectedContractorId = results.ContractorId;
                                $scope.userIsContractor = true;
                                $scope.userIsAdmin = false;
                                $scope.getBackupRequests();
                            }
                        }
                        else {
                            $scope.header = 'Error. We were not able to retrieve your current user context. Please come back later.';
                        }

                        $scope.busyGettingCurrentUser = false;

                    });

                });
            };

            $scope.getBackupRequests = function () {
                $scope.busyGettingBackupRequests = true;
                $scope.selectedBackupRequest = null;
                backupTruckService.getBackupRequests($scope.selectedContractorId).then(function (results) {

                    $scope.backupRequests = [];

                    for (var i = 0; i < results.length; i++) {
                        if (results[i].BackupRequestShiftsAndDates) {
                            results[i].dateAndShifts = [];
                            for (var ii = 0; ii < results[i].BackupRequestShiftsAndDates.length; ii++) {

                                var dateAndShift = results[i].BackupRequestShiftsAndDates[ii];
                                results[i].dateAndShifts.push(new backupRequestShiftAndDate(dateAndShift.Id, dateAndShift.BackupDate, dateAndShift.AMRequested, dateAndShift.MIDRequested, dateAndShift.PMRequested, dateAndShift.AMSatisfied, dateAndShift.MIDSatisfied, dateAndShift.PMSatisfied));
                            }
                        }
                        $scope.backupRequests.push(results[i]);
                    }

                    $scope.busyGettingBackupRequests = false;

                });
            }

            $scope.canRespond = function () {
                var retValue = true;
                if ($scope.selectedBackupRequest && $scope.selectedContractorId) {

                    if ($scope.selectedBackupRequest.BackupAccepted === true) {
                        retValue = false;
                    }
                    else {
                        //responded already
                        if ($scope.selectedBackupRequest.PrimaryBackupContractor.ContractorId === $scope.selectedContractorId &&
                            $scope.selectedBackupRequest.PrimaryBackupResponded === true)
                            retValue = false;
                        if ($scope.selectedBackupRequest.SecondaryBackupContractor.ContractorId === $scope.selectedContractorId &&
                            $scope.selectedBackupRequest.SecondaryBackupResponded === true)
                            retValue = false;
                        if ($scope.selectedBackupRequest.TertiaryBackupContractor.ContractorId === $scope.selectedContractorId &&
                            $scope.selectedBackupRequest.TertiaryBackupResponded === true)
                            retValue = false;

                        //declined or expired
                        //REMOVED THIS CONSTRAINT ON 12/1/2015. T.K.
                        //if ($scope.selectedBackupRequest.PrimaryBackupContractor.ContractorId === $scope.selectedContractorId && $scope.selectedBackupRequest.PrimaryBackupResponseTimeExpiredOrDeclined === true)
                        //    retValue = false;
                        //if ($scope.selectedBackupRequest.SecondaryBackupContractor.ContractorId === $scope.selectedContractorId && $scope.selectedBackupRequest.SecondaryBackupResponseTimeExpiredOrDeclined === true)
                        //    retValue = false;
                        //if ($scope.selectedBackupRequest.TertiaryBackupContractor.ContractorId === $scope.selectedContractorId && $scope.selectedBackupRequest.TertiaryBackupResponseTimeExpiredOrDeclined === true)
                        //    retValue = false;
                    }


                }
                return retValue;
            };

            $scope.submitBackupResponse = function () {

                $scope.isBusyRespondingToBackup = true;

                var backupAssigmentLevel = 100;

                if ($scope.selectedBackupRequest.PrimaryBackupContractor.ContractorId === $scope.selectedContractorId)
                    backupAssigmentLevel = 0;
                else if ($scope.selectedBackupRequest.SecondaryBackupContractor.ContractorId === $scope.selectedContractorId)
                    backupAssigmentLevel = 1;
                else if ($scope.selectedBackupRequest.TertiaryBackupContractor.ContractorId === $scope.selectedContractorId)
                    backupAssigmentLevel = 2;

                var vm = {
                    BackupRequestId: $scope.selectedBackupRequest.Id,
                    ContractorId: $scope.selectedContractorId,
                    BackupResponseStatus: $scope.selectedBackupResponseStatus,
                    BackupDeclinationReasonId: $scope.selectedBackupDeclinationReasonId,
                    BackupAssignmentLevel: backupAssigmentLevel,
                    BackupDatesAndShifts: $scope.selectedBackupRequest.dateAndShifts,
                    Comments: $scope.comments
                };

                backupTruckService.responseToBackupRequest(vm).then(function (result) {

                    toastr.success("Back-up Response", "Success");
                    $scope.isBusyRespondingToBackup = false;

                    $scope.selectedContractorId = null;
                    $scope.selectedBackupRequest = null;
                    $scope.selectedBackupDeclinationReasonId = null;
                    $scope.selectedBackupResponseStatus = null;
                    $scope.comments = '';
                    $scope.backupRequests = [];

                    $scope.init();
                });

            };

            $scope.init();


            function backupRequestShiftAndDate(id, backupDate, amRequested, midRequested, pmRequested, amSatisfied, midSatisfied, pmSatisfied) {

                var self = this;

                self.Id = id;
                self.BackupDate = moment(backupDate).format("MM/DD/YYYY");

                self.AMRequested = amRequested;
                self.MIDRequested = midRequested;
                self.PMRequested = pmRequested;

                self.previouslyAMSatisfied = amSatisfied;
                self.previouslyMIDSatisfied = midSatisfied;
                self.previouslyPMSatisfied = pmSatisfied;

                self.AMSatisfied = true;
                self.MIDSatisfied = true;
                self.PMSatisfied = true;

            }
        }
    );
}());