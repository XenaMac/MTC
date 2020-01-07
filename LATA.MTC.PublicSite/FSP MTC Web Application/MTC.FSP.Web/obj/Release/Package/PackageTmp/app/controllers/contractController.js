'use strict';
(function () {
    mtcApp.controller('contractController',
        function contractController($scope, $filter, $rootScope, $location, $routeParams, generalService, contractsService) {

            $scope.header = 'Contract';
            $scope.isBusy = false;
            $scope.contractors = [];
            $scope.busyGettingContractors = true;
            $scope.busyGettingBeats = true;
            $scope.beats = [];


            $scope.contract = {
                ContractID: 0,
                AgreementNumber: '',
                StartDate: '',
                EndDate: '',
                MaxObligation: '',
                ContractorId: '',
                BeatId: ''
            };

            $scope.init = function (contractId) {
                $scope.contract.ContractID = contractId;
                if ($scope.contract.ContractID != undefined && $scope.contract.ContractID != "") {
                    $scope.header = "Edit";
                    $scope.getContract();
                    $scope.getContractors();
                    $scope.getBeats();
                } else {
                    $scope.header = "Add";
                    $scope.getContractors();
                    $scope.getBeats();
                }

            };

            $scope.getContractors = function () {
                generalService.getContractors().then(function (results) {
                    for (var i = 0; i < results.length; i++) {
                        $scope.contractors.push(results[i]);
                    }
                    $scope.busyGettingContractors = false;
                });
            };
            $scope.getBeats = function () {
                generalService.getBeatNumbers().then(function (results) {
                    for (var i = 0; i < results.length; i++) {
                        $scope.beats.push(results[i]);
                    };
                    $scope.busyGettingBeats = false;
                });
            };
            
            $scope.getContract = function () {

                $scope.isBusyGettingContract = true;

                contractsService.getContract($scope.contract.ContractID).then(function (result) {
                    
                    $scope.contract = result[0];

                    if (moment($scope.contract.StartDate).isValid())
                        $scope.contract.StartDate = moment($scope.contract.StartDate).format('MM/DD/YYYY');
                    if (moment($scope.contract.EndDate).isValid())
                        $scope.contract.EndDate = moment($scope.contract.EndDate).format('MM/DD/YYYY');
                   
                    $scope.header = "Edit Contract " + $scope.contract.AgreementNumber;

                    $scope.isBusyGettingContract = false;

                });
            };
            $scope.saveContract = function () {

                $scope.isBusy = true;
               
                contractsService.saveContract($scope.contract).then(function (result) {

                    $scope.isBusy = false;
                    $scope.contract.ContractID = result;
                    toastr.success("Saved", "Contract");
                    $scope.getContract();
                });


            };



        }
    );
}());