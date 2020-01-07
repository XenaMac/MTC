(function () {
    'use strict';
    mtcApp.controller('trainingMediaController',
        function trainingMediaController($scope, $rootScope, generalService) {

            $scope.header = "Training Media";
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