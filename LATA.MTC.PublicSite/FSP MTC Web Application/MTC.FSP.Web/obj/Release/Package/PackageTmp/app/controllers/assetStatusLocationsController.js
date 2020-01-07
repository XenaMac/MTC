'use strict';
(function () {
    mtcApp.controller('assetStatusLocationsController',
        function assetStatusLocationsController($scope, $filter, $interval, $rootScope, $location, $routeParams, assetsService, generalService) {

            $scope.header = 'Status & Location Tracker';
            $scope.assets = [];
            $scope.isBusyGettingAssets = true;

            $scope.getAssets = function () {
                assetsService.getAssetsAndLocations().then(function (results) {
                    $scope.assets = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.assets.push(new asset(results[i]));
                    }
                    $scope.isBusyGettingAssets = false;
                    $rootScope.redrawTable();
                });
            };
            $scope.getAssets();

            $scope.canEdit = false;
            generalService.getCanEdit('AssetStatusLocations').then(function (result) {
                $scope.canEdit = result;
            });

            function asset(dbAsset) {

                var self = this;
                self.Id = dbAsset.Id;
                self.Item = dbAsset.Item;
                self.OEMSerialNumber = dbAsset.OEMSerialNumber;
                self.Location = dbAsset.Location;
                if (dbAsset.Truck)
                    self.TruckNumber = dbAsset.Truck.VehicleNumber;
                self.IPAddress = dbAsset.IPAddress;

                self.StatusName = dbAsset.AssetStatus.StatusName;
                self.StatusColor = dbAsset.AssetStatus.Color;

                self.RepairCycleTimeInDays = dbAsset.RepairCycleTimeInDays;
                self.LATARMANumber = dbAsset.LATARMANumber;
                self.OEMRMANumber = dbAsset.OEMRMANumber;
                self.OEMRMANumberIssueDate = moment(dbAsset.OEMRMANumberIssueDate).format('MM/DD/YY');

            }

        }
    );
}());