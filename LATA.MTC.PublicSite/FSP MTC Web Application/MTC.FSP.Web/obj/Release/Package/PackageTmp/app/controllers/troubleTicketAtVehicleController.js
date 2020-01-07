'use strict';
(function () {
    mtcApp.controller('troubleTicketAtVehicleController',
        function troubleTicketAtVehicleController($scope, $filter, $interval, $rootScope, $location, $routeParams, generalService, troubleTicketService) {

            $scope.isBusyGettingTroubleTickets = false;
            $scope.associatedTowContractorId;
            $scope.towContractors = [];
            $scope.contractorType = 'General';

            $scope.selectedTroubleTicket;
            $scope.troubleTickets = [];

            $scope.isBusySaving = false;

            generalService.getContractors('Tow-Contractor').then(function (results) {
                $scope.isBusyGettingTowContractors = true;
                $scope.towContractors = [];
                for (var i = 0; i < results.length; i++) {
                    $scope.towContractors.push(results[i]);
                }
                $scope.isBusyGettingTowContractors = false;
            });

            $scope.canEdit = false;
            generalService.getCanEdit('TroubleTickets').then(function (result) {
                $scope.canEdit = result;
            });


            $scope.getTowContractorsTickets = function () {

                $scope.isBusyGettingTroubleTickets = true;

                troubleTicketService.getTowContractorsTroubleTickets($scope.associatedTowContractorId, $scope.contractorType).then(function (results) {

                    $scope.troubleTickets = [];

                    for (var i = 0; i < results.length; i++) {

                        if (moment(results[i].ReplacmentDate).isValid())
                            results[i].ReplacmentDateNice = moment(results[i].ReplacmentDate).format("MM/DD/YY");

                        $scope.troubleTickets.push(results[i]);
                    }

                    $scope.isBusyGettingTroubleTickets = false;
                    $scope.redrawTable();

                });


            };

            $scope.showInputForm = function (t) {
                $scope.selectedTroubleTicket = t;
                $("#atVehicleInputFormModal").modal('show');
            };

            $scope.saveForm = function () {

                $scope.isBusySaving = true;

                var vm = {
                    Id: $scope.selectedTroubleTicket.Id,
                    ReplacmentIsFixed: $scope.selectedTroubleTicket.ReplacmentIsFixed,
                    ReplacmentDate: $scope.selectedTroubleTicket.ReplacmentDateNice,
                    ReplacementOEMSerialNumber: $scope.selectedTroubleTicket.ReplacementOEMSerialNumber,
                    ReplacementIPAddress: $scope.selectedTroubleTicket.ReplacementIPAddress,
                    ReplacementWiFiSSID: $scope.selectedTroubleTicket.ReplacementWiFiSSID,
                    InVehicleContractorNotes: $scope.selectedTroubleTicket.InVehicleContractorNotes
                };

                troubleTicketService.saveTroubleTicketReplacementData(vm).then(function (result) {

                    var res = JSON.parse(result);

                    if (res === true)
                        toastr.success("Saved", "Trouble Ticket Replacement Data");
                    else
                        toastr.error("Error", "Saving Trouble Ticket Replacement Data");

                    $scope.selectedTroubleTicket = null;
                    $scope.isBusySaving = false;

                    $("#atVehicleInputFormModal").modal("hide");
                  
                    $scope.getTowContractorsTickets();

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