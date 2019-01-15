'use strict';
(function () {
    mtcApp.controller('dispatchingController',
        function dispatchingController($scope, $filter, $rootScope, $location, $routeParams, dispatchingService) {

            $scope.header = 'Dispatching';
            $scope.message;
            $scope.selectedTrucks = [];
            $scope.direction = '';
            $scope.freeway = '';
            $scope.location = '';
            $scope.crossStreet1 = '';
            $scope.crossStreet2 = '';
            $scope.laneNumber = '';
            $scope.comments = '';
            $scope.dispatchCodes = [];
            $scope.selectedDispatchCodeId;
            $scope.isBusy = false;

            $scope.doDispatch = function () {

                $scope.isBusy = true;

                var vm = {
                    Direction: $scope.direction,
                    Freeway: $scope.freeway,
                    Location: $scope.location,
                    CrossStreet1: $scope.crossStreet1,
                    CrossStreet2: $scope.crossStreet2,
                    Comments: $scope.comments,
                    LaneNumber: $scope.laneNumber,
                    DispatchCodeId: $scope.selectedDispatchCodeId,
                    trucks: $scope.selectedTrucks
                };

                dispatchingService.doDispatch(vm).then(function (results) {

                    if (results.OperationSuccess === true)
                        toastr.success('Successfully dispatched trucks', $scope.header);
                    else
                        toastr.error('Failed to dispatch trucks ' + results.Message, $scope.header);

                    $scope.isBusy = false;

                    $scope.message = '';
                    $scope.selectedTrucks = [];
                    $scope.direction = '';
                    $scope.freeway = '';
                    $scope.location = '';
                    $scope.crossStreet1 = '';
                    $scope.crossStreet2 = '';
                    $scope.comments = '';
                    $scope.laneNumber = '';
                    $scope.selectedDispatchCodeId = null;

                });

            };

            $scope.getDispatchCodes = function () {

                dispatchingService.getDispatchCodes().then(function (results) {
                    $scope.dispatchCodes = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.dispatchCodes.push(results[i]);
                    }

                });
            };
            $scope.getDispatchCodes();

            $scope.$on('selectedTruck', function (event, object) {

                var isNotDuplicate = true;

                for (var i = 0; i < $scope.selectedTrucks.length; i++) {
                    if ($scope.selectedTrucks[i].truckNumber === object.selectedTruck.truckNumber)
                        isNotDuplicate = false;
                }

                if (object.selectedTruck.vehicleState.toLowerCase() === 'on patrol')
                {
                    if(isNotDuplicate)
                        $scope.selectedTrucks.push(object.selectedTruck);
                }
                else{
                    toastr.warning('Truck must be available for dispatch', 'Unavailable');
                }

                
                    
            });

            $scope.remove = function (truck) {
                var idx = $scope.selectedTrucks.indexOf(truck);
                $scope.selectedTrucks.splice(idx, 1);
            }
        }
    );
}());