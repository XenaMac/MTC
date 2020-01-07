(function () {
    "use strict";
    mtcApp.controller("reportAssistsLoggedController",
        function reportAssistsLoggedController($scope, $rootScope, reportService, generalService) {

            $scope.header = "Assists Logged";
            $scope.isBusy = false;
            $scope.isBusyExporting = false;
            $scope.userIsContractor = true;
            $scope.beatNumbers = [];
            $scope.contractors = [];
            $scope.drivers = [];


            $scope.query = {
                beatNumber: "",
                contractCompanyName: "",
                driverId: "",
                callSign: "",
                format: "json"
            };


            $scope.busyGettingContractors = true;
            $scope.busyGettingDrivers = true;
            $scope.busyGettingBeatNumbers = true;

            generalService.getContractors().then(function (results) {
                $scope.contractors = results;
                $scope.busyGettingContractors = false;

                generalService.getCurrentUser().then(function (results) {
                    if (results) {

                        var contractorId = "";

                        if (results.SelectedRoleName === "TowContractor") {
                            contractorId = results.ContractorId;
                            for (var i = 0; i < $scope.contractors.length; i++) {
                                if ($scope.contractors[i].Id === contractorId)
                                    $scope.query.contractCompanyName = $scope.contractors[i].Text;
                            }
                            $scope.userIsContractor = true;
                        }
                        else
                            $scope.userIsContractor = false;

                        if ($scope.userIsContractor) {
                            generalService.getContractorDrivers(contractorId).then(function (results) {
                                $scope.drivers = results;
                                $scope.busyGettingDrivers = false;
                            });
                            generalService.getContractorBeats(contractorId).then(function (results) {
                                $scope.drivers = results;
                                $scope.busyGettingBeatNumbers = false;
                            });
                        }
                        else {
                            generalService.getDrivers().then(function (results) {
                                $scope.drivers = results;
                                $scope.busyGettingDrivers = false;
                            });
                            generalService.getBeatNumbers().then(function (results) {
                                $scope.beatNumbers = results;
                                $scope.busyGettingBeatNumbers = false;
                            });
                        }

                    }
                });
            });

            $scope.getReportData = function () {
                $scope.isBusy = true;
                reportService.getAssistsLogged($scope.query).then(function (results) {
                    results.forEach(function(result) {
                        result.BeatNumber = parseInt(result.BeatNumber);
                    });

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