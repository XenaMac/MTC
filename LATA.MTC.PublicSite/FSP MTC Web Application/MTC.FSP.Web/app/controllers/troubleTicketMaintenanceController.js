'use strict';
(function () {
    mtcApp.controller('troubleTicketMaintenanceController',
        function troubleTicketMaintenanceController($scope, $filter, $interval, $rootScope, $location, $routeParams, generalService, troubleTicketService) {

            $scope.isBusyGettingTroubleTickets = false;
            $scope.associatedTowContractorId;
            $scope.towContractors = [];
            $scope.contractorType = 'General';

            $scope.selectedTroubleTicket;
            $scope.troubleTickets = [];

            $scope.isBusySaving = false;
           
            $scope.getTroubleTickets = function () {

                $scope.isBusyGettingTroubleTickets = true;

                troubleTicketService.getUnResolvedTroubleTickets().then(function (results) {

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



                        if (moment(results[i].TroubleShootingDate).isValid())
                            results[i].TroubleShootingDateNice = moment(results[i].TroubleShootingDate).format("MM/DD/YY");

                        if (moment(results[i].RemovedAssetDate).isValid())
                            results[i].RemovedAssetDateNice = moment(results[i].RemovedAssetDate).format("MM/DD/YY");

                        if (moment(results[i].ShippedAssetDate).isValid())
                            results[i].ShippedAssetDateNice = moment(results[i].ShippedAssetDate).format("MM/DD/YY");

                        if (moment(results[i].ReceivedAssetDate).isValid())
                            results[i].ReceivedAssetDateNice = moment(results[i].ReceivedAssetDate).format("MM/DD/YY");

                        if (moment(results[i].InstalledAssetDate).isValid())
                            results[i].InstalledAssetDateNice = moment(results[i].InstalledAssetDate).format("MM/DD/YY");

                        if (moment(results[i].FixedDate).isValid())
                            results[i].FixedDateNice = moment(results[i].FixedDate).format("MM/DD/YY");



                        $scope.troubleTickets.push(results[i]);
                    }

                    $scope.isBusyGettingTroubleTickets = false;
                    $scope.redrawTable();

                });

            };
            $scope.getTroubleTickets();

            $scope.canEdit = false;
            generalService.getCanEdit('TroubleTickets').then(function (result) {
                $scope.canEdit = result;
            });


            $scope.showInputForm = function (t) {
                $scope.selectedTroubleTicket = t;
                $("#maintenanceInputFormModal").modal('show');
            };
            

            $scope.saveForm = function () {

                $scope.isBusySaving = true;

                var vm = {
                    Id: $scope.selectedTroubleTicket.Id,
                    TroubleShootingDate: $scope.selectedTroubleTicket.TroubleShootingDateNice,
                    RemovedAssetDate: $scope.selectedTroubleTicket.RemovedAssetDateNice,
                    ShippedAssetDate: $scope.selectedTroubleTicket.ShippedAssetDateNice,
                    ReceivedAssetDate: $scope.selectedTroubleTicket.ReceivedAssetDateNice,
                    InstalledAssetDate: $scope.selectedTroubleTicket.InstalledAssetDateNice,
                    FixedDate: $scope.selectedTroubleTicket.FixedDateNice,
                    LATARMANumber: $scope.selectedTroubleTicket.LATARMANumber,
                    MaintenanceNotes: $scope.selectedTroubleTicket.MaintenanceNotes
                };

                troubleTicketService.saveTroubleTicketMaintenanceData(vm).then(function (result) {

                    var res = JSON.parse(result);

                    if (res === true)
                        toastr.success("Saved", "Trouble Ticket Maintenance Data");
                    else
                        toastr.error("Error", "Saving Trouble Ticket Maintenance Data");

                    $scope.selectedTroubleTicket = null;
                    $scope.isBusySaving = false;

                    $("#maintenanceInputFormModal").modal("hide");
                  
                    $scope.getTroubleTickets();

                });
            };

            $scope.resolveTroubleTicket = function (t) {

                if (confirm('Are you sure to close this trouble ticket?')) {
                    $scope.isBusySaving = true;

                    troubleTicketService.resolveTroubleTicket(t.Id).then(function (result) {

                        var res = JSON.parse(result);

                        if (res === true)
                            toastr.success("Saved", "Trouble Ticket Closed");
                        else
                            toastr.error("Error", "Closing Trouble Ticket");

                        $scope.selectedTroubleTicket = null;
                        $scope.isBusySaving = false;

                        $scope.getTroubleTickets();

                    });
                }
                
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