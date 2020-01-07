(function () {
    'use strict';
    mtcApp.controller('backupResourcesController',
        function backupResourcesController($scope, $filter, $interval, $rootScope, $location, $routeParams, backupTruckService, generalService) {

            $scope.header = 'Back-up Resources';

            $scope.backupBeats = [];
            $scope.backupAssignments = [];
            $scope.backupProviders = [];

            $scope.isBusyGettingBackupBeats = false;
            $scope.isBusyGettingBackupAssignments = false;
            $scope.isBusyGettingBackupProviders = false;
            $scope.isBusyGettingContractors = true;
            $scope.isBusyGettingBeatNumbers = true;
            $scope.isBusyGettingFleetVehicles = true;

            $scope.selectedBackupBeatId = 0;
            $scope.selectedBackupProviderId = 0;
            $scope.selectedBackupAssignmentId = 0;

            $scope.selectedContractorId;
            $scope.contractors = [];
            $scope.selectedBeatId;
            $scope.beatNumbers = [];
            $scope.contractorbeatNumbers = [];
            $scope.selectedFleetVehicleId;
            $scope.fleetVehicles = [];
            $scope.contractorfleetVehicles = [];
            $scope.selectedContactName = '';
            $scope.selectedEmail = '';
            $scope.selectedPhone = '';
            $scope.selectedPrimaryBackupBeatId;
            $scope.selectedSecondaryBackupBeatId;
            $scope.selectedTertiaryBackupBeatId;
            $scope.isAdmin = false;

            generalService.getContractors().then(function (results) {
                for (var i = 0; i < results.length; i++) {
                    $scope.contractors.push(results[i]);
                }
                $scope.isBusyGettingContractors = false;

                generalService.getCurrentUser().then(function (results) {
                    if (results) {
                        if (results.SelectedRoleName === 'Admin') {
                            $scope.isAdmin = true;
                        }
                        else {
                            $scope.isAdmin = false;
                        }
                    }
                    else {
                        alert('Error. We were not able to retrieve your current user context. Please come back later.')
                    }
                });
            });

            generalService.getBeatNumbers().then(function (results) {
                for (var i = 0; i < results.length; i++) {
                    $scope.beatNumbers.push(results[i]);
                }
                $scope.isBusyGettingBeatNumbers = false;
            });

            generalService.getVehicles().then(function (results) {
                for (var i = 0; i < results.length; i++) {
                    $scope.fleetVehicles.push(results[i]);
                }
                $scope.isBusyGettingFleetVehicles = false;
            });

            $scope.getContractorBeats = function () {
                generalService.getContractorBeats($scope.selectedContractorId).then(function (results) {
                    $scope.contractorbeatNumbers = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.contractorbeatNumbers.push(results[i]);
                    }
                    $scope.isBusyGettingBeatNumbers = false;
                });
            };

            $scope.getContractorVehicles = function () {
                generalService.getContractorVehicles($scope.selectedContractorId).then(function (results) {
                    $scope.contractorfleetVehicles = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.contractorfleetVehicles.push(results[i]);
                    }
                    $scope.isBusyGettingFleetVehicles = false;
                });
            };

            $scope.getBackupBeats = function () {

                $scope.isBusyGettingBackupBeats = true;

                backupTruckService.getBackupBeats().then(function (results) {

                    $scope.backupBeats = [];

                    for (var i = 0; i < results.length; i++) {
                        $scope.backupBeats.push(results[i]);
                    }

                    $scope.isBusyGettingBackupBeats = false;

                });

            };
            $scope.getBackupBeats();
            $scope.showManageBackupBeatModal = function () {

                $scope.selectedBackupBatId = null;
                $scope.selectedBeatId = null;

                $("#manageBackBeatModal").modal('show');

            };
            $scope.editBackupBeat = function (backupBeat) {

                $scope.selectedBackupBatId = backupBeat.Id;
                $scope.selectedBeatId = backupBeat.BeatId;


                $("#manageBackBeatModal").modal('show');

            };
            $scope.saveBackupBeat = function () {

                $("#manageBackBeatModal").modal('hide');

                var vm = {
                    Id: $scope.selectedBackupBatId,
                    BeatId: $scope.selectedBeatId
                };

                backupTruckService.saveBackupBeat(vm).then(function (result) {

                    toastr.success("Saved", "Back-up Beat");
                    $scope.getBackupBeats();

                });
            };
            $scope.removeBackupBeat = function (backupBeat) {

                if (confirm('Are you sure to remove this backup beat?')) {

                    backupTruckService.removeBackupBeat(backupBeat.Id).then(function (result) {
                        toastr.success("Removed", "Back-up Beat");
                        $scope.getBackupBeats();
                    });
                }
            };

            $scope.getBackupProviders = function () {

                $scope.isBusyGettingBackupProviders = true;

                backupTruckService.getBackupProviders().then(function (results) {
                    $scope.backupProviders = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.backupProviders.push(results[i]);
                    }
                    $scope.isBusyGettingBackupProviders = false;
                });

            };
            $scope.getBackupProviders();
            $scope.showManageBackupProviderModal = function () {

                $scope.selectedBackupProviderId = null;
                $scope.selectedBeatId = null;
                $scope.selectedContractorId = null;
                $scope.selectedEmail = null;
                $scope.selectedPhone = null;
                $scope.selectedFleetVehicleId = null;

                $("#manageBackProviderModal").modal('show');

            };
            $scope.editBackupProvider = function (backupProvider) {

                $scope.selectedBackupProviderId = backupProvider.Id;
                $scope.selectedBackupBeatId = backupProvider.BackupBeatId;
                $scope.selectedContractorId = backupProvider.ContractorId;
                $scope.selectedFleetVehicleId = backupProvider.FleetVehicleId;

                generalService.getContractorVehicles($scope.selectedContractorId).then(function (results) {
                    $scope.contractorfleetVehicles = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.contractorfleetVehicles.push(results[i]);
                    }
                    $scope.isBusyGettingFleetVehicles = false;
                    $scope.selectedFleetVehicleId = backupProvider.FleetVehicleId;
                });



                $("#manageBackProviderModal").modal('show');

            };
            $scope.saveBackupProvider = function () {

                $("#manageBackProviderModal").modal('hide');

                var vm = {
                    Id: $scope.selectedBackupProviderId,
                    BackupBeatId: $scope.selectedBackupBeatId,
                    ContractorId: $scope.selectedContractorId,
                    FleetVehicleId: $scope.selectedFleetVehicleId
                };

                backupTruckService.saveBackupProvider(vm).then(function (result) {

                    toastr.success("Saved", "Back-up Provider");
                    $scope.getBackupProviders();

                });


            };
            $scope.removeBackupProvider = function (backupProvider) {

                if (confirm('Are you sure to remove this backup provider?')) {

                    backupTruckService.removeBackupProvider(backupProvider.Id).then(function (result) {
                        toastr.success("Removed", "Back-up Provider");
                        $scope.getBackupProviders();
                    });

                }


            };

            $scope.getBackupAssignments = function () {

                $scope.isBusyGettingBackupAssignments = true;
                backupTruckService.getBackupAssignments().then(function (results) {

                    $scope.backupAssignments = [];

                    for (var i = 0; i < results.length; i++) {
                        $scope.backupAssignments.push(results[i]);
                    }

                    $scope.isBusyGettingBackupAssignments = false;

                });

            };
            $scope.getBackupAssignments();
            $scope.showManageBackupAssigmentModal = function () {

                $scope.selectedBackupAssignmentId = null;
                $scope.selectedPrimaryBackupBeatId = null;
                $scope.selectedSecondaryBackupBeatId = null;
                $scope.selectedTertiaryBackupBeatId = null;

                $("#manageBackAssignemtnModal").modal('show');
            };
            $scope.editBackupAssignment = function (backupAssignment) {
               
                $scope.selectedBackupAssignmentId = backupAssignment.Id;
                $scope.selectedBeatId = backupAssignment.BeatId;
                $scope.selectedPrimaryBackupBeatId = backupAssignment.PrimaryBackupBeatId;
                $scope.selectedSecondaryBackupBeatId = backupAssignment.SecondaryBackupBeatId;
                $scope.selectedTertiaryBackupBeatId = backupAssignment.TertiaryBackupBeatId;

                $("#manageBackAssignemtnModal").modal('show');

            };
            $scope.saveBackupAssignment = function () {

                $("#manageBackAssignemtnModal").modal('hide');

                var vm = {
                    Id: $scope.selectedBackupAssignmentId,
                    BeatId: $scope.selectedBeatId,
                    PrimaryBackupBeatId: $scope.selectedPrimaryBackupBeatId,
                    SecondaryBackupBeatId: $scope.selectedSecondaryBackupBeatId,
                    TertiaryBackupBeatId: $scope.selectedTertiaryBackupBeatId
                };

                console.log(vm);

                backupTruckService.saveBackupAssignment(vm).then(function (result) {

                    toastr.success("Saved", "Back-up Coverage");
                    $scope.getBackupAssignments();

                });


            };
            $scope.removeBackupAssignment = function (backupAssignment) {

                if (confirm('Are you sure to remove this backup assignment?')) {

                    backupTruckService.removeBackupAssignment(backupAssignment.Id).then(function (result) {
                        toastr.success("Removed", "Back-up Assignment");
                        $scope.getBackupAssignments();
                    });

                }


            };

        }
    );
}());