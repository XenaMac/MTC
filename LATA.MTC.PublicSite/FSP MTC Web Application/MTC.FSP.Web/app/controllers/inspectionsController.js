'use strict';
(function () {
    mtcApp.controller('inspectionsController',
        function inspectionsController($scope, $filter, $rootScope, $location, $routeParams, inspectionsService) {

            $scope.header = 'CHP Inspections';
            $scope.inspections = [];
                    
            var getInpections = function () {

                $scope.isBusyGettingInspections = true;

                inspectionsService.getInspections().then(function (results) {
                    for (var i = 0; i < results.length; i++) {
                        results[i].InspectionDateFriendly = moment(results[i].InsepectionDate).format('MM/DD/YY');
                        $scope.inspections.push(results[i]);
                    }
                    $scope.isBusyGettingInspections = false;
                    $rootScope.redrawTable();
                });
            };
            getInpections();
         

            
        }
    );
}());