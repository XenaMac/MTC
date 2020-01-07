(function () {
    'use strict';
    mtcApp.controller('beatCallSignController',
        function beatCallSignController($scope, $rootScope, beatsService) {

            $scope.beatId;
            $scope.beat = {
                BeatNumber: '',
                OnCallAreas: '',
                Freq: '',
                CHPArea: '',
                CallSigns: ''
            };
            $scope.newCallSign = '';


            $scope.callSigns = [];

            $scope.init = function (beatId) {
                $scope.beatId = beatId;

                if ($scope.beatId != undefined && $scope.beatId != "") {
                    $scope.header = "Edit";
                    $scope.getBeatCallSign();
                } else {
                    $scope.header = "Add";
                }

            };

            $scope.getBeatCallSign = function () {
                beatsService.getBeatCallSign($scope.beatId).then(function (results) {
                    $scope.beat = results[0];
                });

            };

            $scope.removeAssignedCallSign = function (c) {

                if (confirm("Are you sure you want to remove call sign " + c.CallSign + " from this beat?")) {
                    if ($scope.beat.CallSigns.indexOf(c) >= 0)
                        $scope.beat.CallSigns.splice($scope.beat.CallSigns.indexOf(c), 1);
                }
            }

            $scope.addCallSign = function () {

                var callSign = {
                    CallSign: $scope.newCallSign
                };

                $scope.beat.CallSigns.push(callSign);

                $scope.newCallSign = '';

            };

            $scope.save = function () {

                $scope.isBusy = true;
               
                beatsService.saveBeatCallSign($scope.beat).then(function (result) {

                    $scope.isBusy = false;
                    $scope.beatId = result;
                    toastr.success("Saved", "Beat & Call Signs");
                    $scope.getBeatCallSign();                    
                });


            };
        }
    );
}());