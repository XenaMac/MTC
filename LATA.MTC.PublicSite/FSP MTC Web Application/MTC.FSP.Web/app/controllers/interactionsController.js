'use strict';
(function () {
    mtcApp.controller('interactionsController',
        function interactionsController($scope, $filter,$interval, $rootScope, $location, $routeParams, interactionsService) {

            $scope.header = 'Driver Interactions';
            $scope.interactions = [];
            $scope.columns = [];
            $scope.columns.push(new $rootScope.mtcColumn("Driver", "Driver", true, true));
            $scope.columns.push(new $rootScope.mtcColumn("Contractor", "Contractor", true, true));
            $scope.columns.push(new $rootScope.mtcColumn("Contact Type", "InteractionType", true, true));
            $scope.columns.push(new $rootScope.mtcColumn("Pass/Fail", "InspectionPassFail", true, true));
            $scope.columns.push(new $rootScope.mtcColumn("Contact Location", "InteractionArea", true, true));

            $scope.columns.push(new $rootScope.mtcColumn("Description", "InteractionDescription", false, true));
            $scope.columns.push(new $rootScope.mtcColumn("Accident Preventable", "AccidentPreventable", false, true));
            $scope.columns.push(new $rootScope.mtcColumn("Followup Required", "FollowupRequired", false, true));
            $scope.columns.push(new $rootScope.mtcColumn("Followup Description", "FollowupDescription", false, true));
            $scope.columns.push(new $rootScope.mtcColumn("Followup Date", "FollowupDate", false, true));
            $scope.columns.push(new $rootScope.mtcColumn("Followup Completion Date", "FollowupCompletionDate", false, true));
            $scope.columns.push(new $rootScope.mtcColumn("Followup Comments", "FollowupComments", false, true));
            $scope.columns.push(new $rootScope.mtcColumn("Close Date", "CloseDate", false, true));
            $scope.columns.push(new $rootScope.mtcColumn("Badge ID", "BadgeID", false, true));
            $scope.columns.push(new $rootScope.mtcColumn("Contact Date", "InteractionDate", false, true));
            $scope.columns.push(new $rootScope.mtcColumn("Vehicle Number", "VehicleNumber", false, true));
            $scope.columns.push(new $rootScope.mtcColumn("Beat Number", "BeatNumber", false, true));

            var orderBy = $filter('orderBy');
            $scope.order = function (predicate, reverse) {
                $scope.interactions = orderBy($scope.interactions, predicate, reverse);
            };

            var getInteractions = function () {

                $scope.isBusyGettingInteractions = true;

                interactionsService.getInteractions().then(function (results) {
                    for (var i = 0; i < results.length; i++) {
                        $scope.interactions.push(new interaction(results[i]));
                    }

                    $scope.isBusyGettingInteractions = false;
                    $scope.redrawTable();
                });
            };

            getInteractions();

            function interaction(dbInteraction) {

                var self = this;

                self.InteractionID = dbInteraction.InteractionID;
                self.Contractor = dbInteraction.Contractor;
                self.Driver = dbInteraction.Driver;
                self.InteractionType = dbInteraction.InteractionType;
                self.InteractionArea = dbInteraction.InteractionArea;
                self.InteractionDescription = dbInteraction.InteractionDescription;
                self.InspectionPassFail = dbInteraction.InspectionPassFail;
                self.AccidentPreventable = dbInteraction.AccidentPreventable;
                self.FollowupRequired = dbInteraction.FollowupRequired;
                self.FollowupDescription = dbInteraction.FollowupDescription;

                self.FollowupComments = dbInteraction.FollowupComments;
                self.BadgeID = dbInteraction.BadgeID;
                self.VehicleNumber = dbInteraction.VehicleNumber;
                self.BeatNumber = dbInteraction.BeatNumber;

                if (dbInteraction.FollowupCompletionDate != null)
                    self.FollowupCompletionDate = moment(dbInteraction.FollowupCompletionDate).format('MM/DD/YY');

                if (dbInteraction.FollowupDate != null)
                    self.FollowupDate = moment(dbInteraction.FollowupDate).format('MM/DD/YY');

                if (dbInteraction.InteractionDate != null)
                    self.InteractionDate = moment(dbInteraction.InteractionDate).format('MM/DD/YY');

                if (dbInteraction.CloseDate != null)
                    self.CloseDate = moment(dbInteraction.CloseDate).format('MM/DD/YY');
            };

            $scope.redrawTable = function () {

                var redrawInterval = $interval(function () {
                    $interval.cancel(redrawInterval);
                    $('.footable').trigger('footable_redraw');
                    console.log('redrawing table');
                }, 100);

            };
        }
    );
}());