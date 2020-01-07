'use strict';
(function () {
    mtcApp.controller('assetWarrantyConfigurationController',
        function assetWarrantyConfigurationController($scope, $filter, $interval, $rootScope, $location, $routeParams, assetsService, generalService) {

            $scope.header = 'Warranty & Configuration Tracker';
            $scope.assetWarranties = [];            
            $scope.isBusyGettingAssetWarranties = true;

            $scope.getAssetsWarranties = function () {
                assetsService.getAssetsWarranties().then(function (results) {
                    $scope.isBusyGettingAssetWarranties = [];
                    for (var i = 0; i < results.length; i++) {
                        $scope.assetWarranties.push(new asset(results[i]));
                    }
                    $scope.isBusyGettingAssetWarranties = false;
                    $rootScope.redrawTable();
                });
            };
            $scope.getAssetsWarranties();
           
            $scope.canEdit = false;
            generalService.getCanEdit('AssetWarranties').then(function (result) {
                $scope.canEdit = result;
            });

            function asset(dbAsset) {

                var self = this;
                self.Id = dbAsset.Id;
                self.Item = dbAsset.Item;
                self.LATAIDNumber = dbAsset.LATAIDNumber;
                self.OEMSerialNumber = dbAsset.OEMSerialNumber;
                self.WarrantyEndDate = moment(dbAsset.WarrantyEndDate).format('MM/DD/YY');
                self.OperatingSystem = dbAsset.OperatingSystem;
                self.OEMSoftware = dbAsset.OEMSoftware;
                self.LATASoftware = dbAsset.LATASoftware;
                self.Notes = dbAsset.Notes;

            }

        }
    );
}());