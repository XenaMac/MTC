(function () {
    'use strict';
    mtcApp.controller('reportsController',
        function reportsController($scope, $rootScope, generalService) {

            $scope.header = "Reports";
            $scope.currentUserRoleName = "";

            //Roles
            //Admin, TowContractor, DataConsultant

            generalService.getCurrentUser().then(function (results) {
                if (results) {
                    $scope.currentUserRoleName = results.SelectedRoleName;
                }
            });


        }
    );
}());