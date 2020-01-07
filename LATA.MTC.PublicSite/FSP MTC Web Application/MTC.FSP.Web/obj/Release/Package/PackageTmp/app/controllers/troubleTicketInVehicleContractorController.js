(function () {
    'use strict';
    mtcApp.controller('troubleTicketInVehicleContractorController',
        function troubleTicketInVehicleContractorController($scope, $filter, $rootScope, $location, $routeParams, $interval, generalService, troubleTicketService) {

            $scope.isBusySaving = false;
            $scope.isBusyGettingTroubleTickets = false;
            $scope.isInVehicleGeneralContractor = false;
            $scope.isInVehicleLATATraxContractor = false;

            $scope.ticketType = 1;
            $scope.troubleTicketId = 0;

            $scope.associatedTowContractorId; //this would be a in-vehicle or lata contractor the tow-contractor assigned a ticket to
            $scope.associatedInVehicleContractorId; //this is the logged on tow-contractor
            $scope.associatedInVehicleLATATraxContractorId; //selected contractors from dropdown. This are going to be "in-vehicle" or "lata" contractors

            $scope.towContractors = [];
            $scope.inVehicleContractors = [];
            $scope.inVehicleLATATraxContractors = [];

            $scope.troubleTickets = [];

            $scope.selectedTicketStatus = 0;
            $scope.isBusyGettingFleetVehicles = false;
            $scope.isBusyGettingDrivers = false;
            $scope.selectedFleetVehicleId;
            $scope.fleetVehicles = [];

            $scope.selectedDrivers = [];
            $scope.drivers = [];

            $scope.troubleTicketComponentIssues = [];
            $scope.selectedTroubleTicketComponentIssues = [];

            $scope.troubleTicketLATATraxIssues = [];
            $scope.selectedTroubleTicketLATATraxIssues = [];

            $scope.init = function (ticketType) {
                $scope.ticketType = eval(ticketType);
                $scope.changeView($scope.ticketType);
                $scope.getTroubleTickets();
                $scope.getTroubleTicketComponentIssues();
                $scope.getTroubleTicketLATATraxIssues();
            };

            $scope.canEdit = false;
            generalService.getCanEdit('TroubleTickets').then(function (result) {
                $scope.canEdit = result;
            });


            generalService.getCurrentUser().then(function (results) {
                if (results) {
                    if (results.SelectedRoleName === 'InVehicleContractor') {
                        $scope.loggedOnContractorId = results.ContractorId;
                        $scope.isInVehicleLATATraxContractor = false;
                        $scope.isInVehicleGeneralContractor = true;
                    }
                    else {
                        $scope.isInVehicleLATATraxContractor = true;
                        $scope.isInVehicleGeneralContractor = false;
                    }
                }
                else {
                    $scope.header = 'Error. We were not able to retrieve your current user context. Please come back later.';
                }
            });

            generalService.getContractors('Tow-Contractor').then(function (results) {
                $scope.isBusyGettingTowContractors = true;
                $scope.towContractors = [];
                for (var i = 0; i < results.length; i++) {
                    $scope.towContractors.push(results[i]);
                }
                $scope.isBusyGettingTowContractors = false;
            });
            generalService.getContractors('In-Vehicle').then(function (results) {
                $scope.inVehicleContractors = [];
                for (var i = 0; i < results.length; i++) {
                    $scope.inVehicleContractors.push(results[i]);
                }
            });
            //generalService.getContractors('In-Vehicle LATATrax').then(function (results) {
            //    $scope.inVehicleLATATraxContractors = [];
            //    for (var i = 0; i < results.length; i++) {
            //        $scope.inVehicleLATATraxContractors.push(results[i]);
            //    }
            //});
            generalService.getContractors('In-Vehicle').then(function (results) {
                $scope.inVehicleLATATraxContractors = [];
                for (var i = 0; i < results.length; i++) {
                    $scope.inVehicleLATATraxContractors.push(results[i]);
                }
            });
            

            $scope.changeView = function (ticketType) {

                $scope.ticketType = ticketType;

                var contractorTypeName = '';

                if ($scope.ticketType === 1) {
                    $scope.header = 'In-Vehicle Equipment General';
                    contractorTypeName = 'In-Vehicle';
                }
                else if ($scope.ticketType === 2) {
                    $scope.header = 'In-Vehicle Equipment LATATrax';
                    contractorTypeName = 'In-Vehicle LATATrax';
                }
                else if ($scope.ticketType === 3)
                    $scope.header = 'At Vehicle';

                //if (contractorTypeName) {
                //    generalService.getContractors(contractorTypeName).then(function (results) {
                //        $scope.isBusyGettingContractors = true;
                //        $scope.contractors = [];
                //        for (var i = 0; i < results.length; i++) {
                //            $scope.contractors.push(results[i]);
                //        }
                //        $scope.selectedContractorId = $scope.contractors[0].Id;
                //        $scope.isBusyGettingContractors = false;
                //    });
                //}

                $scope.redrawTable();

            };


            $scope.getTroubleTickets = function () {

                $scope.isBusyGettingTroubleTickets = true;

                troubleTicketService.getTroubleTickets().then(function (results) {

                    $scope.troubleTickets = [];

                    for (var i = 0; i < results.length; i++) {

                        if (moment(results[i].CreatedOn).isValid())
                            results[i].CreateOnNice = moment(results[i].CreatedOn).format("MM/DD/YY hh:mm");

                        if (moment(results[i].ProblemStartedOn).isValid())
                            results[i].ProblemStartedOnNice = moment(results[i].ProblemStartedOn).format("MM/DD/YY");

                        if (moment(results[i].DateTruckBackInService).isValid())
                            results[i].DateTruckBackInServiceNice = moment(results[i].DateTruckBackInService).format("MM/DD/YY");

                        if (moment(results[i].DateTruckOutOfService).isValid())
                            results[i].DateTruckOutOfServiceNice = moment(results[i].DateTruckOutOfService).format("MM/DD/YY");

                        if (moment(results[i].FirstShiftTruckMissed).isValid())
                            results[i].FirstShiftTruckMissed = moment(results[i].FirstShiftTruckMissed).format("MM/DD/YY");

                        if (moment(results[i].LastShiftTruckMissed).isValid())
                            results[i].LastShiftTruckMissed = moment(results[i].LastShiftTruckMissed).format("MM/DD/YY");

                        $scope.troubleTickets.push(results[i]);
                    }

                    $scope.isBusyGettingTroubleTickets = false;
                    $scope.redrawTable();

                });

            };

            $scope.getTroubleTicketComponentIssues = function () {
                troubleTicketService.getTroubleTicketComponentIssues().then(function (results) {
                    $scope.troubleTicketComponentIssues = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.troubleTicketComponentIssues.push(results[i]);
                    }
                });
            };

            $scope.getTroubleTicketLATATraxIssues = function () {
                troubleTicketService.getTroubleTicketLATATraxIssues().then(function (results) {
                    $scope.troubleTicketLATATraxIssues = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.troubleTicketLATATraxIssues.push(results[i]);
                    }
                });
            };


            $scope.toggleComponentIssuesSelection = function (id) {
                var idx = $scope.selectedTroubleTicketComponentIssues.indexOf(id);
                if (idx > -1) {
                    $scope.selectedTroubleTicketComponentIssues.splice(idx, 1);
                }
                else {
                    $scope.selectedTroubleTicketComponentIssues.push(id);
                }
            };

            $scope.toggleLATATraxIssuesSelection = function (id) {
                var idx = $scope.selectedTroubleTicketLATATraxIssues.indexOf(id);
                if (idx > -1) {
                    $scope.selectedTroubleTicketLATATraxIssues.splice(idx, 1);
                }
                else {
                    $scope.selectedTroubleTicketLATATraxIssues.push(id);
                }
            };

            $scope.toggleDriversSelection = function (id) {
                var idx = $scope.selectedDrivers.indexOf(id);
                if (idx > -1) {
                    $scope.selectedDrivers.splice(idx, 1);
                }
                else {
                    $scope.selectedDrivers.push(id);
                }

                console.log('Selected Drivers ' + $scope.selectedDrivers.length);
            };


            $scope.getTowContractorData = function () {

                $scope.isBusyGettingFleetVehicles = true;
                generalService.getContractorVehicles($scope.associatedTowContractorId).then(function (results) {
                    $scope.fleetVehicles = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.fleetVehicles.push(results[i]);
                    }
                    $scope.isBusyGettingFleetVehicles = false;
                });

                $scope.isBusyGettingDrivers = true;
                generalService.getContractorDrivers($scope.associatedTowContractorId).then(function (results) {
                    $scope.drivers = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.drivers.push(results[i]);
                    }
                    $scope.isBusyGettingDrivers = false;
                });

            };

            $scope.showManageTroubleTicketModal = function () {

                $scope.troubleTicketId = null;
                $scope.selectedTicketStatus = 0;
                $scope.selectedFleetVehicleId = null;

                $scope.selectedTroubleTicketComponentIssues = [];
                $scope.selectedTroubleTicketLATATraxIssues = [];
                $scope.selectedDrivers = [];

                $scope.dateProblemStarted = '';
                $scope.dateTruckOutOfService = '';
                $scope.dateTruckBackInService = '';
                $scope.shiftsToMiss = '';

                $scope.mtcNotes = '';
                $scope.contractorNotes = '';
                $scope.inVehicleContractorNotes = '';
                $scope.lATANotes = '';

                $scope.reliaGateOEMSerialNumber = '';

                if ($scope.ticketType === 1)
                    $("#manageInVehicleEquipmentTroubleTicketModal").modal("show");
                else if ($scope.ticketType === 2)
                    $("#manageInVehicleEquipmentLATATraxTroubleTicketModal").modal("show");
            };

            $scope.editTroubleTicket = function (ticket) {

                $scope.troubleTicketId = ticket.Id;
                $scope.ticketType = ticket.TroubleTicketType;
                $scope.selectedTicketStatus = ticket.TroubleTicketStatus;

                $scope.associatedTowContractorId = ticket.AssociatedTowContractorId;
                $scope.associatedInVehicleContractorId = ticket.AssociatedInVehicleContractorId;
                $scope.associatedInVehicleLATATraxContractorId = ticket.AssociatedInVehicleLATATraxContractorId;

                $scope.getTowContractorData();


                var delay = $interval(function () {
                    if ($scope.isBusyGettingDrivers === false) {
                        $interval.cancel(delay);
                        $scope.selectedDrivers = [];
                        for (var i = 0; i < ticket.TroubleTicketDrivers.length; i++) {
                            $scope.selectedDrivers.push(ticket.TroubleTicketDrivers[i].DriverId);
                        }
                        console.log('Selected Drivers ' + $scope.selectedDrivers);
                    }
                }, 50);

                $scope.selectedFleetVehicleId = ticket.VehicleId;

                $scope.selectedTroubleTicketComponentIssues = [];
                for (var i = 0; i < ticket.TroubleTicketComponentIssues.length; i++) {
                    $scope.selectedTroubleTicketComponentIssues.push(ticket.TroubleTicketComponentIssues[i].TroubleTicketComponentIssueId);
                }

                $scope.selectedTroubleTicketLATATraxIssues = [];
                for (var i = 0; i < ticket.TroubleTicketLATATraxIssues.length; i++) {
                    $scope.selectedTroubleTicketLATATraxIssues.push(ticket.TroubleTicketLATATraxIssues[i].TroubleTicketLATATraxIssueId);
                }

                if (moment(ticket.ProblemStartedOn).isValid())
                    $scope.dateProblemStarted = moment(ticket.ProblemStartedOn).format("MM/DD/YY");

                if (moment(ticket.DateTruckOutOfService).isValid())
                    $scope.dateTruckOutOfService = moment(ticket.DateTruckOutOfService).format("MM/DD/YY");

                if (moment(ticket.DateTruckBackInService).isValid())
                    $scope.dateTruckBackInService = moment(ticket.DateTruckBackInService).format("MM/DD/YY");

                $scope.shiftsToMiss = ticket.ShiftsMissed;
                $scope.contactName = ticket.ContactName;
                $scope.contactPhone = ticket.ContactPhone;
                $scope.mtcNotes = ticket.MTCNotes;
                $scope.contractorNotes = ticket.ContractorNotes;
                $scope.inVehicleContractorNotes = ticket.InVehicleContractorNotes;
                $scope.lATANotes = ticket.LATANotes;

                $scope.reliaGateOEMSerialNumber = ticket.ReliaGateOEMSerialNumber;

                if ($scope.ticketType === 1)
                    $("#manageInVehicleEquipmentTroubleTicketModal").modal("show");
                else if ($scope.ticketType === 2)
                    $("#manageInVehicleEquipmentLATATraxTroubleTicketModal").modal("show");

            };

            $scope.saveTroubleTicket = function () {

                $scope.isBusySaving = true;

                var vm = {
                    Id: $scope.troubleTicketId,
                    BackupRequestSubmitted: $scope.backupRequestSubmitted,

                    AssociatedTowContractorId: $scope.associatedTowContractorId,
                    AssociatedInVehicleContractorId: $scope.associatedInVehicleContractorId,
                    AssociatedInVehicleLATATraxContractorId: $scope.associatedInVehicleLATATraxContractorId,

                    DateTruckBackInService: $scope.dateTruckBackInService,
                    DateTruckOutOfService: $scope.dateTruckOutOfService,

                    MTCNotes: $scope.mtcNotes,
                    ContractorNotes: $scope.contractorNotes,
                    InVehicleContractorNotes: $scope.inVehicleContractorNotes,
                    LATANotes: $scope.lATANotes,

                    ReliaGateOEMSerialNumber: $scope.reliaGateOEMSerialNumber,

                    TroubleTicketProblemIds: $scope.selectedTroubleTicketProblems,
                    TroubleTicketComponentIssueIds: $scope.selectedTroubleTicketComponentIssues,
                    TroubleTicketDriverIds: $scope.selectedDrivers,
                    TroubleTicketLATATraxIssueIds: $scope.selectedTroubleTicketLATATraxIssues,
                    ProblemStartedOn: $scope.dateProblemStarted,
                    ShiftsMissed: $scope.shiftsToMiss,
                    TroubleTicketStatus: $scope.selectedTicketStatus,
                    TroubleTicketType: $scope.ticketType,
                    VehicleId: $scope.selectedFleetVehicleId,

                    ContactName: $scope.contactName,
                    ContactPhone: $scope.contactPhone
                };

                troubleTicketService.saveTroubleTicket(vm).then(function (result) {

                    var res = JSON.parse(result);

                    if (res === true)
                        toastr.success("Saved", "Trouble Ticket");
                    else
                        toastr.error("Error", "Saving Trouble Ticket");

                    $scope.selectedFleetVehicleId = null;
                    $scope.problem = '';
                    $scope.dateProblemStarted = '';
                    $scope.shiftsToMiss = '';
                    $scope.actionTakenByContractor = '';
                    $scope.mtcNotes = '';

                    $scope.isBusySaving = false;

                    $("#manageInVehicleEquipmentTroubleTicketModal").modal("hide");
                    $("#manageInVehicleEquipmentLATATraxTroubleTicketModal").modal("hide");

                    $scope.getTroubleTickets();

                });

            };

            $scope.redrawTable = function () {

                var browserDiscoveryInterval = $interval(function () {
                    $interval.cancel(browserDiscoveryInterval);
                    $('.footable').trigger('footable_redraw');
                }, 10);

            };
        }
    );
}());