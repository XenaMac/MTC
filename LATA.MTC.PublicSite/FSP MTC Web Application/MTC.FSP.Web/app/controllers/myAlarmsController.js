(function () {
    'use strict';
    mtcApp.controller('myAlarmsController',
        function myAlarmsController($scope, $rootScope, alarmSubscriptionService) {

            $scope.alarms = [];
            $scope.alarms.push(new alarm('LONGBREAK', false));
            $scope.alarms.push(new alarm('LONGLUNCH', false));
            $scope.alarms.push(new alarm('STATIONARY', false));
            $scope.alarms.push(new alarm('OFFBEAT', false));
            $scope.alarms.push(new alarm('LATEONPATROL', false));
            $scope.alarms.push(new alarm('EARLYOUTOFSERVICE', false));
            $scope.alarms.push(new alarm('SPEEDING', false));
            $scope.alarms.push(new alarm('GPSISSUE', false));
            $scope.alarms.push(new alarm('LONGINCIDENT', false));
            $scope.alarms.push(new alarm('OVERTIMEACTIVITY', false));

            $scope.isBusy = false;

            $scope.getAlarms = function () {

                alarmSubscriptionService.getMyAlarms().then(function (result) {

                    for (var i = 0; i < result.length; i++) {
                        for (var ii = 0; ii < $scope.alarms.length; ii++) {
                            if ($scope.alarms[ii].alarmName === result[i])
                                $scope.alarms[ii].isSelected = true;
                        }
                    }

                    
                });

            };
            $scope.getAlarms();

            $scope.save = function () {
                $scope.isBusy = true;

                var selectedAlarms = [];
                for (var i = 0; i < $scope.alarms.length; i++) {
                    if ($scope.alarms[i].isSelected === true)
                        selectedAlarms.push($scope.alarms[i].alarmName);
                }

                alarmSubscriptionService.save(selectedAlarms).then(function (result) {

                    toastr.success("Saved", "Success");
                    $scope.isBusy = false;
                    selectedAlarms = [];
                    for (var i = 0; i < $scope.alarms.length; i++) {
                        $scope.alarms[i].isSelected = false;
                    }

                    $scope.getAlarms();
                    
                });

            };


            function alarm(alarmName, isSelected) {

                var self = this;
                self.alarmName = alarmName;
                self.isSelected = isSelected;

            };

        }
    );
}());