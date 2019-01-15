(function() {
    'use strict';
    mtcApp.controller('liveIncidentsController',
        function liveIncidentsController($scope, $filter, $interval, $rootScope, $location, $routeParams, liveIncidentsService) {

            $scope.isBusy = false;
            $scope.incidents = [];
            $scope.header = 'Live Incidents ' + moment().format('MMMM DD, YYYY');

            $scope.columns = [];
            $scope.columns.push(new $rootScope.mtcColumn("Incident Type", "incidentType", true, true));
            $scope.columns.push(new $rootScope.mtcColumn("Beat #", "beatNumber", true, true));
            $scope.columns.push(new $rootScope.mtcColumn("Call Sign", "callSign", true, true));
            $scope.columns.push(new $rootScope.mtcColumn("Truck #", "truckNumber", true, true));
            $scope.columns.push(new $rootScope.mtcColumn("Driver", "driverName", true, true));
            $scope.columns.push(new $rootScope.mtcColumn("Dispatch Summary Message", "dispatchComments", true, true));
            $scope.columns.push(new $rootScope.mtcColumn("Dispatch Time", "timeStamp", true, true));
            $scope.columns.push(new $rootScope.mtcColumn("Status", "state", true, true))
            $scope.columns.push(new $rootScope.mtcColumn("Location", "Location", true, true))
            $scope.columns.push(new $rootScope.mtcColumn("User Posted", "UserPosted", true, true))
            $scope.columns.push(new $rootScope.mtcColumn("Completed", "isIncidentComplete", false, false))

            var orderBy = $filter('orderBy');
            $scope.predicate = '-beatNumber';
            $scope.order = function(predicate, reverse) {
                $scope.incidents = orderBy($scope.incidents, predicate, reverse);
            };

            var getLiveIncidentsRefreshRate = function() {
                $scope.isBusy = true;
                liveIncidentsService.getLiveIncidentsRefreshRate().then(function(results) {
                    console.log("Incident page refresh rate %O", results);
                    getIncidents();
                    $interval(function() {
                        getIncidents();
                    }, eval(results));
                });
            };

            var getIncidents = function() {
                $scope.isBusy = true;
                console.time("Getting Incidents");
                liveIncidentsService.getLiveIncidents().then(function(results) {
                    console.timeEnd("Getting Incidents");
                    $scope.isBusy = false;
                    for (var i = 0; i < results.length; i++) {
                        var exists = false;
                        for (var ii = 0; ii < $scope.incidents.length; ii++) {
                            if ($scope.incidents[ii].incidentID === results[i].IncidentID) {
                                exists = true;
                                $scope.incidents[ii].incidentNumber = results[i].IncidentNumber;
                                $scope.incidents[ii].beatNumber = results[i].BeatNumber;
                                $scope.incidents[ii].callSign = results[i].CallSign;
                                $scope.incidents[ii].truckNumber = results[i].TruckNumber;
                                $scope.incidents[ii].driverName = results[i].DriverName;
                                $scope.incidents[ii].dispatchComments = results[i].DispatchComments;
                                $scope.incidents[ii].timeStamp = results[i].Timestamp;
                                $scope.incidents[ii].state = results[i].State;
                                $scope.incidents[ii].dispatchNumber = results[i].DispatchNumber;
                                $scope.incidents[ii].isIncidentComplete = results[i].IsIncidentComplete;
                                $scope.incidents[ii].isAcked = results[i].IsAcked;
                                $scope.incidents[ii].incidentType = results[i].IncidentType;
                                $scope.incidents[ii].location = results[i].Location;
                                $scope.incidents[ii].userPosted = results[i].UserPosted;
                            }
                        }
                        if (!exists)
                            $scope.incidents.push(new incident(results[i]));
                    }
                });
            };

            getLiveIncidentsRefreshRate();

            function incident(dbIncident) {
                var self = this;
                self.incidentID = dbIncident.IncidentID;
                self.incidentNumber = dbIncident.IncidentNumber;
                self.beatNumber = dbIncident.BeatNumber;
                self.callSign = dbIncident.CallSign;
                self.truckNumber = dbIncident.TruckNumber;
                self.driverName = dbIncident.DriverName;
                self.dispatchComments = dbIncident.DispatchComments;
                self.timeStamp = dbIncident.Timestamp;
                self.state = dbIncident.State;
                self.dispatchNumber = dbIncident.DispatchNumber;
                self.isIncidentComplete = dbIncident.IsIncidentComplete;
                self.isAcked = dbIncident.IsAcked;
                self.incidentType = dbIncident.IncidentType;
                self.location = dbIncident.Location;
                self.UserPosted = dbIncident.UserPosted;
            }
        }
    );
}());