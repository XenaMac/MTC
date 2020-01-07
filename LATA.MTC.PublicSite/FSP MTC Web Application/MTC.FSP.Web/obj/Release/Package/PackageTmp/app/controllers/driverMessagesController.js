"use strict";
(function () {
    mtcApp.controller("driverMessagesController",
        function driverMessagesController($scope, $filter, $interval, $rootScope, $location, $routeParams, driverMessagesService) {

            $scope.header = "Messages for " + moment().format("MMMM DD, YYYY");
            $scope.message = "";
            $scope.selectedTrucks = [];
            $scope.isBusy = false;
            $scope.requireAck = false;
            $scope.messages = [];


            $scope.sendMessage = function () {

                var vm = {
                    message: $scope.message,
                    trucks: $scope.selectedTrucks,
                    requireAck: $scope.requireAck === true ? 1 : 0
                };

                $scope.isBusy = true;

                driverMessagesService.sendDriverMessage(vm).then(function (results) {
                    toastr.success("Successfully sent message", "Driver Messaging");
                    $scope.message = "";
                    $scope.selectedTrucks = [];
                    $scope.isBusy = false;
                });

            };

            $scope.$on("selectedTruck", function (event, object) {
                $scope.selectedTrucks.push(object.selectedTruck);
            });

            $scope.remove = function (truck) {
                var idx = $scope.selectedTrucks.indexOf(truck);
                $scope.selectedTrucks.splice(idx, 1);
            };

            $scope.getMessages = function () {

                driverMessagesService.getMessages().then(function (results) {
                    $scope.messages = [];
                    for (var i = 0; i < results.length; i++) {

                        if (moment(results[i].SentTime).isValid())
                            results[i].SentTime = moment(results[i].SentTime).format("MM/DD/YY HH:mm A");

                        if (moment(results[i].AckedTime).isValid())
                            results[i].AckedTime = moment(results[i].AckedTime).format("MM/DD/YY HH:mm A");

                        results[i].BeatNumber = parseInt(results[i].Beat);

                        $scope.messages.push(results[i]);
                    }
                });
            };
            $scope.getMessages();

            $interval(function () {
                console.log("getting new messages");
                $scope.getMessages();
            }, 10000);
        }
    );
}());