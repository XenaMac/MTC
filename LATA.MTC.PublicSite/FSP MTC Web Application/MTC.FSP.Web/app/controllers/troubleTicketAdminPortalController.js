'use strict';
(function () {
    mtcApp.controller('troubleTicketAdminPortalController',
        function troubleTicketAdminPortalController($scope, $filter, $rootScope, $location, $routeParams, generalService, troubleTicketService) {

            $scope.header = 'Contract';
            $scope.isAdmin = false;
            $scope.isContractor = false;
            $scope.isInVehicleContractor = false;
            $scope.troubleTicketsSnapshot = [];

            generalService.getCurrentUser().then(function (results) {
                if (results) {
                    if (results.SelectedRoleName === 'Admin') {
                        $scope.isAdmin = true;
                    }
                    else if (results.SelectedRoleName === 'TowContractor') {
                        if (results.ContractorTypeName === 'Tow-Contractor')
                            $scope.isContractor = true;
                        else
                            $scope.isInVehicleContractor = true;
                    }                   
                }
                else {
                    $scope.header = 'Error. We were not able to retrieve your current user context. Please come back later.';
                }
            });

            $scope.getTroubleTicketsSnapshot = function () {

                $scope.isBusyGettingTroubleTicketsSnapshot = true;

                troubleTicketService.getTroubleTicketsSnapshot().then(function (results) {

                    $scope.troubleTicketsSnapshot = [];

                    for (var i = 0; i < results.length; i++) {

                        results[i].Total = results[i].NumberOfMechanical + results[i].NumberOfInVehicleEquipmentGeneral + results[i].NumberOfInVehcileEquipmentLATATrax;

                        $scope.troubleTicketsSnapshot.push(results[i]);
                    }

                    $scope.isBusyGettingTroubleTicketsSnapshot = false;

                });

            };
            $scope.getTroubleTicketsSnapshot();

        }
    );
}());