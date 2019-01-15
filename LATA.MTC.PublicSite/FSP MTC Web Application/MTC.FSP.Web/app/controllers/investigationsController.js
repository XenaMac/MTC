'use strict';
(function () {
    mtcApp.controller('investigationsController',
        function investigationsController($scope, $filter, $interval, $rootScope, $location, $routeParams, investigationsService, generalService) {

            $scope.header = 'Investigations';
            $scope.investigations = [];
            $scope.investigation;
            $scope.isBusyGettingInvestigations = true;
            $scope.contractors = [];
            $scope.violationTypes = [];
            $scope.chpOfficers = [];
            $scope.contractorBeatNumbers = [];
            $scope.contractorDrivers = [];

            $scope.getInvestigations = function () {

                $scope.isBusyGettingInvestigations = true;
                investigationsService.getInvestigations().then(function (results) {
                    $scope.investigations = [];
                    for (var i = 0; i < results.length; i++) {

                        if (moment(results[i].Date).isValid())
                            results[i].DateFriendly = moment(results[i].Date).format('MM/DD/YYYY');
                        $scope.investigations.push(results[i]);
                    }
                    $scope.isBusyGettingInvestigations = false;
                    $rootScope.redrawTable();
                });

            };
            $scope.getInvestigations();
            $scope.addInvestigation = function () {
                $scope.investigation = null;
                $("#manageInvestigationModal").modal('show');
            };
            $scope.manageInvestigation = function (i) {
                $scope.investigation = i;
                $scope.getContractorData(i.ContractorId);
                $("#manageInvestigationModal").modal('show');
            };
            $scope.saveInvestigation = function () {

                $scope.isBusyGettingInvestigations = true;

                var vm = {
                    Id: $scope.investigation.Id,
                    Date: $scope.investigation.DateFriendly,
                    DriverId: $scope.investigation.DriverId,
                    BeatId: $scope.investigation.BeatId,
                    ContractorId: $scope.investigation.ContractorId,
                    CHPOfficerId: $scope.investigation.CHPOfficerId,
                    ViolationTypeId: $scope.investigation.ViolationTypeId,
                    Summary: $scope.investigation.Summary
                };

                investigationsService.saveInvestigation(vm).then(function (result) {

                    $("#manageInvestigationModal").modal('hide');
                    toastr.success('Investigation', 'Successfullye saved');
                    $scope.getInvestigations();
                    $scope.isBusyGettingInvestigations = false;
                });

            };
            $scope.removeInvestigation = function (i) {

                if (confirm("Are you sure to remove this investigation?")) {

                    $scope.isBusyGettingInvestigations = true;

                    investigationsService.removeInvestigation(i.Id).then(function (result) {
                        toastr.warning("Investigation", "Successfully removed");
                        $scope.getInvestigations();
                        $scope.isBusyGettingInvestigations = false;
                    });
                };

            };

            //common
            generalService.getContractors().then(function (results) {
                for (var i = 0; i < results.length; i++) {
                    $scope.contractors.push(results[i]);
                }

            });
            generalService.getViolationTypes().then(function (results) {
                for (var i = 0; i < results.length; i++) {
                    $scope.violationTypes.push(results[i]);
                }

            });
            generalService.getCHPOfficers().then(function (results) {
                for (var i = 0; i < results.length; i++) {
                    $scope.chpOfficers.push(results[i]);
                }

            });

            $scope.getContractorData = function (contractorId) {
                //$scope.getContractorBeats(contractorId);
                $scope.getContractorDrivers(contractorId);
            };

            generalService.getBeatNumbers().then(function (results) {
                $scope.contractorBeatNumbers = [];
                for (var i = 0; i < results.length; i++) {
                    $scope.contractorBeatNumbers.push(results[i]);
                }
            });

            $scope.getContractorBeats = function (contractorId) {
                generalService.getContractorBeats(contractorId).then(function (results) {
                    $scope.contractorBeatNumbers = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.contractorBeatNumbers.push(results[i]);
                    }
                });
            };
            $scope.getContractorDrivers = function (contractorId) {
                generalService.getContractorDrivers(contractorId).then(function (results) {
                    $scope.contractorDrivers = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.contractorDrivers.push(results[i]);
                    }
                });
            };

        }
    );
}());