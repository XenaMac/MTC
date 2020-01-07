(function () {
    "use strict";
    mtcApp.controller("reportWazeMessagesController",
        function reportWazeMessagesController($scope, $rootScope, reportService, generalService) {

            $scope.header = "WAZE Messages";
            $scope.isBusy = false;
            $scope.isBusyExporting = false;
            $scope.userIsContractor = true;
                        
            $scope.getReportData = function () {
                $scope.isBusy = true;
                reportService.GetWazeMessagesReport().then(function (results) {
                    results.forEach(function(result) {});

                    $scope.records = results;
                    $scope.isBusy = false;

                    $rootScope.redrawTable();
                    $(".footable").trigger("footable_initialize");
                    $(".footable").trigger("footable_redraw");
                    $(".footable").trigger("footable_resize");
                });
            };

            $scope.exportReportData = function () {
                $scope.isBusyExporting = true;

                //requesting excel and not json
                $scope.query.format = "excel";

                reportService.getAssistsLogged($scope.query).then(function (results) {
                    $scope.isBusyExporting = false;
                    var element = angular.element("<a/>");
                    element.attr({
                        href: "data:attachment/csv;charset=utf-8," + encodeURI(results),
                        target: "_blank",
                        download: "AssistsLogged.csv"
                    })[0].click();

                });
            };

        }
    );
}());