(function() {
    'use strict';
    mtcApp.controller('alertsController', function alertsController($scope, $filter, $interval, $rootScope, $location, $routeParams, alertsService) {
        $scope.header = moment().format('MMMM DD, YYYY') + ' Current Alarms';
        $scope.alerts = [];
        $scope.selectedAlert;
        $scope.selectedAlertType;
        $scope.excuseComments = '';
        $scope.defaultDate = '01/01/2001';
        var getSpecialDisplay = function(input) {
            try {
                if (input.indexOf("|") >= 0) {
                    var alarmTime = '';
                    var scheduledTime = '';
                    if (moment(input.split("|")[0]).isValid()) alarmTime = moment(input.split("|")[0]);
                    if (moment(input.split("|")[1]).isValid()) scheduledTime = moment(input.split("|")[1]);
                    var special = '';
                    var diff = alarmTime.diff(scheduledTime, 'minutes');
                    //var duration = moment.duration(diff);
                    //var diffenceInHours = duration.asHours();
                    //var difference = duration.asMinutes();
                    var difference = diff;
                    if (difference > 0) {
                        special = 'Late : ' + Math.ceil(difference) + ' mins';
                    } else {
                        special = 'Early : ' + Math.ceil(difference) + ' mins';
                    }
                    return {
                        Special: special,
                        AlarmTime: alarmTime.format('hh:mm:ss')
                    }
                }
            } catch (e) {}
        }
        var getAlertsRefreshRate = function() {
            alertsService.getAlertsRefreshRate().then(function(results) {
                getAlerts();
                $interval(function() {
                    console.log('getting new alerts');
                    getAlerts();
                }, eval(results));
            });
        };
        var getAlerts = function() {
            $scope.isBusyGettingAlerts = true;
            alertsService.getAlerts().then(function(results) {
                $scope.alerts = [];
                for (var i = 0; i < results.length; i++) {
                    var result = results[i];
                    if (result.OnPatrolAlarm === true) {
                        if (result.OnPatrolAlarmComments) {
                            var data = getSpecialDisplay(result.OnPatrolAlarmComments);
                            if (data != undefined) {
                                result.OnPatrolAlarmTime = data.AlarmTime;
                                result.OnPatrolSpecial = data.Special;
                            }
                        }
                    }
                    if (result.RollInAlarm === true) {
                        if (result.RollInAlarmComments) {
                            var data = getSpecialDisplay(result.RollInAlarmComments);
                            if (data != undefined) {
                                result.RollInAlarmTime = data.AlarmTime;
                                result.RollInAlarmSpecial = data.Special;
                            }
                        }
                    }
                    $scope.alerts.push(result);
                }
                $scope.isBusyGettingAlerts = false;
            });
        };
        $scope.clearAlert = function(alertObject, alertType) {
            console.log(alertObject);
            console.log(alertType);
            var alertId;
            if (alertType === 'LOGON') alertId = alertObject.OutOfBoundsAlarmID;
            else if (alertType === 'LATEONPATROL') alertId = alertObject.OnPatrolAlarmID;
            else if (alertType === 'EARLYOUTOFSERVICE') alertId = alertObject.RollInAlarmID;
            else if (alertType === 'OFFBEAT') alertId = alertObject.OutOfBoundsAlarmID;
            else if (alertType === 'LONGINCIDENT') alertId = alertObject.IncidentAlarmID;
            else if (alertType === 'STATIONARY') alertId = alertObject.StationaryAlarmID;
            else if (alertType === 'GPSISSUE') alertId = alertObject.GPSIssueAlarmID;
            else if (alertType === 'LONGBREAK') alertId = alertObject.LongBreakAlarmID;
            else if (alertType === 'OVERTIMEACTIVITY') alertId = alertObject.OvertimeAlarmID;
            var vm = {
                vehicleNumber: alertObject.VehicleNumber,
                alertType: alertType,
                alertId: alertId
            };
            console.log(vm);
            alertsService.clearAlert(vm).then(function() {
                toastr.success("Alert Cleared");
                getAlerts();
            });
        };
        $scope.excuseAlert = function(alertObject, alertType) {
            $scope.selectedAlert = alertObject;
            $scope.selectedAlertType = alertType;
            $scope.selectedAlertId = null;
            $("#alertExcuseConfirmationModal").modal('show');
        };
        $scope.cancelExcuse = function() {
            $("#alertExcuseConfirmationModal").modal('hide');
            $scope.selectedAlert = null;
            $scope.selectedAlertType = null;
            $scope.excuseComments = '';
        };
        $scope.submitExcuse = function() {
            $("#alertExcuseConfirmationModal").modal('hide');
            var alertId;
            var alertType = $scope.selectedAlertType;
            var alertObject = $scope.selectedAlert;
            if (alertType === 'LOGON') alertId = alertObject.OutOfBoundsAlarmID;
            else if (alertType === 'LATEONPATROL') alertId = alertObject.OnPatrolAlarmID;
            else if (alertType === 'EARLYOUTOFSERVICE') alertId = alertObject.RollInAlarmID;
            else if (alertType === 'OFFBEAT') alertId = alertObject.OutOfBoundsAlarmID;
            else if (alertType === 'LONGINCIDENT') alertId = alertObject.IncidentAlarmID;
            else if (alertType === 'STATIONARY') alertId = alertObject.StationaryAlarmID;
            else if (alertType === 'GPSISSUE') alertId = alertObject.GPSIssueAlarmID;
            else if (alertType === 'LONGBREAK') alertId = alertObject.LongBreakAlarmID;
            else if (alertType === 'OVERTIMEACTIVITY') alertId = alertObject.OvertimeAlarmID;
            var vm = {
                vehicleNumber: $scope.selectedAlert.VehicleNumber,
                alertType: $scope.selectedAlertType,
                comments: $scope.excuseComments,
                alertId: alertId
            };
            console.log(vm);
            alertsService.excuseAlert(vm).then(function() {
                toastr.success("Alert Excused");
                getAlerts();
            });
        };
        getAlertsRefreshRate();
    });
}());
