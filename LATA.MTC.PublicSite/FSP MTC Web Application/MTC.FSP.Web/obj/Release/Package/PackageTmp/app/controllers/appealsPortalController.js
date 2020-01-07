'use strict';
(function () {
    mtcApp.controller('appealsPortalController',
        function appealsPortalController($scope, $filter, $rootScope, $location, $routeParams, generalService) {
            $scope.isBusy = true;
            $scope.isAdmin = false;
            $scope.isContractor = false;

            generalService.getCurrentUser().then(function (results) {
                if (results) {
                    if (results.SelectedRoleName === 'Admin') {
                        $scope.isAdmin = true;
                    }
                    else if (results.SelectedRoleName === 'TowContractor') {
                        $scope.isContractor = true;
                    }
                }
                else {
                    $scope.header = 'Error. We were not able to retrieve your current user context. Please come back later.';
                }
                $scope.isBusy = false;
            });

        }
    );
}());