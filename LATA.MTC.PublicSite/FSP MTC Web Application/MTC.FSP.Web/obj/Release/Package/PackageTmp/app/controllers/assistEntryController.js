(function () {
    'use strict';
    mtcApp.controller('assistEntryController',
        function assistEntryController($scope, $filter, $rootScope, $location, $routeParams, generalService, assistEntryService, dispatchingService) {

            $scope.header = "Assist Entry";
            $scope.assist = null;

            //common            
            $scope.freeways = [];
            $scope.busyGettingFreeways = true;
            generalService.getFreeways().then(function (results) {
                for (var i = 0; i < results.length; i++) {
                    $scope.freeways.push(results[i]);
                }
                $scope.busyGettingFreeways = false;
            });

            $scope.beats = [];
            $scope.isBusyGettingBeats = true;
            generalService.getBeatNumbers().then(function (results) {
                $scope.beats = [];
                for (var i = 0; i < results.length; i++) {
                    $scope.beats.push(results[i]);
                }
                $scope.isBusyGettingBeats = false;
            });

            $scope.dispatchCodes = [];
            $scope.isBusyGettingDispatchCodes = true;
            dispatchingService.getDispatchCodes().then(function (results) {
                $scope.dispatchCodes = [];
                for (var i = 0; i < results.length; i++) {
                    $scope.dispatchCodes.push(results[i]);
                }
                $scope.isBusyGettingDispatchCodes = false;
            });
           
        }
    );
}());