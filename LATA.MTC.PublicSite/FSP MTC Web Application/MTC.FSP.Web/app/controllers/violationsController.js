(function() {
    'use strict';
    mtcApp.controller('violationsController',
        function violationsController($scope, $filter, $interval, $rootScope, $location, $routeParams, violationsService, generalService) {

            $scope.header = 'Violations';
            $scope.canEdit = false;
            $scope.violations = [];
            $scope.violation;
            $scope.isBusyGettingViolations = true;
            $scope.busyGettingCallSigns = true;
            $scope.busyGettingVehicles = true;
            $scope.busyGettingBeatNumbers = true;
            $scope.contractors = [];
            $scope.violationTypes = [];
            $scope.alarmNames = [];
            $scope.violationStatusTypes = [];
            $scope.chpOfficers = [];
            $scope.contractorBeatNumbers = [];
            $scope.contractorDrivers = [];
            $scope.contractorVehicles = [];

            $scope.totalRecordsFound = 0;
            $scope.page = 0;
            $scope.pageSize = 20;
            $scope.pagesCount = 0;
            $scope.searchValue = 'policy';

            $scope.query = {
                page: $scope.page,
                pageSize: $scope.pageSize,
                beatId: '',
                driverId: '',
                contractorId: '',
                vehicleId: '',
                violationTypeId: '',
                startDate: '',
                endDate: '',
                alarmName: ''
            };

            //can Edit
            generalService.getCanEdit('Violations').then(function(result) {
                $scope.canEdit = result;
            });

            $scope.getViolations = function(page) {

                $scope.isBusyGettingViolations = true;
                $scope.query.page = page || 0;

                violationsService.getViolations($scope.query).then(function(results) {

                    $scope.violations = [];
                    $scope.page = results.Page;
                    $scope.pagesCount = results.TotalPages;
                    $scope.totalRecordsFound = results.TotalCount;

                    $scope.query.page = $scope.page;

                    if (results.Items.length > 0) {
                        for (var i = 0; i < results.Items.length; i++) {

                            results.Items[i].DateTimeOfViolationFriendly = moment(results.Items[i].DateTimeOfViolation).format('MM/DD/YYYY hh:mm A');
                            results.Items[i].DateOfViolation = moment(results.Items[i].DateTimeOfViolation).format('MM/DD/YYYY');
                            results.Items[i].TimeOfViolation = new Date();
                            results.Items[i].TimeOfViolation.setHours(moment(results.Items[i].DateTimeOfViolation).format('HH'));
                            results.Items[i].TimeOfViolation.setMinutes(moment(results.Items[i].DateTimeOfViolation).format('mm'));
                            results.Items[i].CreatedOnFriendly = moment(results.Items[i].CreatedOn).format('MM/DD/YYYY');
                            $scope.violations.push(results.Items[i]);
                        }
                    } else
                        $scope.totalRecordsFound = 0;

                    $scope.isBusyGettingViolations = false;
                    $rootScope.redrawTable();

                });

            };

            //table pager
            $scope.getRecords = function(page) {
                page = page || 0;
                $scope.page = page;
                $scope.getViolations($scope.page);
            };
            $scope.reset = function(page) {
                $scope.page = 0;
                $scope.query = {
                    page: $scope.page,
                    pageSize: $scope.pageSize,
                    beatId: '',
                    driverId: '',
                    contractorId: '',
                    vehicleId: '',
                    violationTypeId: '',
                    startDate: '',
                    endDate: '',
                    alarmType: ''
                };
                $scope.getViolations($scope.page);
            };

            $scope.addViolation = function() {
                $scope.violation = null;
                $("#manageViolationModal").modal('show');
            };
            $scope.manageViolation = function(i) {
                $scope.violation = i;
                $scope.getContractorData(i.ContractorId);
                $("#manageViolationModal").modal('show');
            };
            $scope.saveViolation = function() {

                $scope.isBusyGettingViolations = true;

                var vm = {
                    Id: $scope.violation.Id,
                    ViolationTypeId: $scope.violation.ViolationTypeId,
                    ContractorId: $scope.violation.ContractorId,
                    DateTimeOfViolation: $scope.violation.DateOfViolation + ' ' + moment($scope.violation.TimeOfViolation).format('HH:mm'),
                    BeatId: $scope.violation.BeatId,
                    DriverId: $scope.violation.DriverId,
                    FleetVehicleId: $scope.violation.FleetVehicleId,
                    CallSign: $scope.violation.CallSign,
                    OffenseNumber: $scope.violation.OffenseNumber,
                    ViolationStatusTypeId: $scope.violation.ViolationStatusTypeId,
                    DeductionAmount: $scope.violation.DeductionAmount,
                    Notes: $scope.violation.Notes,
                    PenaltyForDriver: $scope.violation.PenaltyForDriver
                };

                violationsService.saveViolation(vm).then(function(result) {

                    $("#manageViolationModal").modal('hide');
                    toastr.success('Violation', 'Successfully saved');
                    $scope.getViolations();
                    $scope.isBusyGettingViolations = false;
                });

            };
            $scope.removeViolation = function(i) {

                if (confirm("Are you sure to remove this violation?")) {

                    $scope.isBusyGettingViolations = true;

                    violationsService.removeViolation(i.Id).then(function(result) {
                        toastr.warning("violation", "Successfully removed");
                        $scope.getViolations();
                        $scope.isBusyGettingViolations = false;
                    });
                };

            };

            //common
            generalService.getDrivers().then(function(results) {
                $scope.drivers = results;
                $scope.busyGettingDrivers = false;
            });
            generalService.getBeatNumbers().then(function(results) {
                $scope.beatNumbers = results;
                $scope.busyGettingBeatNumbers = false;
            });
            generalService.getVehicles().then(function(results) {
                $scope.vehicles = results;
                $scope.busyGettingVehicles = false;
            });
            generalService.getAlarmTypes().then(function(results) {
                for (var i = 0; i < results.length; i++) {
                    if (results[i] != null || results[i] != 'null') {
                        $scope.alarmNames.push(results[i]);
                    }
                }
                $scope.busyGettingCallSigns = false;
            });
            generalService.getContractors().then(function(results) {
                for (var i = 0; i < results.length; i++) {
                    $scope.contractors.push(results[i]);
                }

            });
            generalService.getViolationTypes().then(function(results) {
                for (var i = 0; i < results.length; i++) {
                    $scope.violationTypes.push(results[i]);
                }
            });
            generalService.getViolationStatusTypes().then(function(results) {
                for (var i = 0; i < results.length; i++) {
                    if (results[i].Text !== 'Deducted')
                        $scope.violationStatusTypes.push(results[i]);
                }
            });
            generalService.getCHPOfficers().then(function(results) {
                for (var i = 0; i < results.length; i++) {
                    $scope.chpOfficers.push(results[i]);
                }

            });

            $scope.getContractorData = function(contractorId) {
                $scope.getContractorBeats(contractorId);
                $scope.getContractorDrivers(contractorId);
                $scope.getContractorVehicles(contractorId);
            };
            $scope.getContractorBeats = function(contractorId) {
                generalService.getContractorBeats(contractorId).then(function(results) {
                    $scope.contractorBeatNumbers = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.contractorBeatNumbers.push(results[i]);
                    }
                });
            };
            $scope.getContractorDrivers = function(contractorId) {
                generalService.getContractorDrivers(contractorId).then(function(results) {
                    $scope.contractorDrivers = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.contractorDrivers.push(results[i]);
                    }
                });
            };
            $scope.getContractorVehicles = function(contractorId) {
                generalService.getContractorVehicles(contractorId).then(function(results) {
                    $scope.contractorVehicles = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.contractorVehicles.push(results[i]);
                    }
                });
            };

            $scope.getViolations(0);

        }
    );
}());