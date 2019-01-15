(function () {
    'use strict';
    mtcApp.controller('mtcInstructionsController',
        function mtcInstructionsController($scope, $rootScope,backupTruckService) {
            
            $scope.urgentPriorityInHours = '';
            $scope.standardPriorityInHours = '';
            
            backupTruckService.getUrgentBackupValue().then(function (result) {

                var minutes = result;
                if (minutes < 60) {
                    $scope.urgentPriorityInHours = minutes + ' minutes';
                }
                else {
                    var hours = eval(minutes / 60);
                    $scope.urgentPriorityInHours = hours + ' hours';
                }



            });
            backupTruckService.getStandardBackupValue().then(function (result) {

                var minutes = result;
                if (minutes < 60) {
                    $scope.standardPriorityInHours = minutes + ' minutes';
                }
                else {
                    var hours = eval(minutes / 60);
                    $scope.standardPriorityInHours = hours + ' hours';
                }

            });
           
        }
    );
}());