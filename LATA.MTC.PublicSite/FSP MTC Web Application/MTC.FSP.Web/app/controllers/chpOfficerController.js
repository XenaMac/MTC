(function () {
    'use strict';
    mtcApp.controller('chpOfficerController',
        function chpOfficerController($scope, $filter, $rootScope, $location, $routeParams, chpOfficerService) {

            $scope.header = 'CHP Officer';
            $scope.chpOfficerId = 0;
            $scope.isBusy = false;

            $scope.selectedBeat;
            $scope.beatNumbers = [];
            $scope.busyGettingBeatNumbers = true;
            $scope.assignedBeats = [];

            $scope.init = function (chpOfficerId) {
                $scope.chpOfficerId = chpOfficerId;

                if ($scope.chpOfficerId != undefined && $scope.chpOfficerId != "") {
                    $scope.header = "Edit";
                    $scope.getCHPOfficer();
                } else {
                    $scope.header = "Add";
                }

            };

            $scope.getAvailableBeatNumbers = function () {
                chpOfficerService.getAvailableBeatNumbers().then(function (results) {
                    $scope.beatNumbers = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.beatNumbers.push(results[i]);
                    }
                    $scope.busyGettingBeatNumbers = false;
                });
            };
            $scope.getAvailableBeatNumbers();

            $scope.getCHPOfficer = function () {

                $scope.isBusy = true;

                chpOfficerService.getCHPOfficer($scope.chpOfficerId).then(function (results) {

                    $scope.chpOfficer = results[0];
                    $scope.assignedBeats = results[0].Beats;
                    $scope.isBusy = false;

                });
            };
            $scope.removeAssignedBeat = function (beat) {

                if (confirm("Are you sure you want to remove beat " + beat.BeatNumber + " from this contract?")) {
                    if ($scope.assignedBeats.indexOf(beat) >= 0)
                        $scope.assignedBeats.splice($scope.assignedBeats.indexOf(beat), 1);
                }
            }
            $scope.addBeat = function () {

                var b = new beat($scope.selectedBeat.Id, $scope.selectedBeat.Text);
                $scope.assignedBeats.push(b);

            };

            $scope.save = function () {

                $scope.isBusy = true;

                var vm = {
                    CHPOfficer: $scope.chpOfficer,
                    Beats: $scope.assignedBeats
                };

                chpOfficerService.save(vm).then(function (result) {

                    $scope.isBusy = false;
                    $scope.chpOfficerId = result;
                    toastr.success("Saved", "CHP Officer");
                    $scope.getCHPOfficer();
                    $scope.getAvailableBeatNumbers();
                });


            };

            function beat(beatId, beatNumber) {
                var self = this;
                self.BeatId = beatId;
                self.BeatNumber = beatNumber;
            }
        }
    );
}());