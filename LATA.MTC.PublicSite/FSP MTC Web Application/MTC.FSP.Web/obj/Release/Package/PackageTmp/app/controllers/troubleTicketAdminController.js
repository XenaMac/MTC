(function () {
    'use strict';
    mtcApp.controller('troubleTicketAdminController',
        function troubleTicketAdminController($scope, $filter, $rootScope, $location, $routeParams, $interval, generalService, troubleTicketService) {

            $scope.troubleTicketId = 0;
            $scope.ticketType = 0;
            $scope.selectedTicketStatus = 0;
            $scope.isBusyGettingContractors = true;
            $scope.isBusyGettingFleetVehicles = true;
            $scope.isBusyGettingTroubleTickets = false;
            $scope.isBusySaving = false;


            $scope.associatedTowContractorId; //this would be a in-vehicle or lata contractor the tow-contractor assigned a ticket to
            $scope.associatedInVehicleContractorId; //this is the logged on tow-contractor
            $scope.associatedInVehicleLATATraxContractorId; //selected contractors from dropdown. This are going to be "in-vehicle" or "lata" contractors

            $scope.towContractors = [];
            $scope.inVehicleContractors = [];
            $scope.inVehicleLATATraxContractors = [];

            $scope.selectedFleetVehicleId;
            $scope.fleetVehicles = [];

            $scope.troubleTickets = [];
            $scope.troubleTicketProblems = [];
            $scope.selectedTroubleTicketProblems = [];

            $scope.troubleTicketComponentIssues = [];
            $scope.selectedTroubleTicketComponentIssues = [];

            $scope.troubleTicketLATATraxIssues = [];
            $scope.selectedTroubleTicketLATATraxIssues = [];

            $scope.init = function (ticketType) {
                $scope.ticketType = eval(ticketType);
                $scope.changeView($scope.ticketType);
                $scope.getTroubleTickets();
                $scope.getTroubleTicketProblems();
                $scope.getTroubleTicketComponentIssues();
                $scope.getTroubleTicketLATATraxIssues();
            }

            $scope.canEdit = false;
            generalService.getCanEdit('TroubleTickets').then(function (result) {
                $scope.canEdit = result;
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

                if ($scope.ticketType === 0) {
                    $scope.header = 'Mechanical/Truck Out of Service';
                    contractorTypeName = 'Tow-Contractor';
                }
                else if ($scope.ticketType === 1) {
                    $scope.header = 'In-Vehicle Equipment General';
                    contractorTypeName = 'In-Vehicle';
                }
                else if ($scope.ticketType === 2) {
                    $scope.header = 'In-Vehicle Equipment LATATrax';
                    contractorTypeName = 'In-Vehicle LATATrax';
                }
                else if ($scope.ticketType === 3)
                    $scope.header = 'Back in Service';

                //if (contractorTypeName) {
                //    generalService.getContractors(contractorTypeName).then(function (results) {
                //        $scope.isBusyGettingContractors = true;
                //        $scope.contractors = [];
                //        for (var i = 0; i < results.length; i++) {
                //            $scope.contractors.push(results[i]);
                //        }
                //        $scope.isBusyGettingContractors = false;
                //    });
                //}

                $scope.redrawTable();

            };

            $scope.showTicketsFilter = function (item) {

                if ($scope.ticketType < 3) {
                    return item.TroubleTicketType === $scope.ticketType;
                }
                else {
                    //back in-service
                    if (item.TroubleTicketStatus === 2)
                        return true;
                    else
                        return false;
                }


            };

            $scope.showManageTroubleTicketModal = function () {
                $("#manageTroubleTicketModal").modal("show");
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

            $scope.getContractorVehicles = function () {

                $scope.isBusyGettingFleetVehicles = false;

                //if ($scope.ticketType === 0) {
                //    generalService.getContractorVehicles($scope.associatedTowContractorId).then(function (results) {
                //        $scope.fleetVehicles = [];

                //        for (var i = 0; i < results.length; i++) {
                //            $scope.fleetVehicles.push(results[i]);
                //        }
                //        $scope.isBusyGettingFleetVehicles = false;
                //    });
                //}
                //else {
                //    generalService.getVehicles().then(function (results) {
                //        $scope.fleetVehicles = [];
                //        for (var i = 0; i < results.length; i++) {
                //            $scope.fleetVehicles.push(results[i]);
                //        }
                //        $scope.isBusyGettingFleetVehicles = false;
                //    });

                //}

                generalService.getContractorVehicles($scope.associatedTowContractorId).then(function (results) {
                    $scope.fleetVehicles = [];

                    for (var i = 0; i < results.length; i++) {
                        $scope.fleetVehicles.push(results[i]);
                    }
                    $scope.isBusyGettingFleetVehicles = false;
                });

            };

            $scope.getTroubleTicketProblems = function () {
                troubleTicketService.getTroubleTicketProblems().then(function (results) {
                    $scope.troubleTicketProblems = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.troubleTicketProblems.push(results[i]);
                    }
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


            $scope.toggleSelection = function toggleSelection(id) {
                var idx = $scope.selectedTroubleTicketProblems.indexOf(id);
                if (idx > -1) {
                    $scope.selectedTroubleTicketProblems.splice(idx, 1);
                }
                else {
                    $scope.selectedTroubleTicketProblems.push(id);
                }
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

            $scope.resolveMechanicalTroubleTicket = function (ticket) {

                $scope.contractorNotes = ticket.ContractorNotes;
                $scope.mtcNotes = ticket.MTCNotes;
                $scope.troubleTicketId = ticket.Id;

                $("#resolveMechanicalTroubleTicketModal").modal("show");
            };

            $scope.resolveTroubleTicket = function () {

                $scope.isBusySaving = true;

                var vm = {
                    Id: $scope.troubleTicketId,
                    FirstShiftTruckMissed: $scope.firstShiftTruckMissed,
                    LastShiftTruckMissed: $scope.lastShiftTruckMissed,
                    ContractorNotes: $scope.contractorNotes,
                    MTCNotes: $scope.mtcNotes
                };

                troubleTicketService.resolveTroubleTicketAdmin(vm).then(function (result) {

                    var res = JSON.parse(result);

                    if (res === true)
                        toastr.success("Resolved", "Trouble Ticket");
                    else
                        toastr.error("Error", "Resolving Trouble Ticket");

                    $scope.isBusySaving = false;

                    $("#resolveMechanicalTroubleTicketModal").modal("hide");

                    $scope.getTroubleTickets();

                });

            };

            $scope.editTroubleTicket = function (ticket) {

                $scope.troubleTicketId = ticket.Id;
                //$scope.ticketType = ticket.TroubleTicketType;
                $scope.selectedTicketStatus = ticket.TroubleTicketStatus;

                $scope.associatedTowContractorId = ticket.AssociatedTowContractorId;
                $scope.associatedInVehicleContractorId = ticket.AssociatedInVehicleContractorId;
                $scope.associatedInVehicleLATATraxContractorId = ticket.AssociatedInVehicleLATATraxContractorId;

                $scope.selectedFleetVehicleId = ticket.VehicleId;

                $scope.getContractorVehicles();

                $scope.selectedTroubleTicketProblems = [];
                for (var i = 0; i < ticket.TroubleTicketProblems.length; i++) {
                    $scope.selectedTroubleTicketProblems.push(ticket.TroubleTicketProblems[i].TroubleTicketProblemId);
                }

                $scope.selectedTroubleTicketComponentIssues = [];
                for (var i = 0; i < ticket.TroubleTicketComponentIssues.length; i++) {
                    $scope.selectedTroubleTicketComponentIssues.push(ticket.TroubleTicketComponentIssues[i].TroubleTicketComponentIssueId);
                }

                $scope.selectedTroubleTicketLATATraxIssues = [];
                for (var i = 0; i < ticket.TroubleTicketLATATraxIssues.length; i++) {
                    $scope.selectedTroubleTicketLATATraxIssues.push(ticket.TroubleTicketLATATraxIssues[i].TroubleTicketLATATraxIssueId);
                }

                $scope.selectedDrivers = [];
                for (var i = 0; i < ticket.TroubleTicketDrivers.length; i++) {
                    $scope.selectedDrivers.push(ticket.TroubleTicketDrivers[i].DriverId);
                }

                if (moment(ticket.ProblemStartedOn).isValid())
                    $scope.dateProblemStarted = moment(ticket.ProblemStartedOn).format("MM/DD/YY");

                if (moment(ticket.DateTruckOutOfService).isValid())
                    $scope.dateTruckOutOfService = moment(ticket.DateTruckOutOfService).format("MM/DD/YY");

                if (moment(ticket.DateTruckBackInService).isValid())
                    $scope.dateTruckBackInService = moment(ticket.DateTruckBackInService).format("MM/DD/YY");

                $scope.contactName = ticket.ContactName;
                $scope.contactPhone = ticket.ContactPhone;

                $scope.shiftsToMiss = ticket.ShiftsMissed;
                $scope.actionTakenByContractor = ticket.ActionTaken;

                $scope.mtcNotes = ticket.MTCNotes;
                $scope.contractorNotes = ticket.ContractorNotes;
                $scope.inVehicleContractorNotes = ticket.InVehicleContractorNotes;
                $scope.lATANotes = ticket.LATANotes;

                $scope.reliaGateOEMSerialNumber = ticket.ReliaGateOEMSerialNumber;

                $scope.createdBy = ticket.CreatedBy;
                $scope.createdOn = moment(ticket.CreatedOn).format("MM/DD/YYYY hh:mm:ss");

                $("#manageTroubleTicketModal").modal('show');

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


                    $scope.associatedTowContractorId = null;
                    $scope.associatedInVehicleContractorId = null;
                    $scope.associatedInVehicleLATATraxContractorId = null;

                    $scope.selectedTicketStatus = 0;
                    $scope.selectedFleetVehicleId = null;
                    $scope.problem = '';
                    $scope.dateProblemStarted = '';
                    $scope.shiftsToMiss = '';
                    $scope.actionTakenByContractor = '';
                    $scope.mtcNotes = '';

                    $("#manageTroubleTicketModal").modal("hide");
                    $scope.getTroubleTickets();
                    $scope.isBusySaving = false;
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