 'use strict';
(function () {
    mtcApp.controller('backupStatusController',
        function backupStatusController($scope, $filter, $interval, $rootScope, $location, $routeParams, backupTruckService, generalService) {

            $scope.header = 'Back-up Truck Requests Status';
            $scope.backupRequestStatuses = [];
            $scope.cancellationReasons = [];
            $scope.selectedBackupRequestId = 0;
            $scope.selectedCancellationReasonId = 0;

            $scope.isBusy = false;

            $scope.getBackupRequestStatuses = function () {

                $scope.isBusy = true;

                backupTruckService.getBackupRequestStatuses().then(function (results) {
                    $scope.backupRequestStatuses = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.backupRequestStatuses.push(new backupRequestStatus(results[i]));
                    }
                    $scope.isBusy = false;
                });

            };
            $scope.getBackupRequestStatuses();

            $scope.prepareCancelRequest = function (Id) {

                $scope.selectedBackupRequestId = Id;
                $("#cancelRequestModal").modal('show');
                $scope.cancellationComments = '';

                $scope.isBusyGettingCancellationReasons = true;
                backupTruckService.getBackupCancellationReasons().then(function (results) {
                    $scope.cancellationReasons = [];

                    for (var i = 0; i < results.length; i++) {
                        $scope.cancellationReasons.push(results[i]);
                    }
                    $scope.isBusyGettingCancellationReasons = false;
                });

            };

            $scope.cancelRequest = function () {

                if (confirm("Ok to cancel this request?")) {

                    $("#cancelRequestModal").modal('hide');

                    var vm = {
                        Id: $scope.selectedBackupRequestId,
                        cancellationReasonId: $scope.selectedCancellationReasonId,
                        comments: $scope.cancellationComments
                    };

                    backupTruckService.cancelBackupRequest(vm).then(function (result) {

                        toastr.success("Back-up", "Request Successfully Cancelled");
                        $scope.getBackupRequestStatuses();
                    });
                }


            };

            function backupRequestStatus(dbStatus) {
                var self = this;
                self.Id = dbStatus.Id,
                self.RequestNumber = dbStatus.RequestNumber;
                self.BackupRequestId = dbStatus.BackupRequestId
                self.ContractorId = dbStatus.ContractorId;
                self.ContractorCompanyName = dbStatus.ContractorCompanyName;
                self.CreatedOn = moment(dbStatus.CreatedOn).format('MM/DD/YY hh:mm');
                self.BeatId = dbStatus.BeatId;
                self.BeatNumber = dbStatus.BeatNumber;
                self.IsCancelled = dbStatus.IsCancelled;
                self.IsResolved = dbStatus.IsResolved;
                self.IsPartiallyResolved = dbStatus.IsPartiallyResolved;
                self.HasPermissionToCancel = dbStatus.HasPermissionToCancel;

                self.CancellationComment = dbStatus.CancellationComment;
                self.CancelledBy = dbStatus.CancelledBy;
                self.CancelledOn = moment(dbStatus.CancelledOn).format("MM/DD/YY hh:mm");
                self.BackupCancellationReason = dbStatus.BackupCancellationReason;

                self.BackupRequestShiftsAndDates = [];
                if (dbStatus.BackupRequestShiftsAndDates) {
                    for (var ii = 0; ii < dbStatus.BackupRequestShiftsAndDates.length; ii++) {
                        self.BackupRequestShiftsAndDates.push(new backupRequestShiftAndDate(dbStatus.BackupRequestShiftsAndDates[ii]));
                    }                    
                }
                 


                self.PrimaryComments = '';
                self.SecondaryComments = '';
                self.TertiaryComments = '';
                self.PrimaryContractorCompanyName = '';
                self.SecondaryContractorCompanyName = '';
                self.TertiaryContractorCompanyName = '';


                for (var i = 0; i < dbStatus.BackupResponses.length; i++) {

                    if (dbStatus.BackupResponses[i].BackupAssignmentLevel === 0) {
                        self.PrimaryComments = dbStatus.BackupResponses[i].Comments;
                        self.PrimaryContractorCompanyName = dbStatus.BackupResponses[i].ContractorCompanyName;
                    }
                    else if (dbStatus.BackupResponses[i].BackupAssignmentLevel === 1) {
                        self.SecondaryComments = dbStatus.BackupResponses[i].Comments;
                        self.SecondaryContractorCompanyName = dbStatus.BackupResponses[i].ContractorCompanyName;
                    }
                    else if (dbStatus.BackupResponses[i].BackupAssignmentLevel === 2) {
                        self.TertiaryComments = dbStatus.BackupResponses[i].Comments;
                        self.TertiaryContractorCompanyName = dbStatus.BackupResponses[i].ContractorCompanyName;
                    }
                    else if (dbStatus.BackupResponses[i].BackupAssignmentLevel === 100) {
                        self.OtherComments = dbStatus.BackupResponses[i].Comments;
                        self.OtherContractorCompanyName = dbStatus.BackupResponses[i].ContractorCompanyName;
                    }
                }

                if (!self.PrimaryComments && dbStatus.PrimaryBackupResponseTimeExpiredOrDeclined === true)
                    self.PrimaryComments = 'Response time expired';
                if (!self.SecondaryComments && dbStatus.SecondaryBackupResponseTimeExpiredOrDeclined === true)
                    self.SecondaryComments = 'Response time expired';
                if (!self.TertiaryComments && dbStatus.TertiaryBackupResponseTimeExpiredOrDeclined === true)
                    self.TertiaryComments = 'Response time expired';


            };

            function backupRequestShiftAndDate(dbBackupRequestShiftsAndDates) {

                var self = this;

                self.Id = dbBackupRequestShiftsAndDates.Id;
                self.BackupDate = moment(dbBackupRequestShiftsAndDates.BackupDate).format("MM/DD/YYYY");

                self.AMRequested = dbBackupRequestShiftsAndDates.AMRequested;
                self.MIDRequested = dbBackupRequestShiftsAndDates.MIDRequested;
                self.PMRequested = dbBackupRequestShiftsAndDates.PMRequested;

            }
        }
    );
}());