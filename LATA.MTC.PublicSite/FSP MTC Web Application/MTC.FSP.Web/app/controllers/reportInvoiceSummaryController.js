(function () {
    'use strict';
    mtcApp.controller('reportInvoiceSummaryController',
        function reportInvoiceSummaryController($scope, $rootScope, reportService) {

            $scope.header = "Monthly Invoice Summary";
            $scope.isBusy = false;
            $scope.isBusyExporting = false;
            $scope.summaries = [];
            $scope.month;
            $scope.export = false;

            $scope.getInvoiceSummary = function () {
                $scope.isBusy = true;
                reportService.getInvoiceSummaryReport($scope.month, $scope.export).then(function (results) {
                    $scope.summaries = [];
                    if (results) {
                        $scope.summaries = results;
                        $rootScope.redrawTable();
                    }
                    $scope.isBusy = false;
                });
            };
            var d = new Date();
            $scope.month = d.getMonth() + 1;
             $scope.getInvoiceSummary();

            $scope.exportReportData = function () {
                $scope.isBusy = true;
                $scope.isBusyExporting = true;

                //requesting excel and not json
                $scope.export = true;

                reportService.getInvoiceSummaryReport($scope.month, $scope.export).then(function (results) {
                    $scope.isBusyExporting = false;
                    $scope.isBusy = false;
                    $scope.export = false;
                    var element = angular.element('<a/>');
                    element.attr({
                        href: 'data:attachment/csv;charset=utf-8,' + encodeURI(results),
                        target: '_blank',
                        download: 'InvoiceSummary_' + $scope.month + '.csv'
                    })[0].click();

                });
            };

        }
    );
}());