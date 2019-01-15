'use strict';
(function () {
    mtcApp.controller('troubleTicketPortalController',
        function troubleTicketPortalController($scope, $filter, $rootScope, $location, $routeParams, generalService) {

            $scope.isAdmin = false;
            $scope.isContractor = false;
            $scope.isInVehicleContractor = false;

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
                    else if (results.SelectedRoleName === 'InVehicleContractor') {
                        $scope.isInVehicleContractor = true;
                    }
                    else if (results.SelectedRoleName === 'DataConsultant') {
                        $scope.isAdmin = true;
                        $scope.isContractor = true;
                        $scope.isInVehicleContractor = true;
                    }
                }
                else {
                    $scope.header = 'Error. We were not able to retrieve your current user context. Please come back later.';
                }
            });

        }
    );
}());